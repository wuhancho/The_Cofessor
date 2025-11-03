using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace The_cofessor.Personajes.Dialogs
{
}
[System.Serializable]
public class DialogNode
{
    public string uniqueID;
    public string text;
    public List<string> childrenIDs = new List<string>();
    public Rect rect = new Rect(0,0, 300,200);


}
