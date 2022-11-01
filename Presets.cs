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
    public class ChannelsCollection
    {
        public List<Channel> Channels { get; set; } = new List<Channel>();
    }

    [Serializable]
    public class Channel
    {
        public string ChannelName { get; set; }
        public List<Preset> Presets{ get;set;} = new List<Preset>();
    }

    [Serializable]
    public class Preset:ICustom
    {
        public Preset()
        {
            TopAZ = new Setting() { Type = SettingType.TopAZ };
            TopPS = new Setting() { Type = SettingType.TopPS };
            BottomPS = new Setting() { Type = SettingType.BottomPS };
            BottomAZ = new Setting() { Type = SettingType.BottomAZ };
        }
        public string Name { get; set; }
        public byte NodeId { get; set; }
        public Int16 Averaging { get; set; }
        public float MinSignalRange { get; set; }
        public float MaxSignalRange { get; set; }
        public Setting TopAZ { get; set; }
        public Setting TopPS { get; set; }
        public Setting BottomPS { get; set; }
        public Setting BottomAZ { get; set; }
    }

    
}
