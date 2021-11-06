﻿using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TestSuite
{
    private GameObject m_characterPrefab;
    [SetUp]
    public void Setup()
    {
        m_characterPrefab = Resources.Load("Prefabs/CharacterTemplate") as GameObject;
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(m_characterPrefab);
    }

    [UnityTest]
    public IEnumerator SetupCombatValues()
    {
        m_characterPrefab = Object.Instantiate(m_characterPrefab);
        m_characterPrefab.GetComponent<CharacterAttributes>().SetAttribute("Hit Points", 200);
        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual(m_characterPrefab.GetComponent<CharacterAttributes>().FindAttribute("Hit Points").Value, 200);
    }
}
