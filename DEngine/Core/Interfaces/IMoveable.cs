using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEngine.Actor;

namespace DEngine.Core.Interfaces {
    public interface IMoveable {
        ActionResult Move(int dx, int dy);
        ActionResult Move(Point p);
        ActionResult Wait();
    }
}
