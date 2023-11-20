<<<<<<< HEAD
﻿namespace P01_StudentSystem.Data.Models
=======
﻿
namespace P01_StudentSystem.Data.Models
>>>>>>> 3de2fc7a089a91fbf62f41a83e6d4d80fc4ab88d
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.EntityFrameworkCore;

    using Common;
<<<<<<< HEAD
=======

>>>>>>> 3de2fc7a089a91fbf62f41a83e6d4d80fc4ab88d
    public class Course
    {
        public Course()
        {
<<<<<<< HEAD
            this.StudentsCourses = new HashSet<StudentCourse>();
            this.Resources = new HashSet<Resource>();
            this.Homeworks = new HashSet<Homework>();
=======
            this.Students = new HashSet<Student>();
            this.Resources = new HashSet<Resource>();
            this.Homework = new HashSet<Homework>();
>>>>>>> 3de2fc7a089a91fbf62f41a83e6d4d80fc4ab88d
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
<<<<<<< HEAD
        public virtual ICollection<Resource> Resources { get; set; }
        public virtual ICollection<Homework> Homeworks { get; set; }
        public virtual ICollection<StudentCourse> StudentsCourses { get; set; }
     
=======
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Resource> Resources { get; set; }
        public virtual ICollection<Homework> Homework { get; set; }
>>>>>>> 3de2fc7a089a91fbf62f41a83e6d4d80fc4ab88d
    }
}
