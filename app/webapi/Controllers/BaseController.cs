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

        protected ActionResult HandleAppError(HumAppError error)
        {
            if (error != null)
            {
                switch (error.Type)
                {
                    case HumAppErrorType.RestrictedAccess:
                        return StatusCode(403, string.IsNullOrWhiteSpace(error.Message) ? null : new { message = error.Message });
                    case HumAppErrorType.Unauthorized:
                        return StatusCode(401, string.IsNullOrWhiteSpace(error.Message) ? null : new { message = error.Message });
                    case HumAppErrorType.Unexpected:
                        return StatusCode(500, string.IsNullOrWhiteSpace(error.Message) ? null : new { message = error.Message });
                    case HumAppErrorType.Validation:
                        return BadRequest(string.IsNullOrWhiteSpace(error.Message) ? null : new { message = error.Message });
                    default:
                        return StatusCode(500, string.IsNullOrWhiteSpace(error.Message) ? null : new { message = error.Message });
                }
            }
            else
                return StatusCode(500);
        }

    }
}