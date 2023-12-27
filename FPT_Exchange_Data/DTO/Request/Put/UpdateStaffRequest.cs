namespace FPT_Exchange_Data.DTO.Request.Put
{
    public class UpdateStaffRequest
    {
        public string? Name { get; set; }

        public string? Password { get; set; }

        public string? Status { get; set; }

        public Guid? StationId { get; set; }
    }
}
