using System;
using System.Linq;
using System.Collections.Generic;
using Hum.Modules.DomainObjects;
using Hum.Common.DTOs;
using Hum.Common.Utility;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Hum.Modules.Rules
{
    public class TaskService
    {
        public async Task<List<TaskItemDTO>> GetTaskItemsAsync(int taskid = default(int))
        {
            using (var ctx = new HumDataContext())
            {
                var tq = ctx.TaskItem.Include(t => t.TaskHistory).Select(t => t);
                if (taskid != default(int))
                    tq = tq.Where(t => t.Id == taskid);

                List<TaskItem> results = await tq.ToListAsync();
                return results.Select(t => t.ExportDTO()).ToList();
            }
        }

        public TaskItemDTO UpdateTaskStatus(int taskid, TaskStatus newstatus)
        {
            // using (var context = new HumDataContext())
            // {
            //     TaskItem task = context.TaskItem
            //         .Where(t => t.Id == taskid)
            //         .Include(t => t.TaskHistory)
            //         .FirstOrDefault();

            //     if (task == null)
            //         throw new Exception($"Unable to Find Task ID: {taskid}");

            //     if (task.TaskHistory == null || task.TaskHistory.Count == 0)
            //         throw new Exception($"Unable to Find History for Task ID: {taskid}");

            //     TaskStatus currentstatus = task.TaskHistory
            //         .OrderBy(x => x.StatusDate)
            //         .Last().TaskStatus;
            //     if (currentstatus != newstatus)
            //     {
            //         switch(currentstatus)
            //         {
            //             case TaskStatus.BACKLOG:
            //                 if (newstatus != TaskStatus.WIP)
            //                     throw new Exception("Tasks in the Backlog can only Move to WIP");
            //                 break;
            //             case TaskStatus.WIP:
            //                 if (newstatus == TaskStatus.COMPLETE)
            //                     throw new Exception("Tasks in WIP can only Move to Backlog or QA");
            //                 break;
            //             case TaskStatus.QA:
            //                 break;
            //             case TaskStatus.COMPLETE:
            //                 throw new Exception("Completed Tasks Cannot Change Status");
            //         }

            //         task.TaskHistory.Add(new TaskHistoryItem
            //         {
            //             TaskStatus = newstatus,
            //             StatusDate = DateTime.Now
            //         });

            //         context.SaveChanges();

                    
            //     }
            //     return task.ExportDTO();
            // }
            return new TaskItemDTO();
        }

        public async Task<TaskItemDTO> SaveTaskAsync(TaskItemDTO savetask)
        {
            using (var ctx = new HumDataContext())
            {
                TaskItem tsk = null;
                bool isvalid;
                string message;
                
                // Add New Task
                if (savetask.Id > 0)
                {
                    tsk = await ctx.TaskItem
                        .Include(t => t.TaskHistory)
                        .FirstOrDefaultAsync(t => t.Id == savetask.Id);
                    if (tsk == null)
                        throw new HumAppError(HumAppErrorType.Validation, $"Invalid Task Item Id: {savetask.Id}");
                }

                (isvalid, message) = ValidateTaskItem(savetask, tsk);
                if (isvalid)
                {
                    tsk = tsk ?? new TaskItem();
                    tsk.ImportDTO(savetask);
                    if (tsk.Id == 0) await ctx.TaskItem.AddAsync(tsk);
                    ctx.ChangeTracker.DetectChanges();
                    await ctx.SaveChangesAsync();
                    return tsk.ExportDTO();
                }
                else
                    throw new HumAppError(HumAppErrorType.Validation, message);
            }

            // using (var context = new HumDataContext())
            // {
            //     using (var trans = context.Database.BeginTransaction())
            //     {
            //         try
            //         {
            //             TaskItem task = null;
            //             if (savetask.Id > 0)
            //             {
            //                 task = context.TaskItem
            //                     .Where(t => t.Id == savetask.Id)
            //                     .Include(x => x.TaskHistory)
            //                     .FirstOrDefault();
            //                 if (task == null)
            //                     throw new Exception($"Unable to Find Task ID: {savetask.Id}");
            //                 context.TaskItem.Update(task);
            //             }
            //             else
            //             {
            //                 task = new TaskItem();
            //                 task.TaskHistory = new List<TaskHistoryItem>
            //                 {
            //                     new TaskHistoryItem
            //                     {
            //                         TaskStatus = TaskStatus.BACKLOG,
            //                         StatusDate = System.DateTime.Now
            //                     }
            //                 };
            //                 context.TaskItem.Add(task);
            //             }
                        
            //             task.Title = savetask.Title;
            //             task.Description = savetask.Description;
            //             context.SaveChanges();

            //             trans.Commit();
            //             return task.ExportDTO();
            //         }
            //         catch(Exception err)
            //         {
            //             trans.Rollback();
            //             throw err;
            //         }
            //     }
            // }
            return new TaskItemDTO();
        }

        private (bool, string) ValidateTaskItem(TaskItemDTO dto, TaskItem existingitem)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                return (false, "TASK TITLE CANNOT BE EMPTY");

            if (existingitem == null)
            {
                if (!string.IsNullOrEmpty(dto.Status) &&
                    dto.Status.ToUpper() != TaskItemStatus.Backlog.ToString().ToUpper())
                {
                    return (false, $"Invalid Task Status: {dto.Status}");
                }
            }
            else
            {
                if (System.Enum.TryParse(dto.Status, true, out TaskItemStatus newstatus))
                {
                    TaskHistoryItem lateststatus = existingitem.TaskHistory.OrderByDescending(th => th.StatusDate).FirstOrDefault();
                    if (!CheckStatusChange(lateststatus.TaskStatus, newstatus))
                        return (false, $"Invalid Task Status Transition: {lateststatus.TaskStatus} -> {newstatus}");
                }
                else
                    return (false, $"Invalid Task Status: {dto.Status}");
            }

            return (true, string.Empty);
        }

        private bool CheckStatusChange(TaskItemStatus oldstatus, TaskItemStatus newstatus)
        {
            if (oldstatus != newstatus)
            {
                switch(oldstatus)
                {
                    case TaskItemStatus.Backlog:
                        if (!new TaskItemStatus[]{
                                TaskItemStatus.WIP,
                                TaskItemStatus.Cancelled}.Contains(newstatus))
                            return false;
                        break;
                    case TaskItemStatus.WIP:
                        if (!new TaskItemStatus[]{
                                TaskItemStatus.Backlog,
                                TaskItemStatus.Complete,
                                TaskItemStatus.FollowUp}.Contains(newstatus))
                            return false;
                        break;
                    case TaskItemStatus.FollowUp:
                        if (!new TaskItemStatus[]{
                                TaskItemStatus.WIP,
                                TaskItemStatus.Complete}.Contains(newstatus))
                            return false;
                        break;
                    case TaskItemStatus.Complete:
                        if (!new TaskItemStatus[]{
                                TaskItemStatus.WIP,
                                TaskItemStatus.Archive}.Contains(newstatus))
                            return false;
                        break;
                    case TaskItemStatus.Archive:
                    case TaskItemStatus.Cancelled:
                        if (!new TaskItemStatus[]{
                                TaskItemStatus.Backlog}.Contains(newstatus))
                            return false;
                        break;
                }
            }
            return true;
        }
    }
}

