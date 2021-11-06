using UnityEngine;

[System.Serializable]
public class Attribute
{
    [SerializeField]
    private string m_name;

    [SerializeField]
    private float m_value;

    public float Value
    {
        get { return m_value; }
        set { m_value = value; }
    }

    public string Name
    {
        get { return m_name; }
        set { m_name = value; }
    }
}
