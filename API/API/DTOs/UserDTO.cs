﻿namespace API.DTOs
{
    public class UserDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty;
        public int RoleId { get; set; }
    }
}
