using AcademicIS.Database.Model;
using ImGuiNET;
using static AcademicIS.GUI.Renderer;

namespace AcademicIS.GUI.Menus.Admins.Lectures {
	public class AssignLectures : Menu {
		public override int priority => (int)e_Menus.AssignLecture;

		private List<Lecture> lectures;
		private List<Group> groups;

		private string[] lecture_names;
		private string[] group_names;

		private int sel_lecture = 0;
		private int sel_group = 0;

		public AssignLectures(ContextManager ctx) : base(ctx) {
			if (ctx.database.lectures.Count() == 0) {
				GUI.ThrowError("No lectures to assign");
				ctx.renderer.LoadMenu(e_Menus.Admin);

				return;
			}

			if (ctx.database.groups.Count() == 0) {
				GUI.ThrowError("No groups to assign to");
				ctx.renderer.LoadMenu(e_Menus.Admin);

				return;
			}

			lectures = new(ctx.database.lectures.ToArray());
			groups = new(ctx.database.groups.ToArray());

			var _lecture_names = new List<string>();
			var _group_names = new List<string>();

			foreach (var lecture in lectures)
				_lecture_names.Add(lecture.Name);

			foreach (var group in groups)
				_group_names.Add(group.Name);

			lecture_names = _lecture_names.ToArray();
			group_names = _group_names.ToArray();
		}

		public override void Render() {
			if (lecture_names == null || group_names == null) { ctx.renderer.LoadMenu(e_Menus.Admin); return; }

			ImGui.Combo("Lecture", ref sel_lecture, lecture_names, lecture_names.Length);
			ImGui.Combo("Group", ref sel_group, group_names, group_names.Length);

			ImGui.Separator();

			if (GUI.FullWidthButton("Assign")) {
				GUI.ShowInfo("Lecture successfully assigned to group!");

				ctx.database.groupLectures.Add(new GroupLecture {
					_Group = groups[sel_group],
					_Lecture = lectures[sel_lecture]
				});
				ctx.database.SaveChanges();

				ctx.renderer.LoadMenu(e_Menus.Admin);
			}

			if (GUI.FullWidthButton("Cancel"))
				ctx.renderer.LoadMenu(e_Menus.Admin);
		}
	}
}
