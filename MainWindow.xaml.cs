﻿using Microsoft.Win32;
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
            collection = new ChannelsCollection();
            collection=DefineCollection(pathToPresets);
            DisplayChannelList(ChannelLst, pathToPresets);
            
        }
        ChannelsCollection collection;
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
                DisplayChannelList(ChannelLst,dialog.FileName);
            }
        }
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
        private void SerializeXML(ChannelsCollection items, string path)
        {
            XmlSerializer xml = new XmlSerializer(typeof(ChannelsCollection));
            using(FileStream file = new FileStream(path, FileMode.OpenOrCreate))
            {
                xml.Serialize(file, items);
            }
        }
        private ChannelsCollection DeserializeXML(string path)
        {
            XmlSerializer xml = new XmlSerializer(typeof(ChannelsCollection));
            using (FileStream file = new FileStream(path, FileMode.OpenOrCreate))
            {
                return (ChannelsCollection)xml.Deserialize(file);
            }
        }
        private void DisplayConfigList(ListView view, Channel configs)
        {
                if (configs != null) view.Items.Clear();
                foreach (var cfg in configs.Presets)
                {
                    AddPresetToListView(cfg, view);
                }
            
        }
        private ChannelsCollection DefineCollection( string path)
        {
            try
            {
                return DeserializeXML(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Конфигурационный файл не найден или имеет некорректный формат");
                return null;
            }
        }
        private void DisplayChannelList(ListView view, string path)
        {
            try
            {
                ChannelsCollection list = DeserializeXML(path);
                if (list != null) view.Items.Clear();
                foreach (var cfg in list.Channels)
                {
                    AddChannelToListView(cfg, view);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Конфигурационный файл не найден или имеет некорректный формат");
            }


        }
        private void CallEnterNameForm(object sender, RoutedEventArgs e)
        {
            EnterPresetName modal = new EnterPresetName();
            
            try
            {
                modal.chans= DeserializeXML(pathToPresets);
            }catch (Exception ex)
            {
                MessageBox.Show("Файл пуст или задан некорректно");
            }
            modal.ShowDialog();
            if (modal.DialogResult == true)
            {
                SaveConfig(modal.chn, modal.prest);
            }
        }
        private void SaveConfig( Channel channel, Preset preset)
        {
                var ctrl = (Controller)this.DataContext;
                bool toAdd=true;
                Copier.CopyValues(preset, ctrl.InputData);
                AddPresetToListView(preset, PresetLst);

                    foreach (ListViewItem item in PresetLst.Items)
                    {
                      if (item.Tag != null)
                            channel.Presets.Add((Preset)item.Tag);

                    }
            if (collection != null)
            {
                foreach(Channel ch in collection.Channels)
            {
                if (ch.ChannelName == channel.ChannelName)
                {
                    collection.Channels[collection.Channels.IndexOf(ch)] = channel;
                    toAdd = false;
                    break;
                }
            }
                if (toAdd)
            {
                collection.Channels.Add(channel);
            }

            }
            else
            {
                collection = new ChannelsCollection();
                collection.Channels.Add(channel);
            }
                
            

                      
                
                SerializeXML(collection, pathToPresets);
             
        }

        private void DisplayChannel(object sender, SelectionChangedEventArgs e)
        {
            if (ChannelLst.SelectedItems.Count == 1)
            {
                Channel cfg = (Channel)ChannelLst.SelectedItems.Cast<ListViewItem>().First().Tag;
                DisplayConfigList(PresetLst, cfg);
            }

        }
        private void DisplayConfig(object sender, SelectionChangedEventArgs e)
        {
            var ctrl = (Controller)this.DataContext;

            if (PresetLst.SelectedItems.Count == 1)
            {
                Preset cfg = (Preset)PresetLst.SelectedItems.Cast<ListViewItem>().First().Tag;
                Copier.CopyValues(ctrl.InputData, cfg);
            }

        }


    }
}
