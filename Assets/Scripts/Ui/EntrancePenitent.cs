using UnityEngine;

public class EntrancePenitent : MonoBehaviour
{
    [SerializeField] private Animator entranceAnimation;
    [SerializeField] private float entranceDuration;
    [SerializeField] private float exitDuration;

    public float EntranceDuration { get => entranceDuration; }
    public float ExitDuration { get => exitDuration; }




    //public float DisplayDuration { get => displayDuration; set => displayDuration = value; }
    private void Start()
    {
        //StopAnimation();
    }

    public void PlayEntranceAnimation(/*bool isChangeEntrance*/)
    {
        //entranceAnimation.enabled = true;
        //if (isChangeEntrance == true)
        //{
        entranceAnimation.SetBool("changeEntrance", true);
        //}
    }
    public void PlayExitAnimation(/*bool isChangeEntrance*/)
    {
        //if (isChangeEntrance == false)
        //{
        entranceAnimation.SetBool("changeEntrance", false);
        //}
        //entranceAnimation.enabled = true;
    }

    public void StopAnimation()
    {
        entranceAnimation.StopPlayback();
    }
}
