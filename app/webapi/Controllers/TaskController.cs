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
            return Ok(await base.TaskService.SaveTaskAsync(savetask));
        }
    }
}