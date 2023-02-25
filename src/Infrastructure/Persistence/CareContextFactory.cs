using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    internal class CareContextFactory : CareDesignTimeDbContextFactoryBase<CareContext>
    {
        protected override CareContext CreateNewInstance(DbContextOptions<CareContext> options)
        {
            return new CareContext(options);
        }
    }
}
