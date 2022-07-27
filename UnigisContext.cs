using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnigisTest.Models;

namespace UnigisTest
{
    public class UnigisContext : DbContext
    {
        public UnigisContext() : base("UnigisDB")
        {
            Database.SetInitializer<UnigisContext>(new DropCreateDatabaseAlways<UnigisContext>());
        }

        public DbSet<Raza> Razas { get; set; }
        public DbSet<FotoRaza> FotosRaza  { get; set; }
    }
}
