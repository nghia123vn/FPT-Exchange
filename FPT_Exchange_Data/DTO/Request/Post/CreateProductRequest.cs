using Microsoft.AspNetCore.Http;

namespace FPT_Exchange_Data.DTO.Request.Post
{
    public class CreateProductRequest
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public int Price { get; set; }

        public Guid StationID { get; set; }
        public Guid CategoryId { get; set; }
        public Guid SellerID { get; set; }
        public List<IFormFile> ImageProducts { get; set; } = new List<IFormFile>();
    }
}
