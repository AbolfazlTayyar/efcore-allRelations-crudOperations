namespace MySchool.Models
{
	public class StudentsDropdownVm
	{
		public StudentsDropdownVm()
		{
			Students = new List<Student>();
		}
		public List<Student> Students { get; set; }
	}
}
