using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace PPH_153P_Configurator
{
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            pathToPresets = "Presets.xml";
            DisplayConfigList(PresetLst, pathToPresets);
            
        }
        string pathToPresets;
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
        private void CheckIntNumberInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }
        private void RedirectFocus(object sender, MouseButtonEventArgs e)
        {
            MainGrid.Focus();
            PresetLst.SelectedItem = null;
        }

        private void MainWindow1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (this.DataContext as Controller).StopThread();
        }

        private void MainWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            var controller = (Controller)this.DataContext;
            Copier.CopyValues(controller.InputData, controller.MainData);
        }


        private void ButtonClickRefreshInputData(object sender, RoutedEventArgs e)
        {
            var controller = (Controller)this.DataContext;
            Copier.CopyValues(controller.InputData, controller.MainData);
        }

        private void ButtonClickSendData(object sender, RoutedEventArgs e)
        {
            var controller = (Controller)this.DataContext;
            controller.SendData(controller.CompareDataToSend(controller.InputData, controller.MainData));
        }

        private void OpenFromFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                DisplayConfigList(PresetLst,dialog.FileName);
            }
        }
        private void AddPreset(Preset preset, ListView target)
        {
            ListViewItem item = new ListViewItem();
            item.Content = $"{preset.PresetName}";
            item.Tag = preset;
            target.Items.Add(item);
        }
        private void SerializeXML(PresetsCollection items, string path)
        {
            XmlSerializer xml = new XmlSerializer(typeof(PresetsCollection));
            using(FileStream file = new FileStream(path, FileMode.OpenOrCreate))
            {
                xml.Serialize(file, items);
            }
        }
        private PresetsCollection DeserializeXML(string path)
        {
            XmlSerializer xml = new XmlSerializer(typeof(PresetsCollection));
            using (FileStream file = new FileStream(path, FileMode.OpenOrCreate))
            {
                return (PresetsCollection)xml.Deserialize(file);
            }
        }
        private void DisplayConfigList(ListView view, string path)
        {
            try
            {
                PresetsCollection list = DeserializeXML(path);
                if(list != null) view.Items.Clear();
                foreach (var cfg in list.Presets)
                {
                    AddPreset(cfg, view);
                }
            }catch (Exception ex)
            {
                MessageBox.Show("Конфигурационный файл не найден или имеет некорректный формат");
            }
            

        }
        private void CallEnterNameForm(object sender, RoutedEventArgs e)
        {
            EnterPresetName name = new EnterPresetName();
            name.Owner = this;
            name.Submit.Click += SaveConfig;
            name.Show();
        }
        private void SaveConfig(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string name = btn.Tag.ToString();
            if (name.Length != 0)
            {
                var ctrl = (Controller)this.DataContext;
                Preset newCfg = new Preset();
                newCfg.PresetName = name+" - "+DateTime.Now;
                Copier.CopyValues(newCfg, ctrl.InputData);
                AddPreset(newCfg, PresetLst);
                PresetsCollection collection = new PresetsCollection();
                foreach (ListViewItem item in PresetLst.Items)
                {
                    if (item.Tag != null)
                        collection.Presets.Add((Preset)item.Tag);

                }
                SerializeXML(collection, pathToPresets);
                
            }else MessageBox.Show("Конфигурация не может быть сохранена под пустым названием");

        }

        private void DisplayConfig(object sender, SelectionChangedEventArgs e)
        {
            var ctrl = (Controller)this.DataContext;

            if (PresetLst.SelectedItems.Count == 1)
            {
                Preset cfg = (Preset)PresetLst.SelectedItems.Cast<ListViewItem>().First().Tag;
                Copier.CopyValues(ctrl.InputData,cfg);
            }
            
        }

        
    }
}
