using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Miachyn.Domain.Entities;

namespace Miachyn.Domain.Models
{
    public class CartItem
    {
        public Furniture Item { get; set; }
        public int Qty { get; set; }
    }
}

