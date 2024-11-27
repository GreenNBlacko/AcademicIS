using AcademicIS.Database;
using AcademicIS.Database.Model.Users;
using AcademicIS.GUI;
using Microsoft.EntityFrameworkCore;

namespace AcademicIS {
	public class ContextManager {
		public DB database { get; private set; }
		public User user { get; private set; }
		public Renderer renderer { get; private set; }

		public ContextManager(DB database) {
			this.database = database;
		}

		public void SetRenderer(Renderer _renderer) {
			renderer = _renderer;
		}

		public bool Login(string username, string password) {
			try {
				user = database.students.Include(s => s._Group).Include(s => s.Grades).Where(u => u.Username == username && u.Password == password).ElementAt(0);
				return true;
			} catch {
				try {
					user = database.lecturers.Where(u => u.Username == username && u.Password == password).ElementAt(0);
					return true;
				} catch {
					try {
						user = database.admins.Where(u => u.Username == username && u.Password == password).ElementAt(0);
						return true;
					} catch {
						return false;
					}
				}
			}
		}
	}
}
