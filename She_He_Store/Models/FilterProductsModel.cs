namespace She_He_Store.Models
{
    public class FilterProductsModel
    {
        public string ?Category { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public string ?ProductName { get; set; }
        public string SaleSort { get; set; }
        public string PriceSort { get; set; }
    }
}
