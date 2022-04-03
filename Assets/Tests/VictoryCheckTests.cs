using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;

public class VictoryCheckTests
{
    private List<GameObject> m_players = new List<GameObject>();
    private List<GameObject> m_enemies = new List<GameObject>();

    [SetUp]
    public void Setup()
    {
        for (int i = 0; i < 4; i++)
        {
            CharacterAttributes attrs = new CharacterAttributes();
            attrs.ClearAttributes();
            attrs.Playable = false;
            attrs.Gold = 2;
            attrs.Xp = 1;

            Attribute hp = new Attribute("HP", 100);

            attrs.AddAttribute(hp);
            m_players.Add(CharacterUtil.CreateCharacter(attrs, null));

            attrs.FindAttribute("HP").Value = 0;
            m_enemies.Add(CharacterUtil.CreateCharacter(attrs, null));
        }
    }

    [TearDown]
    public void Teardown()
    {
        foreach (var character in m_players)
        {
            Object.Destroy(character);
        }

        foreach (var character in m_enemies)
        {
            Object.Destroy(character);
        }
    }

    [UnityTest]
    public IEnumerator VictoryCheckTest()
    {
        GameObject victory = new GameObject();
        victory.AddComponent<VictoryCheck>();

        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual(victory.GetComponent<VictoryCheck>().CheckVictory(m_players, m_enemies), true);
    }
}
