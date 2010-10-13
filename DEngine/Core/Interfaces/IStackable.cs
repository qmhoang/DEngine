using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roguelike.Universe.Items
{
    public interface IStackable
    {
        int Count { get; set; }
    }
}
