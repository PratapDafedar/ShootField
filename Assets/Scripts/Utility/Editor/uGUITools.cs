using UnityEditor;
using UnityEngine;
using System.IO;

public class uGUITools : MonoBehaviour
{
    [MenuItem("Tools/uGUI/Anchors to Corners _[")]
    static void AnchorsToCorners()
    {
        RectTransform t = Selection.activeTransform as RectTransform;
        RectTransform pt = Selection.activeTransform.parent as RectTransform;

        if (t == null || pt == null) return;

        Vector2 newAnchorsMin = new Vector2(t.anchorMin.x + t.offsetMin.x / pt.rect.width,
                                            t.anchorMin.y + t.offsetMin.y / pt.rect.height);
        Vector2 newAnchorsMax = new Vector2(t.anchorMax.x + t.offsetMax.x / pt.rect.width,
                                            t.anchorMax.y + t.offsetMax.y / pt.rect.height);

        t.anchorMin = newAnchorsMin;
        t.anchorMax = newAnchorsMax;
        t.offsetMin = t.offsetMax = new Vector2(0, 0);
    }

    [MenuItem("Tools/uGUI/Corners to Anchors _]")]
    static void CornersToAnchors()
    {
        RectTransform t = Selection.activeTransform as RectTransform;

        if (t == null) return;

        t.offsetMin = t.offsetMax = new Vector2(0, 0);
    }

    [MenuItem("Tools/PlayerPrefs Clear")]
    static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

	[MenuItem("Tools/Cache clear")]
	static void CleanCache()
	{
		Caching.CleanCache();
	}

	[MenuItem("Tools/Data path clear")]
	static void CleanDatapath()
	{
		string[] filePaths = Directory.GetFiles(Application.persistentDataPath);
		foreach (string filePath in filePaths) 
		{
			File.Delete (filePath);
			Debug.Log ("Deleted file :" + filePath);
		}
	}

	[MenuItem ("Tools/DeleteEmptyFolders")]
	public static void DeleteEmptyFolders ()
	{
		DirectoryInfo dirInfo = new DirectoryInfo( Application.dataPath + "/");
		SearchForEmptyFolder (dirInfo);
		AssetDatabase.Refresh ();
	}

	[MenuItem("Tools/Editor/CreateMaterial")]
	public static void CreateAssetBunldes ()
	{
		string[] guids = Selection.assetGUIDs;
		foreach (string guid in guids) 
		{
			string path =  AssetDatabase.GUIDToAssetPath(guid);
            string extension = System.IO.Path.GetExtension(path);
            string materialPath = path.Substring(0, path.Length - extension.Length) + ".mat";
            var material = new Material (Shader.Find("Unlit With Shadows"));
            material.mainTexture = AssetDatabase.LoadAssetAtPath<Texture>(path);
            AssetDatabase.CreateAsset(material, materialPath);
		}
	}
	
	//********** Private methods **********//
	
	/// <summary>
	/// Search for an empty folder and deletes it.
	/// </summary>
	/// <param name="dirInfo">Dir info.</param>
	static void SearchForEmptyFolder (DirectoryInfo dirInfo)
	{
		DirectoryInfo[] dirInfos = dirInfo.GetDirectories("*.*");
		if (dirInfos.Length != 0)
		{
			foreach (DirectoryInfo tempDirInfo in dirInfos) 
			{
				SearchForEmptyFolder (tempDirInfo);
			}
		}
		
		AssetDatabase.Refresh ();
		
		if (dirInfo.GetDirectories("*.*").Length == 0)
		{
			if (dirInfo.GetFiles("*.*").Length == 0)
			{
				UnityEditor.FileUtil.DeleteFileOrDirectory (dirInfo.FullName);
				bool result = UnityEditor.FileUtil.DeleteFileOrDirectory (dirInfo.FullName + ".meta");
				Debug.Log (result + "Deleted Dir : " + dirInfo.FullName);
				return;
			}
		}
	}
}