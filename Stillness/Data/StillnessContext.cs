using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stillness.Models;

namespace Stillness.Data
{
    public class StillnessContext : DbContext
    {
        public StillnessContext (DbContextOptions<StillnessContext> options)
            : base(options)
        {
        }

        public DbSet<Stillness.Models.User> User { get; set; } = default!;
        public DbSet<Stillness.Models.Pictures> Pictures { get; set; } = default!;
    }
}
