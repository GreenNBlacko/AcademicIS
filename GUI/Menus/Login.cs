using AcademicIS.Database.Model.Users;
using ImGuiNET;

namespace AcademicIS.GUI.Menus {
	public class Login(ContextManager ctx) : Menu(ctx) {
		public override int priority => (int)Renderer.e_Menus.Login;

		private string username = "";
		private string password = "";

		public override void Render() {
			ImGui.SeparatorText("Login");
			ImGui.InputText("Username", ref username, 12);
			ImGui.InputText("Password", ref password, 12, ImGuiInputTextFlags.Password);

			GUI.SpaceY(5);

			ImGui.Separator();

			if (GUI.FullWidthButton("Log in")) {
				if (!ctx.Login(username, password)) {
					GUI.ThrowError("Login failed!\nMake sure you are entering the correct logins");
					return;
				}

				ctx.renderer.LoadMenu(ctx.user.Type switch {
					User.UserType.Student => Renderer.e_Menus.Student,
					User.UserType.Lecturer => Renderer.e_Menus.Lecturer,
					User.UserType.Admin => Renderer.e_Menus.Admin,
					_ => throw new ArgumentException("Edge case")
				});
			}

			if (GUI.FullWidthButton("Quit")) {
				Environment.Exit(0);
			}
		}
	}
}
