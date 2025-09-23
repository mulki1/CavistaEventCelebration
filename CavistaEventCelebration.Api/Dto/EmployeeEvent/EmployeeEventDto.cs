namespace CavistaEventCelebration.Api.Dto
{
    public class EmployeeEventDto
    {
        public Guid EmployeeId { get; set; }
        public int EventId { get; set; }
        public DateOnly EventDate { get; set; }
        public string EmployeeFirstName { get; set; }
        public string EmployeeLastName { get; set; }
        public string EmployeeEmailAddress { get; set; }
        public string EventTitle { get; set; }
    }
}
