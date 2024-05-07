﻿namespace JobBoard.Domain.Entity
{
    public class EmployeeAccount : IEntity
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? AboutMe { get; set; }
    }
}
