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
        private byte[] saveReq = { 115, 97, 118, 101 };
        private SolidColorBrush _colorChange;
        private SolidColorBrush _redColor= new SolidColorBrush(Color.FromRgb(226, 22, 12));
        private SolidColorBrush _yellowColor = new SolidColorBrush(Color.FromRgb(249, 240, 12));
        private SolidColorBrush _greenColor = new SolidColorBrush(Color.FromRgb(6, 176, 37));
        private CanMessage[] requestsArray;
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
            thread = new Thread(() => RecieveCanMessage());
            thread.Start();
        }
        private void RecieveCanMessage()
        {
            while (threadCloser)
            {
                ParseData(channel.ReadAll());
                CompareReauquests();
                RequestData(requestsArray);
            }
        }
        private void RequestData(CanMessage[] reqs)
        {
            foreach(var item in reqs)
            {
                channel.Write(item);
            }
            
        }
       
        private void ParseData(CanMessage[] arr)
        {
            
            foreach (var item in arr)
            {
                uint nodeId = arr[0].Id & 0x7F;
                uint msgId = item.Id & 0xffffff80;
                MainData.NodeId= (int)nodeId;
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
        private void CompareReauquests()
        {
            requestsArray = new CanMessage[] { 
                new CanMessage { Id = (uint)(0x600 + MainData.NodeId), Data = new byte[] { 0x40, 0x21, 0x61, 0x1 },Size=0x4 },
                new CanMessage { Id = (uint)(0x600 + MainData.NodeId), Data = new byte[] { 0x40, 0x23, 0x61, 0x1 },Size=0x4 },
                new CanMessage { Id = (uint)(0x600 + MainData.NodeId), Data = new byte[] { 0x40, 0xA1, 0x61, 0x1 },Size=0x4 },
                new CanMessage { Id = (uint)(0x600 + MainData.NodeId), Data = new byte[] { 0x40, 0x0B, 0x65, 0x1 },Size=0x4 }
            };
        }
        private void DefineObjectViaFunctionCode(CanMessage mes)
        {
            if (mes.Data[0] == 0x42)
            {
                var target = BitConverter.ToInt16(mes.Data, 1);
                //throw new Exception();
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
                        MainData.TopAZ.Histeresis = BitConverter.ToSingle(mes.Data, 4);
                        break;
                }
                //throw new Exception();

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
        private SolidColorBrush ProgressBarColorChange(byte state)
        {
            switch (state)
            {
                case 1:
                    return _redColor;
                case 2:
                    return _yellowColor;
                default: return _greenColor;
            }
        }
        public void StopThread()
        {
            threadCloser = false;
        }
    }
}
