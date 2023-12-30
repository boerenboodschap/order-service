using order_service.Models;
using order_service.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace order_service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrdersService _OrdersService;

    static readonly string orderApiUrl = "http://localhost:8080";
    private static HttpClient sharedClient = new()
    {
        BaseAddress = new Uri(orderApiUrl),
    };

    public OrdersController(OrdersService OrdersService) =>
        _OrdersService = OrdersService;

    [HttpGet("test")]
    public async Task<ActionResult<string>> GetTestApi()
    {
        using HttpResponseMessage response = await sharedClient.GetAsync("api/products/");

        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"{jsonResponse}\n");

        return jsonResponse;
    }

    [HttpGet]
    public async Task<List<Order>> Get()
    {
        return await _OrdersService.GetAsync();
    }

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

    [HttpGet("{id:length(24)}/submit")]
    public async Task<IActionResult> SubmitOrder(string id)
    {
        var Order = await _OrdersService.GetAsync(id);

        if (Order is null)
        {
            return NotFound();
        }

        if (Order.Products.Count <= 0)
        {
            return BadRequest("order has no products");
        }

        if (Order.Status == "CONFIRMED")
        {
            return BadRequest("order is already confirmed");
        }

        foreach (KeyValuePair<string, int> Product in Order.Products)
        {
            // make a synchronous request to the product service to remove products from the stock
            using HttpResponseMessage response = await sharedClient.GetAsync($"api/products/{Product.Key}/stock/{Product.Value}");
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("stock check failed");
            }
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"{jsonResponse}\n");
        }

        Order.Status = "CONFIRMED";

        await _OrdersService.UpdateAsync(id, Order);

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