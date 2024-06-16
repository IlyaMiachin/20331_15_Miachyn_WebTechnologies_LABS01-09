using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Miachyn.Domain.Entities;

namespace Miachyn.Domain.Models
{
    public class Cart
    {
        public int Id { get; set; }
        /// <summary>
        /// Список объектов в корзине
        /// key - идентификатор объекта
        /// </summary>
        public Dictionary<int, CartItem> CartItems { get; set; } = new();
        /// <summary>
        /// Добавить объект в корзину
        /// </summary>
        /// <param name="furniture">Добавляемый объект</param>
        public virtual void AddToCart(Furniture furniture)
        {
            if (CartItems.ContainsKey(furniture.Id))
            {
                CartItems[furniture.Id].Qty++;
            }
            else
            {
                CartItems.Add(furniture.Id, new CartItem
                {
                    Item = furniture,
                    Qty = 1
                });
            };
        }
        /// <summary>
        /// Удалить объект из корзины
        /// </summary>
        /// <param name="furniture">удаляемый объект</param>
        public virtual void RemoveItems(int id)
        {
            CartItems.Remove(id);
        }
        /// <summary>
        /// Очистить корзину
        /// </summary>
        public virtual void ClearAll()
        {
            CartItems.Clear();
        }
        /// <summary>
        /// Количество объектов в корзине
        /// </summary>
        public int Count { get => CartItems.Sum(item => item.Value.Qty); }
        /// <summary>
        /// Общая стоимость
        /// </summary>
        public double TotalPrice
        {
            get => CartItems.Sum(item => item.Value.Item.Price * item.Value.Qty);
        }
    }
}
