using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SmsWebAPI.SeedConfig
{
	public class UserRoleConfig : IEntityTypeConfiguration<IdentityUserRole<string>>
	{
		public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
		{
			// No default user-role seeds — roles are assigned at registration or via the rolechanger endpoint.
		}
	}
}
