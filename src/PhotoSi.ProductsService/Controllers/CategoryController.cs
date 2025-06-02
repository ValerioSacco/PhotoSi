using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoSi.ProductsService.Features.ListCategories;
using Swashbuckle.AspNetCore.Annotations;

namespace PhotoSi.ProductsService.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [SwaggerOperation(Summary = "Get list of product categories", Description = "Retrieves the list of available categories")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the categories details", typeof(ListCategoriesResponse))]
        [HttpGet("/categories", Name = "List all products categories")]
        public async Task<IActionResult> List(CancellationToken cancellationToken)
        {
            var categories = await _mediator.Send(new ListCategoriesQuery(), cancellationToken);
            return Ok(categories);
        }
    }
}
