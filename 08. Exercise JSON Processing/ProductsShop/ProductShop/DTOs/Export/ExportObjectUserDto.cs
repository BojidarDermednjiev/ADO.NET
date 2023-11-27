namespace ProductShop.DTOs.Export
{
    public class ExportObjectUserDto
    {
        public ICollection<ExportUserInfoDto> Users { get; set; } = null!;
    }
}
