using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TestSuite
{

    [SetUp]
    public void Setup()
    {
       
    }

    [TearDown]
    public void Teardown()
    {
       
    }

    // 1
    [UnityTest]
    public IEnumerator SetupCombatValues()
    {
        yield return null;
    }
}
