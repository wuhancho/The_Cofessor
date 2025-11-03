using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New RespondDialog", menuName = "Scriptable Objects/Dialogue/Dialogue Player")]
public class RespondDialog : ScriptableObject
{
    [SerializeField] private string[] responses;
    public IEnumerable<string> GetResponses()
    {
        yield return "response 1";
    }
}
