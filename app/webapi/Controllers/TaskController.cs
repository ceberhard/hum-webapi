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
        [Route("api/v1/task")]
        [Route("api/v1/task/{taskid}")]
        public async Task<IActionResult> GetTaskItemsAsync(int taskid = default(int))
        {
            return Ok(await base.TaskService.GetTaskItemsAsync(taskid));
        }

        [HttpPut]
        [Route("api/v1/task")]
        public async Task<IActionResult> SaveTaskAsync([FromBody] TaskItemDTO savetask)
        {
            return Ok(await base.TaskService.SaveTaskAsync(savetask));
        }
    }
}