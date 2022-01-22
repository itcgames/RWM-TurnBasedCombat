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
            attrs.Name = "Wolf";
            attrs.AddAttribute(new Attribute("HP", 25));
            attrs.AddAttribute(new Attribute("Attack", 19));

            GameObject characterTemp = CharacterUtil.CreateCharacter(attrs, null);
            GameObject enemy = Instantiate(characterTemp);
            
            EnemyList.Add(enemy);
        }
    }
}
