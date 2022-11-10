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
        DataModel.Setting TopAZ { get; set; }
        DataModel.Setting TopPS { get; set; }
        DataModel.Setting BottomPS { get; set; }
        DataModel.Setting BottomAZ { get; set; }
    }
}
