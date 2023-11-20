namespace P01_StudentSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD

=======
    
>>>>>>> 3de2fc7a089a91fbf62f41a83e6d4d80fc4ab88d
    using Enum;
    using Common;
    public class Resource
    {
        [Key]
        public int ResourceId { get; set; }

        [Unicode]
        [MaxLength(ValidationConstants.MaxLengthResourceName)]
        public string Name { get; set; } = null!;

        [Unicode(false)]
<<<<<<< HEAD
        public string Url { get; set; } = null!;
=======
        public string? Url { get; set; }
>>>>>>> 3de2fc7a089a91fbf62f41a83e6d4d80fc4ab88d
        public ResourceType ResourceType { get; set; }

        [ForeignKey(nameof(Course))]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; } = null!;
    }
}
