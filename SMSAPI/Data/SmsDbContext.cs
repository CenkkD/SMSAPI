using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmsWebAPI.SeedConfig;
using SmsWebAPI.Entities;

namespace SmsWebAPI.Data
{
	public class SmsDbContext : IdentityDbContext<User, Role, string>
	{
		public SmsDbContext(DbContextOptions<SmsDbContext> options) : base(options)
		{


		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.ApplyConfiguration(new RoleConfig());
			builder.ApplyConfiguration(new UserRoleConfig());
		}


	}
}
