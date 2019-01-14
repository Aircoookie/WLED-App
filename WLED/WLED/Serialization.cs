using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace WLED
{
    /*
     *  Source: https://stackoverflow.com/questions/2434534/serialize-an-object-to-string
     */

    //convert WLEDDevice list <-> string
    static class Serialization
    {
        public static string SerializeObject<T>(T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            XmlWriterSettings ws = new XmlWriterSettings();
            ws.NewLineHandling = NewLineHandling.None;
            ws.Indent = false;
            StringBuilder stringBuilder = new StringBuilder();
            using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, ws))
            {
                xmlSerializer.Serialize(xmlWriter, toSerialize);
                return stringBuilder.ToString();
            }
        }

        public static ObservableCollection<WLEDDevice> Deserialize(string toDeserialize)
        {
            System.Diagnostics.Debug.WriteLine(toDeserialize);

            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<WLEDDevice>));
                using (StringReader textReader = new StringReader(toDeserialize))
                {
                    return xmlSerializer.Deserialize(textReader) as ObservableCollection<WLEDDevice>;
                }
            }
            catch
            {     
                return null;
            }
        }
    }
}
