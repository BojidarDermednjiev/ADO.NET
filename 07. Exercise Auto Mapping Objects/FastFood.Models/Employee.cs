namespace FastFood.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Common.EntityConfigurations;

    public class Employee
    {
        public Employee()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Orders = new HashSet<Order>();
        }

        [Key]
        [MaxLength(ValidationConstants.GuidMaxLength)]
        public string Id { get; set; }

        [Required]
        [StringLength(ValidationConstants.EmployeeMaxLength, MinimumLength = ValidationConstants.EmployeeMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [Range(15, 80)]
        public int Age { get; set; }

        [Required]
        [StringLength(ValidationConstants.AddressMaxLength, MinimumLength = ValidationConstants.AddressMinLength)]
        public string Address { get; set; } = null!;
        
        [ForeignKey(nameof(Position))]
        public int PositionId { get; set; }

        [Required]
        public virtual Position Position { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; }
    }
}