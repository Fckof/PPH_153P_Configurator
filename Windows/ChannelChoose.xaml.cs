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

namespace PPH_153P_Configurator.Windows
{
    /// <summary>
    /// Логика взаимодействия для ChannelChoose.xaml
    /// </summary>
    public partial class ChannelChoose : Window
    {
        public ChannelChoose(ChannelsCollection items, DataModel model)
        {
            InitializeComponent();
            preset=new Preset();
            Copier.CopyValues(preset, model);
            DisplayChannelList(ChannelLst,items);
        }
        private string InputName { get { return config.Text.Trim(' ').Replace(" ", "_"); } }
        private Preset preset { get; set; }
        private bool IsPresetNameFree(string text, Channel chan)
        {
            bool result = true;
            foreach (var channel in chan.Presets)
            {
                if (channel.Name == text) result = false;
            }
            return result;
        }
        private void AddChannelToListView(Channel channel, ListView target)
        {
            ListViewItem item = new ListViewItem();
            item.Content = channel.ChannelName;
            item.Tag = channel;
            target.Items.Add(item);
        }
        private void DisplayChannelList(ListView view, ChannelsCollection ChansList)
        {
            if (ChansList != null)
            {
                view.Items.Clear();
                foreach (var cfg in ChansList.Channels)
                {
                    AddChannelToListView(cfg, view);
                }
            }

        }

        private void AddPreset(object sender, RoutedEventArgs e)
        {
            if (ChannelLst.SelectedItems.Count == 1)
            {
                var chan = (Channel)ChannelLst.SelectedItems.Cast<ListViewItem>().First().Tag;

                if (IsPresetNameFree(InputName, chan) && InputName.Length != 0)
                {
                    preset.Name = InputName;
                    chan.Presets.Add(preset);
                    DialogResult = true;
                }
                else MessageBox.Show("Имя недоступно");
            }
            else MessageBox.Show("Канал не выбран");
        }
    }
}
