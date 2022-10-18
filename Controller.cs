using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Controls;
using System.Threading.Tasks;
namespace PPH_153P_Configurator
{
    
    public class Controller:ObservableObject
    {
        public DataModel MainData { get; private set; }
        public DataModel InputData { get; private set; }
        Can channel;
        

        public Controller()
        {
            channel = new Can(0);
            channel.SetBaud(CanBaudRate.BCI_1M);

            MainData = new DataModel();
            InputData= new DataModel();
            MainData.MinSignalRange = 0;
            MainData.MaxSignalRange = 120;
            MainData.TopAZ.Value = 90;
            MainData.TopPS.Value = 75;
            MainData.BottomPS.Value = 35;
            MainData.BottomAZ.Value = 15;
        }
        public void ProgressBarColorChange(object sender, Exception e)
        {
            var progress = (ProgressBar)sender;
            progress.Foreground = progress.Value>=MainData.TopAZ.Value ? new SolidColorBrush(Color.FromRgb())
        }
    }
}
