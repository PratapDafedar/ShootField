using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class MessageData
{
    public static byte[] ToByteArray(string msg)
    {
        // Create a memory stream, and serialize.
        MemoryStream stream = new MemoryStream();

        // Create a binary formatter.
        BinaryFormatter formatter = new BinaryFormatter();

        // Serialize.
        formatter.Serialize(stream, msg);

        // Now return the array.
        return stream.ToArray();
    }

    public static string ToStringArray(byte[] data)
    {
        // Create a memory stream, and serialize.
        MemoryStream stream = new MemoryStream(data);

        // Create a binary formatter.
        BinaryFormatter formatter = new BinaryFormatter();

        // Serialize.
        string msg = (string) formatter.Deserialize(stream);

        // Now return the array.
        return msg;
    }
}