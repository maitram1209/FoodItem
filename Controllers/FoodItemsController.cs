using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DemoAPI.Models;
using DemoAspCore.Models;
using Newtonsoft.Json;
using System.IO;

namespace DemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodItemsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public FoodItemsController(DatabaseContext context)
        {
            _context = context;
            //get file json
            using (StreamReader r = new StreamReader("FoodItems.json"))
            {
                string json = r.ReadToEnd();
                List<FoodItem> items = JsonConvert.DeserializeObject<List<FoodItem>>(json);

                _context.foodItems.AddRange(items);
                _context.SaveChanges();
            }
        }

        // GET: api/FoodItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodItem>>> GetfoodItems()
        {
            
            return await _context.foodItems.ToListAsync();
        }

        // GET: api/FoodItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FoodItem>> GetFoodItem(int id)
        {
            var foodItem = await _context.foodItems.FindAsync(id);

            if (foodItem == null)
            {
                return NotFound();
            }

            return foodItem;
        }

       /// <summary>
       /// Update item by id
       /// </summary>
       /// <param name="id"></param>
       /// <param name="foodItem"></param>
       /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFoodItem(int id, FoodItem foodItem)
        {
            if (id != foodItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(foodItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

       /// <summary>
       /// Add new FoodItems
       /// Test function by Postman
       /// </summary>
       /// <param name="foodItem"></param>
       /// <returns></returns>
        public async Task<ActionResult<FoodItem>> AddNewFoodItem(FoodItem foodItem)
        {
            _context.foodItems.Add(foodItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFoodItem", new { id = foodItem.Id }, foodItem);
        }
        [HttpPost]

        /// <summary>
        /// Func delete FoodItems 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<FoodItem>> DeleteFoodItem(int id)
        {
            var foodItem = await _context.foodItems.FindAsync(id);
            if (foodItem == null)
            {
                return NotFound();
            }

            _context.foodItems.Remove(foodItem);
            await _context.SaveChangesAsync();

            return foodItem;
        }

        private bool FoodItemExists(int id)
        {
            return _context.foodItems.Any(e => e.Id == id);
        }

    }
}
