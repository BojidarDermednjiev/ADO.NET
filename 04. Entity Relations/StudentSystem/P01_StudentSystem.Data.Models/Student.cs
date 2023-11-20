<<<<<<< HEAD
﻿
namespace P01_StudentSystem.Data.Models
=======
﻿namespace P01_StudentSystem.Data.Models
>>>>>>> 3de2fc7a089a91fbf62f41a83e6d4d80fc4ab88d
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.EntityFrameworkCore;

    using Common;
    public class Student
    {
        public Student()
        {
<<<<<<< HEAD
            this.StudentsCourses = new HashSet<StudentCourse>();
            this.Homeworks = new HashSet<Homework>();
=======
            this.Courses = new HashSet<Course>();
            this.Homework = new HashSet<Homework>();
>>>>>>> 3de2fc7a089a91fbf62f41a83e6d4d80fc4ab88d
        }

        [Key]
        public int StudentId { get; set; }

        [Unicode(true)]
        [MaxLength(ValidationConstants.MaxLengthStudentName)]
        public string Name { get; set; } = null!;

        [Unicode(false)]
        [MaxLength(ValidationConstants.MaxLengthPhoneNumber)]
        public string? PhoneNumber { get; set; }
<<<<<<< HEAD
        public DateTime RegisteredOn { get; set; }
        public DateTime? Birthday { get; set; }
        public virtual ICollection<StudentCourse> StudentsCourses { get; set; }
        public virtual ICollection<Homework> Homeworks { get; set; }
=======
        public bool RegisteredOn { get; set; }
        public DateTime? Birthday { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Homework> Homework { get; set; }
>>>>>>> 3de2fc7a089a91fbf62f41a83e6d4d80fc4ab88d
    }
}
