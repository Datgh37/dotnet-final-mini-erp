namespace MiniERP_API.Models.DTOs
{
    public class SupplierDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }

    public class SupplierCreateUpdateDto
    {
        /// <example>Global Logistics Ltd</example>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Tên nhà cung cấp là bắt buộc.")]
        public string Name { get; set; }
        
        /// <example>John Doe</example>
        public string ContactPerson { get; set; }

        /// <example>0283344556</example>
        public string Phone { get; set; }
        
        /// <example>info@globallogistics.com</example>
        [System.ComponentModel.DataAnnotations.EmailAddress(ErrorMessage = "Email không đúng định dạng.")]
        public string Email { get; set; }
        
        /// <example>Tokyo, Japan</example>
        public string Address { get; set; }
    }
}
