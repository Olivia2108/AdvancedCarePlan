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
        DbSet<AuditTrail>? AuditLogs { get; set; }
        DbSet<PatientCarePlan>? PatientCarePlan { get; set; }
        Task<int> SaveChangesAsync(string? ipAddress);
    }
}
