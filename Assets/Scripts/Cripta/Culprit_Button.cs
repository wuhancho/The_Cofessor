using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Culprit_Button : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameCulpableText;
    [SerializeField] private GameObject ControlSizeIcon;
    [SerializeField] private Button button;
    private GameObject CulpritIcon;
    private SPenitent currentPenitent;
    public event Action<SPenitent> OnCulpritClicked; // evento para notificar selección de culpable

    private void Awake()
    {
        button.onClick.AddListener(SelectCulprit);
    }

    private void SelectCulprit()
    {
       OnCulpritClicked?.Invoke(currentPenitent); // Invocar el evento con el penitent seleccionado
    }

    public void Initialize(SPenitent penitent)
    {
        currentPenitent = penitent;
        string characterName = penitent.CharacterName;
        characterName = characterName.Split(",")[0]; // Obtener solo el nombre antes de la coma)
        nameCulpableText.text = characterName;

        // Instanciar el icono del penitente si existe
        if (penitent.IconPenitent != null)
        {
            CulpritIcon = Instantiate(penitent.IconPenitent, ControlSizeIcon.transform);
        }
    }
    
}
