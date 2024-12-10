using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using API.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDataContext>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod();
    });
});
var app = builder.Build();

app.UseCors("AllowReactApp");

app.MapGet("/", () => "Prova Final =(");

//----------------- TIMES -------------------------------

app.MapPost("/api/times/cadastrar", ([FromServices] AppDataContext ctx, [FromBody] Time time) => 
{
    ctx.Times.Add(time);
    ctx.SaveChanges();
    return Results.Created("Time Criado Com sucesso!", time);
});

app.MapGet("/api/times/listar", ([FromServices] AppDataContext ctx) => 
{
    if(ctx.Times.Any()){
        return Results.Ok(ctx.Times.ToList());
    }
    return Results.NotFound("Nenhum time encontrado");
});


app.MapGet("/api/times/{idTime}", async (int idTime, AppDataContext ctx) =>
{
    // Busca o time pelo ID
    var time = await ctx.Times.FindAsync(idTime);
    
    // Verifica se o time foi encontrado
    if (time == null)
    {
        return Results.NotFound("Time não encontrado.");
    }
    
    // Retorna o time encontrado
    return Results.Ok(time);
});

app.MapPut("/api/times/alterar/{idTime}", async (int idTime, Time timeAtualizado, AppDataContext ctx) => 
{
    var time = await ctx.Times.FindAsync(idTime);
    if (time == null){
        return Results.NotFound("Time nao encontrado");
    }

    time.Nome = timeAtualizado.Nome;
    time.Liga = timeAtualizado.Liga;
    await ctx.SaveChangesAsync();
    return Results.Ok("Time Atualizado");    
});

app.MapDelete("/api/times/deletar/{idTime}", async (int idTime, AppDataContext ctx) => 
{
    var time = await ctx.Times.FindAsync(idTime);
    if (time == null){
        return Results.NotFound("Time nao encontrado");
    }

    ctx.Times.Remove(time);
    await ctx.SaveChangesAsync();
    return Results.Ok ("Time Deletado");    
});

//---------------- JOGADORES -----------------------------

app.MapPost("/api/jogadores/cadastrar", ([FromServices] AppDataContext ctx, [FromBody] Jogador jogador) => 
{

    Time? time = ctx.Times.Find(jogador.IdTime);
    if(time == null){
        return Results.NotFound("Time nao encontrado");
    }
    jogador.Time = time;
    ctx.Jogadores.Add(jogador);
    ctx.SaveChanges();
    return Results.Created("Time Criado Com sucesso!", jogador);
});

app.MapGet("/api/jogadores/listar", ([FromServices] AppDataContext ctx) => 
{
    if(ctx.Jogadores.Any()){
        return Results.Ok(ctx.Jogadores.Include(x => x.Time).ToList());
    }
    return Results.NotFound("Nenhum jogador encontrado");
});

app.MapPut("/api/jogadores/alterar/{idJogador}", async (int idJogador, Jogador jogadorAtualizado, AppDataContext ctx) => 
{
    var jogador = await ctx.Jogadores.FindAsync(idJogador);
    if (jogador == null){
        return Results.NotFound("Jogador nao encontrado");
    }

    jogador.Nome = jogadorAtualizado.Nome;
    jogador.Numero = jogadorAtualizado.Numero;
    await ctx.SaveChangesAsync();
    return Results.Ok("Time Atualizado");    
});

app.MapDelete("/api/jogadores/deletar/{idJogador}", async (int idJogador, AppDataContext ctx) => 
{
    var jogador = await ctx.Jogadores.FindAsync(idJogador);
    if (jogador == null){
        return Results.NotFound("jogador nao encontrado");
    }

    ctx.Jogadores.Remove(jogador);
    await ctx.SaveChangesAsync();
    return Results.Ok ("Jogador Deletado");    
});

app.MapGet("/api/jogadores/listar/{idTime}", async (int idTime, AppDataContext ctx) => {
    var jogador = await ctx.Jogadores
                            .Where(p => p.IdTime == idTime)
                            .Include(j => j.Time)
                            .ToListAsync();

    return jogador.Any() ? Results.Ok(jogador):Results.NotFound("Não encontrado");
});


app.Run();
