using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace RceServer.Data.Migrations.Models
{
	public class PersistedGrantDbContextWrapper : PersistedGrantDbContext
	{
		private const string ConnectionString = "";

		public PersistedGrantDbContextWrapper()
			: base(new DbContextOptions<PersistedGrantDbContext>(), new OperationalStoreOptions())
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);

			optionsBuilder.UseMySql(ConnectionString);
		}
	}
}
