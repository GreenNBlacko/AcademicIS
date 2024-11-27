using AcademicIS.Database.Model;
using ImGuiNET;
using System.Drawing;

namespace AcademicIS.GUI.Menus.Admins.Groups {
	public class AddGroup(ContextManager _ctx) : Menu(_ctx) {
		public override int priority => (int)Renderer.e_Menus.AddGroup;

		private string groupName = string.Empty;

		public override void Render() {
			ImGui.SeparatorText("Add group");

			ImGui.InputText("Group name", ref groupName, 32);

			if (groupName == string.Empty) {
				ImGui.TextColored(Color.Red.ToVector(), "Group name cannot be empty");

				GUI.SpaceY(5);

				ImGui.Separator();

				goto skipCreate;
			}

			GUI.SpaceY(5);

			ImGui.Separator();

			if (GUI.FullWidthButton("Add group")) {
				ctx.database.groups.Add(new Group { Name = groupName });
				ctx.database.SaveChanges();

				GUI.ShowInfo("Group created successfully!");
				ctx.renderer.LoadMenu(Renderer.e_Menus.Admin);
			}

		skipCreate:

			if (GUI.FullWidthButton("Cancel"))
				ctx.renderer.LoadMenu(Renderer.e_Menus.Admin);
		}
	}
}
