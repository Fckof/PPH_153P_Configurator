using Newtonsoft.Json;
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

        public static T CloneJSON<T>(T source)
        {
            if (ReferenceEquals(source, null)) return default;
            var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source), deserializeSettings);
        }
        public static void CopyValues(DataModel target, DataModel source)
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
        public static void CopyValues(DataModel target, Preset source)
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
        public static void CopyValues(Preset target, DataModel source)
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
    }
}
