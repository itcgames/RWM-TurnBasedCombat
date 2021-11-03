using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTemplate : MonoBehaviour
{
    [SerializeField]
    private string m_name;

    [SerializeField]
    private bool m_playable;

    [SerializeField]
    private List<Attribute> m_attributes;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
