using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hum.Common.DTOs;

namespace Hum.Modules.DomainObjects
{
    public enum TaskStatus
    {
        BACKLOG = 1,
        WIP = 2,
        QA = 3,
        COMPLETE = 4
    }

    [Table("task_history")]
    public class TaskHistoryItem
    {
        [Column("id")]
        [Required]
        public int Id {get;set;}

        [Column("task_id")]
        [Required]
        public int TaskId {get;set;}

        [Column("task_status_id")]
        [Required]
        public TaskStatus TaskStatus {get;set;}

        [Column("status_dttm")]
        [Required]
        public DateTime StatusDate {get;set;}

        [ForeignKey("TaskId")]
        public TaskItem Task {get;set;}

        public TaskHistoryItemDTO ExportDTO()
        {
            return new TaskHistoryItemDTO
            {
                Status = this.TaskStatus.ToString(),
                HistoryDate = this.StatusDate.ToString()
            };
        }
    }
}
