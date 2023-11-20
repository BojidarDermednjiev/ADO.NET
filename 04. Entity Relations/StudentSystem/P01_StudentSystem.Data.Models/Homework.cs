<<<<<<< HEAD
﻿namespace P01_StudentSystem.Data.Models
=======
﻿
namespace P01_StudentSystem.Data.Models
>>>>>>> 3de2fc7a089a91fbf62f41a83e6d4d80fc4ab88d
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Enum;
    public class Homework
    {
        [Key]
        public int HomeworkId { get; set; }

<<<<<<< HEAD
        [Unicode(false)] 
        public string Content { get; set; } = null!;

        public  ContentType ContentType { get; set; }
=======
        [Unicode(false)]
        public string? Content { get; set; }

        public ContentType ContentType { get; set; }
>>>>>>> 3de2fc7a089a91fbf62f41a83e6d4d80fc4ab88d

        public DateTime SubmissionTime { get; set; }

        [ForeignKey(nameof(Student))]
        public int StudentId { get; set; }

        public virtual Student Student { get; set; } = null!;

        [ForeignKey(nameof(Course))]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; } = null!;

    }
}
