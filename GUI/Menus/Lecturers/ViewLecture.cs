using AcademicIS.Database.Model;
using AcademicIS.Database.Model.Users;
using ImGuiNET;
using Microsoft.EntityFrameworkCore;
using static AcademicIS.GUI.Renderer;

namespace AcademicIS.GUI.Menus.Lecturers {
	public class ViewLecture : Menu {
		private Lecture lecture;

		public override int priority => (int)e_Menus.ViewLectures;

		private string query = string.Empty;

		private List<GroupLecture> groupLectures;
		private List<Student> students = new List<Student>();
		private List<Grade> grades = new List<Grade>();

		private List<string> groups = new List<string>();

		private int sel_group;

		private bool filter_groups;

		public ViewLecture(ContextManager _ctx) : base(_ctx) { }

		public ViewLecture(ContextManager _ctx, params object?[] args) : base(_ctx) {
			lecture = args[0] as Lecture ?? throw new ArgumentException("Wrong arguments, received " + args[0].GetType().Name);

			groupLectures = ctx.database.groupLectures.Include(l => l._Group).Include(l => l._Lecture).Where(l => l._Lecture == lecture).ToList();

			foreach (var gLecture in groupLectures) {
				groups.Add(gLecture._Group.Name);
				students.AddRange(ctx.database.students.Include(s => s._Group).Where(s => s._Group == gLecture._Group).ToList());
			}

			foreach (var student in students) {
				grades.AddRange(ctx.database.grades.Include(g => g._Student).Include(g => g._GroupLecture).Where(g => g._Student == student && groupLectures.Contains(g._GroupLecture)).ToList());
			}
		}

		public override void Render() {
			ImGui.SeparatorText("View lecture");

			ImGui.InputText("Name", ref query, 32);
			ImGui.Checkbox("Filter by group", ref filter_groups);

			if (filter_groups) {
				ImGui.Combo("Group", ref sel_group, groups.ToArray(), groups.Count);
			}

			GUI.SpaceY(5);

			GUI.Table("Students", ["First name", "Last name", "Group", "Grade", ""], delegate {
				foreach (var student in students) {
					if (!student.Name.ToLower().Contains(query.ToLower()))
						continue;

					if (filter_groups && student._Group.Name != groups[sel_group])
						continue;

					DisplayStudent(student);
				}
			});

			GUI.SpaceY(5);

			ImGui.Separator();
			if (GUI.FullWidthButton("Go back"))
				ctx.renderer.LoadMenu(e_Menus.Lecturer);
		}

		private void DisplayStudent(Student student) {
			ImGui.TableNextRow();

			var grade = grades.Find(g => g._GroupLecture._Lecture == lecture && g._Student == student);

			ImGui.TableNextColumn();
			ImGui.TextWrapped(student.Name);

			ImGui.TableNextColumn();
			ImGui.TextWrapped(student.Surname);

			ImGui.TableNextColumn();
			ImGui.TextWrapped(student._Group.Name);

			ImGui.TableNextColumn();
			ImGui.TextWrapped(grade != null ? grade._Grade.ToString() : "N/A");

			ImGui.TableNextColumn();
			if (GUI.FullWidthButton($"Modify##{student.Id}{student.Name}{student.Surname}")) {
				GroupLecture lecture = groupLectures.Find(l => l._Group == student._Group) ?? throw new ArgumentNullException("Lecture does not exist(edge case)");

				ctx.renderer.LoadMenu(e_Menus.ModifyGrade, lecture, student, grade);
			}
		}

		public override float GetMenuWidth() {
			float size = 0;

			var spacing = ImGui.GetStyle().ItemSpacing;
			var padding = ImGui.GetStyle().CellPadding;

			foreach(var student in students) {
				var grade = grades.Find(g => g._GroupLecture._Lecture == lecture && g._Student == student);

				size = Math.Max(size, spacing.X * 4 + (padding.X * 6 +
					ImGui.CalcTextSize(student.Name).X * 2 +
					ImGui.CalcTextSize(student.Surname).X * 2 +
					ImGui.CalcTextSize(student._Group.Name).X * 2 +
					ImGui.CalcTextSize(grade != null ? grade._Grade.ToString() : "N/A").X * 2) / 4 * 5);
			}

			return size;
		}
	}
}
