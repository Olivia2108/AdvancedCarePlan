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
        DbSet<PatientCarePlans>? PatientCarePlans { get; set; }
        DbSet<AuditTrail>? AuditLogs { get; set; }

        Task<int> SaveChangesAsync(string? ipAddress);
    }
}
