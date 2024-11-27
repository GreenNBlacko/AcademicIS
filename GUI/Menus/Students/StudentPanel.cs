using AcademicIS.Database.Model;
using AcademicIS.Database.Model.Users;
using ImGuiNET;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using static AcademicIS.GUI.Renderer;

namespace AcademicIS.GUI.Menus.Students {
	public class StudentPanel : Menu {
		public override int priority => (int)e_Menus.Student;

		private Student student;

		private List<GroupLecture> lectures;

		public StudentPanel(ContextManager _ctx) : base(_ctx) {
			student = ctx.user as Student;

			if (student == null)
				return;

			lectures = ctx.database.groupLectures.Include(g => g._Group).Include(g => g._Lecture).Include(g => g._Lecturer).Where(g => g._Group == student._Group).ToList();
		}

		public override void Render() {
			ImGui.SeparatorText("Student panel");

			GUI.Table("Grades", ["Subject", "Lecturer", "Grade"], delegate {
				foreach (var lecture in lectures) {
					ImGui.TableNextRow();

					ImGui.TableNextColumn();
					ImGui.TextWrapped(lecture._Lecture.Name);

					var lecturer = lecture._Lecturer;

					ImGui.TableNextColumn();
					ImGui.TextWrapped($"{lecturer.Name} {lecturer.Surname}");

					var grade = student.Grades.Where(g => g._GroupLecture == lecture);

					ImGui.TableNextColumn();
					ImGui.TextWrapped(grade.Count() > 0 ? grade.ElementAt(0)._Grade.ToString() : "N/A");
				}
			});

			GUI.SpaceY(5);

			ImGui.Separator();

			if (GUI.FullWidthButton("Log out"))
				ctx.renderer.LoadMenu(e_Menus.Login);
		}

		public override float GetMenuWidth() {
			return 700;
		}
	}
}
