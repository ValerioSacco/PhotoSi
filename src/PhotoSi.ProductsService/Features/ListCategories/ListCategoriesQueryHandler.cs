using MediatR;
using PhotoSi.ProductsService.Repositories;

namespace PhotoSi.ProductsService.Features.ListCategories
{
    public class ListCategoriesQueryHandler : IRequestHandler<ListCategoriesQuery, ListCategoriesResponse>
    {
        private readonly ICategoryRepository _categoryRespository;

        public ListCategoriesQueryHandler(ICategoryRepository categoryRespository)
        {
            _categoryRespository = categoryRespository;
        }

        public async Task<ListCategoriesResponse> Handle(ListCategoriesQuery request, CancellationToken cancellationToken)
        {

            var categories = await _categoryRespository
                .ListAllAsync(cancellationToken);

            return new ListCategoriesResponse
            (
                categories.Select(c => new CategoryResponse(
                    c.Id,
                    c.Name,
                    c.Description
                )).ToList()
            );

        }
    }
}
