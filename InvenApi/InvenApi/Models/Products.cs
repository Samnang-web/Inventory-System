namespace InvenApi.Models
{
    public class Products
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Sku {  get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Image_Url { get; set; }
        public int Category_Id {  get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
