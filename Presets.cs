using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace PPH_153P_Configurator
{
    [Serializable]
    public class PresetsCollection
    {
        public List<Preset> Presets{ get;set;} = new List<Preset>() ;
    }

    [Serializable]
    public class Preset
    {
        public Preset(DataModel source)
        {
            TopAZ=new Setting(SettingType.TopAZ);
            TopPS = new Setting(SettingType.TopPS);
            BottomPS = new Setting(SettingType.BottomPS);
            BottomAZ = new Setting(SettingType.BottomAZ);

            NodeId = source.NodeId;
            TopAZ.Value = source.TopAZ.Value;
            TopPS.Value = source.TopPS.Value;
            BottomPS.Value = source.BottomPS.Value;
            BottomAZ.Value = source.BottomAZ.Value;
            MinSignalRange = source.MinSignalRange;
            MaxSignalRange = source.MaxSignalRange;
            Averaging = source.Averaging;

            TopAZ.Histeresis = source.TopAZ.Histeresis;
            TopPS.Histeresis = source.TopPS.Histeresis;
            BottomPS.Histeresis = source.BottomPS.Histeresis;
            BottomAZ.Histeresis = source.BottomAZ.Histeresis;

            TopAZ.IsSet = source.TopAZ.IsSet;
            TopPS.IsSet = source.TopPS.IsSet;
            BottomPS.IsSet = source.BottomPS.IsSet;
            BottomAZ.IsSet = source.BottomAZ.IsSet;

            TopAZ.SettingSetter = source.TopAZ.SettingSetter;
            TopPS.SettingSetter = source.TopPS.SettingSetter;
            BottomPS.SettingSetter = source.BottomPS.SettingSetter;
            BottomAZ.SettingSetter = source.BottomAZ.SettingSetter;
        }
        public int NodeId { get; set; }
        public Int16 Averaging { get; set; }
        public float MinSignalRange { get; set; }
        public float MaxSignalRange { get; set; }
        public Setting TopAZ { get; set; }
        public Setting TopPS { get; set; }
        public Setting BottomPS { get; set; }
        public Setting BottomAZ { get; set; }
    }

    
}
