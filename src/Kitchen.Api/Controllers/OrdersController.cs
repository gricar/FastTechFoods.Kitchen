using Kitchen.Application.Orders.AcceptOrder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace Kitchen.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _dispatcher;

        public OrdersController(IMediator dispatcher)
        {
            _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        }

        [HttpPut("{orderId}/accept")]
        [ProducesResponseType(typeof(Guid), Status201Created)]
        [ProducesResponseType(Status400BadRequest)]
        public async Task<ActionResult<Guid>> AcceptOrder([FromBody] AcceptOrderCommand command)
        {
            var response = await _dispatcher.Send(command);
            return CreatedAtAction(nameof(AcceptOrder), response);
        }
    }
}
