using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BookCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private GameObject groupLeftLayout;
    [SerializeField] private GameObject groupRightLayout;
    [SerializeField] private GameObject returnButton;
    private int day = 6;

    public void EditDateText(int Day, string Month = "Abril")
    {
        if (Day == 0)
        {
            dateText.text = "6 de Abril de 1679";
            day = 6;
            return;
        }
        else if (Day > 0)
        {
            dateText.text = (Day + day) + " de " + Month + " de 1679";
        }
    }
    private void InstanceNotesLeft(GameObject notePrefab, out GameObject note)
    {
        if(groupLeftLayout.transform.childCount <= 3)
        {
            note = Instantiate(notePrefab, groupLeftLayout.transform);
            return;
        }
        note = null;
    }
    private void InstanceNotesRight(GameObject notePrefab, out GameObject note)
    {
        if (groupRightLayout.transform.childCount <= 3)
        {
            note = Instantiate(notePrefab, groupRightLayout.transform);
            return;
        }
        note = null;
    }
    public GameObject WriteName(GameObject notePrefab, bool isLeft = true)
    {
        notePrefab.GetComponent<Notes>().Initialized();
        if (isLeft)
        {
            InstanceNotesLeft(notePrefab, out GameObject note);
            return note;
        }
        else
        {
            InstanceNotesRight(notePrefab, out GameObject note);
            return note;
        }
    }
}
