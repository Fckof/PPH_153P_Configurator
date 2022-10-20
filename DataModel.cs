using System;
using System.Collections;
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
        private int _nodeId;
        public int NodeId
        {
            get { return _nodeId; }
            set { _nodeId = value;
                OnPropertyChanged("NodeId");
            }
        }
        private int[] _stateBitArray;

        public int[] StateBitArray
        {
            get { return _stateBitArray; }
            set { _stateBitArray = value;
                OnPropertyChanged("StateBitArray");
            }
        }


        private int _minSignalRange;
        public int MinSignalRange
        {
            get { return _minSignalRange; }
            set { _minSignalRange = value;
                OnPropertyChanged("MaxSignalRange");
            }
        }


        private int _maxSignalRange;
        public int MaxSignalRange
        {
            get { return _maxSignalRange; }
            set { _maxSignalRange = value;
                OnPropertyChanged("MaxSignalRange");
            }
        }


        private int _averaging;

        public int Averaging
        {
            get { return _averaging; }
            set { _averaging = value;
                OnPropertyChanged("Averaging");
            }
        }


        private byte _deviceStatus;
        public byte DeviceStatus
        {
            get { return _deviceStatus; }
            set { 
                _deviceStatus = value;
                OnPropertyChanged("DeviceStatus");
            }
        }

        private int _inputSignalCode;
        public int InputSignalCode
        {
            get { return _inputSignalCode; }
            set { 
                _inputSignalCode = value;
                OnPropertyChanged("InputSignalCode");
            }
        }


        private double _value;
        public double Value { get { return _value; } 
            set {
                _value = value;
                OnPropertyChanged("Value");
                } }

        public Setting TopAZ { get; private set; }
        public Setting TopPS { get; private set; }
        public Setting BottomPS { get; private set; }
        public Setting BottomAZ { get; private set; }
        public DataModel()
        {
            TopAZ = new Setting(SettingType.TopAZ);
            TopPS = new Setting(SettingType.TopPS);
            BottomPS = new Setting(SettingType.BottomPS);
            BottomAZ = new Setting(SettingType.BottomAZ);
        }
        
        
    }
    public class Setting:ObservableObject
    {
        public Setting(SettingType type)
        {
            Type = type;
            Histeresis = 0;
            IsSet = true;
            SettingSetter = false;
        }

        private SettingType _type;
        public SettingType Type { 
            get { return _type; } 
            set { _type = value;
                OnPropertyChanged("Type");
            } 
        }
        private int _value;
        public int Value { 
            get { return _value; }
            set { _value= value;
                OnPropertyChanged("Value");
            }    
        }
        private int _histeresis;
        public int Histeresis { 
            get { return _histeresis; }
            set { _histeresis = value;
                OnPropertyChanged("Histeresis");
            } 
        }
        private bool _isSet;

        public bool IsSet
        {
            get { return _isSet; }
            set { _isSet = value;
                OnPropertyChanged("IsSet");
            }
        }
        private bool _settingSetter;

        public bool SettingSetter
        {
            get { return _settingSetter; }
            set { _settingSetter = value;
                OnPropertyChanged("SettingSetter");
            }
        }

    }
}
