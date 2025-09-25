using CavistaEventCelebration.Api.Dto;
using CavistaEventCelebration.Api.Dto.EmployeeEvent;
using CavistaEventCelebration.Api.Dto.Event;
using CavistaEventCelebration.Api.Models;
using CavistaEventCelebration.Api.Repositories.Interface;
using CavistaEventCelebration.Api.Services.Interface;
namespace CavistaEventCelebration.Api.Services.Implementation
{
    public class EventService : IEventService
    {
        private readonly IEventRepo _eventRepo;
        public EventService(IEventRepo eventRepo)
        {
            _eventRepo = eventRepo;
        }

        public async Task<Response<bool>> AddEvent(EventDto newEvent)
        {
            if (string.IsNullOrWhiteSpace(newEvent.Name) || string.IsNullOrWhiteSpace(newEvent.Message))
            {
                return Response<bool>.Failure("Name or Message can not empty");
            }

            if ( await _eventRepo.DoesEventExist(newEvent.Name))
            {
                return Response<bool>.Failure($"Event with {newEvent.Name} already exist");
            }
            
            var eventItem = new Event
            {
                Name = newEvent.Name,
                Message = newEvent.Message, 
                IsDeprecated = false
            };

            var result = await _eventRepo.AddEvent(eventItem);

            if (result)
            {
                return Response<bool>.Success(result, "Event created");
            }

            return Response<bool>.Failure("Could not create event");
        }

        public async Task<Response<bool>> DeleteEvent(int id)
        {
            var eventItem = await _eventRepo.GetById(id);
            if (eventItem == null)
            {
                return Response<bool>.Failure("Event does not exist");
            }
            var result = await _eventRepo.Remove(eventItem);

            if (result)
            {
                return Response<bool>.Success(result, "Event Deleted");
            }

            return Response<bool>.Failure("Could not delete event, try again later");
        }

        public async Task<Response<List<Event>>> Events()
        {
            var events = await _eventRepo.Events();
            return Response<List<Event>>.Success(events);
        }

        public async Task<Response<bool>> UpdateEvent(int id, EventDto updateEvent)
        {
            var eventItem = await _eventRepo.GetById(id);
            if (eventItem == null)
            {
                return Response<bool>.Failure("Event does not exist");
            }
            eventItem.Name = updateEvent.Name;
            eventItem.Message = updateEvent.Message;
            var result = await _eventRepo.UpdateEvent(eventItem);
            if (result)
            {
               return  Response<bool>.Success(true, "Event update");
            }
            return Response<bool>.Failure("Event could not be update, please try again");
        }
        
        public async Task<Response<bool>> AddEmployeeEvent(bool canApprove, Guid userId, AddEmployeeEventDto employeeEvent)
        {
            if(employeeEvent == null)
            {
                return Response<bool>.Failure("Invalid request");
            }

            if( await _eventRepo.DoesEmployeeEventExist(employeeEvent.EmployeeId, employeeEvent.EventId))
            {
                return Response<bool>.Failure("Employee event already exist");
            }

            if (!canApprove)
            {
                //get employee id from user id
                var user = await _eventRepo.user(userId);
                if(user == null)
                {
                    return Response<bool>.Failure("user does not exist");
                }
                employeeEvent.EmployeeId = user.EmployeeId;
            }

            var newEployeeEvent = new EmployeeEvent()
            {
                Id = new Guid(),
                EmployeeId = employeeEvent.EmployeeId,
                EventId = employeeEvent.EventId,
                EventDate = employeeEvent.EventDate,
                IsDeprecated = false
            };
            if (canApprove)
            {
                newEployeeEvent.ApprovedBy = userId;
                newEployeeEvent.IsApproved = true;
                newEployeeEvent.DateApproved = DateTime.UtcNow;
            }

            var result = await _eventRepo.AddEmployeeEvent(newEployeeEvent);
            if (result)
            {
                return Response<bool>.Success(true, " Employee Event Added");
            }
            return Response<bool>.Failure("Event could not be Added, please try again");
        }

        public async Task<Response<bool>> DeleteEmployeeEvent(Guid id)
        {
            var eventItem = await _eventRepo.GetEmployeeEventById(id);
            if (eventItem == null)
            {
                return Response<bool>.Failure("Employee Event does not exist");
            }
            eventItem.IsDeprecated = true;
            var result = await _eventRepo.UpdateEmployeeEvent(eventItem);

            if (result)
            {
                return Response<bool>.Success(result, "Event Deleted");
            }
            return Response<bool>.Failure("Could not delete employee event, try again later");
        }
        
        public async Task<PaginatedList<EmployeeEventDto>> EmployeeEvents(Guid currentUserId, int? index, int? pageSize, string? searchString)
        {
            var eventResp = new List<EmployeeEventDto>();
            var result = new PaginatedList<EmployeeEventDto>(eventResp, 0, 1, 10);
            var events =  _eventRepo.EmployeeEventGet(currentUserId);
            if (events != null)
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    events = events.Where(e => e.EmployeeLastName.ToLower().Contains(searchString.ToLower())
                    || e.EmployeeFirstName.ToLower().Contains(searchString.ToLower())
                    || e.EmployeeEmailAddress.ToLower().Contains(searchString.ToLower())
                    || e.EventTitle.ToLower().Contains(searchString.ToLower()));
                }

                result = await PaginatedList<EmployeeEventDto>.CreateAsync(events, index ?? 1, pageSize ?? 10);
                return result;
            }

            return result;
        }

        public async Task<Response<bool>> UpdateEmployeeEvent(UpdateEmployeeEventDto employeeEvent)
        {
            var eventItem = await _eventRepo.GetEmployeeEventById(employeeEvent.Id);
            if (eventItem == null)
            {
                return Response<bool>.Failure("Employee Event does not exist");
            }
            eventItem.EmployeeId = employeeEvent.EmployeeId;
            eventItem.EventId = employeeEvent.EventId;
            eventItem.EventDate = employeeEvent.EventDate;

            var result = await _eventRepo.UpdateEmployeeEvent(eventItem);
            if (result)
            {
                return Response<bool>.Success(true, "Employee Event updated");
            }
            return Response<bool>.Failure("Employee Event could not be updated, please try again");
        }

        public async Task<Response<bool>> ApproveEmployeeEvent(Guid userId, List<Guid> employeeIds)
        {
            var eventItems = await _eventRepo.GetEmployeeEvents(employeeIds);
            if (eventItems == null || !eventItems.Any())
            {
                return Response<bool>.Failure("Employee Events do not exist");
            }
            eventItems.ForEach( e =>
            {
                e.IsApproved = true;
                e.DateApproved = DateTime.UtcNow;
                e.ApprovedBy = userId;
                });

            var result = await _eventRepo.UpdateEmployeeEvents(eventItems);
            if (result)
            {
                return Response<bool>.Success(true, "Employee Events Aprroved");
            }
            return Response<bool>.Failure("Employee Event could not be approved, please try again");
        }
    }
}
