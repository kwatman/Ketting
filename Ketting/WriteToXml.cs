using System;
using System.IO;
using System.Xml.Serialization;

namespace Ketting
{
    public class WriteToXml
    {
        public void WriteXml(string path, string fileName, Object @object )
        {
            try
            {
                if (!File.Exists(Path.Combine(path, fileName)))
                {
                    Console.WriteLine("true"); // halen we er later uit
                    XmlSerializer serializer = new XmlSerializer(typeof(Block));
                    FileStream stream = File.OpenWrite(Path.Combine(path, fileName));
                    serializer.Serialize(stream, @object );
                    stream.Dispose();
                }
                else
                {
                    Console.WriteLine("false"); // halen we er later uit
                    XmlSerializer serializer = new XmlSerializer(typeof(Block));
                    StreamWriter stream = File.AppendText(Path.Combine(path, fileName));
                    serializer.Serialize(stream, @object );
                    stream.Dispose();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                // schrijf naar een log als we dit doen. 
            }
        }
        
        public void ReadXml(string path, string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Block));
            FileStream streamRead = File.OpenRead(path + fileName);
            var result = (Block)(serializer.Deserialize(streamRead));
        }
    }
}