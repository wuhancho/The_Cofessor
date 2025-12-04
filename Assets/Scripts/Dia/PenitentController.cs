using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class PenitentController : MonoBehaviour
{
    [SerializeField] private SPenitent[] sPenitents;
    [SerializeField] private GameObject EntracePenitent;
    [SerializeField] private TextMeshProUGUI penitentText;
    private void Awake()
    {
        penitentText = EntracePenitent.GetComponentInChildren<TextMeshProUGUI>();
    }
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
    public SPenitent[] GetSPenitents(int day)
    {
        List<SPenitent> matchedPenitents = new List<SPenitent>();

        foreach (SPenitent sPenitent in sPenitents)
        {
            if (sPenitent.Day == day)
            {
                Debug.Log("consigo los penitent del dia");
                matchedPenitents.Add(sPenitent);
            }
        }
        return matchedPenitents.ToArray();
    }
    public SPenitent[] GetAllPenitents()
    {
        foreach (SPenitent sPenitent in sPenitents)
        {
            Debug.Log("SPenitent ID: " + sPenitent.Id + ", Name: " + sPenitent.CharacterName);
        }
        return sPenitents;
    }
    public Texture2D[] GetPenitentImagesById(string id)
    {
        SPenitent sPenitent = GetSPenitent(id);
        return sPenitent.GetTexture2Ds();
    }
    public GameObject GetEntracePenitent()
    {
        return EntracePenitent;
    }
    public void UpdatePenitentText(string newText)
    {
        if (penitentText != null)
        {
            penitentText.text = newText;
        }
        else
        {
            Debug.LogWarning("Penitent TextMeshProUGUI reference is not set.");
        }
    }

}