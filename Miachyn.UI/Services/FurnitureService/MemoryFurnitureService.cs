using Miachyn.Domain.Entities;
using Miachyn.Domain.Models;
using Miachyn.UI.Services.CategoryService;
using Microsoft.AspNetCore.Mvc;

namespace Miachyn.UI.Services.FurnitureService
{
    public class MemoryFurnitureService : IFurnitureService
    {
        List<Furniture> _furniture;
        List<Category> _categories;
        IConfiguration _config;
        public MemoryFurnitureService([FromServices] IConfiguration config, ICategoryService categoryService)
        {
            _config = config; 
            _categories = categoryService.GetCategoryListAsync()
            .Result
            .Data;
            SetupData();
        }
        // Инициализация списков
        private void SetupData()
        {
            _furniture = new List<Furniture>
            {
                new Furniture
                {
                    Id = 1,
                    Name = "VALFRED / SIBBEN",
                    Description = "Вращающийся стул детский, белый.",
                    Price = 199,
                    Image = "Images/Chair.jpg",
                    CategoryId = _categories.Find(c=>c.NormalizedName.Equals("chairs")).Id
                },
                new Furniture
                {
                    Id = 2,
                    Name = "MICKE",
                    Description = "Письменный стол, белый, 105x50 см.",
                    Price = 599,
                    Image = "Images/Desk.jpg",
                    CategoryId = _categories.Find(c=>c.NormalizedName.Equals("tables")).Id
                },
                new Furniture
                {
                    Id = 3,
                    Name = "GLADOM",
                    Description = "Стол сервировочный, серо-бежевый, 45x53 см.",
                    Price = 99,
                    Image = "Images/Table.jpg",
                    CategoryId = _categories.Find(c=>c.NormalizedName.Equals("tables")).Id
                },
                new Furniture
                {
                    Id = 4,
                    Name = "FÖRA",
                    Description = "Полка навесная / настенная, дуб беленый, 40x25 см.",
                    Price = 24,
                    Image = "Images/MountedShelf.jpg",
                    CategoryId = _categories.Find(c=>c.NormalizedName.Equals("shelves")).Id
                },
                new Furniture
                {
                    Id = 5,
                    Name = "FÖRA",
                    Description = "Полка навесная / настенная, белый, 60x25 см.",
                    Price = 32,
                    Image = "Images/Shelf.jpg",
                    CategoryId = _categories.Find(c=>c.NormalizedName.Equals("shelves")).Id
                },
                new Furniture
                {
                    Id = 6,
                    Name = "VÅRMA",
                    Description = "Комод / тумба, 4 ящика, шпон ясень черный, 160x54x40 см.",
                    Price = 289,
                    Image = "Images/Drawer.jpg",
                    CategoryId = _categories.Find(c=>c.NormalizedName.Equals("drawers")).Id
                },
                new Furniture
                {
                    Id = 7,
                    Name = "VÅRMA",
                    Description = "Комод / тумба, 4 ящика, белый, 160x54x40 см.",
                    Price = 32,
                    Image = "Images/Dresser.jpg",
                    CategoryId = _categories.Find(c=>c.NormalizedName.Equals("drawers")).Id
                }
            };
        }
        public Task<ResponseData<ListModel<Furniture>>> GetFurnitureListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            // Создать объект результата
            var result = new ResponseData<ListModel<Furniture>>();
            // Id категории для фильтрации
            int? categoryId = null;
            // Если требуется фильтрация, то найти Id категории с заданным categoryNormalizedName
            if (categoryNormalizedName != null)
                categoryId = _categories.Find(c => c.NormalizedName.Equals(categoryNormalizedName))?.Id;
            // Выбрать объекты, отфильтрованные по Id категории, если этот Id имеется
            var data = _furniture.Where(d => categoryId == null || d.CategoryId.Equals(categoryId))?.ToList();

            // Получить размер страницы из конфигурации
            int pageSize = _config.GetSection("Pagination:ItemsPerPage").Get<int>();
            // Получить общее количество страниц
            int totalPages;
            if (pageSize == 0)
            {
                pageSize = -1;
                totalPages = 1;
            }
            else
            {
                totalPages = (int)Math.Ceiling(data.Count / (double)pageSize);
            }
            // Получить данные страницы
            var listData = new ListModel<Furniture>()
            {
                Items = pageSize == -1 ? data.ToList() : data.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList(),
                CurrentPage = pageNo,
                TotalPages = totalPages
            };
            // Поместить разные в объект результата
            // result.Data = new ListModel<Furniture>() { Items = data };
            result.Data = listData;

            // Если список пустой
            if (data.Count == 0)
            {
                result.Success = false;
                result.ErrorMessage = "Нет объектов в выбранной категории :(";
            }
            // Вернуть результат
            return Task.FromResult(result);
        }
        public Task<ResponseData<Furniture>> GetFurnitureByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task UpdateFurnitureAsync(int id, Furniture flowers, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
        public Task DeleteFurnitureAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<ResponseData<Furniture>> CreateFurnitureAsync(Furniture furniture, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}
