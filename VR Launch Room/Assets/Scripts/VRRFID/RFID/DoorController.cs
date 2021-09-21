using System;
using cakeslice;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class DoorController : MonoBehaviour
{
    private bool topHandleOpen = false;

    private bool topHandleHover = false;
    private bool bottomHandleHover = false;

    [SerializeField] private float maxAngle = 120f;
    [SerializeField] private AudioClip openingSound;
    [SerializeField] private AudioClip closingSound;
    
    [SerializeField] private BoxCollider bottomHandleCollider;
    [SerializeField] private BoxCollider topHandleCollider;
    
    [SerializeField] private GameObject topHandle;
    [SerializeField] private GameObject bottomHandle;

    private AudioSource doorAudioSource;
    private CircularDrive door;

    public bool twoHandsNeeded = true;


    public bool IsChamberLocked { get; set; } = true;
    public bool IsChamberDoorOpen { get; set; } = false;
    public bool BottomHandleOpen { get; set; } = false;

    public bool TopHandleHover
    {
        get => topHandleHover;
        set => topHandleHover = value;
    }

    public bool BottomHandleHover
    {
        get => bottomHandleHover;
        set => bottomHandleHover = value;
    }

    public void AdjustHighlightColor()
    {
        if ((!twoHandsNeeded) || (TopHandleHover && BottomHandleHover))
        {
            topHandle.GetComponent<Outline>().color = 1;
            bottomHandle.GetComponent<Outline>().color = 1;
            topHandle.GetComponent<CircularDrive>().pauseCircularDrive = false;
            bottomHandle.GetComponent<CircularDrive>().pauseCircularDrive = false;
        }
        else
        {
            topHandle.GetComponent<Outline>().color = 0;
            bottomHandle.GetComponent<Outline>().color = 0;
            topHandle.GetComponent<CircularDrive>().pauseCircularDrive = true;
            bottomHandle.GetComponent<CircularDrive>().pauseCircularDrive = true;
        }
           
    }

    private void Start()
    {
        door = GetComponent<CircularDrive>();
        door.maxAngle = maxAngle;
        doorAudioSource = GetComponent<AudioSource>();
        DisableDoor();
        GetComponent<Outline>().enabled = false;
        
        topHandle.GetComponent<Outline>().enabled = false;
        bottomHandle.GetComponent<Outline>().enabled = false;

        if (twoHandsNeeded)
        {
            topHandle.GetComponent<CircularDrive>().pauseCircularDrive = true;
            bottomHandle.GetComponent<CircularDrive>().pauseCircularDrive = true;
        }
    }

    private void Update()
    {
        if (door.outAngle <= 0)
        {
            bottomHandleCollider.enabled = true;
            topHandleCollider.enabled = true;
        }
        else
        {
            bottomHandleCollider.enabled = false;
            topHandleCollider.enabled = false;
        }
    }

    public void OpenTopHandle()
    {
        topHandleOpen = true;
        IsChamberLocked = false;

        if(BottomHandleOpen) 
            EnableDoor();
    }
    public void OpenBottomHandle()
    {
        BottomHandleOpen = true;
        IsChamberLocked = false;
        
        if(topHandleOpen) 
            EnableDoor();
    }
    public void CloseBottomHandle()
    {
        BottomHandleOpen = false;
        
        if(!topHandleOpen)
            LockChamberDoor();
            
        DisableDoor();
    }
    public void CloseTopHandle()
    {
        topHandleOpen = false;
        
        if(!BottomHandleOpen)
            LockChamberDoor();
        
        DisableDoor();
    }

    public void OpenChamberDoor()
    {
        IsChamberDoorOpen = true;
        doorAudioSource.clip = openingSound;
        doorAudioSource.Play();
        GetComponent<Outline>().enabled = true;
    }

    public void LockChamberDoor()
    {
        IsChamberLocked = true;
        doorAudioSource.clip = closingSound;
        doorAudioSource.Play();
    }
    
    public void CloseChamberDoor()
    {
        IsChamberDoorOpen = false;
    }

    private void DisableDoor()
    {
        GetComponent<BoxCollider>().enabled = false;
    }

    private void EnableDoor()
    {
        GetComponent<BoxCollider>().enabled = true;
    }
}
