using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
public class RuntimeImageLoader 
{	
	public class ModelImage 
	{
		public Bounds bounds;
		private TextAsset imageTextAsset;
		private Sprite spriteImage;
		private Texture2D imageTexture;
		private GameObject spriteGameObject;
		private SpriteRenderer renderer;

		private Image uiImage;

		public void LoadImage (string imageName, Vector3 imagePosition,TextureFormat textureFormat, float width, float height, GameObject obj, bool isSpriteimage)
		{
			imageTextAsset = Resources.Load (imageName) as TextAsset;
			imageTexture = new Texture2D (1, 1, textureFormat, false);
			imageTexture.LoadImage (imageTextAsset.bytes);

			if (width == -1)
				width = imageTexture.width;
			
			if(height == -1)
				height = imageTexture.height;

			if(obj == null)
			{
				spriteGameObject = new GameObject(imageName);
			}
			else {
				spriteGameObject = obj;
			}

			if (isSpriteimage)
			{
				if(spriteGameObject.GetComponent<SpriteRenderer>()==null)
				{
					renderer = spriteGameObject.AddComponent<SpriteRenderer>();
				}
				else {
					renderer = spriteGameObject.GetComponent<SpriteRenderer>();
				}
			}
			else
			{
				if(spriteGameObject.GetComponent<Image>()==null)
				{
					uiImage = spriteGameObject.AddComponent<Image>();
				}
				else {
					uiImage = spriteGameObject.GetComponent<Image>();
				}

			}

			spriteImage = Sprite.Create(imageTexture, 
			                            new Rect(0, 0, width, height),
			                            Vector2.one / 2.0f);

			if (isSpriteimage)
			{
				renderer.sprite = spriteImage;
			}
			else
				uiImage.sprite = spriteImage;

					
//			spriteGameObject.name = imageName;
			if (obj == null)
				spriteGameObject.transform.position = imagePosition;
		}
		
		public void DestroyImage()
		{
			if(spriteGameObject.GetComponent<SpriteRenderer>()!=null)
				renderer.sprite = null;

			if(spriteGameObject.GetComponent<Image>()!=null)
				uiImage.sprite = null;

			imageTextAsset = null;
			spriteImage = null;
			imageTexture = null;
			spriteGameObject = null;
			renderer = null;

			Resources.UnloadAsset (imageTextAsset);
		}

		~ModelImage ()
		{
			Debug.Log("Called Destructor.....>>>>>");
		}
	}
	
	private static Dictionary <int, ModelImage> runTimeImageDict;
	private static int generateId =- 1;

	public static int LoadByteImage (string imageName, Vector3 imagePosition, TextureFormat textFormat = TextureFormat.RGB24, GameObject gObj =null, bool isSprite=true, float width =-1, float height =-1)
	{
		generateId++;

		if (runTimeImageDict == null)
			runTimeImageDict = new Dictionary<int, ModelImage>();

		ModelImage runTime = new ModelImage ();
		runTime.LoadImage (imageName, imagePosition, textFormat, width, height, gObj, isSprite);
		runTimeImageDict.Add (generateId, runTime);
		return generateId;
	}	
	
	public static void DestroyByteImage(int id)
	{
		runTimeImageDict[id].DestroyImage ();
		runTimeImageDict.Remove (id);
		Resources.UnloadUnusedAssets ();
	}
	
	public static void DestroyAllByteImages()
	{
		if (runTimeImageDict != null)
		{
			foreach(int key in runTimeImageDict.Keys)
			{
				ModelImage obj = runTimeImageDict[key];
				obj.DestroyImage();
			}
		}
		runTimeImageDict.Clear ();
		generateId = -1;
		Resources.UnloadUnusedAssets ();
	}	
}