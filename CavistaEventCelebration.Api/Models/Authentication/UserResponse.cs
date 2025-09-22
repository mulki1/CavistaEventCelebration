﻿namespace CavistaEventCelebration.Api.Models.Authentication
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string? UserName { get; set; }     

        public Guid EmployeeId { get; set; }    

        public string? Email { get; set; }

        public string Status { get; set; }

        public string? PhoneNumber { get; set; } 
    }
}
