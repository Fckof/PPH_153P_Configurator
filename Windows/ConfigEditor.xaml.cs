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
    /// Логика взаимодействия для ConfigEditor.xaml
    /// </summary>
    public partial class ConfigEditor : Window
    {
        public DataModel Model { get; private set; }
        public ConfigEditor()
        {
            InitializeComponent();
            Model = new DataModel();
            this.DataContext = Model;
            pathToPresets = "Presets.xml";
            DisplayChannelList(ChannelLst, pathToPresets);
            
        }
        
        string pathToPresets;
        private void AddPresetToListView(Preset preset, ListView target)
        {
            ListViewItem item = new ListViewItem();
            item.Content = $"{preset.Name}";
            item.Tag = preset;
            target.Items.Add(item);
        }
        private void AddChannelToListView(Channel channel, ListView target)
        {
            ListViewItem item = new ListViewItem();
            item.Content = channel.ChannelName;
            item.Tag = channel;
            target.Items.Add(item);
        }
        //Вспомогательные функции

        //Выводят список каналов/конфигов в заданный listview
        private void DisplayConfigList(ListView view, Channel configs)
        {
            chName.Text = configs.ChannelName;
            if (configs != null) view.Items.Clear();
            foreach (var cfg in configs.Presets)
            {
                AddPresetToListView(cfg, view);
            }
        }
        private void DisplayChannelList(ListView view, string path)
        {
            try
            {
                ChannelsCollection list = XML.DeserializeXML(path);
                if (list != null) view.Items.Clear();
                foreach (var cfg in list.Channels)
                {
                    AddChannelToListView(cfg, view);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Конфигурационный файл не найден или имеет некорректный формат");
            }
        }
        //

        //Вывод списка конфигов выбранного канала
        private void DisplayChannel(object sender, SelectionChangedEventArgs e)
        {
            var df = (DataModel)this.DataContext;
            if (ChannelLst.SelectedItems.Count == 1)
            {
                Channel cfg = (Channel)ChannelLst.SelectedItems.Cast<ListViewItem>().First().Tag;
                chName.Text = cfg.ChannelName;
                DisplayConfigList(PresetLst, cfg);
            }
        }

        //Отображение значений конфигурации в полях ввода
        private void DisplayConfig(object sender, SelectionChangedEventArgs e)
        {
            if (PresetLst.SelectedItems.Count == 1)
            {
                Preset cfg = (Preset)PresetLst.SelectedItems.Cast<ListViewItem>().First().Tag;
                Copier.CopyValues(Model, cfg);
            }
        }

        private void CheckFloatNumberInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (!(Char.IsDigit(e.Text, 0) || (e.Text == ".")
               && (!textBox.Text.Contains(".")
               && textBox.Text.Length != 0)))
            {
                e.Handled = true;
            }
        }

        //Проверка ввода целого числа
        private void CheckIntNumberInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        //Перенаправление фокуса с некоторых элементов
        private void RedirectFocus(object sender, MouseButtonEventArgs e)
        {
            MainGrid.Focus();
            PresetLst.SelectedItem = null;
            ChannelLst.SelectedItem = null;
        }

        //Если поля ввода пусты выводит 0
        private void CheckEmptyInput(object sender, RoutedEventArgs e)
        {
            var textbox = (TextBox)sender;
            //Regex template = new Regex(@"(\d*\.?\d*){0}?");
            string cleanText = textbox.Text.Replace(" ", string.Empty);
            textbox.Text = cleanText.Length != 0 ? cleanText : "0";
        }
        private void InitChannels(ChannelsCollection items, ComboBox list)
        {
            if (items != null)
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
            else chans = new ChannelsCollection();
        }
        //public string PresetName { get { return config.Text.Trim(' ').ToLower().Replace(" ", "_"); } }
        //public string ChannelName { get { return channel.Text.Trim(' ').ToLower().Replace(" ", "_"); } }
        public ChannelsCollection chans { get; set; }
        public Channel chn { get; set; }
        public Preset prest { get; set; }

        private void ResetFields(object sender, RoutedEventArgs e)
        {
            Copier.SetToNull(Model);
        }
    }
}
