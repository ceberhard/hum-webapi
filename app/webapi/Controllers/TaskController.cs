using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using hum_webapi.DTOs;
using hum_webapi.Rules;

namespace hum_webapi.Controllers
{
    public class TaskController : Controller
    {
        [HttpGet]
        [Route("api/task")]
        public IEnumerable<TaskItemDTO> GetTasks()
        {
            var svc = new TaskService();
            return svc.GetTaskItems();
        }

        [HttpGet]
        [Route("api/task/{taskid}")]
        public IActionResult GetTask(int taskid)
        {
            var svc = new TaskService();
            return new ObjectResult(svc.GetTaskItem(taskid));
        }

        [HttpPut]
        [Route("api/task")]
        public IActionResult SaveTask([FromBody] TaskItemDTO savetask)
        {
            var svc = new TaskService();
            return new ObjectResult(svc.SaveTask(savetask));
        }

        [HttpDelete]
        [Route("api/task/{taskid}")]
        public IActionResult DeleteTask(int taskid)
        {
            var svc = new TaskService();
            bool success = svc.DeleteTaskItem(taskid);
            if (success)
                return new ObjectResult(new { message = $"Task Deleted: {taskid}" });
            else
                return new ObjectResult(new { message = "Task Delete Failed" });
        }
    }
}