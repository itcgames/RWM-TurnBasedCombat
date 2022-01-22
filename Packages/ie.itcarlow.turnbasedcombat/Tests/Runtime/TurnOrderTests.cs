using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;

public class TurnOrderTests
{
    private GameObject m_player;
    
    [SetUp]
    public void Setup()
    {
        CharacterAttributes attrs = new CharacterAttributes();
        attrs.Name = "Frank";
        m_player = CharacterUtil.CreateCharacter(attrs, null);
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(m_player);
    }

    [UnityTest]
    public IEnumerator TurnOrderTest()
    {
        GameObject gameController = new GameObject();
        gameController.AddComponent<TurnOrder>();
        gameController.AddComponent<FirstStrikeChance>();
        gameController.AddComponent<EnemyGen>();

        gameController.GetComponent<FirstStrikeChance>().SetType(FirstStrikeChance.CheckType.BooleanExpressions);
        gameController.GetComponent<FirstStrikeChance>().SetOnAdvantage(false);

        gameController.GetComponent<EnemyGen>().GenerateEnemies(1);

        List<GameObject> playables = new List<GameObject>();
        playables.Add(m_player);

        Dictionary<int, GameObject> expectedOrder = gameController.GetComponent<TurnOrder>().DecideTurnOrder(playables, gameController.GetComponent<EnemyGen>().EnemyList);

        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual(expectedOrder[2].GetComponent<CharacterAttributes>().Name, "Frank");
    }
}
