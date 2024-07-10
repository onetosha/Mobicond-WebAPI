using Mobicond_WebAPI.Models;

namespace Mobicond_WebAPI.Repositories.Interfaces.Base
{
    //Интерфейс для работы с бд над основными сущностями
    public interface IBaseRepository<T>
    {
        Task Create(T entity);
        Task Update(T entity);
        Task Delete(int id);
        Task<T> Get(int id);
        Task<IEnumerable<T>> GetAll();
    }
}
