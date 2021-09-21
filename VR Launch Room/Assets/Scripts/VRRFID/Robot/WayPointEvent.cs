using UnityEngine;
using UnityEngine.Events;

public class WayPointEvent : MonoBehaviour
{
    public UnityEvent myEvent;

    private void Awake()
    {
        if(myEvent == null)
            myEvent = new UnityEvent();
    }

    public void Execute()
    {
        myEvent.Invoke();
    }
}
