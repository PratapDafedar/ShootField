using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Grid 
{
	[SerializeField]
	public bool filled;
	[SerializeField]
	public int x;
	[SerializeField]
	public int y;
	[SerializeField]
	public Vector3 pos;
	
	public Grid (bool _filled, int _x, int _y, Vector3 _pos)
	{
		filled = _filled;
		x = _x;
		y = _y;
		pos = _pos;
	}
};

[System.Serializable]
public class GridArray
{	
	[SerializeField]
	public Grid[] gridRow;

	public GridArray (Grid[] _gridArray)
	{
		gridRow = _gridArray;
	}
};

public class BaseMesh : MonoBehaviour {

    public const int BASE_SIZE_MULTIPLIER = 4;

	[SerializeField]
    public int GridColumnCount
	{
		get	{
			return gridColumnCount;
		}
		set {
			if (gridColumnCount != value)
			{
				gridColumnCount = value;
				isDirty = true;
			}
		}
	}
	[SerializeField]
	private int gridColumnCount;

	[SerializeField]
    public int GridRowCount
	{
		get {
			return gridRowCount;
		}
		set {
			if (gridRowCount != value)
			{
				gridRowCount = value;
				isDirty = true;
			}
		}
	}
	[SerializeField]
	private int gridRowCount;
	

	[SerializeField]
	public float GridXOffset
	{
		get	{
			return gridXOffset;
		}
		set {
			if (gridXOffset != value)
			{
				gridXOffset = value;
				isDirty = true;
			}
		}
	}
	[SerializeField]
	private float gridXOffset;

	[SerializeField]
	public float GridZOffset
	{
		get	{
			return gridZOffset;
		}
		set {
			if (gridZOffset != value)
			{
				gridZOffset = value;
				isDirty = true;
			}
		}
	}
	[SerializeField]
	private float gridZOffset;

	[SerializeField]
	public bool editGrid = true;

	[SerializeField]
	public GridArray[] gridArrayColumn;

	[HideInInspector]
	public float yOffset
    {
        get {
            return 0.1001f;
        }
    }	
	public float gridScale
    {
        get {
            return 2;
        }
    }

	private bool isDirty = false;

	private Material baseMaterial;

	public void UpdateBaseMesh ()
	{
		transform.position = new Vector3(gridXOffset + (gridColumnCount * gridScale) / 2, -0.1f, gridZOffset + (gridRowCount * gridScale) / 2);
		transform.localScale = new Vector3 ((gridColumnCount * gridScale), transform.localScale.y, (gridRowCount * gridScale));

		if (baseMaterial == null)
		{
			baseMaterial = new Material(GetComponent<Renderer>().sharedMaterial);

			DestroyImmediate (GetComponent<Renderer>().sharedMaterial);

			baseMaterial.name = "Base-Grid-Custom";
			GetComponent<Renderer>().material = baseMaterial;
		}
		GetComponent<Renderer>().sharedMaterial.mainTextureScale = new Vector2 (gridColumnCount, gridRowCount);
	}

	void OnDrawGizmos()
	{		
		for (int i = 0; i <= gridColumnCount; i++)
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawLine(new Vector3(gridXOffset + i * gridScale, yOffset, gridZOffset + 0),
			                new Vector3(gridXOffset + i * gridScale, yOffset, gridZOffset + gridRowCount * gridScale));
		}

		for (int i = 0; i <= gridRowCount; i++)
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawLine(new Vector3(gridXOffset + 0, yOffset, gridZOffset + i * gridScale),
			                new Vector3(gridXOffset + gridColumnCount * gridScale, yOffset, gridZOffset + i * gridScale));
		}

		if (isDirty)
		{
			UpdateBaseMesh ();

			RefreshGridArray ();

			isDirty = false;
		}

		if (gridArrayColumn != null)
		{
			for (int i = 0; i < gridColumnCount; i++)
			{
				for (int j = 0; j < gridRowCount; j++)
				{
					Gizmos.color = gridArrayColumn[i].gridRow[j].filled ? Color.black : Color.white;
					Gizmos.DrawSphere (gridArrayColumn[i].gridRow[j].pos, 0.2f);
				}
			}
		}
	}

	void RefreshGridArray ()
	{
		if (gridArrayColumn == null)
		{
			gridArrayColumn = new GridArray [gridColumnCount];
			for (int i = 0; i < gridArrayColumn.GetLength(0); i++)
			{
				gridArrayColumn[i] = new GridArray (new Grid [gridRowCount]);
			}
		}

		GridArray [] tempGridArray = new GridArray[gridColumnCount];
		for (int i = 0; i < tempGridArray.GetLength(0); i++)
		{
			tempGridArray[i] = new GridArray (new Grid [gridRowCount]);
		}

		for (int i = 0; i < gridColumnCount; i++) 
		{
			for (int j = 0; j < gridRowCount; j++) 
			{
				if (i < gridArrayColumn.GetLength (0) && 
				    gridArrayColumn[i] != null &&
				    j < gridArrayColumn[i].gridRow.GetLength (0) &&
				    gridArrayColumn [i].gridRow [j] != null)
				{
					tempGridArray[i].gridRow[j] = gridArrayColumn[i].gridRow[j];

					tempGridArray[i].gridRow[j].pos = new Vector3 (gridXOffset + i * gridScale + gridScale / 2, 
					                                        yOffset, 
					                                        gridZOffset + j * gridScale + gridScale / 2);
				}
				else {
					tempGridArray[i].gridRow[j] = new Grid (false, i, j, new Vector3 (gridXOffset + i * gridScale + gridScale / 2, 
					                                                           yOffset, 
					                                                           gridZOffset + j * gridScale + gridScale / 2));
				}
			}
		}
		gridArrayColumn = null;
		gridArrayColumn = tempGridArray;
	}

	public Vector3 GetValidNearestGrid (Vector3 _pos)
	{		
		int column = Mathf.CeilToInt ((_pos.x - gridXOffset) / gridScale) - 1;
		int row = Mathf.CeilToInt ((_pos.z - gridZOffset) / gridScale) - 1;

		if (row < gridRowCount &&
		    column < gridColumnCount)
		{
			return gridArrayColumn[column].gridRow[row].pos;
		}
		else {
			return gridArrayColumn[0].gridRow[0].pos;
		}
	}


#region MONO_RUNTIME

    [SerializeField]
	public static BaseMesh Instance;

	void Awake ()
	{
		Instance = this;
	}

	void Start ()
	{
		CreateWallFromGridArray ();
	}

	public void CreateWallFromGridArray ()
	{
		GameObject wallPrefab = Resources.Load ("Wall/Wall", typeof (GameObject)) as GameObject;
		for (int i = 0; i < gridColumnCount; i++)
		{
			for (int j = 0; j < gridRowCount; j++)
			{
				if (gridArrayColumn[i].gridRow[j].filled)
				{
					Vector3 tempPos = gridArrayColumn[i].gridRow[j].pos;
					tempPos.y = 0.5f + gridScale;
					GameObject wallInstance = Instantiate (wallPrefab, tempPos, Quaternion.identity) as GameObject;
					wallInstance.transform.parent = transform;
				}
			}
		}

		MeshUtility.CombinetheMeshesInChildren (transform);
	}

#endregion
}
