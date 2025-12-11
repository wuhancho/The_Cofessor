using UnityEngine;

public class EntrancePenitent : MonoBehaviour
{
    [SerializeField] private Animator entranceAnimation;
    [SerializeField] private float displayDuration;



    public float DisplayDuration { get => displayDuration; set => displayDuration = value; }
    private void Start()
    {
        StopAnimation();
    }
    public void PlayEntranceAnimation()
    {
        entranceAnimation.enabled = true;
        if (entranceAnimation != null)
        {
            entranceAnimation.SetTrigger("changeEntrance");
        }
        else
        {
            Debug.LogWarning("Entrance animation reference is not set.");
        }
    }
    public void PlayExitAnimation()
    {
        entranceAnimation.enabled = true;
        if (entranceAnimation != null)
        {
            entranceAnimation.SetTrigger("changeEntrance");
        }
        else
        {
            Debug.LogWarning("Exit animation reference is not set.");
        }
    }
    public void StopAnimation()
    {
        entranceAnimation.enabled = false;
    }
}
