using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PPH_153P_Configurator
{
    
    public class Controller:ObservableObject
    {
        public DataModel MainData { get; private set; }
        public DataModel SecondaryData { get; private set; }
        Can channel;
        private int myVar;

        public int MyProperty
        {
            get { return myVar; }
            set { myVar = value;
                OnPropertyChanged("MyProperty");
            }
        }

        public Controller()
        {
            channel = new Can(0);
            channel.SetBaud(CanBaudRate.BCI_1M);

            MainData = new DataModel();
            SecondaryData= new DataModel();
        }
    }
}
