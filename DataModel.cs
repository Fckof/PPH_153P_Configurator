using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PPH_153P_Configurator
{
    public enum SettingType
    {
        TopAZ,
        TopPS,
        BottomPS,
        BottomAZ
    }
    [Serializable]
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

    [Serializable]
    public class DataModel: ObservableObject, ICustom
    {
        private byte _nodeId;
        public byte NodeId
        {
            get { return _nodeId; }
            set { _nodeId = value;
                OnPropertyChanged("NodeId");
            }
        }
        [NonSerialized]
        private int[] _stateBitArray;
        [XmlIgnore]
        public int[] StateBitArray
        {
            get { return _stateBitArray; }
            set { _stateBitArray = value;
                OnPropertyChanged("StateBitArray");
            }
        }
        [NonSerialized]
        private float _halfRangeValue;
        [XmlIgnore]
        public float HalfRangeValue
        {
            get { return _halfRangeValue; }
            set
            {
                _halfRangeValue = (MaxSignalRange - MinSignalRange) / 2f;
                OnPropertyChanged("HalfRangeValue");
            }
        }
        [NonSerialized]
        private int[] _analogStateBitArray;
        [XmlIgnore]
        public int[] AnalogStateBitArray
        {
            get { return _analogStateBitArray; }
            set
            {
                _analogStateBitArray = value;
                OnPropertyChanged("AnalogStateBitArray");
            }
        }

        private float _minSignalRange;
        public float MinSignalRange
        {
            get { return _minSignalRange; }
            set {_minSignalRange = value;
                OnPropertyChanged("MinSignalRange");
            }
        }


        private float _maxSignalRange;
        public float MaxSignalRange
        {
            get { return _maxSignalRange; }
            set { _maxSignalRange = value;
                HalfRangeValue = 0;
                OnPropertyChanged("MaxSignalRange");
            }
        }

        private Int16 _averaging;
        public Int16 Averaging
        {
            get { return _averaging; }
            set { _averaging = value<=(255*16) ? Convert.ToInt16(Math.Round(value/16f)*16) : (short)(255 * 16);
                OnPropertyChanged("Averaging");
            }
        }

        [NonSerialized]
        private byte _deviceStatus;
        [XmlIgnore]
        public byte DeviceStatus
        {
            get { return _deviceStatus; }
            set { 
                _deviceStatus = value;
                OnPropertyChanged("DeviceStatus");
            }
        }
        [NonSerialized]
        private string _inputSignalCode;
        [XmlIgnore]
        public string InputSignalCode
        {
            get { return _inputSignalCode; }
            set { 
                _inputSignalCode = value;
                OnPropertyChanged("InputSignalCode");
            }
        }

        [NonSerialized]
        private float _value;
        [XmlIgnore]
        public float Value { get { return Transform.ToFourDigits(_value); } 
            set {
                _value = value;
                OnPropertyChanged("Value");
                } }

        public Setting TopAZ { get; set; }
        public Setting TopPS { get;  set; }
        public Setting BottomPS { get;  set; }
        public Setting BottomAZ { get;  set; }
        public DataModel()
        {
            TopAZ = new Setting() { Type= SettingType.TopAZ };
            TopPS = new Setting() { Type = SettingType.TopPS };
            BottomPS = new Setting() { Type = SettingType.BottomPS };
            BottomAZ = new Setting() { Type = SettingType.BottomAZ };
        }
        [Serializable]
        public class Setting : ObservableObject
        {
            [XmlIgnore]
            private SettingType _type;
            [XmlIgnore]
            public SettingType Type
            {
                get { return _type; }
                set
                {
                    _type = value;
                    OnPropertyChanged("Type");
                }
            }
            private float _value;
            public float Value
            {
                get { return _value; }
                set
                {
                    _value = Transform.ToFourDigits(value);
                    OnPropertyChanged("Value");
                }
            }
            private float _histeresis;
            public float Histeresis
            {
                get { return _histeresis; }
                set
                {
                    _histeresis = value;
                    OnPropertyChanged("Histeresis");
                }
            }
            [XmlIgnore]
            public byte isSetValue;
            private bool _isSet;
            public bool IsSet
            {
                get { return _isSet; }
                set
                {
                    _isSet = value;
                    if (value)
                    {
                        switch (_type)
                        {
                            case SettingType.TopAZ:
                            case SettingType.TopPS:
                                isSetValue = 2;
                                break;
                            case SettingType.BottomAZ:
                            case SettingType.BottomPS:
                                isSetValue = 3;
                                break;
                        }
                    }
                    else isSetValue = 0;
                    OnPropertyChanged("IsSet");
                }
            }
            private bool _settingSetter;
            public bool SettingSetter
            {
                get { return _settingSetter; }
                set
                {
                    _settingSetter = value;
                    OnPropertyChanged("SettingSetter");
                }
            }

        }
    }
    /*[Serializable]
    public class Setting:ObservableObject
    {
        [XmlIgnore]
        private SettingType _type;
        [XmlIgnore]
        public SettingType Type { 
            get { return _type; } 
            set { _type = value;
                OnPropertyChanged("Type");
            } 
        }
        private float _value;
        public float Value { 
            get { return _value; }
            set { _value= Transform.ToFourDigits(value);
                OnPropertyChanged("Value");
            }    
        }
        private float _histeresis;
        public float Histeresis { 
            get { return _histeresis; }
            set { _histeresis = value;
                
                OnPropertyChanged("Histeresis");
            } 
        }
        [XmlIgnore]
        public byte isSetValue;
        private bool _isSet;
        public bool IsSet
        {
            get { return _isSet; }
            set { _isSet = value;
                if (value)
                {
                    switch (_type)
                    {
                        case SettingType.TopAZ:case SettingType.TopPS:
                            isSetValue = 2;
                            break;
                        case SettingType.BottomAZ:case SettingType.BottomPS:
                            isSetValue = 3;
                            break;
                    }
                }else isSetValue = 0;
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

    }*/
}
