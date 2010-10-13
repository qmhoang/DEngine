using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEngine.Core {
    /// <summary>
    /// Global unique id for every game element in the universe
    /// </summary>
    public interface IGuid {
        long Guid { get; }
    }
}
