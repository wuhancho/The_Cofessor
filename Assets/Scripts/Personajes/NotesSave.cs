using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "NotesSave", menuName = "Scriptable Objects/NotesSafes")]
public class NotesSave : ScriptableObject
{
    [SerializeField] private List<NoteData> _notesData = new List<NoteData>();

}
