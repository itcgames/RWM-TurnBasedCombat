using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUtil
{
  public static GameObject CreateCharacter(CharacterAttributes attrs, string spritePath)
    {
        GameObject character = new GameObject();
        character.AddComponent<CharacterAttributes>();
        character.GetComponent<CharacterAttributes>().Name = attrs.Name;
        character.GetComponent<CharacterAttributes>().Playable = attrs.Playable;
        character.GetComponent<CharacterAttributes>().Attributes = attrs.Attributes;

        if(spritePath != null || spritePath != "")
        {
            character.AddComponent<SpriteRenderer>();
            character.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(spritePath);
        }

        Debug.Log(character.GetComponent<CharacterAttributes>().Name);

        return character;
    }
}
