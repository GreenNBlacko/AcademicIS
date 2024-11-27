using AcademicIS.Database.Model.Users;
using ImGuiNET;
using static AcademicIS.GUI.Renderer;

namespace AcademicIS.GUI.Menus.Admins.Users {
	public class AddUser(ContextManager ctx) : Menu(ctx) {
		public override int priority => (int)e_Menus.AddUsers;

		private string firstName = string.Empty;
		private string lastName = string.Empty;
		private int type = 0;
		private static readonly string[] items = ["Student", "Lecturer"];
		private static readonly string charset = "1234567890abcdefghijklmnoprstquvwxyz";

		public override void Render() {
			ImGui.SeparatorText("Add user");

			ImGui.InputText("First name", ref firstName, 32);
			ImGui.InputText("Last name", ref lastName, 32);

			ImGui.Combo("User type", ref type, items, items.Length);

			GUI.SpaceY(5);

			ImGui.Separator();

			if (GUI.FullWidthButton("Create"))
				CreateUser();

			if (GUI.FullWidthButton("Cancel"))
				ctx.renderer.LoadMenu(e_Menus.Admin);
		}

		private void CreateUser() {
			switch (type) {
				case 0: // Student
					var student = new Student {
						Name = firstName,
						Surname = lastName,
						Password = lastName,
						Username = firstName
					};

					ctx.database.students.Add(student);
					ctx.database.SaveChanges();

					GUI.ShowInfo($"User created successfully!\nLogin details:\nUsername: {student.Username}\nPassword: {student.Password}");
					ctx.renderer.LoadMenu(e_Menus.Admin);
					break;

				case 1: // lecturer
					var lecturer = new Lecturer {
						Name = firstName,
						Surname = lastName,
						Password = lastName,
						Username = firstName
					};

					ctx.database.lecturers.Add(lecturer);
					ctx.database.SaveChanges();

					GUI.ShowInfo($"User created successfully!\nLogin details:\nUsername: {lecturer.Username}\nPassword: {lecturer.Password}");
					ctx.renderer.LoadMenu(e_Menus.Admin);
					break;
			}
		}
	}
}
