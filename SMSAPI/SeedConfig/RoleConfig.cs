using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmsWebAPI.Entities;

namespace SmsWebAPI.SeedConfig
{
	public class RoleConfig : IEntityTypeConfiguration<Role>
	{
		public void Configure(EntityTypeBuilder<Role> entityTypeBuilder)
		{
			entityTypeBuilder.HasData(
				new Role
				{
					Id = "639de03f-7876-4fff-96ec-37f8bd3bf180",
					Name = "Customer",
					NormalizedName = "CUSTOMER",
					Description = "Customer role for users"

				},
				new Role
				{
					Id = "78udf5dc-d9s5-h581-6u5g-16k3dt3sd762",
					Name = "Stock Manager",
					NormalizedName = "STOCKMANAGER",
					Description = "Stock Manager role for users"

				},
				new Role
				{
					Id = "43mtp8kr-b7a3-d725-3c8g-75s8ba8uf529",
					Name = "Admin",
					NormalizedName = "ADMIN",
					Description = "Admin role for users"

				}
			);
		}
	}
}
