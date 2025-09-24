using CavistaEventCelebration.Api.Dto.DashBoard;
using CavistaEventCelebration.Api.Models;
using CavistaEventCelebration.Api.Repositories.Interface;
using CavistaEventCelebration.Api.Services.Interface;
using DocumentFormat.OpenXml.Wordprocessing;

namespace CavistaEventCelebration.Api.Services.Implementation
{
    public class DashBoardService : IDashBoardService
    {
        private readonly IEmployeeRepo _employeeRepo;
        private readonly IEventRepo _eventRepo;
        public DashBoardService(IEmployeeRepo employeeRepo, IEventRepo eventRepo)
        {
           _employeeRepo = employeeRepo;
            _eventRepo = eventRepo;
        }

        public async Task<DashBoardDto> Get()
        {
            var dashBoardEvents = new DashBoardDto();
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek);   // Sunday
            var endOfWeek = startOfWeek.AddDays(6);                   // Saturday
            var startOfMonth = new DateOnly(today.Year, today.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
            var monthEvents = await _eventRepo.GetEventsInRangeAsync(startOfMonth, endOfMonth);
            var employees =  _employeeRepo.Get().ToList();
            var limitedMonthEvents = monthEvents.OrderBy(d => d.EventDate).Take(10); // limit number of event se

            dashBoardEvents.NumberOfEmployees = employees.Count();
            if(monthEvents != null && monthEvents.Count > 0)
            {
                dashBoardEvents.EventsForTheMonth = monthEvents.OrderBy(d => d.EventDate).ThenBy(d => d.EmployeeFirstName).Take(10).ToList();
                dashBoardEvents.NumberOfEventsForTheMonth = monthEvents.Count();
                dashBoardEvents.EventsForTheWeek = monthEvents.Where(e => e.EventDate >= startOfWeek && e.EventDate <= endOfWeek).ToList();
                dashBoardEvents.EventsForToday = monthEvents.Where(e => e.EventDate >= today && e.EventDate <= today).ToList();
                dashBoardEvents.BirthdaysForTheMonth = monthEvents.Where(e => e.EventId == 1).ToList();
                dashBoardEvents.BirthdaysForTheWeek = monthEvents.Where(e => e.EventId == 1 && e.EventDate >= startOfWeek && e.EventDate <= endOfWeek).ToList();
                dashBoardEvents.BirthdaysForToday = monthEvents.Where(e => e.EventId == 1 && e.EventDate >= today && e.EventDate <= today).ToList();
                dashBoardEvents.WorkAnniversaryForTheMonth = monthEvents.Where(e => e.EventId == 2).ToList();
                dashBoardEvents.WorkAnniversaryForTheWeek = monthEvents.Where(e => e.EventId == 2 && e.EventDate >= startOfWeek && e.EventDate <= endOfWeek).ToList();
                dashBoardEvents.WorkAnniversaryForToday = monthEvents.Where(e => e.EventId == 2 && e.EventDate >= today && e.EventDate <= today).ToList();
                dashBoardEvents.WeddingAnniversaryForTheMonth = monthEvents.Where(e => e.EventId == 3).ToList();
                dashBoardEvents.WeddingAnniversaryForTheWeek = monthEvents.Where(e => e.EventId == 3 && e.EventDate >= startOfWeek && e.EventDate <= endOfWeek).ToList();
                dashBoardEvents.WeddingAnniversaryForToday = monthEvents.Where(e => e.EventId == 3 && e.EventDate >= today && e.EventDate <= today).ToList();
                dashBoardEvents.NumberOfBirthDaysForTheMonth = dashBoardEvents.BirthdaysForTheMonth.Count();
                dashBoardEvents.NumberOfBirthDaysForTheWeek = dashBoardEvents.BirthdaysForTheWeek.Count();
                dashBoardEvents.NumberOfBirthDaysToday = dashBoardEvents.BirthdaysForToday.Count();
                dashBoardEvents.NumberOfWorkAnniversaryForTheMonth = dashBoardEvents.WorkAnniversaryForTheMonth.Count();
                dashBoardEvents.NumberOfAnniversaryForTheWeek = dashBoardEvents.WorkAnniversaryForTheWeek.Count();
                dashBoardEvents.NumberOfAnniversaryToday = dashBoardEvents.WorkAnniversaryForToday.Count();
                dashBoardEvents.NumberOfWeddingAnniversaryForTheMonth = dashBoardEvents.WeddingAnniversaryForTheMonth.Count();
                dashBoardEvents.NumberOfWeddingAnniversaryForTheWeek = dashBoardEvents.WeddingAnniversaryForTheWeek.Count();
                dashBoardEvents.NumberOfWeddingAnniversaryToday = dashBoardEvents.WeddingAnniversaryForToday.Count();
            }

            return dashBoardEvents;




        }

    }
}
