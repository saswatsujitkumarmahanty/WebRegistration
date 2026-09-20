using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Application.Dto
{
    public class SetRoleDto
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty; // "User" or "Admin"

    }
}
