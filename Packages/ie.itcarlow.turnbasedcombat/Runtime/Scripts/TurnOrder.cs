using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrder : MonoBehaviour
{
    public Dictionary<int, GameObject> m_battleOrder;

    public Dictionary<int, GameObject> DecideTurnOrder(List<GameObject> playableGroup, List<GameObject> enemyGroup)
    {
        m_battleOrder = new Dictionary<int, GameObject>();

        FirstStrikeChance firstStrikeScript = GetComponent<FirstStrikeChance>();

        if (firstStrikeScript.FirstStrikeCheck())
        {
            for (int i = 0; i < playableGroup.Count; ++i)
            {
                m_battleOrder.Add(i + 1, playableGroup[i]);
            }

            for (int i = 0; i < enemyGroup.Count; ++i)
            {
                m_battleOrder.Add(i + playableGroup.Count + 1, enemyGroup[i]);
            }

            foreach (var item in m_battleOrder)
            {
                Debug.Log(item.Key + ": " + item.Value.GetComponent<CharacterAttributes>().Name);
            }
        }
        else
        {
            for (int i = 0; i < enemyGroup.Count; ++i)
            {
                m_battleOrder.Add(i + 1, enemyGroup[i]);
            }

            for (int i = 0; i < playableGroup.Count; ++i)
            {
                m_battleOrder.Add(i + enemyGroup.Count + 1, playableGroup[i]);
            }

            foreach (var item in m_battleOrder)
            {
                Debug.Log(item.Key + ": " + item.Value.GetComponent<CharacterAttributes>().Name);
            }
        }

        return m_battleOrder;
    }
}
