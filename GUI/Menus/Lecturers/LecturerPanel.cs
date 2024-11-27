using AcademicIS.Database.Model;
using AcademicIS.Database.Model.Users;
using ImGuiNET;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using static AcademicIS.GUI.Renderer;

namespace AcademicIS.GUI.Menus.Lecturers {
	public class LecturerPanel : Menu {
		public override int priority => (int)e_Menus.Lecturer;

		private List<GroupLecture> lectures;
		private string query = string.Empty;

		public LecturerPanel(ContextManager _ctx) : base(_ctx) {
			lectures = new(ctx.database.groupLectures
				.Include(l => l._Lecture)
				.Include(l => l._Group)
				.AsEnumerable()
				.DistinctBy(l => l._Lecture)
				.Where(l => l._Lecturer == ctx.user as Lecturer)
				.ToArray());
		}

		public override void Render() {
			ImGui.SeparatorText("Lecturer panel");

			ImGui.InputText("Name", ref query, 32);

			GUI.SpaceY(5);

			var list = new List<GroupLecture>();

			foreach (var item in lectures) {
				if (!item._Lecture.Name.ToLower().Contains(query.ToLower()))
					continue;

				list.Add(item);
			}

			GUI.Table("Lectures", ["Lecture name", ""], delegate {
				for (int i = 0; i < list.Count; i++) {
					var lecture = list[i];
					DisplayLecture(i, lecture);
				}
			});

			if (list.Count == 0) {
				ImGui.TextColored(Color.Red.ToVector(), "No lectures match the filter");
			}

			GUI.SpaceY(5);

			ImGui.Separator();

			if (GUI.FullWidthButton("Log out"))
				ctx.renderer.LoadMenu(e_Menus.Login);
		}

		public override float GetMenuWidth() {
			float width = 0;

			foreach (var lecture in lectures)
				width = Math.Max(width, ImGui.CalcTextSize(lecture._Lecture.Name).X * 2 + 50);

			return width;
		}

		private void DisplayLecture(int i, GroupLecture lecture) {
			ImGui.TableNextRow();

			ImGui.TableNextColumn();
			ImGui.TextWrapped(lecture._Lecture.Name);

			ImGui.TableNextColumn();
			if (GUI.FullWidthButton($"View##{i}")) {
				ctx.renderer.LoadMenu(e_Menus.ViewLectures, lecture._Lecture);
			}
		}
	}
}
