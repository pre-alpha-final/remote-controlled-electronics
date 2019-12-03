using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RceServer.Data.Migrations.Models
{
	public class UsersDbContext : IdentityDbContext<IdentityUser>
	{
		private const string ConnectionString = "";

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);

			optionsBuilder.UseMySql(ConnectionString);
		}
	}
}
