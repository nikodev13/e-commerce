using ECommerce.Application.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Products;

[Route("api/product/category")]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllCategoriesQuery());
        return Ok(result);
    } 
    
    [HttpGet(template: "{id}")]
    public Task<IActionResult> GetById([FromRoute] int id)
    {
        return null;
    } 
}