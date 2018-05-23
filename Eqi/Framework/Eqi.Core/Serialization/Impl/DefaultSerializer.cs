using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Eqi.Core.Serialization.Impl
{
    [DependencyInjection(typeof(ISerializer))]
    public class DefaultSerializer : ISerializer
    {
        #region Json

        /// <summary>
        /// Serialize object to JSON.
        /// </summary>
        /// <param name="value">object instance.</param>
        /// <returns>Json string.</returns>
        public string SerializeToJson(object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        /// <summary>
        /// Deserialize JSON string to object.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="value">Json string.</param>
        /// <returns>Object instance.</returns>
        public T DeserializeJsonToObject<T>(string value)
        {
            if (typeof(T) == typeof(string))
            {
                return (T)((object)value);
            }

            return JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// Deserialize JSON string to object.
        /// </summary>
        /// <param name="value">Json string.</param>
        /// <returns>Object instance.</returns>
        public object DeserializeJsonToObject(string value)
        {
            return JsonConvert.DeserializeObject(value);
        }

        /// <summary>
        /// Deserialize data from json file.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="filePath">File path.</param>
        /// <returns>Object intance.</returns>
        public T DeserializeJsonFromFile<T>(string filePath)
        {
            var text = DeserializeTextFromFile(filePath);
            return JsonConvert.DeserializeObject<T>(text);
        }

        #endregion

        #region Xml

        /// <summary>
        /// Serialize object to XML.
        /// </summary>
        /// <param name="value">object instance.</param>
        /// <returns>Xml string.</returns>
        public string SerializeToXml<T>(T value)
        {
            using (var memory = new MemoryStream())
            {
                using (TextReader reader = new StreamReader(memory))
                {
                    SerializeToXmlStream(value, memory);
                    memory.Position = 0L;
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Serialize object to XML.
        /// </summary>
        /// <param name="value">Object instance.</param>
        /// <param name="stream">Object stream.</param>
        public void SerializeToXmlStream<T>(T value, Stream stream)
        {
            var serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stream, value);
        }

        /// <summary>
        /// Deserialize xml string to object.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="value">Xml string.</param>
        /// <returns>Object instance.</returns>
        public T DeserializeXmlToObject<T>(string value)
        {
            using (var memory = new MemoryStream())
            {
                using (TextWriter writer = new StreamWriter(memory))
                {
                    writer.Write(value);
                    writer.Flush();
                    return DeserializeXmlStreamToObject<T>(memory);
                }
            }
        }

        /// <summary>
        /// Deserialize data from XML stream.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="stream">Stream data.</param>
        /// <returns>Object intance.</returns>
        public T DeserializeXmlStreamToObject<T>(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var reader = new StreamReader(stream))
            {
                stream.Position = 0;
                reader.ReadToEnd();                
                return (T)serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Deserialize data from xml file.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="filePath">File path.</param>
        /// <returns>Object intance.</returns>
        public T DeserializeXmlFromFile<T>(string filePath)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return (T)serializer.Deserialize(stream);
            }
        }

        /// <summary>
        /// Deserialize text data from file.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <returns>File text.</returns>
        public string DeserializeTextFromFile(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (TextReader reader = new StreamReader(stream))
                {
                    stream.Position = 0L;
                    return reader.ReadToEnd();
                }
            }
        }

        #endregion
    }
}
