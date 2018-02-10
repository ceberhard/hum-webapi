using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using hum_webapi.DomainObjects;
using hum_webapi.DTOs;

namespace hum_webapi.Rules
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
            try
            {
                using (var context = new HumDataContext())
                {
                    TaskItem task = context.TaskItem
                        .Where(t => t.Id == taskid)
                        .Include(x => x.TaskHistory)
                        .FirstOrDefault();
                    return task?.ExportDTO();
                }
            }
            catch (System.Exception err)
            {
                throw err;
            }
        }

        public TaskItemDTO[] GetTaskItems()
        {
            try
            {
                using (var context = new HumDataContext())
                {
                    return context.TaskItem
                        .Include(x => x.TaskHistory)
                        .Select(t => t.ExportDTO()).ToArray();
                }
            }
            catch (System.Exception err)
            {
                throw err;
            }
        }

        public TaskItemDTO SaveTask(TaskItemDTO savetask)
        {
            try
            {
                using (var context = new HumDataContext())
                {
                    using (var trans = context.Database.BeginTransaction())
                    {
                        try
                        {
                            TaskItem task = null;
                            if (savetask.Id > 0)
                            {
                                task = context.TaskItem
                                    .Where(t => t.Id == savetask.Id)
                                    .Include(x => x.TaskHistory)
                                    .FirstOrDefault();
                                if (task == null)
                                    throw new Exception($"Unable to Find Task ID: {savetask.Id}");
                                context.TaskItem.Update(task);
                            }
                            else
                            {
                                task = new TaskItem();
                                task.TaskHistory = new List<TaskHistoryItem>
                                {
                                    new TaskHistoryItem
                                    {
                                        TaskStatus = TaskStatus.BACKLOG,
                                        StatusDate = System.DateTime.Now
                                    }
                                };
                                context.TaskItem.Add(task);
                            }
                            
                            task.Title = savetask.Title;
                            task.Description = savetask.Description;
                            context.SaveChanges();

                            trans.Commit();
                            return task.ExportDTO();
                        }
                        catch(Exception err)
                        {
                            trans.Rollback();
                            throw err;
                        }
                    }
                }
            }
            catch (System.Exception err)
            {
                throw err;
            }
        }
    }
}

