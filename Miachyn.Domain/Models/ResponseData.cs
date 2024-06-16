using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miachyn.Domain.Models
{
    public class ResponseData<T>
    {
        // Запрашиваемые данные
        public T Data { get; set; }
        // Признак успешного завершения запроса
        public bool Success { get; set; } = true;
        // Сообщение в случае неуспешного завершения
        public string? ErrorMessage { get; set; }
    }
}
