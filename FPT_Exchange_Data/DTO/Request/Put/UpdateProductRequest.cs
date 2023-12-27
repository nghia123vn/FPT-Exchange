using Microsoft.AspNetCore.Http;

namespace FPT_Exchange_Data.DTO.Request.Put
{
    public class UpdateProductRequest
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public int? Price { get; set; }

        public Guid? StationID { get; set; }
        public Guid? CategoryId { get; set; }
        public List<IFormFile>? ImageProducts { get; set; }
        public Guid? StatusId { get; set; }
    }
}
