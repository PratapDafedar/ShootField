using UnityEngine;
using System.Collections;
using System.IO;

public static class FileUtil 
{
    public static void WriteAllBytes(string path, byte[] bytes)
    {
        FileStream fs;
        if (!File.Exists(path))
        {
            fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
            fs.Close();
        }

        fs = new FileStream(path, FileMode.Truncate, FileAccess.Write);
        // Writes a block of bytes to this stream using data from a byte array.
        fs.Write(bytes, 0, bytes.Length);
        // close file stream.
        fs.Close();
    }

    public static byte[] ReadAllBytes(string path)
    {
        FileStream fs;
        if (!File.Exists(path))
        {
            Debug.LogError("File doesn't exist at path :" + path);
            return null;
        }

        byte[] buff = null;
        fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        BinaryReader br = new BinaryReader(fs);
        long numBytes = new FileInfo(path).Length;
        buff = br.ReadBytes((int)numBytes);
        return buff;
    }

    public static void WriteAllString(string path, string data)
    {
        byte[] buff = System.Text.Encoding.ASCII.GetBytes(data);
        WriteAllBytes(path, buff);
    }

    public static string ReadString(string path)
    {
        byte[] buff = ReadAllBytes(path);
		if (buff == null)
			return null;
        return System.Text.Encoding.UTF8.GetString(buff);
    }
}
