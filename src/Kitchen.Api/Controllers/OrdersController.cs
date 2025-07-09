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

        [HttpPut("{orderId:guid}/accept")]
        [ProducesResponseType(typeof(AcceptOrderResponse), Status200OK)]
        [ProducesResponseType(Status400BadRequest)]
        public async Task<ActionResult<AcceptOrderResponse>> AcceptOrder([FromRoute] Guid orderId, CancellationToken cancellationToken)
        {
            var response = await _dispatcher.Send(new AcceptOrderCommand(orderId), cancellationToken);
            return Ok(response);
        }
    }
}
