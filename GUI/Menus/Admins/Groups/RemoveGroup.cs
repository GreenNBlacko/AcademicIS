using AcademicIS.Database.Model;
using ImGuiNET;
using System.Drawing;
using static AcademicIS.GUI.Renderer;

namespace AcademicIS.GUI.Menus.Admins.Groups {
	public class RemoveGroup : Menu {
		public override int priority => (int)e_Menus.RemoveGroup;

		private List<Group> groups = new List<Group>();
		private string query = string.Empty;

		public RemoveGroup(ContextManager _ctx) : base(_ctx) {
			groups.AddRange(ctx.database.groups.ToArray());
		}

		public override void Render() {
			ImGui.SeparatorText("Remove group");

			// Apply filters
			ImGui.InputText("Name", ref query, 32);

			GUI.SpaceY(5);

			var list = new List<Group>();

			foreach (var item in groups) {
				if (!item.Name.ToLower().Contains(query.ToLower()))
					continue;

				list.Add(item);
			}

			GUI.Table("Groups", ["Group name", ""], delegate {
				for (int i = 0; i < list.Count; i++) {
					var group = list[i];
					DisplayGroup(i, group);
				}
			});

			if (list.Count == 0) {
				ImGui.TextColored(Color.Red.ToVector(), "No groups match the filter");
			}

			GUI.SpaceY(5);

			ImGui.Separator();

			if (GUI.FullWidthButton("Cancel"))
				ctx.renderer.LoadMenu(e_Menus.Admin);
		}

		private void DisplayGroup(int i, Group group) {
			ImGui.TableNextRow();

			ImGui.TableNextColumn();
			ImGui.TextWrapped(group.Name);

			ImGui.TableNextColumn();
			if (GUI.FullWidthButton($"Remove##{i}")) {
				GUI.ShowDialog("Are you sure you want to remove this group?", ["No", "Yes"], delegate (int selection) {
					if (selection == 1) {
						ctx.database.groups.Remove(group);
						ctx.database.SaveChanges();
					}

					ctx.renderer.LoadMenu(e_Menus.RemoveGroup);
				});
			}
		}
	}
}
