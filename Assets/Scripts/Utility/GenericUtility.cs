using System;
using UnityEngine;
using System.Collections;
using System.Reflection;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class GenericUtility 
{
	public static IEnumerator WaitForTimeInSeconds(float waitDurationSeconds, System.Action<bool> m)
	{
		yield return new WaitForSeconds (waitDurationSeconds);
		m(true);
	}

	public static Color RandomColorRGB (this Color color)
	{
		return new Color (UnityEngine.Random.Range (0, 1),
		                  UnityEngine.Random.Range (0, 1),
		                  UnityEngine.Random.Range (0, 1));
	}

	public static Color RandomColorRGBA (this Color color)
	{
		return new Color (UnityEngine.Random.Range (0, 1),
		                  UnityEngine.Random.Range (0, 1),
		                  UnityEngine.Random.Range (0, 1),
		                  UnityEngine.Random.Range (0, 1));
	}

	public static string ColorToHex(Color color)
	{
		string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		return hex;
	}

	public static Color HexToColor(string hex)
	{
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);

		return new Color( (float)r / 255.0f, (float)g / 255.0f, (float)b / 255.0f);
	}

	public static T GetCopyOf<T>(this Component comp, T other, BindingFlags flags = BindingFlags.Default) where T : Component
	{
		Type type = comp.GetType();

		if (type != other.GetType()) return null; // type mis-match

		if (flags == BindingFlags.Default)
		{
			flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
		}
		PropertyInfo[] pinfos = type.GetProperties(flags);
		foreach (var pinfo in pinfos) {
			if (pinfo.CanWrite) {
				try {
					pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
				}
				catch {
					// In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
					//Debug.LogError ("NotImplementedException being thrown");
				} 
			}
		}
		FieldInfo[] finfos = type.GetFields(flags);
		foreach (var finfo in finfos) {
			finfo.SetValue(comp, finfo.GetValue(other));
		}
		return comp as T;
	}

	public static T AddComponent<T>(this GameObject go, T toAdd, BindingFlags flags = BindingFlags.Default) where T : Component
	{
		return go.AddComponent<T>().GetCopyOf(toAdd) as T;
	}
		
	public static T GetEnumTypeForString <T> (string text)
	{
		T defaultValue = default (T);
		if (!System.Enum.IsDefined(typeof(T), text))
		{
			return defaultValue;
		}
		else {
			return  (T) System.Enum.Parse(typeof(T), text, true);
		}
	}

    public static T DeepCopy<T>(T other)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, other);
            ms.Position = 0;
            return (T)formatter.Deserialize(ms);
        }
    }

    public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dtDateTime;
    }

	public static double UnixDateTimeStampToTime(DateTime unixDateTime)
	{
		var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		return (unixDateTime - epoch).TotalSeconds;
	}

    public static bool IsPortrait ()
    {
        return (Screen.height > Screen.width);
    }

    public static bool IsLandscape ()
    {
        return (!IsPortrait());
    }
}
