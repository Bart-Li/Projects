using System.IO;

namespace Eqi.Core.Serialization
{
    public interface ISerializer
    {
        #region Json

        /// <summary>
        /// Serialize object to JSON.
        /// </summary>
        /// <param name="value">object instance.</param>
        /// <returns>Json string.</returns>
        string SerializeToJson(object value);

        /// <summary>
        /// Deserialize JSON string to object.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="value">Json string.</param>
        /// <returns>Object instance.</returns>
        T DeserializeJsonToObject<T>(string value);

        /// <summary>
        /// Deserialize JSON string to object.
        /// </summary>
        /// <param name="value">Json string.</param>
        /// <returns>Object instance.</returns>
        object DeserializeJsonToObject(string value);

        /// <summary>
        /// Deserialize data from json file.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="filePath">File path.</param>
        /// <returns>Object intance.</returns>
        T DeserializeJsonFromFile<T>(string filePath);

        #endregion

        #region Xml

        /// <summary>
        /// Serialize object to XML.
        /// </summary>
        /// <param name="value">object instance.</param>
        /// <returns>Xml string.</returns>
        string SerializeToXml<T>(T value);

        /// <summary>
        /// Serialize object to XML.
        /// </summary>
        /// <param name="value">Object instance.</param>
        /// <param name="stream">Object stream.</param>
        void SerializeToXmlStream<T>(T value, Stream stream);

        /// <summary>
        /// Deserialize xml string to object.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="value">Xml string.</param>
        /// <returns>Object instance.</returns>
        T DeserializeXmlToObject<T>(string value);

        /// <summary>
        /// Deserialize data from XML stream.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="stream">Stream data.</param>
        /// <returns>Object intance..</returns>
        T DeserializeXmlStreamToObject<T>(Stream stream);

        /// <summary>
        /// Deserialize data from xml file.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="filePath">File path.</param>
        /// <returns>Object intance.</returns>
        T DeserializeXmlFromFile<T>(string filePath);

        /// <summary>
        /// Deserialize text data from file.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <returns>File text.</returns>
        string DeserializeTextFromFile(string filePath);

        #endregion
    }
}
