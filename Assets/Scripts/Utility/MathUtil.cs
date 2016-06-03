using UnityEngine;
using System.Collections;

public static class MathUtil
{
    #region VECTOR_OPERATION

    public static float DistanceXZ(Vector3 a, Vector3 b)
    {
        return Mathf.Sqrt((a.x - b.x) * (a.x - b.x) + (a.z - b.z) * (a.z - b.z));
    }

    public static float DistanceXZ(Vector3 a, Vector2 b)
    {
        return Mathf.Sqrt((a.x - b.x) * (a.x - b.x) + (a.z - b.y) * (a.z - b.y));
    }


    public static Vector3 DiffXZ(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x - b.x, a.y, a.z - b.z);
    }

    public static Vector3 DiffXZ_b(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x - b.x, b.y, a.z - b.z);
    }

    public static float Mod(float a)
    {
        return a > 0 ? a : -a;
    }

    public static Vector3 EaseLerpXZ(Vector3 from, Vector3 to, float t)
    {
        t = Mathf.Clamp01(t);
        t = t * t * t * (t * (6f * t - 15f) + 10f);
        t = 1 - t;
        return new Vector3(from.x + (to.x - from.x) * t, from.y, from.z + (to.z - from.z) * t);
    }

    public static Vector3 LerpXZ(Vector3 from, Vector3 to, float t)
    {
        t = Mathf.Clamp01(t);
        return new Vector3(from.x + (to.x - from.x) * t, from.y, from.z + (to.z - from.z) * t);
    }

    public static Vector3 LerpLinear(Vector3 from, Vector3 to, float t)
    {
        //return (1-t)*v0 + t*v1;
        t = Mathf.Clamp01(t);
        return new Vector3((1 - t) * from.x + t * to.x,
                           (1 - t) * from.y + t * to.y,
                           (1 - t) * from.z + t * to.z);
    }
    
    public static Vector3 ClampMagnitude(Vector3 vector, float minLength, float maxLength)
    {
        float mag = vector.sqrMagnitude;
        if (mag >= maxLength * maxLength)
        {
            return vector.normalized * maxLength;
        }
        else if (mag <= minLength * minLength)
        {
            return vector.normalized * minLength;
        }
        return vector;
    }

    public static Vector3 ClampMagnitude(Vector3 vector, float maxLength)
    {
        if (vector.sqrMagnitude > maxLength * maxLength)
        {
            return vector.normalized * maxLength;
        }
        return vector;
    }

    public static float LerpLinear(float from, float to, float t)
    {
        //return (1-t)*v0 + t*v1;
        t = Mathf.Clamp01(t);
        return ((1 - t) * from + t * to);
    }

    public static Vector3 SlerpXZ(Vector3 from, Vector3 to, float t)
    {
        from.y = to.y;
        return Vector3.Slerp(from, to, t);
    }

    public static float Lerp(float from, float to, float t)
    {
        t = Mathf.Clamp01(t);
        return (from + (to - from) * t);
    }

    public static float LerpAngle(float a, float b, float t)
    {
        float num = Mathf.Repeat(b - a, 360f);
        if (num > 180f)
        {
            num -= 360f;
        }
        return a + num * Mathf.Clamp01(t);
    }

    public static float Lerp(float from, float to, float time, float duration, EaseUtility.Ease easeType)
    {
        float t = EaseUtility.EaseConstant(easeType, time, duration);
        t = Mathf.Clamp01(t);
        return (from + (to - from) * t);
    }

    public static bool IsEqual(this Vector3 a, Vector3 b)
    {
        return ((Mathf.Abs(a.x - b.x) < 0.001f)
                && (Mathf.Abs(a.y - b.y) < 0.001f)
                && (Mathf.Abs(a.z - b.z) < 0.001f));
    }

    public static bool IsEqual(this Vector3 a, Vector3 b, float precision)
    {
        return ((Mathf.Abs(a.x - b.x) < precision)
                && (Mathf.Abs(a.y - b.y) < precision)
                && (Mathf.Abs(a.z - b.z) < precision));
    }

    public static bool IsEqual(this Quaternion a, Quaternion b)
    {
        return ((Mathf.Abs(a.w - b.w) < 0.001f)
                && (Mathf.Abs(a.x - b.x) < 0.001f)
                && (Mathf.Abs(a.y - b.y) < 0.001f)
                && (Mathf.Abs(a.z - b.z) < 0.001f));
    }

    public static float ClampAngle (float angle, float min, float max) 
    {
	    if (angle < -360)
		    angle += 360;
	    if (angle > 360)
		    angle -= 360;
	    return Mathf.Clamp (angle, min, max);
    }

    public static float RoundAngle(float angle)
    {
        if (angle < 0)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return angle;
    }

    public static float AngleBetween(Vector3 a, Vector3 b, Vector3 n)
    {
        Vector3 dir = (a - b);
        Vector3 cross = Vector3.Cross(n, dir);
        float angle = Vector3.Angle(n, dir);
        angle = cross.y > 0 ? angle : angle;
        return angle;
    }
    
	public static void ExtendLine (ref Vector3 A, ref Vector3 B, float scale)
	{
		float dist = Vector2.Distance (A, B);

		Vector3 newA = A;
		newA.x = B.x + ((B.x - A.x) / dist) * scale;
		newA.y = B.y + ((B.y - A.y) / dist) * scale;

		Vector3 newB = B;
		newB.x = A.x + ((A.x - B.x) / dist) * scale;
		newB.y = A.y + ((A.y - B.y) / dist) * scale;

		A = newA;
		B = newB;
	}

    #endregion


    #region BIT_OPERATION

    public static int[] ExtractBitFlag(int bitInteger)
    {
        System.Collections.Generic.List<int> intList = new System.Collections.Generic.List<int>();
        int decimalPos = bitInteger;

        int i = 0;
        while (decimalPos > 0)
        {
            int lastBit = (bitInteger & (1 << i)) >> i;

            if (lastBit != 0)
            {
                int value = (int)Mathf.Pow(2, i);
                intList.Add(value);
            }

            decimalPos = decimalPos / 2;
            i++;
        }
        return intList.ToArray();
    }

    public static int[] ExtractBitFlagInDecimal(int bitInteger)
    {
        System.Collections.Generic.List<int> intList = new System.Collections.Generic.List<int>();

        int decimalPos = bitInteger;

        int i = 0;
        while (decimalPos > 0)
        {
            int lastBit = (bitInteger & (1 << i)) >> i;

            if (lastBit != 0)
            {
                int value = (int)Mathf.Pow(2, i);
                value = CountBits(value);
                intList.Add(value);
            }

            decimalPos = decimalPos / 2;
            i++;
        }

        return intList.ToArray();
    }

	public static int ExtractSigleBitFlagInDecimal(int bitInteger)
	{
		int decimalPos = bitInteger;
		int finalvalue = 0;
		int i = 0;
		while (decimalPos > 0)
		{
			int lastBit = (bitInteger & (1 << i)) >> i;
			
			if (lastBit != 0)
			{
				int value = (int)Mathf.Pow(2, i);
				value = CountBits(value);
				finalvalue = value;
			}
			
			decimalPos = decimalPos / 2;
			i++;
		}
		
		return finalvalue - 1;
	}

    public static int CountBits(int value)
    {
        int count = 0;
        while (value != 0)
        {
            count++;
            value = value >> 1;
        }
        return count;
    }

    #endregion
}