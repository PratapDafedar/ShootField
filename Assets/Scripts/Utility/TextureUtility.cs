using UnityEngine;
using System.Collections;

public static class TextureUtility
{
    public static Sprite ToSprite (this Texture2D texture)
    {
        Sprite sprite;
        sprite = Sprite.Create(texture,
                                new Rect(0, 0, texture.width, texture.height),
                                Vector2.one / 2.0f);
        return sprite;
    }

    public static Sprite ToSprite(this Texture texture)
    {
        return ((Texture2D)texture).ToSprite();
    }
}
