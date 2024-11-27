using AcademicIS.Database.Model;
using AcademicIS.Database.Model.Users;
using ImGuiNET;
using static AcademicIS.GUI.Renderer;

namespace AcademicIS.GUI.Menus.Admins.Users {
	public class AssignStudents : Menu {
		public override int priority => (int)e_Menus.AssignStudents;

		private List<Student> students;
		private List<Group> groups;

		private string[] student_names;
		private string[] group_names;

		private int sel_student = 0;
		private int sel_group = 0;

		public AssignStudents(ContextManager ctx) : base(ctx) {
			if (ctx.database.students.Count() == 0) {
				GUI.ThrowError("No students to assign");
				ctx.renderer.LoadMenu(e_Menus.Admin);

				return;
			}

			if (ctx.database.groups.Count() == 0) {
				GUI.ThrowError("No groups to assign to");
				ctx.renderer.LoadMenu(e_Menus.Admin);

				return;
			}

			students = new(ctx.database.students.ToArray());
			groups = new(ctx.database.groups.ToArray());

			var _student_names = new List<string>();
			var _group_names = new List<string>();

			foreach (var student in students)
				_student_names.Add($"{student.Name} {student.Surname}");

			foreach (var group in groups)
				_group_names.Add(group.Name);

			student_names = _student_names.ToArray();
			group_names = _group_names.ToArray();
		}

		public override void Render() {
			if (student_names == null || group_names == null) { ctx.renderer.LoadMenu(e_Menus.Admin); return; }

			ImGui.Combo("Student", ref sel_student, student_names, student_names.Length);
			ImGui.Combo("Group", ref sel_group, group_names, group_names.Length);

			GUI.SpaceY(5);

			ImGui.Separator();

			if (GUI.FullWidthButton("Assign")) {
				GUI.ShowInfo("User successfully assigned to group!");

				students[sel_student]._Group = groups[sel_group];
				ctx.database.SaveChanges();

				ctx.renderer.LoadMenu(e_Menus.Admin);
			}

			if (GUI.FullWidthButton("Cancel"))
				ctx.renderer.LoadMenu(e_Menus.Admin);
		}
	}
}
