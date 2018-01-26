using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using hum_webapi.DTOs;

namespace hum_webapi.DomainObjects
{
    [Table("task_item")]
    public class TaskItem
    {
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

        public TaskItemDTO ExportDTO()
        {
            return new TaskItemDTO
            {
                Id = this.Id,
                Title = this.Title,
                Description = this.Description
            };
        }
    }

    
}