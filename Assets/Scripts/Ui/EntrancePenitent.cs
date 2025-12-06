using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class EntrancePenitent : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI entrancePenitentText;
    [SerializeField] Animation entranceAnimation;
    [SerializeField] float displayDuration = 1.5f;

    public void SetEntrancePenitentText(string text)
    {
        entrancePenitentText.text = text;
    }

    public void SetActive()
    {
        gameObject.SetActive(true);
    }
    public void SetInactive()
    {
        difumination();
        gameObject.SetActive(false);
    }

    private IEnumerator difumination()
    {
        yield return new WaitForSeconds(displayDuration);
        Debug.Log("Difuminating entrance penitent text.");
    }
}
