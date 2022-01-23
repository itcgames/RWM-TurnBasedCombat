using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatAction
{
    [SerializeField]
    private string m_name;

    public string Name
    {
        get { return m_name; }
        set { m_name = value; }
    }

public delegate void ActionDelegate(GameObject currentCharacter, GameObject targetCharacter);

    [SerializeField]
    private ActionDelegate m_action;

    public ActionDelegate Action
    {
        get { return m_action; }
        set { m_action = value; }
    }
}
