using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hum.Common.DTOs;

namespace Hum.Modules.DomainObjects
{
    public enum TaskItemStatus
    {
        Backlog = 1,
        WIP = 2,
        FollowUp = 3,
        Complete = 4,
        Archive = 5,
        Cancelled = 6
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
        public TaskItemStatus TaskStatus {get;set;}

        [Column("status_dttm")]
        [Required]
        public DateTime StatusDate {get;set;}

        [ForeignKey("TaskId")]
        public TaskItem Task {get;set;}
    }
}
