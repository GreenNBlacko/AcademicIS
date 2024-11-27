using AcademicIS.Database.Model;
using ImGuiNET;
using System.Drawing;
using static AcademicIS.GUI.Renderer;

namespace AcademicIS.GUI.Menus.Admins.Lectures {
	public class RemoveLecture : Menu {
		public override int priority => (int)e_Menus.RemoveLecture;

		private List<Lecture> lectures = new();
		private string query = string.Empty;

		public RemoveLecture(ContextManager _ctx) : base(_ctx) {
			lectures.AddRange(ctx.database.lectures.ToArray());
		}

		public override void Render() {
			ImGui.SeparatorText("Remove lecture");

			// Apply filters
			ImGui.InputText("Name", ref query, 32);

			GUI.SpaceY(5);

			var list = new List<Lecture>();

			foreach (var item in lectures) {
				if (!item.Name.ToLower().Contains(query.ToLower()))
					continue;

				list.Add(item);
			}

			GUI.Table("Lectures", ["Lecture name", ""], delegate {
				for (int i = 0; i < list.Count; i++) {
					var lecture = list[i];
					DisplayGroup(i, lecture);
				}
			});

			if (list.Count == 0) {
				ImGui.TextColored(Color.Red.ToVector(), "No lectures match the filter");
			}

			GUI.SpaceY(5);

			ImGui.Separator();

			if (GUI.FullWidthButton("Cancel"))
				ctx.renderer.LoadMenu(e_Menus.Admin);
		}

		private void DisplayGroup(int i, Lecture lecture) {
			ImGui.TableNextRow();

			ImGui.TableNextColumn();
			ImGui.TextWrapped(lecture.Name);

			ImGui.TableNextColumn();
			if (GUI.FullWidthButton($"Remove##{i}")) {
				GUI.ShowDialog("Are you sure you want to remove this lecture?", ["No", "Yes"], delegate (int selection) {
					if (selection == 1) {
						ctx.database.lectures.Remove(lecture);
						ctx.database.SaveChanges();
					}

					ctx.renderer.LoadMenu(e_Menus.RemoveLecture);
				});
			}
		}
	}
}
