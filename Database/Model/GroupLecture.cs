using AcademicIS.Database.Model.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademicIS.Database.Model {
	public class GroupLecture {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public Lecture _Lecture { get; set; }
		public Group _Group { get; set; }
		public Lecturer _Lecturer { get; set; }
	}
}
