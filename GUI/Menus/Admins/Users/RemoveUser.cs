using AcademicIS.Database.Model;
using AcademicIS.Database.Model.Users;
using ImGuiNET;
using System.Drawing;
using static AcademicIS.GUI.Renderer;

namespace AcademicIS.GUI.Menus.Admins.Users {
	public class RemoveUser : Menu {
		public override int priority => (int)e_Menus.RemoveUsers;

		private List<User> users = new List<User>();
		private string query = string.Empty;
		private bool filterType = false;
		private int userType = 0;
		private static readonly string[] items = ["Student", "Lecturer"];

		public RemoveUser(ContextManager _ctx) : base(_ctx) {
			users.AddRange(ctx.database.students.ToArray());
			users.AddRange(ctx.database.lecturers.ToArray());
		}

		public override void Render() {
			ImGui.SeparatorText("Remove user");

			// Apply filters
			ImGui.InputText("Name", ref query, 32);
			ImGui.Checkbox("Filter by type", ref filterType);

			if (filterType) {
				ImGui.Combo("User type", ref userType, items, items.Length);
			}

			GUI.SpaceY(5);

			var list = new List<User>();

			foreach (var item in users) {
				if (!item.Name.ToLower().Contains(query.ToLower()))
					continue;

				if (!filterType) {
					list.Add(item);
					continue;
				}

				if (item.Type == (User.UserType)userType)
					list.Add(item);
			}

			GUI.Table("Users", ["First name", "Last name", "User type", ""], delegate {
				for (int i = 0; i < list.Count; i++) {
					var user = list[i];
					DisplayUser(i, user);
				}
			});

			if (list.Count == 0) {
				ImGui.TextColored(Color.Red.ToVector(), "No users match the filter");
			}

			GUI.SpaceY(5);

			ImGui.Separator();

			if (GUI.FullWidthButton("Cancel"))
				ctx.renderer.LoadMenu(e_Menus.Admin);
		}

		private void DisplayUser(int i, User user) {
			ImGui.TableNextRow();

			ImGui.TableNextColumn();
			ImGui.Text(user.Name);

			ImGui.TableNextColumn();
			ImGui.Text(user.Surname);

			ImGui.TableNextColumn();
			ImGui.Text(user.Type.ToString());

			ImGui.TableNextColumn();
			if (GUI.FullWidthButton($"Remove##{i}")) {
				GUI.ShowDialog("Are you sure you want to remove this user?", ["No", "Yes"], delegate (int selection) {
					if (selection == 1) {
						switch (user.Type) {
							case User.UserType.Student:
								ctx.database.students.Remove(user as Student);
								ctx.database.SaveChanges();
								break;

							case User.UserType.Lecturer:
								ctx.database.lecturers.Remove(user as Lecturer);
								ctx.database.SaveChanges();
								break;
						}
					}

					ctx.renderer.LoadMenu(e_Menus.RemoveUsers);
				});
			}
		}

		public override float GetMenuWidth() {
			float size = 0;

			var spacing = ImGui.GetStyle().ItemSpacing;
			var padding = ImGui.GetStyle().CellPadding;

			foreach (var user in users) {
				size = Math.Max(size, spacing.X * 4 + (padding.X * 4 +
					ImGui.CalcTextSize(user.Name).X * 2 +
					ImGui.CalcTextSize(user.Surname).X * 2 +
					ImGui.CalcTextSize(user.Type.ToString()).X * 2) / 3 * 4);
			}

			return size;
		}
	}
}
