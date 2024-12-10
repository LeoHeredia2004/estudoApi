using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class Jogador
{
    [Key]
    public int IdJogador { get; set; }
    public string? Nome { get; set; }
    public string? Numero { get; set; }
    public Time? Time { get; set; }
    public int? IdTime { get; set; }

}
