using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Miachyn.Domain.Entities
{
    public class Furniture
    {
        public int Id { get; set; } // Id мебели
        public string Name { get; set; } // Название мебели
        public string Description { get; set; } // Описание мебели
        public int Price { get; set; } // Цена
        public string? Image { get; set; } // Путь к файлу изображения
        // Навигационные свойства
        public int CategoryId { get; set; } // Группа мебели (например, "Стеллажи и книжные шкафы", "Стулья" и т.д.)
        public Category? Category { get; set; }
    }
}
