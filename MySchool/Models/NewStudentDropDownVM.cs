namespace MySchool.Models
{
    public class NewStudentDropDownVM
    {
        public NewStudentDropDownVM()
        {
            Teachers = new List<Teacher>();
            Courses = new List<Course>();
        }
        public List<Teacher> Teachers { get; set; }
        public List<Course> Courses { get; set; }
    }
}
