using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyGen : MonoBehaviour
{
    public List<GameObject> EnemyList;

    public void GenerateEnemies(int enemyCount)
    {
        EnemyList = new List<GameObject>();

        for (int i = 0; i < enemyCount; i++)
        {
            CharacterAttributes attrs = new CharacterAttributes();
            attrs.ClearAttributes();
            attrs.Name = "Wolf";
            attrs.Playable = false;
            attrs.Gold = 2;
            attrs.Xp = 1;

            Attribute hp = new Attribute("HP", 25);
            Attribute dmg = new Attribute("DMG", 19);

            attrs.AddAttribute(hp);
            attrs.AddAttribute(dmg);

            GameObject characterTemp = CharacterUtil.CreateCharacter(attrs, null);
            GameObject enemy = Instantiate(characterTemp);
            
            EnemyList.Add(enemy);
        }
    }
}
