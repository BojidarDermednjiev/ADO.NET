
namespace P01_StudentSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.EntityFrameworkCore;

    using Common;

    public class Course
    {
        public Course()
        {
            this.Students = new HashSet<Student>();
            this.Resources = new HashSet<Resource>();
            this.Homework = new HashSet<Homework>();
        }

        [Key]
        public int CourseId { get; set; }

        [Unicode]
        [MaxLength(ValidationConstants.MaxLengthCourseName)]
        public string Name { get; set; } = null!;

        [Unicode(false)]
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Resource> Resources { get; set; }
        public virtual ICollection<Homework> Homework { get; set; }
    }
}
