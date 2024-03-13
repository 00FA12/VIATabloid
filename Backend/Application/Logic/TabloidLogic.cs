using Application.DaoInterfaces;
using Application.LogicInterfaces;
using Domain.Model;

namespace Application.Logic;

public class TabloidLogic : ITabloidLogic
{
    private readonly ITabloidDao tabloidDao;
    public TabloidLogic(ITabloidDao tabloidDao)
    {
        this.tabloidDao = tabloidDao;
    }
    public async Task<Tabloid> CreateTabloidAsync()
    {
        Tabloid? existing = await tabloidDao.GetTabloidAsync();
        if(existing != null)
        {
            throw new Exception("A Tabloid Already exists");
        }
        return await tabloidDao.CreateTabloidAsync();
    }

    public async Task<Tabloid?> GetTabloidAsync()
    {
        return await tabloidDao.GetTabloidAsync();
    }
}
