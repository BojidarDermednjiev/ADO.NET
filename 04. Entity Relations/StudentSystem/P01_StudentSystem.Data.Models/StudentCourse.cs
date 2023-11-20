namespace P01_StudentSystem.Data.Models
{
<<<<<<< HEAD
    using System.ComponentModel.DataAnnotations;
=======
>>>>>>> 3de2fc7a089a91fbf62f41a83e6d4d80fc4ab88d
    using System.ComponentModel.DataAnnotations.Schema;

    public class StudentCourse
    {
<<<<<<< HEAD
        [Required]
        public int StudentId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; } = null!;

        [Required]
        public int CourseId { get; set; }

        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; } = null!;
=======
        [ForeignKey(nameof(Student))]
        public int StudentId { get; set; }
        public virtual Student Student { get; set; } = null!;

        [ForeignKey(nameof(Course))]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; } = null!;

>>>>>>> 3de2fc7a089a91fbf62f41a83e6d4d80fc4ab88d
    }
}
