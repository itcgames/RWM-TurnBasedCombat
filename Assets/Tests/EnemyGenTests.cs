using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;

public class EnemyGenTests
{
    [SetUp]
    public void Setup()
    {

    }

    [TearDown]
    public void Teardown()
    {
    }

    [UnityTest]
    public IEnumerator EnemyGenerationTest()
    {
        GameObject gameController = new GameObject();
        gameController.AddComponent<EnemyGen>();

        gameController.GetComponent<EnemyGen>().GenerateEnemies(1);

        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual(gameController.GetComponent<EnemyGen>().EnemyList[0].GetComponent<CharacterAttributes>().Name, "Wolf");
    }
}