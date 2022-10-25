using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Input;
using System.Collections;

namespace PPH_153P_Configurator
{
    
    public class Controller:ObservableObject
    {
        public DataModel MainData { get; private set; }
        public DataModel InputData { get; private set; }
        private Can channel=new Can(0);
        private Thread thread;
        private bool threadCloser = true;
        private byte[] saveReq = { 0x22, 0x10, 0x10,0x01, 0x73, 0x61, 0x76, 0x65 };
        private SolidColorBrush _colorChange;
        private SolidColorBrush _redColor= new SolidColorBrush(Color.FromRgb(226, 22, 12));
        private SolidColorBrush _yellowColor = new SolidColorBrush(Color.FromRgb(249, 240, 12));
        private SolidColorBrush _greenColor = new SolidColorBrush(Color.FromRgb(6, 176, 37));
        public SolidColorBrush ColorChange
        {
            get { return _colorChange; }
            set { _colorChange = value;
                OnPropertyChanged("ColorChange");
            }
        }

        public Controller()
        {
            channel.Open(CanOpenFlag.Can11 | CanOpenFlag.Can29) ;
            channel.SetBaud(CanBaudRate.BCI_1M);
            channel.Start();
            MainData = new DataModel();
            InputData= new DataModel();
            
            ColorChange = new SolidColorBrush(Color.FromRgb(6, 176, 37));
            thread = new Thread(RecieveCanMessage);
            thread.Start();
        }
        private Visibility _topAZVisibility;
        public Visibility TopAZVisibility
        {
            get { return _topAZVisibility; }
            set {
                _topAZVisibility = value;
                OnPropertyChanged("TopAZVisibility");
            }
        }
        private Visibility _topPSVisibility;
        public Visibility TopPSVisibility
        {
            get { return _topPSVisibility; }
            set
            {
                _topPSVisibility = value;
                OnPropertyChanged("TopPSVisibility");
            }
        }
        private Visibility _bottomPSVisibility;
        public Visibility BottomPSVisibility
        {
            get { return _bottomPSVisibility; }
            set
            {
                _bottomPSVisibility = value;
                OnPropertyChanged("BottomPSVisibility");
            }
        }
        private Visibility _bottomAZVisibility;
        public Visibility BottomAZVisibility
        {
            get { return _bottomAZVisibility; }
            set
            {
                _bottomAZVisibility = value;
                OnPropertyChanged("BottomAZVisibility");
            }
        }

