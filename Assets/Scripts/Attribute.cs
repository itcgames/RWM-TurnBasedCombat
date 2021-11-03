using UnityEngine;

[System.Serializable]
public class Attribute
{
    [SerializeField]
    private string m_name;

    [SerializeField]
    private float m_value;

    public string name() { return m_name; }
    public float value() { return m_value; }
}
