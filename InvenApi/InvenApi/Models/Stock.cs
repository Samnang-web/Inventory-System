namespace InvenApi.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public int Product_Id { get; set; }
        public int Quantity { get; set; }
        public string Types { get; set; }
        public string Note { get; set; }
        public int Created_by {  get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
