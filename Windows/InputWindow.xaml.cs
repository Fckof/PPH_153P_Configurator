using System;
using System.Collections.Generic;
using System.IO;
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

namespace PPH_153P_Configurator.Windows
{
    /// <summary>
    /// Логика взаимодействия для InputWindow.xaml
    /// </summary>
    public partial class InputWindow : Window
    {
        public InputWindow()
        {
            InitializeComponent();
        }
        public string InputName { get { return config.Text.Trim(' ').ToLower().Replace(" ", "_"); } }
        public string FileName { get; set; }
        private void SubmitName(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(InputName + ".xml") && InputName.Length != 0)
            {
                FileName = InputName+".xml";
                this.DialogResult = true;
            }
            else
            {
                string message = App.Language.Name == "ru-RU" ? "Имя недоступно" : "Name not available";
                MessageBox.Show(message);
            }
        }
    }
}
