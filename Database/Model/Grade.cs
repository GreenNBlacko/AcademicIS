using AcademicIS.Database.Model.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademicIS.Database.Model {
	public class Grade {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public int _Grade { get; set; }
		public Student _Student { get; set; }
		public GroupLecture _GroupLecture { get; set; }
	}
}
