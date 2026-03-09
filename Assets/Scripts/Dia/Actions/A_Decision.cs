using System;
using The_cofessor.Personajes.Dialogs;
using Unity.VisualScripting;
using UnityEngine;

public class A_Decision : MonoBehaviour, IAcciones
{
    [SerializeField] private int dayToActivate;
    private SPenitent[] todayPenitents;
    private int todayPenintentIndex;
    [SerializeField] private VoteCanvas voteCanvas;
    [SerializeField] private CriptaDialogue criptaDialogue;
    [SerializeField] private CanvasCombat canvasCombat;
    private PlayerController playerController;
    private PenitentController penitentController;
    private SPenitent penitentSelected;
    private Texture2D[] penitentImages;
    private string typeDialogue;


    public Action onEndAction;

    public SPenitent PenitentSelected { get => penitentSelected; }
    public string TypeDialogue { get => typeDialogue; }

    private void Start()
    {
        voteCanvas.OnCulpritSelected += HandlePenitentSelected; // Suscribirse al evento de selección de culpable
        criptaDialogue.onDialogueDecisionEnd += ActiveDialogueDecision;
        canvasCombat.Boss.SetActive(false);
        criptaDialogue.IsPunish += isPunish =>
        {
            if (isPunish)
            {
                Debug.Log($"Penitente {penitentSelected.CharacterName} castigado.");
                if (penitentSelected.isGuilty)
                {
                    Debug.Log($"Penitente {penitentSelected.CharacterName} era culpable. Castigo aplicado correctamente.");
                    typeDialogue = "P";
                }
                else
                {
                    Debug.Log($"Penitente {penitentSelected.CharacterName} era inocente. Castigo aplicado incorrectamente.");
                    Dialog selectedDialog = GetDialogueByType(penitentSelected, "P");
                }
            }
            else
            {
                Dialog selectedDialog = GetDialogueByType(penitentSelected, "F");
                if (penitentSelected.isGuilty)
                {
                    Debug.Log($"Penitente {penitentSelected.CharacterName} era culpable. Perdón aplicado incorrectamente.");
                    typeDialogue = "F";
                    //ActiveCombat();
                }
                else
                {
                    Debug.Log($"Penitente {penitentSelected.CharacterName} era inocente. Perdón aplicado correctamente.");
                    criptaDialogue.InitializeDecision(playerController.PlayerConversant, "F");
                    playerController.PlayerConversant.StartDialogue(selectedDialog);
                    onEndAction.Invoke();
                }
            }
        };
    }

    private void ActiveDialogueDecision()
    {
        playerController.PlayerConversant.QuitDialogue();
        //ActiveCombat();
    }

    private void ActiveCombat()
    {
        canvasCombat.gameObject.SetActive(true);
        canvasCombat.Initialize(playerController, this);
    }

    public void Initialize(PlayerController playerController)
    {
        this.playerController = playerController;
    }
    public void Initialize(PlayerController playerController, PenitentController penitentController)
    {
        //Debug.Log("A_confecciones - Initialize invoked.");
        this.playerController = playerController;
        this.penitentController = penitentController;
    }

    public void SetDay(int day)
    {
        dayToActivate = day;
        todayPenitents = penitentController.GetSPenitents(day);
        todayPenintentIndex = 0;
    }
    public void EjecutarAccion(PlayerController playerController)
    {

    }
    public void TriggerAction()
    {
        voteCanvas.gameObject.SetActive(true);
        voteCanvas.Initialize(penitentController, playerController, dayToActivate);

    }
    public void CancelAction()
    {

    }

    public void DebugAccion()
    {

    }
    private void HandlePenitentSelected(SPenitent penitent)
    {
        Debug.Log($"Penitente seleccionado: {penitent.CharacterName}");
        penitentSelected = penitent;
        Dialog selectedDialog = GetDialogueByType(penitentSelected, "C");
        if (selectedDialog == null)
        {
            Debug.LogError($"No se encontró diálogo para {penitentSelected.CharacterName}. No se inicia diálogo.");
            return;
        }
        voteCanvas.gameObject.SetActive(false);
        criptaDialogue.gameObject.SetActive(true);
        criptaDialogue.Initialize(playerController.PlayerConversant);
        Sprite sprite = GetCurrentPeninentSprite();
        criptaDialogue.SetupSpeakerSprite(sprite);
        playerController.PlayerConversant.StartDialogue(selectedDialog);

    }

    private Dialog GetDialogueByType(SPenitent penitent, String type)
    {
        if (penitentSelected == null) return null;

        Debug.Log($"Obteniendo diálogo tipo '{type}' para {penitentSelected.CharacterName} en el día {dayToActivate}");

        Dialog dialog = penitentSelected.GetDialogByTypeAndDay(type, dayToActivate);

        if (dialog == null)
        {
            dialog = penitentSelected.GetDialogByType(type);
        }

        if (dialog != null)
        {
            playerController.PlayerConversant.CurrentSpeakerNPC = penitentSelected.CharacterName;
            Debug.Log($"Diálogo encontrado: {dialog.name} para {penitentSelected.CharacterName}");
        }
        else
        {
            Debug.LogWarning($"No se encontró diálogo tipo {type} para {penitentSelected.CharacterName}");
        }

        return dialog;
    }
    public Sprite GetCurrentPeninentSprite()
    {
        if (penitentSelected != null)
        {
            Texture2D[] textures = penitentSelected.GetTextures2D();
            if (textures != null && textures.Length > 0)
            {
                return Sprite.Create(textures[0], new Rect(0, 0, textures[0].width, textures[0].height), new Vector2(0.5f, 0.5f));
            }
        }
        return null;
    }
}
