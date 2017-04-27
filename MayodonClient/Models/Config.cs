using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace MayodonClient.Models
{
    public class Config
    {
        public Config()
        {
            Clients = new ObservableCollection<Client>();
        }

        const string FileName = "mayodon.xml";

        public static Config Load()
        {
            try
            {
                var serializer = new XmlSerializer(typeof(Config));
                using (var stream = new FileStream(FileName, FileMode.Open))
                    return (Config)serializer.Deserialize(stream);
            }
            catch (IOException)
            {
                return new Config();
            }
        }

        public void Save()
        {
            var serializer = new XmlSerializer(typeof(Config));
            using (var stream = new FileStream(FileName, FileMode.Create))
                serializer.Serialize(stream, this);
        }

        public ObservableCollection<Client> Clients { get; set; }
    }
}
