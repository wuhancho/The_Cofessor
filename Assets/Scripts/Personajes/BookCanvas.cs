using NUnit.Framework;
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
    private void InstanceNotesLeft(GameObject notePrefab)
    {
        if(groupLeftLayout.transform.childCount <= 3)
        {
            GameObject note = Instantiate(notePrefab, groupLeftLayout.transform);
            return;
        }
    }
    private void InstanceNotesRight(GameObject notePrefab)
    {
        if (groupRightLayout.transform.childCount <= 3)
        {
            GameObject note = Instantiate(notePrefab, groupRightLayout.transform);
            return;
        }
    }
    public void InstanceNotes(GameObject notePrefab, bool isLeft = true)
    {
        if (isLeft)
        {
            InstanceNotesLeft(notePrefab);
        }
        else
        {
            InstanceNotesRight(notePrefab);
        }
    }

}
