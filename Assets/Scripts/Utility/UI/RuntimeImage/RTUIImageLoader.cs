using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class RTUIImageLoader : BaseImageLoader 
{
	private Image uiImage;

	public override void InitLoading ()
	{
		uiImage = gameObject.GetComponent <Image>();
		Sprite sprite = LoadSprite (imagePath, textureFormat, out resourceObject);
		uiImage.sprite = sprite;
	}
}
