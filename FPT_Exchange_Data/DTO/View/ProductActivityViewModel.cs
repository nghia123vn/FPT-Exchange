namespace FPT_Exchange_Data.DTO.View
{
    public class ProductActivityViewModel
    {
        public Guid Id { get; set; }
        public string? ActionType { get; set; }

        public virtual StatusViewModel? NewStatusNavigation { get; set; }

        public virtual StatusViewModel OldStatusNavigation { get; set; } = null!;

        public virtual ProductActivityItemViewModel Product { get; set; } = null!;

        public virtual StationViewModel Stations { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public virtual UserViewModel? User { get; set; } = null!;

    }
}
