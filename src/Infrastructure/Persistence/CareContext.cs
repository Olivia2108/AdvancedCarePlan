using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using static Domain.Enums.GeneralEnums;
using Application.Common.Interfaces.IDbContext;
using Application.Dto;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Persistence
{
    public class CareContext: DbContext, ICareContext
	{

        private readonly IHttpContextAccessor _accessor = new HttpContextAccessor();
        public CareContext()
		{
		}

		public CareContext(DbContextOptions<CareContext> options) : base(options)
		{

		}

		public DbSet<PatientCarePlans>? PatientCarePlans { get; set; }
		public DbSet<AuditTrail>? AuditLogs { get; set; }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{

			}
		}


		protected override void OnModelCreating(ModelBuilder? modelBuilder)
		{

			if (modelBuilder == null)
				return;

			modelBuilder.Entity<PatientCarePlans>(entity =>
			{
				entity.Property(e => e.Id).ValueGeneratedOnAdd();

				entity.Property(e => e.UserName).HasMaxLength(450).IsRequired();

				entity.Property(e => e.PatientName).HasMaxLength(450).IsRequired();

				entity.Property(e => e.Title).HasMaxLength(450).IsRequired();

				entity.Property(e => e.Outcome).HasMaxLength(1000);

				entity.Property(e => e.Action).HasMaxLength(1000).IsRequired();

				entity.Property(e => e.Completed).HasDefaultValue(false);

				entity.Property(e => e.Reason).HasMaxLength(1000).IsRequired();

				entity.Property(e => e.IsActive).HasDefaultValue(true);

				entity.Property(e => e.DateCreated).HasDefaultValueSql("getdate()");
			});

			modelBuilder.ApplyConfigurationsFromAssembly(typeof(CareContext).Assembly);

			base.OnModelCreating(modelBuilder);

			 
		}




		public virtual async Task<int> SaveChangesAsync(string? ipAddress)
		{
			PerformEntityAudit();
			OnBeforeSaveChanges(ipAddress);
			var result = await base.SaveChangesAsync();
			return result;
		}

		private string GetModifiedUser()
        {
            var name = _accessor?.HttpContext?.User?.Identity?.Name;
            return !string.IsNullOrEmpty(name) ? name : "Anonymous";
        }

        private void OnBeforeSaveChanges(string? ipAddress = null)
		{
			ChangeTracker.DetectChanges();
			var auditEntries = new List<AuditTrailDto>();
			foreach (var entry in ChangeTracker.Entries())
			{
				if (entry.Entity is AuditTrail || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
					continue;
				var auditEntry = new AuditTrailDto(entry)
				{
					TableName = entry.Entity.GetType().Name,
					IpAddress = ipAddress
				};
				auditEntries.Add(auditEntry);
				foreach (var property in entry.Properties)
				{
					var propertyName = property.Metadata.Name;
					if (property.Metadata.IsPrimaryKey())
					{
						auditEntry.KeyValues[propertyName] = property.CurrentValue;
						continue;
					}
					switch (entry.State)
					{
						case EntityState.Added:
							auditEntry.AuditType = AuditType.Create;
							auditEntry.NewValues[propertyName] = property.CurrentValue;
							break;
						case EntityState.Deleted:
							auditEntry.AuditType = AuditType.Delete;
							auditEntry.OldValues[propertyName] = property.OriginalValue;
							break;
						case EntityState.Modified:
							if (property.IsModified)
							{
								auditEntry.ChangedColumns.Add(propertyName);
								auditEntry.AuditType = AuditType.Update;
								auditEntry.OldValues[propertyName] = property.OriginalValue;
								auditEntry.NewValues[propertyName] = property.CurrentValue;
							}
							break;
						case EntityState.Detached:
							break;
						case EntityState.Unchanged:
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
				}
			}
			foreach (var auditEntry in auditEntries)
            {
                AuditLogs?.Add(auditEntry.ToAudit()); 
				 
			}
		}

		private void PerformEntityAudit()
		{
			foreach (var entry in ChangeTracker.Entries<PatientCarePlans>())
			{
				switch (entry.State)
				{
					case EntityState.Added:
						var currentDateTime = DateTime.Now;
						entry.Entity.DateCreated = currentDateTime;
						entry.Entity.IsDeleted = false;
						entry.Entity.IsActive = true;
						entry.Entity.CreatedBy = GetModifiedUser();

                        break;

					case EntityState.Modified:
                        entry.Entity.ModifiedBy = GetModifiedUser();
                        entry.Entity.DateModified = DateTime.Now;
                        //entry.Entity.IsActive = true;
                        break;

					case EntityState.Deleted:
						entry.State = EntityState.Modified;
						entry.Entity.DateDeleted = DateTime.Now;
						entry.Entity.IsDeleted = true;
						entry.Entity.IsActive = false;
						break;
				}
			}
		}
	}
}
