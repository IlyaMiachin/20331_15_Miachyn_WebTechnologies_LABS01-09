using Miachyn.Domain.Entities;
using Miachyn.Domain.Models;

namespace Miachyn.UI.Services.FurnitureService
{
    public interface IFurnitureService
    {
        /// <summary>
        /// Получение списка всех объектов
        /// </summary>
        /// <param name="categoryNormalizedName">нормализованное имя категории для фильтрации</param>
        /// <param name="pageNo">номер страницы списка</param>
        /// <returns></returns>
        public Task<ResponseData<ListModel<Furniture>>> GetFurnitureListAsync(string? categoryNormalizedName, int pageNo = 1);
        /// <summary>
        /// Поиск объекта по Id
        /// </summary>
        /// <param name="id">Идентификатор объекта</param>
        /// <returns>Найденный объект или null, если объект не найден</returns>
        public Task<ResponseData<Furniture>> GetFurnitureByIdAsync(int id);
        /// <summary>
        /// Обновление объекта
        /// </summary>
        /// <param name="id">Id изменяемого объекта</param>
        /// <param name="furniture">объект с новыми параметрами</param>
        /// <param name="formFile">Файл изображения</param>
        /// <returns></returns>
        public Task UpdateFurnitureAsync(int id, Furniture product, IFormFile? formFile);
        /// <summary>
        /// Удаление объекта
        /// </summary>
        /// <param name="id">Id удаляемого объекта</param>
        /// <returns></returns>
        public Task DeleteFurnitureAsync(int id);
        /// <summary>
        /// Создание объекта
        /// </summary>
        /// <param name="furniture">Новый объект</param>
        /// <param name="formFile">Файл изображения</param>
        /// <returns>Созданный объект</returns>
        public Task<ResponseData<Furniture>> CreateFurnitureAsync(Furniture product, IFormFile? formFile);
    }
}
