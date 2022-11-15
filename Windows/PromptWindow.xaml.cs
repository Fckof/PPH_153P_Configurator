using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
    /// Логика взаимодействия для PromptWindow.xaml
    /// </summary>
    public partial class PromptWindow : Window
    {
        public PromptWindow(string message, double interval)
        {
            InitializeComponent();
            MessageTextBlock.Text = message;
            timer = new Timer(interval);
            timer.Elapsed += new ElapsedEventHandler(Time_Elapsed);
        }
        void Time_Elapsed(object sender,ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                this.Close();
            }), null);
            //this.DialogResult = true;
        }
        Timer timer;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }
    }
}
