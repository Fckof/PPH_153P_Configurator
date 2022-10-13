using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PPH_153P_Configurator.ViewModel
{
    enum SettingType
    {
        TopAZ,
        BottomAZ,
        TopPS,
        BottomPS
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
        private int[] _signalRange = new int[2];
        private double _averaging;
        private string myVar;
        


    }
}
