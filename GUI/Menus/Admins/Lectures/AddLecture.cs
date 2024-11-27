using AcademicIS.Database.Model;
using ImGuiNET;
using System.Drawing;

namespace AcademicIS.GUI.Menus.Admins.Lectures {
	public class AddLecture(ContextManager _ctx) : Menu(_ctx) {
		public override int priority => (int)Renderer.e_Menus.AddLecture;

		private string lectureName = string.Empty;

		public override void Render() {
			ImGui.SeparatorText("Add Lecture");

			ImGui.InputText("Lecture name", ref lectureName, 32);

			if (lectureName == string.Empty) {
				ImGui.TextColored(Color.Red.ToVector(), "lecture name cannot be empty");
				goto skipCreate;
			}

			if (GUI.FullWidthButton("Add lecture")) {
				ctx.database.lectures.Add(new Lecture { Name = lectureName });
				ctx.database.SaveChanges();

				GUI.ShowInfo("Lecture created successfully!");
				ctx.renderer.LoadMenu(Renderer.e_Menus.Admin);
			}

		skipCreate:

			if (GUI.FullWidthButton("Cancel"))
				ctx.renderer.LoadMenu(Renderer.e_Menus.Admin);
		}
	}
}
