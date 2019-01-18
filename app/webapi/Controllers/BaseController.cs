using Hum.Common.Utility;
using Hum.Modules.Rules;
using Microsoft.AspNetCore.Mvc;

namespace Hum.WebAPI.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        private TaskService _task_service = null;
        protected TaskService TaskService
        {
            get { _task_service = _task_service ?? new TaskService(); return _task_service; }
        }
    }
}