using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "Frases", menuName = "ScriptableObjects/Frases", order = 2)]
public class Frases : ScriptableObject
{
    [SerializeField] private string[] frasesNormales;
    [SerializeField] private string[] frasesMenu;
    [SerializeField] private int nivel;
    public string[] FrasesNormales
    {
        get { return frasesNormales; }
        set { frasesNormales = value; }
    }
    public string[] FrasesMenu
    {
        get { return frasesMenu; }
        set { frasesMenu = value; }
    }
    public int Nivel
    {
        get { return nivel; }
        set { nivel = value; }
    }
}
