using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public static class StandaloneWindowsUtility 
{
	#if UNITY_STANDALONE_WIN || UNITY_EDITOR
	[DllImport("user32.dll", EntryPoint = "SetWindowPos")]
	private static extern bool SetWindowPos(IntPtr hwnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
	[DllImport("user32.dll", EntryPoint = "FindWindow")]
	public static extern IntPtr FindWindow(System.String className, System.String windowName);

	public static void SetPosition(int x, int y, int resX = 0, int resY = 0) {
		SetWindowPos(FindWindow(null, "ShootField"), 0, x, y, resX, resY, resX * resY == 0 ? 1 : 0);
	}
	#endif

	public static void SetWindowPosition (int x, int y) 
	{
		SetPosition(x, y);
	}
}