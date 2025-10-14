using UnityEngine;

public class Confessors : MonoBehaviour, IConfessors
{
    [SerializeField] private IConfessors[] IConfesors;
    private void Start()
    {
        foreach (var confessor in IConfesors)
        {
            confessor.Confess();
        }
    }
}
