using UnityEngine;
using System.Collections;

public class BaseImageLoader : MonoBehaviour
{
	public string imagePath;
	public TextureFormat textureFormat;

	protected Object resourceObject;

	void Start ()
	{
		InitLoading ();	
	}

	public virtual void InitLoading ()
	{		
	}

	public static Sprite LoadSprite (string imagePath, TextureFormat textureFormat, out Object resourceObject)
	{
		resourceObject = Resources.Load (imagePath);
		TextAsset imageTextAsset = resourceObject as TextAsset;

		if (imageTextAsset == null) 
		{
			Debug.LogError ("Couldn't able to find the image in specified path.!");
			return null;
		}

		Texture2D imageTexture = new Texture2D (1, 1, textureFormat, false);
		bool result = imageTexture.LoadImage (imageTextAsset.bytes);

		Sprite spriteImage = null;
		if (result)
		{
			spriteImage = Sprite.Create (imageTexture,
				new Rect (0, 0, imageTexture.width, imageTexture.height),
				Vector2.one * 0.5f);
		}
		return spriteImage;
	}

	void OnDestroy ()
	{
		Resources.UnloadAsset (resourceObject);
	}
}