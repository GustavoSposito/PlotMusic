using Microsoft.EntityFrameworkCore;
using PlotMusic2.BdContextPlot;
using PlotMusic2.Interfaces;
using PlotMusic2.Models;


namespace PlotMusic2.Repositories;

public class GeneroRepository : IGeneroRepository
{
    private readonly PlotContext _context;

    public GeneroRepository(PlotContext context)
    {
        _context = context;
    }

    public async Task Atualizar(int id, Genero genero)
    {
        var existente = await _context.Genero.FindAsync(id);
        if (existente != null)
        {
            existente.Nome = genero.Nome;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Genero?> BuscarPorId(int id)
    {
        return await _context.Genero.AsNoTracking().FirstOrDefaultAsync(g => g.IdGenero == id);
    }

    public async Task Cadastrar(Genero genero)
    {
        await _context.Genero.AddAsync(genero);
        await _context.SaveChangesAsync();
    }

    public async Task Excluir(int id)
    {
        var existente = await _context.Genero.FindAsync(id);
        if (existente != null)
        {
            _context.Genero.Remove(existente);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Genero>> Listar()
    {
        return await _context.Genero.AsNoTracking().OrderBy(g => g.Nome).ToListAsync();
    }
}
