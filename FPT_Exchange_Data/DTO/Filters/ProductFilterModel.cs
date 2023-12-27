namespace FPT_Exchange_Data.DTO.Filters
{
    public class ProductFilterModel
    {
        public string? Name { get; set; }
        public string? CategoryName { get; set; }
        public int? MaxPrice { get; set; }
        public int? MinPrice { get; set; }
    }
}
