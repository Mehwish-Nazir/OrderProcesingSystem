namespace OrderProcessingBackEnd.DTO
{
    public class ProductSearchRequestDto
    {
        public string SearchText { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

    }

    public class ProductResponseDto
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string CategoryName { get; set; }
     
        public DateTime CreatedAt { get; set; }
        public string StockStatus { get; set; } // optional if using computed field

    }

    public class PagedProductResponseDto
    {
        public List<ProductResponseDto> Products { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }

}
