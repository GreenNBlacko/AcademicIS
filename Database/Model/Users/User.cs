namespace AcademicIS.Database.Model.Users {
	public abstract class User {
		public int Id { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public virtual UserType Type => UserType.Student;

		public enum UserType {
			Student,
			Lecturer,
			Admin
		}
	}
}
