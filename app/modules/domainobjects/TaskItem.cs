using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using Hum.Common.DTOs;

namespace Hum.Modules.DomainObjects
{
    [Table("task_item")]
    public class TaskItem
    {
        public TaskItem()
        {
            this.TaskHistory = new List<TaskHistoryItem>();
        }

        [Column("id")]
        [Required]
        public int Id {get;set;}

        [Column("title")]
        [StringLength(100)]
        [Required]
        public string Title {get;set;}

        [Column("description")]
        [StringLength(1000)]
        public string Description {get;set;}

        public ICollection<TaskHistoryItem> TaskHistory { get;set; }

        public TaskItemDTO ExportDTO()
        {
            var t = new TaskItemDTO
            {
                Id = this.Id,
                Title = this.Title,
                Description = this.Description
            };

            if (this.TaskHistory.Count > 0)
            {
                TaskHistoryItem lateststatus = this.TaskHistory
                    .OrderByDescending(th => th.StatusDate)
                    .FirstOrDefault();
                t.Status = lateststatus.TaskStatus.ToString();
                t.LastUpdated = lateststatus.StatusDate.ToString("0:s");
            }

            return t;
        }

        public void ImportDTO(TaskItemDTO taskitemdto)
        {
            this.Title = taskitemdto.Title;
            this.Description = taskitemdto.Description;

            if (this.TaskHistory.Count == 0)
            {
                this.TaskHistory.Add(new TaskHistoryItem {
                    TaskStatus = TaskItemStatus.Backlog,
                    StatusDate = System.DateTime.Now
                });
            }
            else
            {
                TaskItemStatus newstatus = (TaskItemStatus) System.Enum.Parse(typeof(TaskItemStatus), taskitemdto.Status, true);
                this.TaskHistory.Add(new TaskHistoryItem {
                    TaskStatus = newstatus,
                    StatusDate = System.DateTime.Now
                });
            }
        }
    }
}