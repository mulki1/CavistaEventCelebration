namespace CavistaEventCelebration.Api.Dto.EmployeeEvent
{
    public class AddEmployeeEventDto
    {
        public Guid EmployeeId { get; set; }
        public int EventId { get; set; }
        public DateOnly EventDate { get; set; }
    }
}
