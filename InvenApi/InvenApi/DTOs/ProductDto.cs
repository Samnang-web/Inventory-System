namespace InvenApi.DTOs
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public string Sku { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public IFormFile? Image_Url { get; set; }
        public int Category_Id { get; set; }
    }

    public class UpdateProductDto
    {
        public string Name { get; set; }
        public string Sku { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public IFormFile? Image_Url { get; set; }
        public int Category_Id { get; set; }
    }
}
