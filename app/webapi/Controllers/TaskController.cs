using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hum.Common.DTOs;
using Hum.Modules.Rules;
using Hum.Common.Utility;

namespace Hum.WebAPI.Controllers
{
    public class TaskController : BaseController
    {
        [HttpGet]
        [Route("api/task")]
        public IActionResult GetTasks()
        {
            return Ok(base.TaskService.GetTaskItems());
        }

        [HttpGet]
        [Route("api/task/{taskid}")]
        public IActionResult GetTask(int taskid)
        {
            var svc = new TaskService();
            return Ok(svc.GetTaskItem(taskid));
        }

        [HttpPut]
        [Route("api/task")]
        public async Task<IActionResult> SaveTask([FromBody] TaskItemDTO savetask)
        {
            throw new Exception("Chris Test");

            // return Ok(base.TaskService.SaveTask(savetask));
            return Ok();
        }

        [HttpPut]
        [Route("api/task/{taskid}/{taskstatus}")]
        public IActionResult UpdateTaskStatus(int taskid, string taskstatus)
        {
            // DomainObjects.TaskStatus status;
            // if (Enum.TryParse(taskstatus.ToUpper(), out status))
            // {
            //     var svc = new TaskService();
            //     return Ok(svc.UpdateTaskStatus(taskid, status));
            // }
            // else
            //     return StatusCode(500, new { message = $"Invalid Status: {taskstatus.ToUpper()}" });
            return Ok();
        }

        [HttpDelete]
        [Route("api/task/{taskid}")]
        public IActionResult DeleteTask(int taskid)
        {
            var svc = new TaskService();
            bool success = svc.DeleteTaskItem(taskid);
            if (success)
                return Ok(new { message = $"Task Deleted: {taskid}" });
            else
                return StatusCode(500, new { message = "Task Delete Failed" });
        }
    }
}