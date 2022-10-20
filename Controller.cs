using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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

        public Controller()
        {
            channel.Open(CanOpenFlag.Can11);
            channel.SetBaud(CanBaudRate.BCI_1M);
            
            channel.Start();
            MainData = new DataModel();
            InputData= new DataModel();
            thread = new Thread(() => RecieveCanMessage());
            thread.Start();
            MainData.MinSignalRange = 0;
            MainData.MaxSignalRange = 120;
            MainData.TopAZ.Value = 90;
            MainData.TopPS.Value = 75;
            MainData.BottomPS.Value = 35;
            MainData.BottomAZ.Value = 15;
        }
        private void RecieveCanMessage()
        {
            while (threadCloser)
            {
                ParseData(channel.ReadAll());
            }
            
        }
        private void ParseData(CanMessage[] arr)
        {
            foreach (var item in arr)
            {
                switch (item.Id)
                {
                    case 385:
                        MainData.Value = BitConverter.ToInt32(item.Data, 0);
                        MainData.InputSignalCode = BitConverter.ToInt16(item.Data,4);
                        MainData.DeviceStatus = item.Data[6];
                        MainData.StateBitArray = FromByteToBitArray(item.Data[6]);
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
        private void ProgressBarColorChange(object sender, Exception e)
        {
            var progress = (ProgressBar)sender;
            //progress.Foreground = progress.Value>=MainData.TopAZ.Value ? new SolidColorBrush(Color.FromRgb())
        }
        public void StopThread()
        {
            threadCloser = false;
        }
    }
}
