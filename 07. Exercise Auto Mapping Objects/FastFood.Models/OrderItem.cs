namespace FastFood.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Common.EntityConfigurations;

    public class OrderItem
    {
        [MaxLength(ValidationConstants.GuidMaxLength)]

        [ForeignKey(nameof(Order))]
        public string OrderId { get; set; } = null!;

        [Required]
        public virtual Order Order { get; set; } = null!;

        [MaxLength(ValidationConstants.GuidMaxLength)]
        [ForeignKey(nameof(Item))]
        public string ItemId { get; set; } = null!;

        [Required]
        public virtual Item Item { get; set; } = null!;

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}