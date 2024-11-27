using AcademicIS.Database.Model;
using AcademicIS.Database.Model.Users;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace AcademicIS.Database {
	public class DB : DbContext {
		public DbSet<Student> students { get; init; }
		public DbSet<Lecturer> lecturers { get; init; }
		public DbSet<Admin> admins { get; init; }
		public DbSet<Group> groups { get; init; }
		public DbSet<Lecture> lectures { get; init; }
		public DbSet<Grade> grades { get; init; }
		public DbSet<GroupLecture> groupLectures { get; init; }

		public DB(DbContextOptions<DB> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Grade>()
				.HasOne(g => g._Student)
				.WithMany(s => s.Grades)
				.HasForeignKey("StudentId");

			modelBuilder.Entity<Grade>()
				.HasOne(g => g._GroupLecture)
				.WithMany()
				.HasForeignKey("GroupLectureId");

			modelBuilder.Entity<Group>()
				.HasMany(g => g.Students)
				.WithOne(s => s._Group)
				.HasForeignKey("GroupId")
				.IsRequired(false);

			modelBuilder.Entity<GroupLecture>()
				.HasOne(l => l._Lecture)
				.WithMany()
				.HasForeignKey("LectureId");

			modelBuilder.Entity<GroupLecture>()
				.HasOne(l => l._Group)
				.WithMany()
				.HasForeignKey("GroupId");

			modelBuilder.Entity<GroupLecture>()
				.HasOne(l => l._Lecturer)
				.WithMany()
				.HasForeignKey("LecturerId")
				.IsRequired(false);
		}

		public int GetNextAutoIncrement(string tableName) {
			var connection = (MySqlConnection)Database.GetDbConnection();
			connection.Open();

			string query = @"
                SELECT AUTO_INCREMENT
                FROM INFORMATION_SCHEMA.TABLES
                WHERE TABLE_SCHEMA = @Schema AND TABLE_NAME = @TableName;";

			using var command = new MySqlCommand(query, connection);
			command.Parameters.AddWithValue("@Schema", connection.Database);
			command.Parameters.AddWithValue("@TableName", tableName);

			var result = command.ExecuteScalar();

			connection.Close();
			return result != null ? Convert.ToInt32(result) : 1; // Default to 1 if no result found
		}
	}
}
