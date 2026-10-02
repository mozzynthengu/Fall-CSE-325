using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PizzaController : ControllerBase
{
    private static readonly List<Pizza> pizzas = new()
    {
        new Pizza { Id = 1, Name = "Classic Italian", IsGlutenFree = false },
        new Pizza { Id = 2, Name = "Veggie", IsGlutenFree = true },
        new Pizza { Id = 3, Name = "Pepperoni", IsGlutenFree = false },
        new Pizza { Id = 4, Name = "BBQ Chicken", IsGlutenFree = false },
        new Pizza { Id = 5, Name = "Hawaiian", IsGlutenFree = false }
    };

    [HttpGet]
    public IEnumerable<Pizza> Get()
    {
        return pizzas;
    }

    [HttpGet("{id}")]
    public ActionResult<Pizza> Get(int id)
    {
        var pizza = pizzas.FirstOrDefault(p => p.Id == id);

        if (pizza == null)
        {
            return NotFound();
        }

        return pizza;
    }

    [HttpPost]
    public ActionResult<Pizza> Post(Pizza pizza)
    {
        pizzas.Add(pizza);

        return CreatedAtAction(nameof(Get), new { id = pizza.Id }, pizza);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, Pizza pizza)
    {
        var index = pizzas.FindIndex(p => p.Id == id);

        if (index == -1)
        {
            return NotFound();
        }

        pizza.Id = id;
        pizzas[index] = pizza;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var index = pizzas.FindIndex(p => p.Id == id);

        if (index == -1)
        {
            return NotFound();
        }

        pizzas.RemoveAt(index);

        return NoContent();
    }
}
