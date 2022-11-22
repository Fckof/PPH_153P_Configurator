using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PPH_153P_Configurator
{
    public static class XML
    {
        public static void SerializeXML<T>(T items, string path)
        {
            XmlSerializer xml = new XmlSerializer(typeof(T));
            using (FileStream file = new FileStream(path, FileMode.OpenOrCreate))
            {
                xml.Serialize(file, items);
            }
        }
        public static ChannelsCollection DeserializeXML(string path)
        {
            XmlSerializer xml = new XmlSerializer(typeof(ChannelsCollection));
            using (FileStream file = new FileStream(path, FileMode.OpenOrCreate))
            {
                return (ChannelsCollection)xml.Deserialize(file);
            }
        }
        public static string DeserializeSettingsXML(string path)
        {
            XmlSerializer xml = new XmlSerializer(typeof(string));
            using (FileStream file = new FileStream(path, FileMode.OpenOrCreate))
            {
                return (string)xml.Deserialize(file);
            }
        }
    }
}
