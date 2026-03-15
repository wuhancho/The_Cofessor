using System;
using System.Collections;
using The_cofessor.Personajes.Dialogs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class A_Decision : MonoBehaviour, IAcciones
{
    [SerializeField, IReadOnly] private int dayToActivate;
    private SPenitent[] todayPenitents;
    private int todayPenintentIndex;
    [SerializeField] private VoteCanvas voteCanvas;
    [SerializeField] private CriptaDialogue criptaDialogue;
    [SerializeField] private CanvasCombat canvasCombat;
    [SerializeField] private float timeToWaitAfterDialogue = 2f;
    private PlayerController playerController;
    private PenitentController penitentController;
    private SPenitent penitentSelected;
    private Texture2D[] penitentImages;
    private string typeDialogue;


    public Action onEndAction;
    [Header("No entrar en combate.")]
    [Tooltip("Invocado cuando el jugador decide a un inocente ya sea culpandolo o perdonandolo o perdona a un culpable. ")]
    public UnityEvent onNotEnterInCombat;
    [Header("Combate.")]
    public UnityEvent onCombat;
    public UnityEvent onEndCombat;
    [Header("Juicio del penitente.")]
    public UnityEvent onPenitentJugedment;
    [Header("Decisión de castigo.")]
    [Tooltip("Invocado cuando se selecciona un penitente que es inocente.")]
    public UnityEvent onPenitentSelectedInocent;
    [Tooltip("Invocado cuando se selecciona un penitente que es culpable.")]
    public UnityEvent onPenitentSelectedPunish;



    public SPenitent PenitentSelected { get => penitentSelected; }
    public string TypeDialogue { get => typeDialogue; }

    private void Start()
    {
        voteCanvas.gameObject.SetActive(true);
        voteCanvas.OnCulpritSelected += HandlePenitentSelected; // Suscribirse al evento de selección de culpable
        //criptaDialogue.onDialogueDecisionEnd += ActiveDialogueDecision;
        canvasCombat.Boss.SetActive(false);
        criptaDialogue.IsPunish += isPunish =>
        {
            onPenitentJugedment?.Invoke();
            if (isPunish)
            {
                Dialog selectedDialog = GetDialogueByType(penitentSelected, "P");
                typeDialogue = "P";
                Debug.Log($"Penitente {penitentSelected.CharacterName} castigado.");
                if (penitentSelected.isGuilty)
                {
                    Debug.Log($"Penitente {penitentSelected.CharacterName} era culpable. Castigo aplicado correctamente.");
                    ActiveDialogueDecisionCulprits(selectedDialog);
                    onPenitentSelectedPunish?.Invoke();
                }
                else
                {
                    Debug.Log($"Penitente {penitentSelected.CharacterName} era inocente. Castigo aplicado incorrectamente.");
                    ActiveDialogueDecisionInocent(selectedDialog);
                    onPenitentSelectedInocent?.Invoke();
                }
            }
            else
            {
                Dialog selectedDialog = GetDialogueByType(penitentSelected, "F");
                typeDialogue = "F";
                if (penitentSelected.isGuilty)
                {
                    Debug.Log($"Penitente {penitentSelected.CharacterName} era culpable. Perdón aplicado incorrectamente.");
                    ActiveDialogueDecisionInocent(selectedDialog);
                    onPenitentSelectedPunish?.Invoke();
                }
                else
                {
                    Debug.Log($"Penitente {penitentSelected.CharacterName} era inocente. Perdón aplicado correctamente.");
                    ActiveDialogueDecisionInocent(selectedDialog);
                    onPenitentSelectedInocent?.Invoke();
                }
            }
        };
    }
    void DeSubcripcion()
    {
        criptaDialogue.DeSubcripcionEvent(true, true);
    }

    private void ActiveDialogueDecisionCulprits(Dialog dialog)
    {
        playerController.PlayerConversant.QuitDialogue();
        playerController.PlayerConversant.StartDialogue(dialog);
        criptaDialogue.InitializeDecision(playerController.PlayerConversant, typeDialogue);
        criptaDialogue.onDialogueDecisionEnd += () =>
        {
            Debug.Log("Diálogo de culpable finalizado. Se activa combate.");
            StartCoroutine(WaitForDialogueEnd(() =>
            {
                onCombat?.Invoke();
                FadeController.Instance.FadeOut(timeToWaitAfterDialogue, () =>
                {
                    FadeController.Instance.FadeIn(1);
                    ActiveCombat();
                });
            }));
            DeSubcripcion();
        };
        //ActiveCombat();
    }
    private void ActiveDialogueDecisionInocent(Dialog dialog)
    {
        playerController.PlayerConversant.QuitDialogue();
        playerController.PlayerConversant.StartDialogue(dialog);
        criptaDialogue.InitializeDecision(playerController.PlayerConversant, typeDialogue);
        criptaDialogue.onDialogueDecisionEnd += () =>
        {
            DeSubcripcion();
            Debug.Log("Diálogo de inocente finalizado. No se activa combate.");
            onNotEnterInCombat?.Invoke();
            StartCoroutine(WaitForDialogueEnd(() =>
            {
                FadeController.Instance.FadeOut(timeToWaitAfterDialogue, () =>
                {
                    onEndAction?.Invoke();
                    FadeController.Instance.FadeIn(1);
                    onEndCombat?.Invoke();
                });
            }));
        };

    }

    private void ActiveCombat()
    {
        criptaDialogue.gameObject.SetActive(false);
        playerController.PlayerConversant.QuitDialogue();
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
    public IEnumerator WaitForDialogueEnd(Action onAction)
    {
        float elapsedTime = 0f;
        if (playerController.PlayerConversant.IsActive())
        {
            while (elapsedTime < 1)
            {
                elapsedTime += Time.deltaTime;
                Debug.Log($"Esperando a que termine el diálogo... {elapsedTime:F2}/{1} segundos");
                yield return null;
            }
            onAction?.Invoke();
        }

    }
}
