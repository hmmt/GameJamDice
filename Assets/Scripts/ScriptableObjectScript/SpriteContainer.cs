using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[CreateAssetMenu(fileName = "SpriteContainer", menuName = "SpriteContainer")]
public class SpriteContainer : ScriptableObject
{
    [SerializeField] List<SpriteAtlas> atlasList;

    public Sprite GetSprite(string spriteName)
    {
        foreach(var atlas in atlasList)
        {
            var targetSprite = atlas.GetSprite(spriteName);
            if (targetSprite != null)
                return targetSprite;
        }
        return null;
    }
}
