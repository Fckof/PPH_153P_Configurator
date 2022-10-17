using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PPH_153P_Configurator
{
    public enum SettingType
    {
        TopAZ,
        TopPS,
        BottomPS,
        BottomAZ
    }

    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
    public class DataModel: ObservableObject
    {
        public int NodeId { get; private set; }
        public double MinSignalRange { get; private set; }
        public double MaxSignalRange { get; private set; }
        public double Averaging { get; private set; }
        public byte DeviceStatus { get; private set; }
        public byte InputSignalCode { get; private set; }
        private double _value;
        public double Value { get { return _value; } 
            set {
                _value = value;
                OnPropertyChanged("Value");
                } }
        Setting TopAZ = new Setting(SettingType.TopAZ,1,true, false);
        Setting TopPS = new Setting(SettingType.TopPS, 1, true, false);
        Setting BottomPS = new Setting(SettingType.BottomPS, 1, true, false);
        Setting BottomAZ = new Setting(SettingType.BottomAZ, 1, true, false);
        public void Clone(DataModel newData)
        {
            NodeId = newData.NodeId;
            MinSignalRange = newData.MinSignalRange;
            MaxSignalRange = newData.MaxSignalRange;
            Averaging = newData.Averaging;
            TopAZ.Clone(newData.TopAZ);
            TopPS.Clone(newData.TopPS);
            BottomPS.Clone(newData.BottomPS);
            BottomAZ.Clone(newData.BottomAZ);
        }
        public void UpdateData()
        {

        }
        
    }
    public class Setting
    {
        public Setting(SettingType type, int histeresis, bool isSet, bool settingSetter)
        {
            Type = type;
            Histeresis = histeresis;
            IsSet = isSet;
            SettingSetter = settingSetter;
        }
        public SettingType Type { get; private set; }
        public double Value { get; private set; }
        public int Histeresis { get; private set; }
        public bool IsSet { get; private set; }
        public bool SettingSetter { get; private set; }
        public void Clone(Setting copy)
        {
            Type=copy.Type;
            Histeresis=copy.Histeresis;
            Value=copy.Value;
            IsSet=copy.IsSet;
            SettingSetter=copy.SettingSetter;
        }
    }
}
