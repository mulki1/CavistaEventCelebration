namespace CavistaEventCelebration.Api.Dto
{
    public class EmployeeEventDto
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public int EventId { get; set; }
        public DateOnly EventDate { get; set; }
        public string EmployeeFirstName { get; set; }
        public string EmployeeLastName { get; set; }
        public string EmployeeEmailAddress { get; set; }
        public string EventTitle { get; set; }
        public bool IsApproved { get; set; }
        public string ApprovedBy { get; set; }
        public string Status
        {
            get
            {
                return IsApproved ? "Approved" : "Pending";
            }
        }
    }
}
