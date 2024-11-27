using AcademicIS.Database.Model;
using AcademicIS.Database.Model.Users;
using ImGuiNET;
using Microsoft.EntityFrameworkCore;
using static AcademicIS.GUI.Renderer;

namespace AcademicIS.GUI.Menus.Admins.Users {
	public class AssignLecturers : Menu {
		public override int priority => (int)e_Menus.AssignLecturers;

		private List<Lecturer> lecturers;
		private List<GroupLecture> lectures;

		private string[] lecturer_names;
		private string[] lecture_names;

		private int sel_lecturer = 0;
		private int sel_lecture = 0;

		public AssignLecturers(ContextManager ctx) : base(ctx) {
			if (ctx.database.lecturers.Count() == 0) {
				GUI.ThrowError("No lecturers to assign");
				ctx.renderer.LoadMenu(e_Menus.Admin);

				return;
			}

			if (ctx.database.groupLectures.Count() == 0) {
				GUI.ThrowError("No lectures to assign to");
				ctx.renderer.LoadMenu(e_Menus.Admin);

				return;
			}

			lecturers = new(ctx.database.lecturers.ToArray());
			lectures = new(ctx.database.groupLectures.Include(s => s._Lecture).Include(s => s._Group).ToArray());

			var _lecturer_names = new List<string>();
			var _lecture_names = new List<string>();

			foreach (var student in lecturers)
				_lecturer_names.Add($"{student.Name} {student.Surname}");

			foreach (var lecture in lectures)
				_lecture_names.Add($"{lecture._Lecture.Name}({lecture._Group.Name})");

			lecturer_names = _lecturer_names.ToArray();
			lecture_names = _lecture_names.ToArray();
		}

		public override void Render() {
			if (lecturer_names == null || lecture_names == null) { ctx.renderer.LoadMenu(e_Menus.Admin); return; }

			ImGui.Combo("Lecturer", ref sel_lecturer, lecturer_names, lecturer_names.Length);
			ImGui.Combo("Lecture", ref sel_lecture, lecture_names, lecture_names.Length);

			GUI.SpaceY(5);

			ImGui.Separator();

			if (GUI.FullWidthButton("Assign")) {
				GUI.ShowInfo("User successfully assigned to lecture!");

				lectures[sel_lecture]._Lecturer = lecturers[sel_lecturer];
				ctx.database.SaveChanges();

				ctx.renderer.LoadMenu(e_Menus.Admin);
			}

			if (GUI.FullWidthButton("Cancel"))
				ctx.renderer.LoadMenu(e_Menus.Admin);
		}
	}
}
