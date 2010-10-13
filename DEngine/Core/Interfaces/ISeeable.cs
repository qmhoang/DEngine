using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DEngine.Core.Interfaces;

namespace DEngine.Core {
    public interface ISeeable {
        bool CanSee(INoticeable thing);
        bool CanSee(Point position);
        int SightRadius { get; }

    }
}
