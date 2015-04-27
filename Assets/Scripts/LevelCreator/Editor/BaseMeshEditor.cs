using UnityEngine;
using System.Collections;
using UnityEditor;

using System.Reflection;

[CustomEditor(typeof(BaseMesh))]
public class BaseMeshEditor : Editor
{
	string exportName;
	Texture2D importImage;

    public void OnEnable ()
    {
        BaseMesh _target = (BaseMesh)target;
        BaseMesh.Instance = _target;
    }

    public override void OnInspectorGUI()
    {
        BaseMesh _target = (BaseMesh)target;

		_target.editGrid = EditorGUILayout.Toggle("Edit Grid", _target.editGrid);
		if(_target.editGrid)
		{
			_target.GridRowCount = EditorGUILayout.IntSlider("Row Count :", _target.GridRowCount, 0, 100, GUILayout.MinWidth(300), GUILayout.MaxWidth (300));

			_target.GridColumnCount = EditorGUILayout.IntSlider("Column Count :", _target.GridColumnCount, 0, 100, GUILayout.MinWidth(300), GUILayout.MaxWidth (300));

	       // _target.GridScale = EditorGUILayout.Slider("Scale :", _target.GridScale, 0.01f, 1);

			_target.GridXOffset = EditorGUILayout.FloatField("Offset X :", _target.GridXOffset);
			
			_target.GridZOffset = EditorGUILayout.FloatField("Offset Y :", _target.GridZOffset);
		

			//Export Level as a image.
			GUILayout.BeginHorizontal();
			exportName = EditorGUILayout.TextField(new GUIContent (), exportName, GUILayout.MinWidth(100), GUILayout.MaxWidth (100));
			if(GUILayout.Button("Export Level", GUILayout.MinWidth(100), GUILayout.MaxWidth (100)))
			{
				ExportImage(_target, exportName);
			}
			GUILayout.EndHorizontal();


			//Import Level from a image.
			GUILayout.BeginHorizontal();
			importImage = EditorGUILayout.ObjectField(new GUIContent (), importImage, typeof(Texture2D), true, GUILayout.MinWidth(100), GUILayout.MaxWidth (100)) as Texture2D;
			if(GUILayout.Button("Import Level", GUILayout.MinWidth(100), GUILayout.MaxWidth (100)))
			{
				ImportImage(_target, importImage);
			}
			GUILayout.EndHorizontal();
					
			GridMapEditor (_target);
		}

		SceneView.RepaintAll();
    }

	void GridMapEditor(BaseMesh _target) 
	{
		for (int i = 0; i < _target.GridColumnCount; i++)
		{
			GUILayout.BeginHorizontal();
			for (int j = 0; j < _target.GridRowCount; j++)
			{
				if (_target.gridArrayColumn == null ||
					i >= _target.gridArrayColumn.Length ||
					_target.gridArrayColumn[i] == null ||
				    j >= _target.gridArrayColumn[i].gridRow.Length ||
				    _target.gridArrayColumn[i].gridRow[j] == null)
					continue;

				_target.gridArrayColumn[i].gridRow[j].filled = EditorGUILayout.Toggle(new GUIContent (),//"Grid[" + i + "," + j + "] :",
				                                                                      _target.gridArrayColumn[i].gridRow[j].filled,
				                                                                      GUILayout.MinWidth(10),
				                                                                      GUILayout.MaxWidth (10));
			}
			GUILayout.EndHorizontal();
		}
	} 

	void ExportImage (BaseMesh _target, string exportName)
	{
		if (exportName == null || exportName == "")
		{
			EditorUtility.DisplayDialog ("Error!", "Export file name field is empty." +
				"Please specify the file name in which you want to export the map.", "Got it!");
			return;
		}

		Texture2D exportImage = new Texture2D (_target.GridColumnCount, _target.GridRowCount, TextureFormat.RGB24, false, true);

		for (int i = 0; i < _target.GridColumnCount; i++)
		{
			for (int j = 0; j < _target.GridRowCount; j++)
			{
				exportImage.SetPixel(i, j, _target.gridArrayColumn[i].gridRow[j].filled ? Color.black : Color.white);
			}
		}

		byte[] bytes = exportImage.EncodeToJPG();

		string path = Application.dataPath + "/Resources/LevelsExport/" + exportName + ".jpg";
		System.IO.File.WriteAllBytes(path, bytes);

		EditorUtility.DisplayDialog ("Success", "Exported successfully here. \n" + path, "Thanks!");

		AssetDatabase.ImportAsset (path);
		AssetDatabase.Refresh ();
	}

	void ImportImage (BaseMesh _target, Texture2D _sourceTexture)
	{
		if (_sourceTexture == null)
		{
			EditorUtility.DisplayDialog ("Error!", "Import image field is empty. " +
				"Please assign any image from project to import it as a level.\n " +
				"And please make that texture read/write enabled in import settings.", "Got it!");
			return;
		}

		//Just to make the texture readable from Import settings.
		string path = AssetDatabase.GetAssetPath( _sourceTexture );
		TextureImporter importSetting = (TextureImporter) AssetImporter.GetAtPath( path );
		importSetting.isReadable = true;
		importSetting.generateMipsInLinearSpace = false;
		importSetting.npotScale = TextureImporterNPOTScale.None;
		AssetDatabase.ImportAsset( path, ImportAssetOptions.ForceUpdate );

		//Parsing image and converting it into Map.
		_target.GridColumnCount = _sourceTexture.width;
		_target.GridRowCount = _sourceTexture.height;

		GridArray [] tempGridArray = new GridArray[_target.GridColumnCount];
		for (int i = 0; i < tempGridArray.GetLength(0); i++)
		{
			tempGridArray[i] = new GridArray (new Grid [_target.GridRowCount]);
		}

		for (int i = 0; i < _target.GridColumnCount; i++)
		{
			for (int j = 0; j < _target.GridRowCount; j++)
			{
				Color readColor = _sourceTexture.GetPixel(i, j);

				bool filled;
				if (((readColor.r + readColor.g + readColor.b) / 3) < 0.5f)
				{
					filled = true;
				}
				else {
					filled = false;
				}

				tempGridArray[i].gridRow[j] = new Grid (filled, i, j, new Vector3 (_target.GridXOffset + i * _target.gridScale + _target.gridScale / 2, 
				                                                                  _target.yOffset, 
				                                                                   _target.GridZOffset + j * _target.gridScale + _target.gridScale / 2));
			}
		}
		_target.gridArrayColumn = tempGridArray;
	}
}
