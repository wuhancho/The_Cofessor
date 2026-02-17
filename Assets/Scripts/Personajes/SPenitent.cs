using The_cofessor.Personajes.Dialogs;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "", menuName = "Scriptable Objects/SPenitent", order = 1)]
public class SPenitent : ScriptableObject
{
    [SerializeField,] private string id;
    [SerializeField] private string characterName;
    [SerializeField] private Texture2D[] characterImage;
    [SerializeField] private Dialog[] dialogs;
    [SerializeField] bool isTrueDialogue = false;
    [SerializeField] private int day;
    [SerializeField] private int[] DaysApear;
    [SerializeField] private GameObject iconPenitent;

    public string CharacterName { get => characterName; set => characterName = value; }

    public Dialog[] Dialogs { get => dialogs; set => dialogs = value; }
    public bool IsTrueDialogue { get => isTrueDialogue; set => isTrueDialogue = value; }
    public int Day { get => day; set => day = value; }
    public string Id { get => id; set => id = value; }
    public GameObject IconPenitent { get => iconPenitent; set => iconPenitent = value; }


    public Texture2D[] GetTextures2D()
    {
        return characterImage;
    }

    public Dialog GetAllTrueDialogues()
    {
        if (isTrueDialogue)
        {
            foreach (var dialog in dialogs)
            {
                if (dialog.IsTrueDialogue)
                {
                    return dialog;
                }
            }
        }
        return null;
    }
    public Dialog GetAllFalseDialogues()
    {
        if (!isTrueDialogue)
        {
            foreach (var dialog in dialogs)
            {
                if (!dialog.IsTrueDialogue)
                {
                    return dialog;
                }
            }
        }
        return null;
    }
    /// <summary>
    /// retorna El ID del dialogo, 
    /// el cual debe seguir el formato "ID_TipoDiaologo.Día", por ejemplo: "Dialogo1_U.1" para un diálogo único del día 1.
    /// </summary>
    public string IDDialogue
    {
        get
        {
            foreach (var dialog in dialogs)
            {
                return dialog.name.Split('_')[0];
            }
            return null; // Agregado para asegurar que todas las rutas devuelvan un valor
        }
    }
    /// <summary>
    /// retorna el tipo de diálogo, ya sea "U" para único, "V" para la verdad, "M" para la mentira, "F" para el perdonar, 
    /// "P" para el castigar y la "C" Para el Dialogo de la cripta, esto se obtiene a partir del nombre del diálogo, 
    /// el cual debe seguir el formato "ID_TipoDiaologo.Día", por ejemplo: "Dialogo1_U.1" para un diálogo único del día 1.
    /// </summary>
    public string TypeDialogue
    {
        get
        {
            foreach (var dialog in dialogs)
            {
                return dialog.name.Split('_')[1].Split('.')[0];
            }
            return null; // Agregado para asegurar que todas las rutas devuelvan un valor
        }
    }
    /// <summary>
    /// Gets the day number associated with the current dialogue.
    /// </summary>
    public int DayDialogue
    {
        get
        {
            foreach (var dialog in dialogs)
            {
                return int.Parse(dialog.name.Split('_')[1].Split('.')[1]);
            }
            return 0; // Cambiado de null a 0 para evitar CS0037
        }
    }
    /// <summary>
    /// Retorna el primer día en el que aparece el penitente, basado en el array DaysApear.
    /// </summary>
    public int FirstDayAppear
    {
        get
        {
            if (DaysApear != null && DaysApear.Length > 0)
            {
                return DaysApear[0]; // Devuelve el primer día del array
            }
            return 0; // Devuelve 0 si el array está vacío o es nulo
        }
    }
    public int DaysAppearCount
    {
        get
        {
            if (DaysApear != null)
            {
                return DaysApear.Length; // Devuelve la cantidad de días en el array
            }
            return 0; // Devuelve 0 si el array es nulo
        }
    }
    /// <summary>
    /// Retorna un array con todos los días en los que aparece el penitente, basado en el array DaysApear.
    /// </summary>
    public int[] DaysApears
    {
        get => DaysApear;
    }


}
