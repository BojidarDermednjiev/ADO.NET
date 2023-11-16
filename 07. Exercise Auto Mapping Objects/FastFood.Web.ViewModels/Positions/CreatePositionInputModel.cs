namespace FastFood.Web.ViewModels.Positions
{
    using System.ComponentModel.DataAnnotations;

    using Common.EntityConfigurations;
    public class CreatePositionInputModel
    {
        [MinLength(ValidationConstants.PositionMinLength)]
        [MaxLength(ValidationConstants.PositionMaxLength)]
        //[StringLength(ValidationConstants.PositionMaxLength, MinimumLength = ValidationConstants.PositionMinLength, ErrorMessage = ErrorMessages.InvalidPositionNameErrorMessage)]
        public string PositionName { get; set; } = null!;
    }
}