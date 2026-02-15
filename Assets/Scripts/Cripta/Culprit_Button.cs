using TMPro;
using UnityEngine;

public class Culprit_Button : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameCulpableText;
    [SerializeField] private GameObject ControlSizeIcon;
    private GameObject CulpritIcon;

    public void Initialize(SPenitent penitent)
    {
        nameCulpableText.text = penitent.CharacterName;

        // Instanciar el icono del penitente si existe
        if (penitent.IconPenitent != null)
        {
            CulpritIcon = Instantiate(penitent.IconPenitent, ControlSizeIcon.transform);
        }
    }
}
