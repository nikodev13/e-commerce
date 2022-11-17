using ECommerce.API.Utilities;
using ECommerce.Application.Categories;
using ECommerce.Application.Categories.Commands;
using ECommerce.Application.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Products;

[Route("api/products/categories")]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<CategoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllCategoriesQuery());
        return result.Resolve(successStatusCode: StatusCodes.Status200OK);
    } 
    
    [HttpGet(template: "{id:guid}")]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new GetCategoryByIdQuery(id));
        return result.Resolve(successStatusCode: StatusCodes.Status200OK);
    } 
    
    [HttpGet(template: "{name}")]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByName([FromRoute] string name)
    {
        var result = await _mediator.Send(new GetCategoryByNameQuery(name));
        return result.Resolve(successStatusCode: StatusCodes.Status200OK);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CategoryNameRequest nameRequest)
    {
        var result = await _mediator.Send(new CreateCategoryCommand(nameRequest.CategoryName));
        return result.Resolve(successStatusCode: StatusCodes.Status201Created);
    }
    
    [HttpPut(template: "{id:guid}")]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] CategoryNameRequest nameRequest)
    {
        var result = await _mediator.Send(new UpdateCategoryCommand(id, nameRequest.CategoryName));
        return result.Resolve(successStatusCode: StatusCodes.Status204NoContent);
    }
    
    [HttpDelete(template: "{id:guid}")]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new DeleteCategoryCommand(id));
        return result.Resolve(successStatusCode: StatusCodes.Status204NoContent);
    }
    
}