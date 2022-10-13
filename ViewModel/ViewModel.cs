using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPH_153P_Configurator.ViewModel
{
   
    class ViewModel: ObservableObject
    {
        private string myVar;
        
        public string MyProperty
        {
            get { return myVar; }
            set
            {
                myVar = value;
                OnPropertyChanged("MyProperty");
            }
        }
    }
}
