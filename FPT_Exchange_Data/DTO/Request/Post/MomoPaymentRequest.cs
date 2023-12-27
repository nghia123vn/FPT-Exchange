namespace FPT_Exchange_Data.DTO.Request.Post
{
    public class MomoPaymentRequest
    {
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Content { get; set; }
        public long Amount { get; set; }
    }
}
