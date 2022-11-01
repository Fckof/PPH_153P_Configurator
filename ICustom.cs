using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPH_153P_Configurator
{
    public interface ICustom
    {
        byte NodeId { get; set; }
        float MinSignalRange { get; set; }
        float MaxSignalRange { get; set; }
        Int16 Averaging { get; set; }
        Setting TopAZ { get; set; }
        Setting TopPS { get; set; }
        Setting BottomPS { get; set; }
        Setting BottomAZ { get; set; }
    }
}
