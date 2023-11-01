using order_service.Models;
using order_service.Services;
using Microsoft.AspNetCore.Mvc;

namespace order_service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrdersService _OrdersService;

    public OrdersController(OrdersService OrdersService) =>
        _OrdersService = OrdersService;

    [HttpGet]
    public async Task<List<Order>> Get() =>
        await _OrdersService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Order>> Get(string id)
    {
        var Order = await _OrdersService.GetAsync(id);

        if (Order is null)
        {
            return NotFound();
        }

        return Order;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Order newOrder)
    {
        await _OrdersService.CreateAsync(newOrder);

        return CreatedAtAction(nameof(Get), new { id = newOrder.Id }, newOrder);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Order updatedOrder)
    {
        var Order = await _OrdersService.GetAsync(id);

        if (Order is null)
        {
            return NotFound();
        }

        updatedOrder.Id = Order.Id;

        await _OrdersService.UpdateAsync(id, updatedOrder);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var Order = await _OrdersService.GetAsync(id);

        if (Order is null)
        {
            return NotFound();
        }

        await _OrdersService.RemoveAsync(id);

        return NoContent();
    }
}