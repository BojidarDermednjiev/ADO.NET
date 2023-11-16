namespace FastFood.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    using Common.EntityConfigurations;

    public class Position
    {
        public Position()
        {
            this.Employees = new HashSet<Employee>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(ValidationConstants.PositionMaxLength)]
        public string Name { get; set; } = null!;
        public virtual ICollection<Employee> Employees { get; set; }
    }
}