using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class RTSpriteRenderer : BaseImageLoader 
{
	private SpriteRenderer spriteRenderer;

	public override void InitLoading ()
	{
		spriteRenderer = gameObject.GetComponent <SpriteRenderer>();
		Sprite sprite = LoadSprite (imagePath, textureFormat, out resourceObject);
		spriteRenderer.sprite = sprite;
	}
}