using Microsoft.AspNetCore.Mvc;
using Validata.Application.Commands.Orders;
using Validata.Application.Commands.Orders.Queries;
using Validata.Domain.Dtos;

namespace Validata.API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : BaseApiController
    {
        public OrderController()
        {
        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="request">The order details.</param>
        /// <returns>Returns the created order.</returns>
        /// <response code="201">Order created successfully.</response>
        /// <response code="400">Invalid order data provided.</response>
        /// <response code="404">Customer or one or more products not found.</response>
        /// <response code="500">Internal server error if the creation fails.</response>
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateRequest request)
        {
            var command = new OrderCreateCommand(request.CustomerId, request.Items);
            var createdOrder = await Mediator.Send(command);

            return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrder);
        }

        /// <summary>
        /// Retrieves an order by its ID.
        /// </summary>
        /// <param name="id">The ID of the order to retrieve.</param>
        /// <returns>Returns the order with the specified ID.</returns>
        /// <response code="200">Order found and returned.</response>
        /// <response code="404">Order not found.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var query = new GetOrderByIdQuery(id);
            var order = await Mediator.Send(query);

            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        /// <summary>
        /// Retrieves all orders.
        /// </summary>
        /// <returns>Returns a list of all orders.</returns>
        /// <response code="200">Orders found and returned.</response>
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var query = new GetAllOrdersQuery();
            var orders = await Mediator.Send(query);

            return Ok(orders);
        }

        /// <summary>
        /// Deletes an order by its ID.
        /// </summary>
        /// <param name="id">The ID of the order to delete.</param>
        /// <returns>Returns a status message indicating whether the deletion was successful.</returns>
        /// <response code="200">Order deleted successfully.</response>
        /// <response code="404">Order not found.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var command = new DeleteOrderCommand(id);
            var result = await Mediator.Send(command);

            if (!result)
            {
                return NotFound("Order not found.");
            }

            return Ok("Order deleted successfully.");
        }

        /// <summary>
        /// Updates an existing order.
        /// </summary>
        /// <param name="id">The ID of the order to update.</param>
        /// <param name="request">The updated order details.</param>
        /// <returns>Returns the updated order.</returns>
        /// <response code="200">Order updated successfully.</response>
        /// <response code="400">Invalid order data provided.</response>
        /// <response code="404">Order not found.</response>
        /// <response code="404">Customer not found.</response>
        /// <response code="404">One or more products not found.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderRequest request)
        {
            if (request == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid order data provided.");
            }

            try
            {
                var command = new UpdateOrderCommand(id, request.CustomerId, request.Items);
                var updatedOrder = await Mediator.Send(command);

                return Ok(updatedOrder);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
