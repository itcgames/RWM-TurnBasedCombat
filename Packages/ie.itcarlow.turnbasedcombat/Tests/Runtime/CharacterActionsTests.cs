using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;

public class CharacterActionsTests
{
    private GameObject m_player;

    [SetUp]
    public void Setup()
    {
        CharacterAttributes attrs = new CharacterAttributes();
        attrs.Name = "Frank";
        attrs.Playable = true;
        attrs.AddAttribute(new Attribute("HP", 30));
        attrs.AddAttribute(new Attribute("DMG", 4));
        m_player = CharacterUtil.CreateCharacter(attrs, null);
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(m_player);
    }

    private void TestAction(GameObject p1, GameObject p2)
    {
        p1.GetComponent<CharacterAttributes>().AddAttribute(new Attribute("Test", 12));
    }

    private void TestAction2(GameObject p1, GameObject p2)
    {
        p2.GetComponent<CharacterAttributes>().FindAttribute("HP").Value -= p1.GetComponent<CharacterAttributes>().FindAttribute("DMG").Value;
    }

    [UnityTest]
    public IEnumerator ActionCreationAndSelectionTest()
    {
        GameObject character = new GameObject();
        character.AddComponent<CharacterAttributes>();

        CombatAction action = new CombatAction();
        action.Name = "TestAction";
        action.Action = TestAction;

        action.Action(character, null);

        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual(character.GetComponent<CharacterAttributes>().FindAttribute("Test").Name, "Test");
    }

    [UnityTest]
    public IEnumerator ActionTriggerTest()
    {
        GameObject gameController = new GameObject();
        gameController.AddComponent<TurnOrder>();
        gameController.AddComponent<FirstStrikeChance>();
        gameController.AddComponent<EnemyGen>();
        gameController.AddComponent<VictoryCheck>();

        gameController.GetComponent<FirstStrikeChance>().SetType(FirstStrikeChance.CheckType.BooleanExpressions);
        gameController.GetComponent<FirstStrikeChance>().SetOnAdvantage(true);

        gameController.GetComponent<EnemyGen>().GenerateEnemies(1);

        gameController.GetComponent<EnemyGen>().EnemyList[0].AddComponent<ActionController>();
        gameController.GetComponent<EnemyGen>().EnemyList[0].GetComponent<ActionController>().SetAction(TestAction);

        m_player.AddComponent<ActionController>();
        m_player.GetComponent<ActionController>().SetAction(TestAction2);
        m_player.GetComponent<ActionController>().Target = gameController.GetComponent<EnemyGen>().EnemyList[0];

        List<GameObject> playables = new List<GameObject>();
        playables.Add(m_player);

        gameController.GetComponent<TurnOrder>().DecideTurnOrder(playables, gameController.GetComponent<EnemyGen>().EnemyList);

        gameController.GetComponent<TurnOrder>().PlayTurn(playables, gameController.GetComponent<EnemyGen>().EnemyList);

        yield return new WaitForSeconds(0.1f);

        Assert.LessOrEqual(gameController.GetComponent<EnemyGen>().EnemyList[0].GetComponent<CharacterAttributes>().FindAttribute("HP").Value, 25);
    }
}