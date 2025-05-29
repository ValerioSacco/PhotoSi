using MediatR;
using Microsoft.AspNetCore.Mvc;
using PhotoSi.ProductsService.Features.ListCategories;

namespace PhotoSi.ProductsService.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/categories", Name = "List all products categories")]
        public async Task<IActionResult> List(CancellationToken cancellationToken)
        {
            var categories = await _mediator.Send(new ListCategoriesQuery(), cancellationToken);
            return Ok(categories);
        }
    }
}
