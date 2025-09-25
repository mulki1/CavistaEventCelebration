using CavistaEventCelebration.Api.Data;
using CavistaEventCelebration.Api.Dto;
using CavistaEventCelebration.Api.Dto.Event;
using CavistaEventCelebration.Api.Models;
using CavistaEventCelebration.Api.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CavistaEventCelebration.Api.Repositories.Implementation
{

    public class EventRepo : IEventRepo
    {
        private readonly AppDbContext _db;
        public EventRepo(AppDbContext db)
        {
            _db = db;
        }


        public async Task<List<DailyEventDto>> GetDailyEvents(int eventId)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            return await (
                from ee in _db.EmployeeEvents
                join emp in _db.Employees on ee.EmployeeId equals emp.Id
                join e in _db.Events on ee.EventId equals e.Id
                where ee.EventId == eventId
                      && ee.EventDate == today
                      && !ee.IsDeprecated && ee.IsApproved
                      && !emp.IsDeprecated
                select new DailyEventDto
                {
                    EmployeeEmailAddress = emp.EmailAddress,
                    EmployeeFirstName = emp.FirstName,
                    EmployeeLastName = emp.LastName,
                    EventId = ee.EventId,
                    EventTitle = e.Name,
                    EventMessage = e.Message
                }
            ).ToListAsync();
        }

        public async Task<List<Event>> Events()
        {
            return await _db.Events.Where(e => !e.IsDeprecated).ToListAsync();
        }

        public async Task<bool> AddEvent(Event eventItem)
        {
            try
            {
                _db.Add(eventItem);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }

        }

        public async Task<bool> Remove(Event eventItem)
        {
            try
            {
                eventItem.IsDeprecated = true;
                var result = _db.Events.Update(eventItem);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }
        }

        public async Task<Event> GetById(int Id)
        {
            try
            {
                return await _db.Events.FirstOrDefaultAsync(e => e.Id == Id && !e.IsDeprecated);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return new Event();
            }
        }

        public async Task<bool> UpdateEvent(Event eventItem)
        {
            try
            {
                _db.Events.Update(eventItem);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }
        }

        public async Task<bool> DoesEventExist(string name)
        {
            try
            {
                var normalizedName = name.Trim();
                var eventItem = await _db.Events.FirstOrDefaultAsync(e => e.Name.Trim().Equals(normalizedName, StringComparison.CurrentCultureIgnoreCase) && !e.IsDeprecated);
                if (eventItem == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }
        }

        public async Task<bool> AddEmployeeEvent(EmployeeEvent eventItem)
        {
            try
            {
                _db.EmployeeEvents.Add(eventItem);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateEmployeeEvent(EmployeeEvent eventItem)
        {
            try
            {
                _db.EmployeeEvents.Update(eventItem);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }
        }

        public IQueryable<EmployeeEventDto> EmployeeEventGet(Guid currentUserId)
        {
            try
            {
                Guid employeeId = Guid.Empty;

                if (currentUserId != Guid.Empty)
                {
                    employeeId = _db.Users
                                    .Where(u => u.Id == currentUserId)
                                    .Select(u => u.EmployeeId)
                                    .FirstOrDefault();
                }

                return (
                    from ee in _db.EmployeeEvents
                    join emp in _db.Employees on ee.EmployeeId equals emp.Id
                    join e in _db.Events on ee.EventId equals e.Id
                    join u in _db.Users on ee.ApprovedBy equals u.Id into approvedUsers
                    from u in approvedUsers.DefaultIfEmpty()
                    join approverEmp in _db.Employees on u.EmployeeId equals approverEmp.Id into approverEmployees
                    from approverEmp in approverEmployees.DefaultIfEmpty()

                    where !ee.IsDeprecated
                          && !emp.IsDeprecated
                          && (employeeId == Guid.Empty || ee.EmployeeId == employeeId) 
                    select new EmployeeEventDto
                    {
                        Id = ee.Id,
                        EmployeeEmailAddress = emp.EmailAddress,
                        EmployeeFirstName = emp.FirstName,
                        EmployeeLastName = emp.LastName,
                        EventId = ee.EventId,
                        EventTitle = e.Name,
                        EventDate = ee.EventDate,
                        EmployeeId = emp.Id,
                        ApprovedBy = approverEmp != null
                                        ? $"{approverEmp.FirstName} {approverEmp.LastName}"
                                        : null,
                        IsApproved = ee.IsApproved
                    }
                ).AsQueryable();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return null;
            }
        }


        public async Task<EmployeeEvent> GetEmployeeEventById(Guid id)
        {
            try
            {
                return await _db.EmployeeEvents.Where(e => e.Id == id && !e.IsDeprecated).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return null;
            }
        }

        public async Task<List<EmployeeEvent>> GetEmployeeEvents(List<Guid> employeeEventIds)
        {
            try
            {
                return await _db.EmployeeEvents.Where(e => employeeEventIds.Contains(e.Id) && !e.IsDeprecated).ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateEmployeeEvents(List<EmployeeEvent> employeeEvents)
        {
            try
            {
                _db.EmployeeEvents.UpdateRange(employeeEvents);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }
        }

        public async Task<bool> DoesEmployeeEventExist(Guid employeeId, int eventId)
        {
            try
            {
                var empEvent = await _db.EmployeeEvents.FirstOrDefaultAsync(emp => emp.EmployeeId == employeeId && emp.EventId == eventId && !emp.IsDeprecated);
                if (empEvent == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return true;
            }
        }

        public async Task<ApplicationUser> user(Guid userId)
        {
            try
            {
                var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                return user;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return null;
            }
        }

        public async Task<List<EmployeeEventDto>> GetEventsInRangeAsync(DateOnly startDate, DateOnly endDate)
        {
            return await (from ee in _db.EmployeeEvents
                          join emp in _db.Employees on ee.EmployeeId equals emp.Id
                          join e in _db.Events on ee.EventId equals e.Id
                          where ee.EventDate >= startDate
                                && ee.EventDate <= endDate
                                && !ee.IsDeprecated
                                && !emp.IsDeprecated
                          select new EmployeeEventDto
                          {
                              EmployeeEmailAddress = emp.EmailAddress,
                              EmployeeFirstName = emp.FirstName,
                              EmployeeLastName = emp.LastName,
                              EventId = ee.EventId,
                              EventTitle = e.Name,
                              EventDate = ee.EventDate,
                              EmployeeId = emp.Id
                          }).ToListAsync();
        }
    }
}
