using UnityEngine;

public class EventDayManager : MonoBehaviour
{
    private Dia dia;
    private PlayerController playerController;
    private PenitentController penitentController;
    private int numberDay;
    [Header("Arrastra los eventos aqui")]
    [SerializeReference] private DayEvent[] eventDays;
    public void Initialized(Dia d, PlayerController controller, PenitentController penitent)
    {
        dia = d;
        playerController = controller;
        penitentController = penitent;
        numberDay = dia.GetNumberDay();
    }
    public DayEvent GetTypeEvent(int day)
    {
        foreach (DayEvent eventDay in eventDays)
        {
            if (eventDay.Day == day)
            {
                return eventDay;
            }
        }
        return null;
    }


}
