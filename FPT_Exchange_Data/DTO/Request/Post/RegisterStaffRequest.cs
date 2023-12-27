namespace FPT_Exchange_Data.DTO.Request.Post
{
    public class RegisterStaffRequest
    {
        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public Guid StationId { get; set; }

    }
}
