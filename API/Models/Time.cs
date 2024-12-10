using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class Time
{
    [Key]
    public int IdTime { get; set; }
    public string? Nome { get; set; }
    public string? Liga { get; set; }

}
