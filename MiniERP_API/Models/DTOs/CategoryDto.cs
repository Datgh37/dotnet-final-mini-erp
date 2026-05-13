namespace MiniERP_API.Models.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
    }

    public class CategoryCreateUpdateDto
    {
        /// <example>Household</example>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Tên danh mục là bắt buộc.")]
        public string Name { get; set; }

        /// <example>null</example>
        public int? ParentCategoryId { get; set; }
    }
}
