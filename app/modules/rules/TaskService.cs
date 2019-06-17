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
        }

        public async Task<string[]> GetTaskStatuses()
        {
            return System.Enum.GetNames(typeof(TaskItemStatus));
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

