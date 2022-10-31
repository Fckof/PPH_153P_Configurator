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
using System.IO;

namespace PPH_153P_Configurator
{
    [Serializable]
    public class Controller:ObservableObject
    {
        public DataModel MainData { get; private set; }
        public DataModel InputData { get; private set; }
        public ChannelsCollection ChannelsList { get; private set; }
        public Channel PresetsList { get; private set; }

        private Can channel=new Can(0);
        private Thread thread;
        private bool threadCloser = true;
        private byte[] saveReq = { 0x22, 0x10, 0x10,0x01, 0x73, 0x61, 0x76, 0x65 };
        private SolidColorBrush _colorChange;
        private SolidColorBrush _redColor= new SolidColorBrush(Color.FromRgb(226, 22, 12));
        private SolidColorBrush _yellowColor = new SolidColorBrush(Color.FromRgb(249, 240, 12));
        private SolidColorBrush _greenColor = new SolidColorBrush(Color.FromRgb(6, 176, 37));
        
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
            PresetsList=new Channel();
            ColorChange = new SolidColorBrush(Color.FromRgb(6, 176, 37));
            thread = new Thread(RecieveCanMessage);
            thread.Start();
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
        public void SendData(CanMessage[] messages)
        {
            threadCloser = false;
            foreach (CanMessage message in messages)
            {
                channel.Write(message);
                ParseWriteErrors(channel.ReadAll());
            }
            threadCloser = true;
            thread = new Thread(RecieveCanMessage);
            thread.Start();
            Thread.Sleep(100);
            Copier.CopyValues(InputData,MainData);
        }
        public CanMessage[] CompareDataToSend(DataModel input, DataModel main)
        {
            byte[] writeMaxSignalRange = (new byte[] { 0x22, 0x23, 0x61, 0x1 }).Concat(BitConverter.GetBytes(input.MaxSignalRange)).ToArray();
            byte[] writeMinSignalRange = (new byte[] { 0x22, 0x21, 0x61, 0x1 }).Concat(BitConverter.GetBytes(input.MinSignalRange)).ToArray();
            byte[] writeAveraging = (new byte[] { 0x22, 0xA1, 0x61, 0x1 }).Concat(BitConverter.GetBytes(input.Averaging)).ToArray();
            byte[] writeNodeId = new byte[] { 0x22, 0x00, 0x20, 0x0, input.NodeId };

            byte[] writeTopAZHisteresis = (new byte[] { 0x22, 0x0B, 0x65, 0x1 }).Concat(BitConverter.GetBytes(Transform.ToAbsValue(input.TopAZ.Histeresis, InputData.MaxSignalRange, InputData.MinSignalRange))).ToArray();
            byte[] writeTopPSHisteresis = (new byte[] { 0x22, 0x1B, 0x65, 0x1 }).Concat(BitConverter.GetBytes(Transform.ToAbsValue(input.TopPS.Histeresis, InputData.MaxSignalRange, InputData.MinSignalRange))).ToArray();
            byte[] writeBottomPSHisteresis = (new byte[] { 0x22, 0x2B, 0x65, 0x1 }).Concat(BitConverter.GetBytes(Transform.ToAbsValue(input.BottomPS.Histeresis, InputData.MaxSignalRange, InputData.MinSignalRange))).ToArray();
            byte[] writeBottomAZHisteresis = (new byte[] { 0x22, 0x3B, 0x65, 0x1 }).Concat(BitConverter.GetBytes(Transform.ToAbsValue(input.BottomAZ.Histeresis, InputData.MaxSignalRange, InputData.MinSignalRange))).ToArray();

            byte[] writeTopAZIsSet = (new byte[] { 0x22, 0x08, 0x65, 0x1 }).Concat(BitConverter.GetBytes(input.TopAZ.isSetValue)).ToArray();
            byte[] writeTopPSIsSet = (new byte[] { 0x22, 0x18, 0x65, 0x1 }).Concat(BitConverter.GetBytes(input.TopPS.isSetValue)).ToArray();
            byte[] writeBottomPSIsSet = (new byte[] { 0x22, 0x28, 0x65, 0x1 }).Concat(BitConverter.GetBytes(input.BottomPS.isSetValue)).ToArray();
            byte[] writeBottomAZIsSet = (new byte[] { 0x22, 0x38, 0x65, 0x1 }).Concat(BitConverter.GetBytes(input.BottomAZ.isSetValue)).ToArray();

            byte[] writeTopAZSettingSetter = (new byte[] { 0x22, 0x0F, 0x65, 0x1 }).Concat(BitConverter.GetBytes(Convert.ToInt32(input.TopAZ.SettingSetter))).ToArray();
            byte[] writeTopPSSettingSetter = (new byte[] { 0x22, 0x1F, 0x65, 0x1 }).Concat(BitConverter.GetBytes(Convert.ToInt32(input.TopPS.SettingSetter))).ToArray();
            byte[] writeBottomPSSettingSetter = (new byte[] { 0x22, 0x2F, 0x65, 0x1 }).Concat(BitConverter.GetBytes(Convert.ToInt32(input.BottomPS.SettingSetter))).ToArray();
            byte[] writeBottomAZSettingSetter = (new byte[] { 0x22, 0x3F, 0x65, 0x1 }).Concat(BitConverter.GetBytes(Convert.ToInt32(input.BottomAZ.SettingSetter))).ToArray();

            byte[] writeTopAZValue = (new byte[] { 0x22, 0x0A, 0x65, 0x1 }).Concat(BitConverter.GetBytes(input.TopAZ.Value)).ToArray();
            byte[] writeTopPSValue = (new byte[] { 0x22, 0x1A, 0x65, 0x1 }).Concat(BitConverter.GetBytes(input.TopPS.Value)).ToArray();
            byte[] writeBottomPSValue = (new byte[] { 0x22, 0x2A, 0x65, 0x1 }).Concat(BitConverter.GetBytes(input.BottomPS.Value)).ToArray();
            byte[] writeBottomAZValue = (new byte[] { 0x22, 0x3A, 0x65, 0x1 }).Concat(BitConverter.GetBytes(input.BottomAZ.Value)).ToArray();


            return new CanMessage[] {
                new CanMessage { Id = 0x0, Data = new byte[] { 0x80, (byte)main.NodeId },Size=(byte)2 },
                new CanMessage { Id = (uint)(0x600 + main.NodeId), Data = writeNodeId, Size=(byte)writeNodeId.Length},
                new CanMessage { Id = (uint)(0x600 + main.NodeId), Data = writeMaxSignalRange, Size=(byte)writeMaxSignalRange.Length},
                new CanMessage { Id = (uint)(0x600 + main.NodeId), Data = writeMinSignalRange, Size=(byte)writeMinSignalRange.Length},
                new CanMessage { Id = (uint)(0x600 + main.NodeId), Data = writeAveraging, Size=(byte)writeAveraging.Length},

                new CanMessage { Id = (uint)(0x600 + main.NodeId), Data = writeTopAZHisteresis, Size=(byte)writeTopAZHisteresis.Length},
                new CanMessage { Id = (uint)(0x600 + main.NodeId), Data = writeTopPSHisteresis, Size=(byte)writeTopPSHisteresis.Length},
                new CanMessage { Id = (uint)(0x600 + main.NodeId), Data = writeBottomPSHisteresis, Size=(byte)writeBottomPSHisteresis.Length},
                new CanMessage { Id = (uint)(0x600 + main.NodeId), Data = writeBottomAZHisteresis, Size=(byte)writeBottomAZHisteresis.Length},

                new CanMessage { Id = (uint)(0x600 + main.NodeId), Data = writeTopAZIsSet, Size=(byte)writeTopAZIsSet.Length},
                new CanMessage { Id = (uint)(0x600 + main.NodeId), Data = writeTopPSIsSet, Size=(byte)writeTopPSIsSet.Length},
                new CanMessage { Id = (uint)(0x600 + main.NodeId), Data = writeBottomPSIsSet, Size=(byte)writeBottomPSIsSet.Length},
                new CanMessage { Id = (uint)(0x600 + main.NodeId), Data = writeBottomAZIsSet, Size=(byte)writeBottomAZIsSet.Length},

                new CanMessage { Id = (uint)(0x600 + main.NodeId), Data = writeTopAZSettingSetter, Size=(byte)writeTopAZSettingSetter.Length},
                new CanMessage { Id = (uint)(0x600 + main.NodeId), Data = writeTopPSSettingSetter, Size=(byte)writeTopPSSettingSetter.Length},
                new CanMessage { Id = (uint)(0x600 + main.NodeId), Data = writeBottomPSSettingSetter, Size=(byte)writeBottomPSSettingSetter.Length},
                new CanMessage { Id = (uint)(0x600 + main.NodeId), Data = writeBottomAZSettingSetter, Size=(byte)writeBottomAZSettingSetter.Length},

                new CanMessage { Id = (uint)(0x600 + main.NodeId), Data = writeTopAZValue, Size=(byte)writeTopAZValue.Length},
                new CanMessage { Id = (uint)(0x600 + main.NodeId), Data = writeTopPSValue, Size=(byte)writeTopPSValue.Length},
                new CanMessage { Id = (uint)(0x600 + main.NodeId), Data = writeBottomPSValue, Size=(byte)writeBottomPSValue.Length},
                new CanMessage { Id = (uint)(0x600 + main.NodeId), Data = writeBottomAZValue, Size=(byte)writeBottomAZValue.Length},
                
                new CanMessage { Id = (uint)(0x600 + main.NodeId), Data = saveReq, Size=(byte)saveReq.Length},
                new CanMessage { Id = 0x0, Data = new byte[] { 0x01, (byte)main.NodeId}, Size=(byte)2 }
            };
        }
        private void ParseWriteErrors(CanMessage[] err)
        {

            foreach (var item in err)
            {
                string index = $"{BitConverter.ToInt16(item.Data, 1):X}";
                switch (item.Data[0])
                {
                    case 0x80:
                        
                        var errorCode = $"{BitConverter.ToInt32(item.Data, 4):X}";

                        string error = DateTime.Now+ "\t| Index: " + index + " | ErrorCode: " + errorCode+"\n";
                        File.AppendAllText(@"errorsLog.txt", error);
                        break;
                    case 0x60:
                        string success = DateTime.Now + "\t| Object Index: " + index + " Written\n";
                        File.AppendAllText(@"writeLog.txt", success);
                        break;
                }
            }
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
                        MainData.TopAZ.Histeresis = Transform.ToPercent(BitConverter.ToSingle(mes.Data, 4), MainData.MaxSignalRange, MainData.MinSignalRange);
                        break;
                    case 0x651B:
                        MainData.TopPS.Histeresis = Transform.ToPercent(BitConverter.ToSingle(mes.Data, 4), MainData.MaxSignalRange, MainData.MinSignalRange);
                        break;
                    case 0x652B:
                        MainData.BottomPS.Histeresis = Transform.ToPercent(BitConverter.ToSingle(mes.Data, 4), MainData.MaxSignalRange, MainData.MinSignalRange);
                        break;
                    case 0x653B:
                        MainData.BottomAZ.Histeresis = Transform.ToPercent(BitConverter.ToSingle(mes.Data, 4), MainData.MaxSignalRange, MainData.MinSignalRange);
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
