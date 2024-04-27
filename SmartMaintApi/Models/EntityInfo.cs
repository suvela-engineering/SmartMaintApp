using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SmartMaintApi.Models;

[Owned]
public class EntityInfo
{
    public DateTime Created { get; set; } = DateTime.UtcNow;
    [StringLength(256)]
    public string? CreatedBy { get; set; }
    public DateTime? Modified { get; set; }
    [StringLength(256)]
    public string? ModifiedBy { get; set; }
    public DateTime? Deleted { get; set; }
    [StringLength(256)]
    public string? DeleteBy { get; set; }
}
