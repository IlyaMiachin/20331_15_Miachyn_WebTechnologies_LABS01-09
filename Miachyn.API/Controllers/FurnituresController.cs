using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Miachyn.API.Data;
using Miachyn.Domain.Entities;
using Miachyn.Domain.Models;

namespace Miachyn.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FurnituresController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public FurnituresController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: api/Furnitures
        [HttpGet]
        public async Task<ActionResult<ResponseData<ListModel<Furniture>>>> GetFurnitures(
            string? category,
            int pageNo = 1,
            int pageSize = 3)
        {
            // Создать объект результата
            var result = new ResponseData<ListModel<Furniture>>();

            // Фильтрация по категории и загрузка данных категории
            var data = _context.Furnitures
                .Include(d => d.Category)
                .Where(d => String.IsNullOrEmpty(category)
                || d.Category.NormalizedName.Equals(category, StringComparison.Ordinal));

            // Подсчет общего количества страниц
            int totalPages = (int)Math.Ceiling(data.Count() / (double)pageSize);

            if (pageNo > totalPages)
                pageNo = totalPages;

            // Создание объекта ProductListModel с нужной страницей данных
            var listData = new ListModel<Furniture>()
            {
                Items = await data
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(),
                CurrentPage = pageNo,
                TotalPages = totalPages
            };

            // Поместить данные в объект результата
            result.Data = listData;

            // Если список пустой
            if (data.Count() == 0)
            {
                result.Success = false;
                result.ErrorMessage = "Нет объектов в выбранной категории :(";
            }
            return result;
        }

        // GET: api/Furnitures/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Furniture>> GetFurniture(int id)
        {
            var furniture = await _context.Furnitures
                                  .Include(f => f.Category) // Включаем данные категории
                                  .FirstOrDefaultAsync(f => f.Id == id);

            if (furniture == null)
            {
                return NotFound();
            }

            return furniture;
        }

        // PUT: api/Furnitures/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFurniture(int id, Furniture furniture)
        {
            if (id != furniture.Id)
            {
                return BadRequest();
            }

            _context.Entry(furniture).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FurnitureExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Furnitures
        [HttpPost]
        public async Task<ActionResult<Furniture>> PostFurniture(Furniture furniture)
        {
            _context.Furnitures.Add(furniture);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFurniture", new { id = furniture.Id }, furniture);
        }

        // DELETE: api/Furnitures/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFurniture(int id)
        {
            var furniture = await _context.Furnitures.FindAsync(id);
            if (furniture == null)
            {
                return NotFound();
            }

            _context.Furnitures.Remove(furniture);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> SaveImage(int id, IFormFile image)
        {
            // Найти объект по Id
            var dish = await _context.Furnitures.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }
            // Путь к папке wwwroot/Images
            var imagesPath = Path.Combine(_env.WebRootPath, "Images");
            // Получить случайное имя файла
            var randomName = Path.GetRandomFileName();
            // Получить расширение в исходном файле
            var extension = Path.GetExtension(image.FileName);
            // Задать в новом имени расширение как в исходном файле
            var fileName = Path.ChangeExtension(randomName, extension);
            // Полный путь к файлу
            var filePath = Path.Combine(imagesPath, fileName);
            // Создать файл и открыть поток для записи
            using var stream = System.IO.File.OpenWrite(filePath);
            // Скопировать файл в поток
            await image.CopyToAsync(stream);
            // Получить Url хоста
            var host = "https://" + Request.Host;
            // Url файла изображения
            var url = $"{host}/Images/{fileName}";
            // Сохранить url файла в объекте
            dish.Image = url;
            await _context.SaveChangesAsync();
            return Ok();
        }        
        private bool FurnitureExists(int id)
        {
            return _context.Furnitures.Any(e => e.Id == id);
        }
    }
}