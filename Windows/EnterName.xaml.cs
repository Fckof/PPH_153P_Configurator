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
    public partial class EnterName : Window
    {
        public EnterName(Type type)
        {
            InitializeComponent();
            this.type = type;
            Preset = new Preset();
        }
        private Type type { get; set; }
        public string InputName { get { return config.Text.Trim(' ').ToLower().Replace(" ", "_"); } }
        public ChannelsCollection Collection { get; set; }
        public Channel Channel { get; set; }
        public Preset Preset { get; set; }
         
        //Если имя канала не занято создает новый канал
        private bool IsChannelNameFree(string text)
        {
            bool result = true;
            foreach(var channel in Collection.Channels)
            {
                if(channel.ChannelName == text) result = false;
            }
            return result;
        }
        private bool IsPresetNameFree(string text)
        {
            bool result = true;
            foreach (var channel in Channel.Presets)
            {
                if (channel.Name == text) result = false;
            }
            return result;
        }

        private void SubmitName(object sender, RoutedEventArgs e)
        {
            switch (type)
            {
                case Type.Channel:
                    if (IsChannelNameFree(InputName) && InputName.Length!=0)
                    {
                        Channel = new Channel()
                        {
                            ChannelName = InputName
                        };
                        Collection.Channels.Add(Channel);
                    }
                    else
                    {
                        MessageBox.Show("Имя недоступно");
                    }
                    break;
                case Type.Preset:
                    if (IsPresetNameFree(InputName))
                    {
                        Preset.Name= InputName;
                        Channel.Presets.Add(Preset);
                    }
                    else
                    {
                        MessageBox.Show("Имя недоступно");
                    }
                    break;
            }
            this.DialogResult = true;
        }
    }
}
