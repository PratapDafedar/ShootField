using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent (typeof(MeshRenderer))]
[ExecuteInEditMode]
public class SlicedSpriteRenderer : MonoBehaviour
{
	[SerializeField]
	private Sprite m_sprite;
	public Sprite sprite
	{
		get {
			return m_sprite;
		}
		set {
			m_sprite = value;
		}
	}

	public float scaleFactorX = 1;
	public float scaleFactorY = 1;

	[SerializeField]
	private int m_layer;
	public int Layer
	{
		get { return m_layer; }
		set { m_layer = value; }
	}

	[SerializeField]
	private string m_sortingLayerName = "Default";
	public string SortingLayerName
	{
		get { return m_sortingLayerName; }
		set { m_sortingLayerName = value; }
	}

	private MeshRenderer mr;
	private MeshFilter mf;
	[SerializeField]
	private Mesh mesh;
	[SerializeField]
	private Material mat;

	#if UNITY_EDITOR
	void Start ()
	{
		if (mr == null)
			mr = gameObject.GetComponent<MeshRenderer> ();
		if (mf == null)
			mf = gameObject.GetComponent<MeshFilter> ();
		if (mesh == null)
			mesh = mf.sharedMesh;

		if (mr.sharedMaterial == null) 
		{
			mat = new Material(Shader.Find("Sprites/Default"));
			mr.sharedMaterial = mat;
		}
	}

	void Update ()
	{
		if (!Application.isPlaying) 
		{
			RefreshRenderer ();
			mr.sharedMaterial.SetTexture ("_MainTex", m_sprite.texture);
		}
	}
	#endif

	void RefreshRenderer ()
	{
		if (mesh == null)
		{
			mesh = new Mesh();
			mesh.Optimize ();
		}

		if (mat != null) 
		{
			mr.sortingOrder = m_layer;
			mr.sortingLayerName = m_sortingLayerName;
		}

		if (sprite != null) 
		{
			float c0 = 0;
			float c1 = ((sprite.border.x / sprite.textureRect.width) / Mathf.Abs (transform.localScale.x)) * scaleFactorX;
			float c2 = (1f - (sprite.border.z / sprite.textureRect.width) / Mathf.Abs (transform.localScale.x)) * scaleFactorX;
			float c3 = 1 * scaleFactorX;
			float r0 = 0;
			float r1 = ((sprite.border.y / sprite.textureRect.height) / Mathf.Abs (transform.localScale.y)) * scaleFactorY;
			float r2 = (1f - (sprite.border.w / sprite.textureRect.height) / Mathf.Abs (transform.localScale.y)) * scaleFactorY;
			float r3 = 1 * scaleFactorY;

			Vector3[] vertices = new Vector3[16];
			vertices [0] = new Vector3 (c0, r0, 0);
			vertices [1] = new Vector3 (c1, r0, 0);
			vertices [2] = new Vector3 (c2, r0, 0);
			vertices [3] = new Vector3 (c3, r0, 0);
			vertices [4] = new Vector3 (c0, r1, 0);
			vertices [5] = new Vector3 (c1, r1, 0);
			vertices [6] = new Vector3 (c2, r1, 0);
			vertices [7] = new Vector3 (c3, r1, 0);
			vertices [8] = new Vector3 (c0, r2, 0);
			vertices [9] = new Vector3 (c1, r2, 0);
			vertices [10] = new Vector3 (c2, r2, 0);
			vertices [11] = new Vector3 (c3, r2, 0);
			vertices [12] = new Vector3 (c0, r3, 0);
			vertices [13] = new Vector3 (c1, r3, 0);
			vertices [14] = new Vector3 (c2, r3, 0);
			vertices [15] = new Vector3 (c3, r3, 0);

			float uvc0 = 0;
			float uvc1 = (sprite.border.x / sprite.textureRect.width);
			float uvc2 = 1 - (sprite.border.z / sprite.textureRect.width);
			float uvc3 = 1;
			float uvr0 = 0;
			float uvr1 = (sprite.border.y / sprite.textureRect.height);
			float uvr2 = 1 - (sprite.border.w / sprite.textureRect.height);
			float uvr3 = 1;

			Vector2[] uvs = new Vector2[16];
			uvs [0] = new Vector2 (uvc0, uvr0);
			uvs [1] = new Vector2 (uvc1, uvr0);
			uvs [2] = new Vector2 (uvc2, uvr0);
			uvs [3] = new Vector2 (uvc3, uvr0);
			uvs [4] = new Vector2 (uvc0, uvr1);
			uvs [5] = new Vector2 (uvc1, uvr1);
			uvs [6] = new Vector2 (uvc2, uvr1);
			uvs [7] = new Vector2 (uvc3, uvr1);
			uvs [8] = new Vector2 (uvc0, uvr2);
			uvs [9] = new Vector2 (uvc1, uvr2);
			uvs [10] = new Vector2 (uvc2, uvr2);
			uvs [11] = new Vector2 (uvc3, uvr2);
			uvs [12] = new Vector2 (uvc0, uvr3);
			uvs [13] = new Vector2 (uvc1, uvr3);
			uvs [14] = new Vector2 (uvc2, uvr3);
			uvs [15] = new Vector2 (uvc3, uvr3);

			int[] tris = new int[54]; // 9 * 6;
			int offset = 0;
			for (int i = 0; i < 9; i++) 
			{
				tris [i * 6] = 0 + offset;
				tris [i * 6 + 1] = 5 + offset;
				tris [i * 6 + 2] = 4 + offset;
				tris [i * 6 + 3] = 0 + offset;
				tris [i * 6 + 4] = 1 + offset;
				tris [i * 6 + 5] = 5 + offset;

				offset += (i + 1) % 3 == 0 ? 2 : 1;
			}

			mesh.vertices = vertices;
			mesh.uv = uvs;
			mesh.triangles = tris;
			mesh.RecalculateBounds ();
			mesh.RecalculateNormals ();
		}
		mf.sharedMesh = mesh;
	}
}