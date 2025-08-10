using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SmsWebAPI.SeedConfig
{
	public class UserRoleConfig : IEntityTypeConfiguration<IdentityUserRole<string>>
	{
		public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
		{
			builder.HasData(
				new IdentityUserRole<string>
				{
					UserId = "0f9fe22a-0347-4fb9-9891-fb801308849b",
					RoleId = "43mtp8kr-b7a3-d725-3c8g-75s8ba8uf529",
				},
				new IdentityUserRole<string>
				{
					UserId = "3cf4fbe9-1a45-4a9a-9427-f717d574d5ac",
					RoleId = "639de03f-7876-4fff-96ec-37f8bd3bf180",

				}
			);
		}
	}
}
