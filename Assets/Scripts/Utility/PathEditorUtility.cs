using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class PathEditorUtility
{

	// Gets the point on the curve at the given percentage (0 to 1).
	// t: The percentage (0 to 1) at which to get the point.
	public static Vector3 GetPoint(Vector3[] points, float t)
	{
		int numSections = points.Length - 3;
		int tSec = (int)Math.Floor(t * numSections);
		int currPt = numSections - 1;
		if (currPt > tSec)
		{
			currPt = tSec;
		}
		float u = t * numSections - currPt;
		
		Vector3 a = points[currPt];
		Vector3 b = points[currPt + 1];
		Vector3 c = points[currPt + 2];
		Vector3 d = points[currPt + 3];
		
		return .5f * (
			(-a + 3f * b - 3f * c + d) * (u * u * u)
			+ (2f * a - 5f * b + 4f * c - d) * (u * u)
			+ (-a + c) * u
			+ 2f * b
			);
	}

}