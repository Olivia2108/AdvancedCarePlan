using CareData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareData.DataContext.Contracts
{
	public interface ICareContext
	{
		DbSet<AuditTrail>? AuditLogs { get; set; }
		DbSet<CarePlan>? CarePlan { get; set; }
		Task<int> SaveChangesAsync(string? ipAddress);
	}
}