        private void RecieveCanMessage()
        {
            while (threadCloser)
            {
                ParseData(channel.ReadAll());
                RequestData(CompareRequests(MainData));
            }
        }
        private void RequestData(CanMessage[] reqs)
        {
            foreach(var item in reqs)
            {
                channel.Write(item);
            }
            
        }
        public CanMessage[] CompareDataToSend(DataModel item)
        {
            byte[] writeMaxSignalRange = (new byte[] { 0x22, 0x23, 0x61, 0x1 }).Concat(BitConverter.GetBytes(item.MaxSignalRange)).ToArray();
            byte[] writeMinSignalRange = (new byte[] { 0x22, 0x21, 0x61, 0x1 }).Concat(BitConverter.GetBytes(item.MinSignalRange)).ToArray();
            byte[] writeAveraging = (new byte[] { 0x22, 0xA1, 0x61, 0x1 }).Concat(BitConverter.GetBytes(item.Averaging)).ToArray();
            byte[] writeNodeId = (new byte[] { 0x22, 0x00, 0x20, 0x1 }).Concat(BitConverter.GetBytes(item.NodeId)).ToArray();

            byte[] writeTopAZHisteresis = (new byte[] { 0x22, 0x0B, 0x65, 0x1 }).Concat(BitConverter.GetBytes(item.TopAZ.Histeresis)).ToArray();
            byte[] writeTopPSHisteresis = (new byte[] { 0x22, 0x1B, 0x65, 0x1 }).Concat(BitConverter.GetBytes(item.TopPS.Histeresis)).ToArray();
            byte[] writeBottomPSHisteresis = (new byte[] { 0x22, 0x2B, 0x65, 0x1 }).Concat(BitConverter.GetBytes(item.BottomPS.Histeresis)).ToArray();
            byte[] writeBottomAZHisteresis = (new byte[] { 0x22, 0x3B, 0x65, 0x1 }).Concat(BitConverter.GetBytes(item.BottomAZ.Histeresis)).ToArray();

            byte[] writeTopAZIsSet = (new byte[] { 0x22, 0x08, 0x65, 0x1 }).Concat(BitConverter.GetBytes(item.TopAZ.IsSet)).ToArray();
            byte[] writeTopPSIsSet = (new byte[] { 0x22, 0x18, 0x65, 0x1 }).Concat(BitConverter.GetBytes(item.TopPS.IsSet)).ToArray();
            byte[] writeBottomPSIsSet = (new byte[] { 0x22, 0x28, 0x65, 0x1 }).Concat(BitConverter.GetBytes(item.BottomPS.IsSet)).ToArray();
            byte[] writeBottomAZIsSet = (new byte[] { 0x22, 0x38, 0x65, 0x1 }).Concat(BitConverter.GetBytes(item.BottomAZ.IsSet)).ToArray();

            byte[] writeTopAZSettingSetter = (new byte[] { 0x22, 0x0F, 0x65, 0x1 }).Concat(BitConverter.GetBytes(item.TopAZ.SettingSetter)).ToArray();
            byte[] writeTopPSSettingSetter = (new byte[] { 0x22, 0x1F, 0x65, 0x1 }).Concat(BitConverter.GetBytes(item.TopPS.SettingSetter)).ToArray();
            byte[] writeBottomPSSettingSetter = (new byte[] { 0x22, 0x2F, 0x65, 0x1 }).Concat(BitConverter.GetBytes(item.BottomPS.SettingSetter)).ToArray();
            byte[] writeBottomAZSettingSetter = (new byte[] { 0x22, 0x3F, 0x65, 0x1 }).Concat(BitConverter.GetBytes(item.BottomAZ.SettingSetter)).ToArray();

            byte[] writeTopAZValue = (new byte[] { 0x22, 0x0A, 0x65, 0x1 }).Concat(BitConverter.GetBytes(item.TopAZ.Value)).ToArray();
            byte[] writeTopPSValue = (new byte[] { 0x22, 0x1A, 0x65, 0x1 }).Concat(BitConverter.GetBytes(item.TopPS.Value)).ToArray();
            byte[] writeBottomPSValue = (new byte[] { 0x22, 0x2A, 0x65, 0x1 }).Concat(BitConverter.GetBytes(item.BottomPS.Value)).ToArray();
            byte[] writeBottomAZValue = (new byte[] { 0x22, 0x3A, 0x65, 0x1 }).Concat(BitConverter.GetBytes(item.BottomAZ.Value)).ToArray();


            return new CanMessage[] {
                new CanMessage { Id = 0x0, Data = new byte[] { 0x80, (byte)MainData.NodeId },Size=(byte)2 },
                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = writeMaxSignalRange, Size=(byte)writeMaxSignalRange.Length},
                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = writeMinSignalRange},
                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = writeAveraging},
                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = writeNodeId},

                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = writeTopAZHisteresis},
                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = writeTopPSHisteresis},
                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = writeBottomPSHisteresis},
                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = writeBottomAZHisteresis},

                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = writeTopAZIsSet},
                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = writeTopPSIsSet},
                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = writeBottomPSIsSet},
                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = writeBottomAZIsSet},

                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = writeTopAZSettingSetter},
                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = writeTopPSSettingSetter},
                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = writeBottomPSSettingSetter},
                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = writeBottomAZSettingSetter},

                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = writeTopAZValue},
                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = writeTopPSValue},
                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = writeBottomPSValue},
                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = writeBottomAZValue},
                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = saveReq, Size=(byte)saveReq.Length},
                new CanMessage { Id = 0x0, Data = new byte[] { 0x01, (byte)item.NodeId}, Size=(byte)2 }
            };
        }
        public void SendData(CanMessage[] messages)
        {
            threadCloser = false;
            foreach (CanMessage message in messages)
            {
                channel.Write(message);
                var j = channel.ReadAll();
            }
            threadCloser = true;
            thread = new Thread(RecieveCanMessage);
            thread.Start();
        }
        private void ParseData(CanMessage[] arr)
        {
            foreach (var item in arr)
            {
                uint nodeId = arr[0].Id & 0x7F;
                uint msgId = item.Id & 0xffffff80;
                MainData.NodeId= (byte)nodeId;
                switch (msgId)
                {
                    case 0x180:
                        MainData.Value = BitConverter.ToSingle(item.Data, 0);
                        MainData.InputSignalCode = BitConverter.ToInt16(item.Data, 4);
                        MainData.DeviceStatus = item.Data[6];
                        MainData.StateBitArray=FromByteToBitArray(item.Data[6]);
                        MainData.AnalogStateBitArray=FromByteToBitArray(item.Data[7]);
                        break;
                    case 0x380:
                        byte state = item.Data[2];
                        ColorChange = ProgressBarColorChange(state);
                        break;
                    case 0x1080:
                        MainData.TopAZ.Value = BitConverter.ToSingle(item.Data, 0);
                        break;
                    case 0x1180:
                        MainData.TopPS.Value = BitConverter.ToSingle(item.Data, 0);
                        break;
                    case 0x1280:
                        MainData.BottomPS.Value = BitConverter.ToSingle(item.Data, 0);
                        break;
                    case 0x1380:
                        MainData.BottomAZ.Value = BitConverter.ToSingle(item.Data, 0);
                        break;
                    case 0x580:
                        DefineObjectViaFunctionCode(item);
                        break;
                }
            }
        }
        private CanMessage[] CompareRequests(DataModel item)
        {
            return new CanMessage[] { 
                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = new byte[] { 0x40, 0x21, 0x61, 0x1 },Size=0x4 },
                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = new byte[] { 0x40, 0x23, 0x61, 0x1 },Size=0x4 },
                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = new byte[] { 0x40, 0xA1, 0x61, 0x1 },Size=0x4 },
                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = new byte[] { 0x40, 0x0B, 0x65, 0x1 },Size=0x4 },
                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = new byte[] { 0x40, 0x1B, 0x65, 0x1 },Size=0x4 },
                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = new byte[] { 0x40, 0x2B, 0x65, 0x1 },Size=0x4 },
                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = new byte[] { 0x40, 0x3B, 0x65, 0x1 },Size=0x4 },
                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = new byte[] { 0x40, 0x08, 0x65, 0x1 },Size=0x4 },
                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = new byte[] { 0x40, 0x18, 0x65, 0x1 },Size=0x4 },
                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = new byte[] { 0x40, 0x28, 0x65, 0x1 },Size=0x4 },
                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = new byte[] { 0x40, 0x38, 0x65, 0x1 },Size=0x4 },
                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = new byte[] { 0x40, 0x0F, 0x65, 0x1 },Size=0x4 },
                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = new byte[] { 0x40, 0x1F, 0x65, 0x1 },Size=0x4 },
                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = new byte[] { 0x40, 0x2F, 0x65, 0x1 },Size=0x4 },
                new CanMessage { Id = (uint)(0x600 + item.NodeId), Data = new byte[] { 0x40, 0x3F, 0x65, 0x1 },Size=0x4 }
            };
        }
        private void DefineObjectViaFunctionCode(CanMessage mes)
        {
            if (mes.Data[0] == 0x42)
            {
                var target = BitConverter.ToInt16(mes.Data, 1);
                switch (target)
                {
                    case 0x6121:
                        MainData.MinSignalRange=BitConverter.ToSingle(mes.Data, 4);
                        break;
                    case 0x6123:
                        MainData.MaxSignalRange = BitConverter.ToSingle(mes.Data, 4);
                        break;
                    case 0x61A1:
                        MainData.Averaging = BitConverter.ToInt16(mes.Data, 4);
                        break;
                    case 0x650B:
                        MainData.TopAZ.Histeresis = Convert.ToInt32(BitConverter.ToSingle(mes.Data, 4));
                        break;
                    case 0x651B:
                        MainData.TopPS.Histeresis = Convert.ToInt32(BitConverter.ToSingle(mes.Data, 4));
                        break;
                    case 0x652B:
                        MainData.BottomPS.Histeresis = Convert.ToInt32(BitConverter.ToSingle(mes.Data, 4));
                        break;
                    case 0x653B:
                        MainData.BottomAZ.Histeresis = Convert.ToInt32(BitConverter.ToSingle(mes.Data, 4));
                        break;
                    case 0x6508:
                        MainData.TopAZ.IsSet = BitConverter.ToBoolean(mes.Data, 4);
                        TopAZVisibility = DisableScrloll(MainData.TopAZ.IsSet);
                        break;
                    case 0x6518:
                        MainData.TopPS.IsSet = BitConverter.ToBoolean(mes.Data, 4);
                        TopPSVisibility = DisableScrloll(MainData.TopPS.IsSet);
                        break;
                    case 0x6528:
                        MainData.BottomPS.IsSet = BitConverter.ToBoolean(mes.Data, 4);
                        BottomPSVisibility = DisableScrloll(MainData.BottomPS.IsSet);
                        break;
                    case 0x6538:
                        MainData.BottomAZ.IsSet = BitConverter.ToBoolean(mes.Data, 4);
                        BottomAZVisibility = DisableScrloll(MainData.BottomAZ.IsSet);
                        break;
                    case 0x650F:
                        MainData.TopAZ.SettingSetter =  BitConverter.ToBoolean(mes.Data, 4);
                        break;
                    case 0x651F:
                        MainData.TopPS.SettingSetter = BitConverter.ToBoolean(mes.Data, 4);
                        break;
                    case 0x652F:
                        MainData.BottomPS.SettingSetter = BitConverter.ToBoolean(mes.Data, 4);
                        break;
                    case 0x653F:
                        MainData.BottomAZ.SettingSetter = BitConverter.ToBoolean(mes.Data, 4);
                        break;
                }

            }
        }
        public void InitInputData()
        {
            InputData.NodeId = MainData.NodeId;
            InputData.TopAZ.Value=MainData.TopAZ.Value;
            InputData.TopPS.Value = MainData.TopPS.Value;
            InputData.BottomPS.Value = MainData.BottomPS.Value;
            InputData.BottomAZ.Value = MainData.BottomAZ.Value;
            InputData.MinSignalRange = MainData.MinSignalRange;
            InputData.MaxSignalRange = MainData.MaxSignalRange;
            InputData.Averaging=MainData.Averaging;

            InputData.TopAZ.Histeresis=MainData.TopAZ.Histeresis;
            InputData.TopPS.Histeresis = MainData.TopPS.Histeresis;
            InputData.BottomPS.Histeresis =MainData.BottomPS.Histeresis;
            InputData.BottomAZ.Histeresis = MainData.BottomAZ.Histeresis;
            

            InputData.TopAZ.IsSet=MainData.TopAZ.IsSet;
            InputData.TopPS.IsSet=MainData.TopPS.IsSet;
            InputData.BottomPS.IsSet=MainData.BottomPS.IsSet;
            InputData.BottomAZ.IsSet = MainData.BottomAZ.IsSet;

            InputData.TopAZ.SettingSetter=MainData.TopAZ.SettingSetter;
            InputData.TopPS.SettingSetter=MainData.TopPS.SettingSetter;
            InputData.BottomPS.SettingSetter = MainData.BottomPS.SettingSetter;
            InputData.BottomAZ.SettingSetter = MainData.BottomAZ.SettingSetter;
        }
        private int[] FromByteToBitArray(byte value)
        {
            int[] arr = new int[8];
            int remain=value;
            for (int i = 0; i < 8; i++)
            {
                if(remain%2==0)
                    arr[i] = 0;
                else arr[i] = 1;
                remain= remain/2;
            }
            return arr;
        }
        private Visibility DisableScrloll(bool state)
        {
            return state ? Visibility.Visible : Visibility.Hidden;
        }
        private SolidColorBrush ProgressBarColorChange(byte state)
        {
            switch (state)
            {
                case 1:case 3:
                    return _redColor;
                case 2:
                    return _yellowColor;
                default: return _greenColor;
            }
        }
        public void StopThread()
        {
            threadCloser = false;
            channel.Stop();
            channel.Close();
        }
    }
}
