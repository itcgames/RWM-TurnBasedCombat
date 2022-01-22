using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryCheck : MonoBehaviour
{
    public bool CheckVictory(List<GameObject> playableGroup, List<GameObject> enemyGroup)
    {
        int playableGroupSize = playableGroup.Count;
        int enemyGroupSize = enemyGroup.Count;

        int survivors = enemyGroupSize;

        foreach (var character in enemyGroup)
        {
            if(character.GetComponent<CharacterAttributes>().FindAttribute("HP").Value <= 0)
            {
                --survivors;

                if (survivors <= 0) return true;
            }
        }

        return false;
    }
}
