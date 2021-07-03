using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoAspCore.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<FoodItem> foodItems { get; set; }

    }
}
