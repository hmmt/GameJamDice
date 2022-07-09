using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[CreateAssetMenu(fileName = "SpriteContainer", menuName = "SpriteContainer")]
public class SpriteContainer : ScriptableObject
{
    [SerializeField] List<SpriteSet> atlasList;

    public SpriteSet GetSpriteSet(int index)
    {
        if (index < 0 || index >= atlasList.Count)
            return null;

        return atlasList[index];
    }
}

[System.Serializable]
public class SpriteSet
{
    public List<Sprite> idle = new List<Sprite>();
    public List<Sprite> attack = new List<Sprite>();
    public List<Sprite> move = new List<Sprite>();
}