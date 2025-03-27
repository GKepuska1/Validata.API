using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Validata.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController() : ControllerBase
    {
        private ISender _mediator = null!;

        /// <summary>
        /// Mediator
        /// </summary>
        protected ISender Mediator
        {
            get
            {
                if (_mediator == null)
                    _mediator = HttpContext.RequestServices.GetRequiredService<ISender>();
                return _mediator;
            }
        }
    }
}
