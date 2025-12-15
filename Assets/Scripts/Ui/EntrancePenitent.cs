using UnityEngine;

public class EntrancePenitent : MonoBehaviour
{
    [SerializeField] private Animator entranceAnimation;
    [SerializeField] private float displayDuration;




    public float DisplayDuration { get => displayDuration; set => displayDuration = value; }
    private void Start()
    {
        //StopAnimation();
    }

    public void PlayEntranceAnimation(bool isChangeEntrance)
    {
        //entranceAnimation.enabled = true;
        if (isChangeEntrance == true)
        {
            entranceAnimation.SetBool("changeEntrance", isChangeEntrance);
        }
    }
    public void PlayExitAnimation(bool isChangeEntrance)
    {
        if (isChangeEntrance == false)
        {
            entranceAnimation.SetBool("changeEntrance", isChangeEntrance);
        }
        //entranceAnimation.enabled = true;
    }
    public void StopAnimation()
    {
        entranceAnimation.StopPlayback();
    }
}
