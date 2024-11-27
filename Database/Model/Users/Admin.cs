namespace AcademicIS.Database.Model.Users {
	public class Admin : User {
		public override UserType Type => UserType.Admin;
	}
}
