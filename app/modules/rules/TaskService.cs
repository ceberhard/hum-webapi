using System;
using System.Linq;
using System.Collections.Generic;
using Hum.Modules.DomainObjects;
using Hum.Common.DTOs;

namespace Hum.Modules.Rules
{
    public class TaskService
    {
        public bool DeleteTaskItem(int taskid)
        {
            try
            {
                using (var context = new HumDataContext())
                {
                    TaskItem task = context.TaskItem.Where(t => t.Id == taskid).FirstOrDefault();
                    if (task == null)
                        throw new Exception($"Unable to Find Task ID: {taskid}");
                    context.TaskItem.Remove(task);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (System.Exception err)
            {
                throw err;
            }
        }

        public TaskItemDTO GetTaskItem(int taskid)
        {
            // try
            // {
            //     using (var context = new HumDataContext())
            //     {
            //         TaskItem task = context.TaskItem
            //             .Where(t => t.Id == taskid)
            //             .Include(x => x.TaskHistory)
            //             .FirstOrDefault();
            //         return task?.ExportDTO();
            //     }
            // }
            // catch (System.Exception err)
            // {
            //     throw err;
            // }
            return new TaskItemDTO();
        }

        public TaskItemDTO[] GetTaskItems()
        {
            // try
            // {
            //     using (var context = new HumDataContext())
            //     {
            //         return context.TaskItem
            //             .Include(x => x.TaskHistory)
            //             .Select(t => t.ExportDTO()).ToArray();
            //     }
            // }
            // catch (System.Exception err)
            // {
            //     throw err;
            // }
            return new TaskItemDTO[0];
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

        public TaskItemDTO SaveTask(TaskItemDTO savetask)
        {
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
    }
}

