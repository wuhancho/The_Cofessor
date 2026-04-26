using The_cofessor.Personajes.Dialogs;
using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;
public enum PenitentTypeDialogueSplit { a, b }

[CreateAssetMenu(fileName = "", menuName = "Scriptable Objects/SPenitent", order = 1)]
public class SPenitent : ScriptableObject
{
    [SerializeField] private string id;
    [SerializeField] private string characterName;
    [SerializeField] private Texture2D[] characterImage;
    [SerializeField] private Dialog[] dialogs;
    [SerializeField] bool isTrueDialogue = false;
    [SerializeField] private int day;
    [SerializeField] private int[] DaysApear;
    [SerializeField] private GameObject iconPenitent;
    [SerializeField] private GameObject notesPrefab;
    [Header("editado por evento")]
    [SerializeField, IReadOnly] internal bool isGuilty;


    public string CharacterName { get => characterName; set => characterName = value; }

    public Dialog[] Dialogs { get => dialogs; set => dialogs = value; }
    public bool IsTrueDialogue { get => isTrueDialogue; set => isTrueDialogue = value; }
    public int Day { get => day; set => day = value; }
    public string Id { get => id; set => id = value; }
    public GameObject IconPenitent { get => iconPenitent; set => iconPenitent = value; }

    public Notes NotesPenitent
    {
        get
        {
            if (notesPrefab != null)
            {
                return notesPrefab.GetComponent<Notes>();
            }
            return null;
        }
    }

    /// <summary>
    /// Gets the type identifier extracted from the first dialogue entry in the collection.
    /// </summary>
    /// <remarks>If the collection of dialogues is empty, the property returns null. The type identifier is
    /// parsed from the dialogue name using a specific naming convention, which may affect the returned value if the
    /// format does not match expectations.</remarks>
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
    /// Gets the day number extracted from the first dialogue entry.
    /// </summary>
    /// <remarks>If no dialogue entries are available, the property returns 0. The value is parsed from the
    /// dialogue name and may depend on the naming convention used for dialogue entries.</remarks>
    public int DayDialogue(Dialog dialog)
    {
        Debug.Log($"Parsing day from dialog name: {dialog.name}, day {int.Parse(dialog.name.Split('_')[1].Split('.')[1])}");
        return int.Parse(dialog.name.Split('_')[1].Split('.')[1]);
    }

    public string IdDialogue
    {
        get
        {
            foreach (var dialog in dialogs)
            {
                return dialog.name.Split('_')[0];
            }
            return null; // Cambiado de null a 0 para evitar CS0037
        }
    }

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
    /// Obtiene el tipo de diálogo de un Dialog individual.
    /// Formato del nombre: "ID_TipoDialogo.Día", por ejemplo: "AG_C.1"
    /// </summary>
    public static string GetDialogType(Dialog dialog, PenitentTypeDialogueSplit split = PenitentTypeDialogueSplit.a)
    {
        if (dialog == null) return null;
        string[] parts = dialog.name.Split('_');
        if (parts.Length < 2) return null;
        switch (split)
        {
            case PenitentTypeDialogueSplit.a:
                return parts[1].Split('.')[0];
            case PenitentTypeDialogueSplit.b:
                return parts[1].Split('&')[0];
            default:
                return null;
        }
    }

    private string GetDialogCombatPhase(Dialog dialog)
    {
        if (dialog == null) return null;
        string[] parts = dialog.name.Split('_');
        if (parts.Length < 2) return null;

        string[] phaseParts = parts[1].Split('&');
        if (phaseParts.Length < 2) return null;  // ← CRÍTICO: evita el IndexOutOfRangeException

        return phaseParts[1];  // "B&1" → ["B", "1"] → retorna "1"
    }


    /// <summary>
    /// Busca un diálogo solo por tipo (para diálogos sin día, como "AG_P" o "AG_F").
    /// </summary>
    public Dialog GetDialogByType(string type)
    {
        if (dialogs == null) return null;
        foreach (var dialog in dialogs)
        {
            if (dialog == null) continue;
            string dialogType = GetDialogType(dialog);
            if (dialogType == type)
            {
                return dialog;
            }
        }
        return null;
    }
    public Dialog GetDialogueBattle(string type, string Phase)
    {
        if (dialogs == null) return null;
        foreach (Dialog dialog in dialogs)
        {
            if (dialog == null) continue;

            string battleTypePhase = GetDialogType(dialog, PenitentTypeDialogueSplit.b);
            string battlePhase = GetDialogCombatPhase(dialog);
            Debug.Log($"Checking dialog: {dialog.name}, battleTypePhase: {battleTypePhase}, battlePhase: {battlePhase}");
            // Verificar que ambos valores existan antes de comparar
            if (battleTypePhase == null || battlePhase == null) continue;
            if (battleTypePhase == type && battlePhase == Phase)
            {
                Debug.Log($"Found matching dialog: {dialog.name}");
                return dialog;
            }
        }
        return null;
    }
    /// <summary>
    /// Obtiene el día de un Dialog individual.
    /// Formato del nombre: "ID_TipoDialogo.Día", por ejemplo: "AG_C.1"
    /// </summary>
    public static int GetDialogDay(Dialog dialog)
    {
        if (dialog == null) return 0;
        string[] parts = dialog.name.Split('_');
        if (parts.Length < 2) return 0;
        string[] subParts = parts[1].Split('.');
        if (subParts.Length < 2) return 0;
        int.TryParse(subParts[1], out int result);
        return result;
    }

    /// <summary>
    /// Busca un diálogo por tipo y día dentro de los diálogos del penitente.
    /// </summary>
    public Dialog GetDialogByTypeAndDay(string type, int targetDay)
    {
        if (dialogs == null) return null;
        foreach (var dialog in dialogs)
        {
            if (dialog == null) continue;
            string dialogType = GetDialogType(dialog);
            int dialogDay = GetDialogDay(dialog);
            if (dialogType == type && dialogDay == targetDay)
            {
                return dialog;
            }
        }
        return null;
    }



    // Las propiedades originales se mantienen por compatibilidad, pero devuelven el PRIMER diálogo
    /// <summary>
    /// retorna El ID del dialogo del PRIMER diálogo del array.
    /// </summary>
    public string GetDialogueID(string id)
    {
        foreach (var dialog in dialogs)
        {
            if (dialog == null) continue;
            if (dialog.name.StartsWith(id + "_"))
            {
                return dialog.name.Split('_')[0];
            }
        }
        return null;
    }
    public int GetDialogueDay(int day)
    {
        foreach (var dialog in dialogs)
        {
            if (dialog == null) continue;
            string[] parts = dialog.name.Split('_');
            if (parts.Length < 2) continue;
            string[] subParts = parts[1].Split('.');
            if (subParts.Length < 2) continue;
            if (int.TryParse(subParts[1], out int dialogDay) && dialogDay == day)
            {
                return dialogDay;
            }
        }
        return 0;
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
                return DaysApear[0];
            }
            return 0;
        }
    }
    public int DaysAppearCount
    {
        get
        {
            if (DaysApear != null)
            {
                return DaysApear.Length;
            }
            return 0;
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
