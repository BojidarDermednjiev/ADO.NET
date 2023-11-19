namespace P01_StudentSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.EntityFrameworkCore;

    using Common;
    public class Student
    {
        public Student()
        {
            this.Courses = new HashSet<Course>();
            this.Homework = new HashSet<Homework>();
        }

        [Key]
        public int StudentId { get; set; }

        [Unicode(true)]
        [MaxLength(ValidationConstants.MaxLengthStudentName)]
        public string Name { get; set; } = null!;

        [Unicode(false)]
        [MaxLength(ValidationConstants.MaxLengthPhoneNumber)]
        public string? PhoneNumber { get; set; }
        public bool RegisteredOn { get; set; }
        public DateTime? Birthday { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Homework> Homework { get; set; }
    }
}
