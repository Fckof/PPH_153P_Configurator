using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPH_153P_Configurator
{
        
    public static class Copier
    {
        //Копирование параметров объектов
        public static void CopyValues(ICustom target, ICustom source)
        {
            target.NodeId = source.NodeId;
            target.TopAZ.Value = source.TopAZ.Value;
            target.TopPS.Value = source.TopPS.Value;
            target.BottomPS.Value = source.BottomPS.Value;
            target.BottomAZ.Value = source.BottomAZ.Value;
            target.MinSignalRange = source.MinSignalRange;
            target.MaxSignalRange = source.MaxSignalRange;
            target.Averaging = source.Averaging;

            target.TopAZ.Histeresis = source.TopAZ.Histeresis;
            target.TopPS.Histeresis = source.TopPS.Histeresis;
            target.BottomPS.Histeresis = source.BottomPS.Histeresis;
            target.BottomAZ.Histeresis = source.BottomAZ.Histeresis;

            target.TopAZ.IsSet = source.TopAZ.IsSet;
            target.TopPS.IsSet = source.TopPS.IsSet;
            target.BottomPS.IsSet = source.BottomPS.IsSet;
            target.BottomAZ.IsSet = source.BottomAZ.IsSet;

            target.TopAZ.SettingSetter = source.TopAZ.SettingSetter;
            target.TopPS.SettingSetter = source.TopPS.SettingSetter;
            target.BottomPS.SettingSetter = source.BottomPS.SettingSetter;
            target.BottomAZ.SettingSetter = source.BottomAZ.SettingSetter;
        }

        //Обнуление значений объекта
        public static void SetToNull(DataModel target)
        {
            target.Value = 0;
            target.NodeId = 0;
            target.StateBitArray = null;
            target.AnalogStateBitArray = null;
            target.MinSignalRange = 0;
            target.MaxSignalRange = 0;
            target.Averaging = 0;
            target.DeviceStatus = 0;
            target.InputSignalCode = 0;

            target.TopAZ.Value = 0;
            target.TopPS.Value = 0;
            target.BottomPS.Value = 0;
            target.BottomAZ.Value = 0;

            target.TopAZ.Histeresis = 0;
            target.TopPS.Histeresis = 0;
            target.BottomPS.Histeresis = 0;
            target.BottomAZ.Histeresis = 0;

            target.TopAZ.IsSet = false;
            target.TopPS.IsSet = false;
            target.BottomPS.IsSet = false;
            target.BottomAZ.IsSet = false;

            target.TopAZ.SettingSetter = false;
            target.TopPS.SettingSetter = false;
            target.BottomPS.SettingSetter = false;
            target.BottomAZ.SettingSetter = false;
        }
    }
}
