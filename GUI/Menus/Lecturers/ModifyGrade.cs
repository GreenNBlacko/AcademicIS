using AcademicIS.Database.Model;
using AcademicIS.Database.Model.Users;
using ImGuiNET;
using static AcademicIS.GUI.Renderer;

namespace AcademicIS.GUI.Menus.Lecturers {
	public class ModifyGrade : Menu {
		public override int priority => (int)e_Menus.ModifyGrade;

		private Grade grade;

		public ModifyGrade(ContextManager ctx) : base(ctx) { }

		public ModifyGrade(ContextManager ctx, params object?[] args) : base(ctx) {
			var lecture = args[0] as GroupLecture ?? throw new ArgumentException("Wrong arguments, received " + args[0].GetType().Name);
			var student = args[1] as Student ?? throw new ArgumentException("Wrong arguments, received " + args[0].GetType().Name);
			grade = args[2] as Grade ?? new Grade {
				_GroupLecture = lecture,
				_Student = student
			};
		}

		public override void Render() {
			ImGui.SeparatorText("Modify grade");

			int _grade = grade._Grade;

			ImGui.InputInt("New grade", ref _grade);

			grade._Grade = _grade;

			GUI.SpaceY(5);

			ImGui.Separator();

			var selection = GUI.ButtonList(["Cancel", "Remove", "Confirm"]);

			if (selection != null) {
				switch (selection) {
					default: break; // Cancel

					case 1: // Remove
						GUI.ShowInfo("Grade removed successfully!");

						if (ctx.database.grades.Contains(grade))
							ctx.database.grades.Remove(grade);

						ctx.database.SaveChanges();
						break;

					case 2: // Confirm
						GUI.ShowInfo("Grade modified successfully!");

						if (!ctx.database.grades.Contains(grade))
							ctx.database.grades.Add(grade);

						ctx.database.SaveChanges();
						break;
				}

				ctx.renderer.LoadMenu(e_Menus.ViewLectures, grade._GroupLecture._Lecture);
			}
		}

	}
}
