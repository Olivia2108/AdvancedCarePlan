using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.IDbContext
{
    public interface ICareContext
    {
        DbSet<T>? Set<T>() where T : class; 
        Task<int> SaveChangesAsync(string? ipAddress);
    }
}
