using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SmartMaintApi.Models
{
    public class User : IdentityUser
    {
        [StringLength(256)]
        public string? FirstName { get; set; }
        [StringLength(256)]
        public string? LastName { get; set; }
        [StringLength(256)]
        public string? Title { get; set; }

        [JsonIgnore] // Exclude from serialization
        public EntityInfo EntityInfo { get; set; } = new EntityInfo();
    }
}