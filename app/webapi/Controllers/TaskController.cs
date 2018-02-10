using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using hum_webapi.DTOs;
using hum_webapi.Rules;
using hum_webapi.DomainObjects;

namespace hum_webapi.Controllers
{
    public class TaskController : Controller
    {
        [HttpGet]
        [Route("api/task")]
        public IActionResult GetTasks()
        {
            var svc = new TaskService();
            return Ok(svc.GetTaskItems());
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
        public IActionResult SaveTask([FromBody] TaskItemDTO savetask)
        {
            var svc = new TaskService();
            return Ok(svc.SaveTask(savetask));
        }

        [HttpPut]
        [Route("api/task/{taskid}/{taskstatus}")]
        public IActionResult UpdateTaskStatus(int taskid, string taskstatus)
        {
            DomainObjects.TaskStatus status;
            if (Enum.TryParse(taskstatus.ToUpper(), out status))
            {
                var svc = new TaskService();
                return Ok(svc.UpdateTaskStatus(taskid, status));
            }
            else
                return StatusCode(500, new { message = $"Invalid Status: {taskstatus.ToUpper()}" });
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