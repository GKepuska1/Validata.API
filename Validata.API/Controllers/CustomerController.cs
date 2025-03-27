using Microsoft.AspNetCore.Mvc;
using Validata.Application.Commands.Customers;
using Validata.Application.Commands.Customers.Queries;
using Validata.Domain.Dtos;

namespace Validata.API.Controllers
{
    public class CustomerController : BaseApiController
    {
        public CustomerController()
        {

        }

        /// <summary>
        /// Creates a new customer.
        /// </summary>
        /// <param name="request">The customer details.</param>
        /// <returns>Returns the created customer.</returns>
        /// <response code="201">Customer created successfully.</response>
        /// <response code="400">Invalid customer data provided.</response>
        /// <response code="500">Internal server error if the creation fails.</response>
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerRequest request)
        {
            var command = new CreateCustomerCommand(request.FirstName, request.LastName, request.Address);
            var createdCustomer = await Mediator.Send(command);

            if (createdCustomer == null)
            {
                return StatusCode(500, "An error occurred while creating the customer.");
            }

            return CreatedAtAction(nameof(GetCustomerById), new { id = createdCustomer.Id }, createdCustomer);
        }

        /// <summary>
        /// Retrieves a customer by ID.
        /// </summary>
        /// <param name="id">The customer ID.</param>
        /// <returns>The requested customer.</returns>
        /// <response code="200">Customer found and returned.</response>
        /// <response code="404">Customer not found.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var query = new GetCustomerByIdQuery(id);
            var customer = await Mediator.Send(query);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        /// <summary>
        /// Retrieves a list of all customers.
        /// </summary>
        /// <returns>A list of customers.</returns>
        /// <response code="200">Returns the list of all customers.</response>
        /// <response code="500">Internal server error if the query fails.</response>
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var query = new GetAllCustomersQuery();
            var customers = await Mediator.Send(query);

            return Ok(customers);
        }

        /// <summary>
        /// Updates an existing customer.
        /// </summary>
        /// <param name="id">The customer ID to update.</param>
        /// <param name="request">The updated customer details.</param>
        /// <returns>Returns the updated customer.</returns>
        /// <response code="200">Customer updated successfully.</response>
        /// <response code="400">Invalid customer data provided.</response>
        /// <response code="404">Customer not found.</response>
        /// <response code="500">Internal server error if the update fails.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerRequest request)
        {
            var command = new UpdateCustomerCommand(id, request.FirstName, request.LastName, request.Address);
            var result = await Mediator.Send(command);

            if (!result)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Deletes an existing customer.
        /// </summary>
        /// <param name="id">The customer ID to delete.</param>
        /// <returns>No content if the deletion was successful.</returns>
        /// <response code="204">Customer deleted successfully.</response>
        /// <response code="404">Customer not found.</response>
        /// <response code="500">Internal server error if the deletion fails.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var command = new DeleteCustomerCommand(id);
            var result = await Mediator.Send(command);

            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}
