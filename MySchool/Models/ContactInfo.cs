namespace MySchool.Models
{
    public class ContactInfo
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }
    }
}
