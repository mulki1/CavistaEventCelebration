namespace CavistaEventCelebration.Api.Dto.DashBoard
{
    public class DashBoardDto
    {
        public int NumberOfEmployees { get; set; }
        public int NumberOfEventsForTheMonth { get; set; }
        public List<EmployeeEventDto> EventsForTheMonth { get; set; }
        public List<EmployeeEventDto> EventsForTheWeek { get; set; }
        public List<EmployeeEventDto> EventsForToday { get; set; }
        public List<EmployeeEventDto> BirthdaysForTheMonth { get; set; }
        public List<EmployeeEventDto> BirthdaysForTheWeek { get; set; }
        public List<EmployeeEventDto> BirthdaysForToday { get; set; }
        public List<EmployeeEventDto> WorkAnniversaryForTheMonth { get; set; }
        public List<EmployeeEventDto> WorkAnniversaryForTheWeek { get; set; }
        public List<EmployeeEventDto> WorkAnniversaryForToday { get; set; }
        public List<EmployeeEventDto> WeddingAnniversaryForTheMonth { get; set; }
        public List<EmployeeEventDto> WeddingAnniversaryForTheWeek { get; set; }
        public List<EmployeeEventDto> WeddingAnniversaryForToday { get; set; }
        public int NumberOfBirthDaysForTheMonth { get; set; }
        public int NumberOfBirthDaysForTheWeek { get; set; }
        public int NumberOfBirthDaysToday { get; set; }
        public int NumberOfWorkAnniversaryForTheMonth { get; set; }
        public int NumberOfAnniversaryForTheWeek { get; set; }
        public int NumberOfAnniversaryToday { get; set; }
        public int NumberOfWeddingAnniversaryForTheMonth { get; set; }
        public int NumberOfWeddingAnniversaryForTheWeek { get; set; }
        public int NumberOfWeddingAnniversaryToday { get; set; }
    }
}
