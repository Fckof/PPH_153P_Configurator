using Microsoft.Win32;
using PPH_153P_Configurator.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace PPH_153P_Configurator
{
    /// <summary>
    /// Логика взаимодействия для ConfigEditor.xaml
    /// </summary>
    public partial class ConfigEditor : Window
    {
        public DataModel Model { get; private set; }
        public ChannelsCollection ChansList { get; set; }
        public ConfigEditor(string filePath)
        {
            InitializeComponent();
            Model = new DataModel();
            this.DataContext = Model;
            pathToPresets = filePath;
            ChansList= GetChannelsCollection(pathToPresets);
            DisplayChannelList(ChannelLst, ChansList);
        }
        public string pathToPresets { get; set; }
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
            if (configs != null) view.Items.Clear();
            foreach (var cfg in configs.Presets)
            {
                AddPresetToListView(cfg, view);
            }
        }

        //Возвращает коллекцию настроек
        private ChannelsCollection GetChannelsCollection(string path)
        {
            try
            {
                return XML.DeserializeXML(path);
            }
            catch
            {
                return null;
            }

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
            else
            {
                view.Items.Clear();
            }
                
        }
        //

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
        }

        //Если поля ввода пусты выводит 0
        private void CheckEmptyInput(object sender, RoutedEventArgs e)
        {
            var textbox = (TextBox)sender;
            string cleanText = textbox.Text.Replace(" ", string.Empty);
            textbox.Text = cleanText.Length != 0 ? cleanText : "0";
        }
        private void CheckEmptyStringInput(object sender, RoutedEventArgs e)
        {
            var textbox = (TextBox)sender;
            string cleanText = textbox.Text.Trim(' ').Replace(" ", "_"); ;
            if(cleanText.Length == 0 )
            {
                textbox.Text = GetLviName(textbox.Name);
            }
            else if(GetLviName(textbox.Name).Length!=0 && cleanText.Length != 0) textbox.Text = cleanText;
            else textbox.Text = string.Empty;
        }
        private string GetLviName(string name)
        {
            switch (name)
            {
                case "chName":
                    return ChannelLst.SelectedItems.Count == 1 ? ((Channel)ChannelLst.SelectedItems.Cast<ListViewItem>().First().Tag).ChannelName : string.Empty;
                case "cfgName":
                    return PresetLst.SelectedItems.Count == 1 ? ((Preset)PresetLst.SelectedItems.Cast<ListViewItem>().First().Tag).Name : string.Empty;
            }
            return string.Empty;
        }
        private void ResetFields(object sender, RoutedEventArgs e)
        {
            Copier.ClearModelValues(Model);
        }

        private void DisplayChannelName(object sender, SelectionChangedEventArgs e)
        {
            var item = (Channel)ChannelLst.SelectedItems.Cast<ListViewItem>().First().Tag;
            chName.Text = item.ChannelName;
        }
        private void DisplayPresetName(object sender, SelectionChangedEventArgs e)
        {
            var item = (Preset)PresetLst.SelectedItems.Cast<ListViewItem>().First().Tag;
            cfgName.Text = item.Name;
        }

        //Отображение значений конфигурации в полях ввода
        private void DisplayConfig(object sender, MouseButtonEventArgs e)
        {
            if (PresetLst.SelectedItems.Count == 1)
            {
                Preset cfg = (Preset)PresetLst.SelectedItems.Cast<ListViewItem>().First().Tag;
                Copier.CopyValues(Model, cfg);
                cfgName.Text = cfg.Name;
            }
        }
        private void DisplayConfig(object sender, SelectionChangedEventArgs e)
        {
            if (PresetLst.SelectedItems.Count == 1)
            {
                Preset cfg = (Preset)PresetLst.SelectedItems.Cast<ListViewItem>().First().Tag;
                Copier.CopyValues(Model, cfg);
                cfgName.Text = cfg.Name;
            }
        }
        //

        //Вывод списка конфигов выбранного канала
        private void DisplayChannel(object sender, SelectionChangedEventArgs e)
        {
            if (ChannelLst.SelectedItems.Count == 1)
            {
                Channel cfg = (Channel)ChannelLst.SelectedItems.Cast<ListViewItem>().First().Tag;
                DisplayConfigList(PresetLst, cfg);
                chName.Text = cfg.ChannelName;
                cfgName.Text = "";
            }
            else
            {
                ClearNameInputs();
                PresetLst.Items.Clear();
            }
        }

        private void DisplayChannel(object sender, MouseButtonEventArgs e)
        {
            if (ChannelLst.SelectedItems.Count == 1)
            {
                Channel cfg = (Channel)ChannelLst.SelectedItems.Cast<ListViewItem>().First().Tag;
                DisplayConfigList(PresetLst, cfg);
                chName.Text = cfg.ChannelName;
                cfgName.Text = "";
            }
        }
        //
        private void ClearNameInputs()
        {
            chName.Text = String.Empty;
            cfgName.Text = String.Empty;
        }
        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            if(ChannelLst.SelectedItems.Count == 1)
            {
                Channel channel = (Channel)ChannelLst.SelectedItems.Cast<ListViewItem>().First().Tag;
                channel.ChannelName=chName.Text;

                if (PresetLst.SelectedItems.Count == 1)
                {
                    Preset preset = (Preset)PresetLst.SelectedItems.Cast<ListViewItem>().First().Tag;
                    preset.Name = cfgName.Text;
                    Copier.CopyValues(preset, Model);
                }
                DisplayConfigList(PresetLst, channel);
            }
            else MessageBox.Show("Для сохранения значений выберите элемент из списка");

            DisplayChannelList(ChannelLst, ChansList);
            ClearNameInputs();
        }

        private void AddNewConfig(object sender, RoutedEventArgs e)
        {
                ChannelChoose form = new ChannelChoose(ChansList,Model);
                form.ShowDialog();
            if (form.DialogResult == true)
            {
                DisplayChannelList(ChannelLst, ChansList);
                PresetLst.Items.Clear();
            }
        }

        private void SaveConfigFile(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(pathToPresets, string.Empty);
            XML.SerializeXML(ChansList, pathToPresets);
            ChansList = GetChannelsCollection(pathToPresets);
            DisplayChannelList(ChannelLst, ChansList);
            PresetLst.Items.Clear();
            PromptWindow prompt = new PromptWindow($"Файл успешно сохранён!", 1000);
            prompt.ShowDialog();
        }

        private void AddNewChannel(object sender, RoutedEventArgs e)
        {
            AddChannel form = new AddChannel();
            form.Collection = ChansList;
            form.ShowDialog();
            if (form.DialogResult == true)
            {
                DisplayChannelList(ChannelLst,ChansList);
                PresetLst.Items.Clear();
            }
        }

        private void DeleteConfig(object sender, RoutedEventArgs e)
        {

            if (PresetLst.SelectedItems.Count == 1)
            {
                var channel = (Channel)ChannelLst.SelectedItems.Cast<ListViewItem>().First().Tag;
                var preset = (Preset)PresetLst.SelectedItems.Cast<ListViewItem>().First().Tag;
                var result = MessageBox.Show($"Вы уверены, что хотите удалить настройку <{preset.Name}>","Удаление",MessageBoxButton.OKCancel,MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    if (channel.Presets.Remove(preset))
                        DisplayConfigList(PresetLst, channel);
                    else MessageBox.Show("Ошибка удаления");
                    
                }
               
            }
            else MessageBox.Show("Выберите настройку для удаления");
        }
        private void DeleteChannel(object sender, RoutedEventArgs e)
        {
            if (ChannelLst.SelectedItems.Count == 1)
            {
                var channel = (Channel)ChannelLst.SelectedItems.Cast<ListViewItem>().First().Tag;
                var result = MessageBox.Show($"Вы уверены, что хотите удалить канал <{channel.ChannelName}>", "Удаление", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    if (ChansList.Channels.Remove(channel))
                    {
                        DisplayChannelList(ChannelLst, ChansList);
                        PresetLst.Items.Clear();
                    }
                    else MessageBox.Show("Ошибка удаления");
                }

            }
            else MessageBox.Show("Выберите канал для удаления");
        }
        private void OpenFromFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                pathToPresets = dialog.SafeFileName;
                ChansList =GetChannelsCollection(pathToPresets);
                XML.SerializeXML(pathToPresets, Global.defaultSettingsPath);
                DisplayChannelList(ChannelLst, ChansList);
                PresetLst.Items.Clear();
            }
        }

        private void CreateConfigFile(object sender, RoutedEventArgs e)
        {
            InputWindow inputWindow = new InputWindow();
            inputWindow.ShowDialog();
            if (inputWindow.DialogResult == true)
            {
                pathToPresets = inputWindow.FileName;
                (File.Create(pathToPresets)).Close();
                XML.SerializeXML(ChansList, pathToPresets);
                XML.SerializeXML(pathToPresets,Global.defaultSettingsPath);
                PromptWindow prompt = new PromptWindow($"Файл <{inputWindow.FileName}> успешно создан!", 1000);
                prompt.ShowDialog();
            }

        }
    }
}
