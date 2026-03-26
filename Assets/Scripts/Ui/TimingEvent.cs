using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TimingEvent : MonoBehaviour
{
    [SerializeField] private float delay;

    [SerializeField] private UnityEvent onTimeEnd;

    private Coroutine timerCoroutine;

    public float Delay { get => delay; set => delay = value; }

    public void StartTimer()
    {
        StopTimer();
        timerCoroutine = StartCoroutine(TimerCoroutine());
    }

    public void StopTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }

    private IEnumerator TimerCoroutine()
    {
        yield return new WaitForSeconds(delay);
        onTimeEnd.Invoke();
        timerCoroutine = null;
    }
}
