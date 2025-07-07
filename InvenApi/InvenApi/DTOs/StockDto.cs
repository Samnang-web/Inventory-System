namespace InvenApi.DTOs
{
    public class CreateStockDto
    {
        public int Product_Id { get; set; }
        public int Quantity { get; set; }
        public string Types { get; set; }
        public string Note { get; set; }
        public int Created_by { get; set; }
    }
    public class UpdateStockDto
    {
        public int Product_Id { get; set; }
        public int Quantity { get; set; }
        public string Types { get; set; }
        public string Note { get; set; }
        public int Created_by { get; set; }
    }
}
