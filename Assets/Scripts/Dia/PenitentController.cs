using System;
using UnityEngine;
public class PenitentController : MonoBehaviour
{
    [SerializeField] private SPenitent[] sPenitents;
    

    public SPenitent GetSPenitent(string id)
    {
        foreach (SPenitent sPenitent in sPenitents)
        {
            if (sPenitent.Id == id)
            {
                return sPenitent;
            }
        }
        throw new Exception("SPenitent with id " + id + " not found.");
    }
}