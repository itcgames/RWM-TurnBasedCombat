using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    private CombatAction m_currentAction = new CombatAction();

    public GameObject Target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAction(CombatAction.ActionDelegate actionFunc)
    {
        m_currentAction.Action = actionFunc;
    }

    public void ExecuteAction()
    {
        m_currentAction.Action(gameObject, Target);
    }
}
