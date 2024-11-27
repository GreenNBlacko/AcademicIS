namespace AcademicIS.Database.Model.Users {
	public class Student : User {
		public Group _Group { get; set; }
		public ICollection<Grade> Grades { get; set; } = new List<Grade>();
		public override UserType Type => UserType.Student;
	}
}
