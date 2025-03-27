using Microsoft.AspNetCore.Mvc;
using Validata.Application.Commands.Products;
using Validata.Application.Commands.Products.Queries;
using Validata.Domain.Dtos;

namespace Validata.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseApiController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductController"/> class.
        /// </summary>
        public ProductController()
        {
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="request">The product details.</param>
        /// <returns>Returns the created product.</returns>
        /// <response code="201">Product created successfully.</response>
        /// <response code="400">Invalid product data provided.</response>
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductRequest request)
        {
            var command = new CreateProductCommand(request.Name, request.Price);
            var createdProduct = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
        }

        /// <summary>
        /// Retrieves a list of all products.
        /// </summary>
        /// <returns>A list of products.</returns>
        /// <response code="200">Returns the list of all products.</response>
        /// <response code="500">Internal server error if the query fails.</response>
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var query = new GetAllProductsQuery();
            var products = await Mediator.Send(query);

            return Ok(products);
        }

        /// <summary>
        /// Retrieves a specific product by ID.
        /// </summary>
        /// <param name="id">The product ID.</param>
        /// <returns>The requested product.</returns>
        /// <response code="200">Product found and returned.</response>
        /// <response code="404">Product not found.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var query = new GetProductByIdQuery(id);
            var product = await Mediator.Send(query);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        /// <summary>
        /// Deletes a product by ID.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <returns>Returns the status of the deletion.</returns>
        /// <response code="204">Product successfully deleted.</response>
        /// <response code="404">Product not found.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var command = new DeleteProductCommand(id);
            var result = await Mediator.Send(command);

            if (!result)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="request">The updated product details.</param>
        /// <returns>
        /// Returns 204 No Content if the update is successful.
        /// Returns 404 Not Found if the product does not exist.
        /// </returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductRequest request)
        {
            var command = new UpdateProductCommand(id, request.Name, request.Price);
            var result = await Mediator.Send(command);

            if (!result)
                return NotFound();

            return NoContent();
        }

    }
}