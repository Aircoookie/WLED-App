using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace WLED
{
    /*
     *  Source: https://stackoverflow.com/questions/2434534/serialize-an-object-to-string
     */

    static class Serialization
    {
        public static string SerializeObject<T>(T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
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
