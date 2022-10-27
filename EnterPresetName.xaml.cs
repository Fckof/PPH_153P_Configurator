using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PPH_153P_Configurator
{
    /// <summary>
    /// Логика взаимодействия для EnterPresetName.xaml
    /// </summary>
    public partial class EnterPresetName : Window
    {
        public EnterPresetName()
        {
            InitializeComponent();
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (InputName.Text.Length != 0)
            {
                this.Close();
            }
        }
    }
}
