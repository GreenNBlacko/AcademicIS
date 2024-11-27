using ImGuiNET;

namespace AcademicIS.GUI.Menus.Admins {
	public class AdminPanel(ContextManager _ctx) : Menu(_ctx) {
		public override int priority => (int)Renderer.e_Menus.Admin;

		private bool f_UserControl = false;
		private bool f_GroupControl = false;
		private bool f_LectureControl = false;

		public override void Render() {
			ImGui.SeparatorText("Admin panel");

			GUI.FoldoutHeader("Manage users", ref f_UserControl, delegate {
				if (GUI.FullWidthButton("Add user"))
					ctx.renderer.LoadMenu(Renderer.e_Menus.AddUsers);

				if (GUI.FullWidthButton("Remove user"))
					ctx.renderer.LoadMenu(Renderer.e_Menus.RemoveUsers);

				if (GUI.FullWidthButton("Assign students"))
					ctx.renderer.LoadMenu(Renderer.e_Menus.AssignStudents);

				if (GUI.FullWidthButton("Assign lecturers"))
					ctx.renderer.LoadMenu(Renderer.e_Menus.AssignLecturers);
			});

			GUI.FoldoutHeader("Manage groups", ref f_GroupControl, delegate {
				if (GUI.FullWidthButton("Add group"))
					ctx.renderer.LoadMenu(Renderer.e_Menus.AddGroup);

				if (GUI.FullWidthButton("Remove group"))
					ctx.renderer.LoadMenu(Renderer.e_Menus.RemoveGroup);
			});

			GUI.FoldoutHeader("Manage Lectures", ref f_LectureControl, delegate {
				if (GUI.FullWidthButton("Add lecture"))
					ctx.renderer.LoadMenu(Renderer.e_Menus.AddLecture);

				if (GUI.FullWidthButton("Remove lecture"))
					ctx.renderer.LoadMenu(Renderer.e_Menus.RemoveLecture);

				if (GUI.FullWidthButton("Assign lecture to group"))
					ctx.renderer.LoadMenu(Renderer.e_Menus.AssignLecture);
			});

			GUI.SpaceY(5);

			ImGui.Separator();

			if (GUI.FullWidthButton("Log out"))
				ctx.renderer.LoadMenu(Renderer.e_Menus.Login);
		}
	}
}
