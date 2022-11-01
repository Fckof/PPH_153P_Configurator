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
        private void InitChannels(ChannelsCollection items, ComboBox list)
        {
            if(items != null)
            {
                foreach (var channel in items.Channels)
                {
                    if (channel != null)
                    {
                        var item = new ComboBoxItem();
                        item.Content = channel.ChannelName;
                        item.Tag = channel;
                        list.Items.Add(item);
                    }
                }
            }
            else chans=new ChannelsCollection();
        }
        public string PresetName { get { return config.Text.Trim(' ').ToLower().Replace(" ", "_"); } }
        public string ChannelName { get { return channel.Text.Trim(' ').ToLower().Replace(" ","_"); } }
        public ChannelsCollection chans { get; set; }
        public Channel chn { get; set; }
        public Preset prest { get; set; }
        private bool IsNameFree(string text, ChannelsCollection chns)
        {
            bool result = true;
            foreach(var channel in chns.Channels)
            {
                if(channel.ChannelName == text) result = false;
            }
            return result;
        }
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (config.Text.Length != 0)
            {
                
                if((channelSel.SelectedIndex != -1 && channelSel.SelectedIndex != 0) && channel.Text.Length==0)
                {
                    chn=(Channel)((ComboBoxItem)channelSel.SelectedItem).Tag;
                    prest = new Preset();
                    prest.Name = PresetName;
                    MessageBox.Show($"Конфигурация {prest.Name} добавлена в канал: {chn.ChannelName}");
                    this.DialogResult = true;
                }
                else if(channel.Text.Length != 0)
                {
                    if (IsNameFree(ChannelName, chans))
                    {
                        chn=new Channel();
                        chn.ChannelName = ChannelName;
                        prest = new Preset();
                        prest.Name = PresetName;
                        MessageBox.Show($"Конфигурация {prest.Name} добавлена в новый канал: {chn.ChannelName}");
                        this.DialogResult = true;
                    }
                    else
                    {
                        MessageBox.Show($"Имя канала {ChannelName} уже занято");
                    }
                    
                }
            }
            else MessageBox.Show("Поля не должны быть пустыми");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitChannels(chans, channelSel);
        }
    }
}
