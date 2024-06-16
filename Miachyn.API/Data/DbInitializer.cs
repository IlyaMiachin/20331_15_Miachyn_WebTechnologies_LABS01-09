using Miachyn.Domain.Entities;
using Miachyn.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Miachyn.API.Data
{
    public static class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            // Uri проекта
            var uri = "https://localhost:7002/";
            // Получение контекста БД
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Заполнение данными
            if (!context.Categories.Any() && !context.Furnitures.Any())
            {
                var categories = new Category[]
                {
                    new Category { Id = 1, Name = "Стулья", NormalizedName = "chairs" },
                    new Category { Id = 2, Name = "Столы и письменные столы", NormalizedName = "tables" },
                    new Category { Id = 3, Name = "Стеллажи и книжные шкафы", NormalizedName = "shelves" },
                    new Category { Id = 4, Name = "Комоды", NormalizedName = "drawers" }
                };

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();

                var furniture = new List<Furniture>
                {
                    new Furniture
                    {
                        Name = "VALFRED / SIBBEN",
                        Description = "Вращающийся стул детский, белый.",
                        Price = 199,
                        Category = categories.FirstOrDefault(c=>c.NormalizedName.Equals("chairs")),
                        Image = uri + "Images/Chair.jpg"
                    },
                    new Furniture
                    {
                        Name = "MICKE",
                        Description = "Письменный стол, белый, 105x50 см.",
                        Price = 599,
                        Category = categories.FirstOrDefault(c=>c.NormalizedName.Equals("tables")),
                        Image = uri + "Images/Desk.jpg"
                    },
                    new Furniture
                    {
                        Name = "GLADOM",
                        Description = "Стол сервировочный, серо-бежевый, 45x53 см.",
                        Price = 99,
                        Category = categories.FirstOrDefault(c=>c.NormalizedName.Equals("tables")),
                        Image = uri + "Images/Table.jpg"
                    },
                    new Furniture
                    {
                        Name = "FÖRA",
                        Description = "Полка навесная / настенная, дуб беленый, 40x25 см.",
                        Price = 24,
                        Category = categories.FirstOrDefault(c=>c.NormalizedName.Equals("shelves")),
                        Image = uri + "Images/MountedShelf.jpg"
                    },
                    new Furniture
                    {
                        Name = "FÖRA",
                        Description = "Полка навесная / настенная, белый, 60x25 см.",
                        Price = 32,
                        Category = categories.FirstOrDefault(c=>c.NormalizedName.Equals("shelves")),
                        Image = uri + "Images/Shelf.jpg"
                    },
                    new Furniture
                    {
                        Name = "VÅRMA",
                        Description = "Комод / тумба, 4 ящика, шпон ясень черный, 160x54x40 см.",
                        Price = 289,
                        Category = categories.FirstOrDefault(c=>c.NormalizedName.Equals("drawers")),
                        Image = uri + "Images/Drawer.jpg"
                    },
                    new Furniture
                    {
                        Name = "VÅRMA",
                        Description = "Комод / тумба, 4 ящика, белый, 160x54x40 см.",
                        Price = 32,
                        Category = categories.FirstOrDefault(c=>c.NormalizedName.Equals("drawers")),
                        Image = uri + "Images/Dresser.jpg"
                    }
                };
                await context.AddRangeAsync(furniture);
                await context.SaveChangesAsync();
            }
        }
    }
}
