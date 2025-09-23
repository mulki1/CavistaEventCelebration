namespace CavistaEventCelebration.Api.Dto.EmployeeEvent
{
    public class UpdateEmployeeEventDto
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public int EventId { get; set; }
        public DateOnly EventDate { get; set; }
    }
}
