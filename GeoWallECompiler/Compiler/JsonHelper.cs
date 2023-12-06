using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Xml.Serialization;

public static class JsonHelper
{
    public static void SerializeObject<T>(T obj, string filename, string path)
    {
        //string fullPath = Path.Combine(path, filename);
        string json = JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }

    public static T DeserializeObject<T>(string filename)
    {
        if (!File.Exists(filename))
        {
            throw new FileNotFoundException("The specified file does not exist.", filename);
        }

        string json = File.ReadAllText(filename);
        T obj = JsonSerializer.Deserialize<T>(json);
        return obj;
    }
}