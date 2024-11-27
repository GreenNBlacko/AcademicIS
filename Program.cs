using AcademicIS.Database;
using AcademicIS.GUI;
using Microsoft.EntityFrameworkCore;

namespace AcademicIS {
	public class Program {
		private Renderer gui;
		private ContextManager ctx;

		public static void Main() { // Entry point
			new Program().Start().Wait();
		}

		private async Task Start() {
			var connectionString = "Server=localhost;Port=3306;Database=ais;User=ais;Password=password;";

			// Create DbContextOptions
			var options = new DbContextOptionsBuilder<DB>()
				.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
				.EnableSensitiveDataLogging()
				.Options;
			var database = new DB(options);

			database.Database.EnsureCreated();

			ctx = new ContextManager(database);

			gui = new Renderer(ctx);

			await gui.Start();
		}
	}
}