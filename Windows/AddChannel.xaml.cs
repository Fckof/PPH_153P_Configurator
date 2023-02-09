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
    public enum Type
    {
        Channel,
        Preset
    }
    /// <summary>
    /// Логика взаимодействия для EnterPresetName.xaml
    /// </summary>
    public partial class AddChannel : Window
    {
        public AddChannel()
        {
            InitializeComponent();
        }
        public string InputName { get { return config.Text.Trim(' ').Replace(" ", "_"); } }
        public ChannelsCollection Collection { get; set; }

        //Проверка доступности имени канала
        private bool IsChannelNameFree(string text)
        {
            bool result = true;
            foreach(var channel in Collection.Channels)
            {
                if(channel.ChannelName == text) result = false;
            }
            return result;
        }

        private void SubmitName(object sender, RoutedEventArgs e)
        {
            if (IsChannelNameFree(InputName) && InputName.Length != 0)
            {
                var chan = new Channel()
                {
                    ChannelName = InputName
                };
                Collection.Channels.Add(chan);
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
