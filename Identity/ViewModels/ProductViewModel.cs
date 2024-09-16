namespace Identity.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string Unit { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
