using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FootstepTrigger : MonoBehaviour
{
    public UnityEvent stepEvent;

    private AvatarBehaviour avatar;
    // Start is called before the first frame update
    void Start()
    {
        avatar = GetComponentInParent<AvatarBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (avatar.CurrentState != AvatarBehaviour.AnimationState.Walking)
            return;

        if(other.CompareTag("Steppable"))
            stepEvent.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
       
    }

    private void OnTriggerStay(Collider other)
    {
       // Debug.Log("stepping on: " + other.gameObject.name);
    }
}
