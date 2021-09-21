using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Scenario : MonoBehaviour
{
    // Head Transform
    [SerializeField] private Transform headTransform;
    [SerializeField] private Path waypoints;
    [SerializeField] private AvatarBehaviour avatar;
    [SerializeField] private RFIDMount rfidMount;
    [SerializeField] private DoorController rfidChamberDoor;
    [SerializeField] private Display rfidDisplay;
    [SerializeField] private GameObject rfidChips;
    [SerializeField] private GameObject hologram;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject stopButton;
    [SerializeField] private GameObject changeModeButton;
    [SerializeField] private GameObject axisButton;
    [SerializeField] private GameObject handHints;
    
    [SerializeField] private GameObject thresholdArrow1;
    [SerializeField] private GameObject thresholdArrow2;
    [SerializeField] private GameObject thresholdArrow3;
    [SerializeField] private GameObject thresholdArrow4;
    [SerializeField] private GameObject thresholdArrow5;
    
    [SerializeField] private GameObject orientationArrow1;
    [SerializeField] private GameObject orientationArrow2;
    [SerializeField] private GameObject orientationArrow3;
    
    [SerializeField] private GameObject orientationResultArrow1;
    [SerializeField] private GameObject orientationResultArrow2;
    
    [SerializeField] private Transform RFIDSpawnPoint;
    [SerializeField] private GameObject RFIDTutorialChip;
    
    private AudioController audioController;
    private bool avatarClicked = false;
    private bool spaceClicked = false;
    private bool startClicked = false;
    private bool tagAttached = false;
    
    private Dictionary<RFID_Tag, RFID_Transponder> availableTransponders;
    public Dictionary<RFID_Tag, RFID_Transponder> AvailableTransponders => availableTransponders;
    
    // Start is called before the first frame update
    void Start()
    {
        audioController = GetComponent<AudioController>();
        LoadExperimentData();
        avatarClicked = false;
        
        // start tutorial!
        StartCoroutine(StartScenario());
        SetActiveAllRFIDChips(false);
    }
    

    private IEnumerator StartScenario()
    {
        avatar.SetRotationTarget(headTransform);
        
        //yield return WaitAvatarClicked();
        yield return WaitSpaceClicked();
      
        startButton.SetActive(false);
        stopButton.SetActive(false);
        changeModeButton.SetActive(false);
        axisButton.SetActive(false);
        GetComponent<Evaluator>().StartEvaluation();

        SetActiveRFIDChip(RFID_Tag.A02,Substrate.Aluminium, true);
        
        yield return audioController.PlayWelcome();
        yield return WaitAvatarClicked();
        //yield return WaitSpaceClicked();

        yield return MoveAvatarTo("NavMeshDesk", AvatarBehaviour.AnimationState.Walking);
        yield return MoveAvatarTo("InFrontOfDesk", AvatarBehaviour.AnimationState.Flying);
        yield return MoveAvatarTo("InFrontOfChamberDesk", AvatarBehaviour.AnimationState.Walking);
        
        yield return MoveAvatarTo("ChamberDeskEntry", AvatarBehaviour.AnimationState.Flying);
        yield return MoveAvatarTo("ChamberDesk", AvatarBehaviour.AnimationState.Walking,AvatarBehaviour.AnimationState.Rotating);

        avatar.TriggerTapChamber(3f);
        yield return audioController.PlayLetsGo();

        yield return WaitMagnifyingGlass();
        StartCoroutine(EnableGameobjectDelayed(thresholdArrow1, 4f, 12f));
        StartCoroutine(EnableGameobjectDelayed(thresholdArrow2, 4f, 12f));
        StartCoroutine(EnableGameobjectDelayed(thresholdArrow3, 16f, 5f));
        StartCoroutine(EnableGameobjectDelayed(thresholdArrow4, 20f, 20f));
        StartCoroutine(EnableGameobjectDelayed(thresholdArrow5, 40f, 10f));
        yield return audioController.PlaySweepSettings();
        
        yield return audioController.PlayOpenChamber();
        handHints.SetActive(true);
        yield return WaitChamberOpened();
        handHints.SetActive(false);
        
        yield return MoveAvatarTo("ChamberDeskEntry", AvatarBehaviour.AnimationState.Walking);
        yield return MoveAvatarTo("InFrontOfChamberDesk", AvatarBehaviour.AnimationState.Flying);
        yield return MoveAvatarTo("InFrontOfChamber", AvatarBehaviour.AnimationState.Walking);
        yield return MoveAvatarTo("ChamberEntry", AvatarBehaviour.AnimationState.Flying);
        yield return MoveAvatarTo("Chamber", AvatarBehaviour.AnimationState.Flying, AvatarBehaviour.AnimationState.Rotating);

        avatar.TriggerPointRight(13f);
        yield return audioController.PlayInsideChamber();

        yield return MoveAvatarTo("ChamberEntry", AvatarBehaviour.AnimationState.Flying);
        yield return MoveAvatarTo("InFrontOfChamber", AvatarBehaviour.AnimationState.Flying);
        yield return MoveAvatarTo("InFrontOfDesk", AvatarBehaviour.AnimationState.Walking);
        yield return MoveAvatarTo("NavMeshDesk", AvatarBehaviour.AnimationState.Flying);
        yield return MoveAvatarTo("Desk", AvatarBehaviour.AnimationState.Walking, AvatarBehaviour.AnimationState.Rotating);
        
        yield return audioController.PlayChooseTag();
        //todo: tag auswahl event
        yield return audioController.PlayNiceTag();
        
        yield return MoveAvatarTo("NavMeshDesk", AvatarBehaviour.AnimationState.Walking);
        yield return MoveAvatarTo("InFrontOfDesk", AvatarBehaviour.AnimationState.Flying);
        yield return MoveAvatarTo("InFrontOfChamberDesk", AvatarBehaviour.AnimationState.Walking);
        yield return MoveAvatarTo("ChamberDeskEntry", AvatarBehaviour.AnimationState.Flying);
        yield return MoveAvatarTo("ChamberDesk", AvatarBehaviour.AnimationState.Walking, AvatarBehaviour.AnimationState.Rotating);
        
        yield return WaitRFIDMounted();
        yield return audioController.PlayCloseChamber();
        yield return WaitChamberClosed();
        yield return audioController.PlayCloseChamberFinished();
        
        startButton.SetActive(true);
        yield return WaitStartClicked();
        startButton.SetActive(false);
        
        yield return audioController.PlayMeasurementIsRunning();
        yield return audioController.PlayMeasurementIsFinished();
        yield return audioController.PlayMeasurementIsFinished2();
        
        rfidDisplay.ToggleResultDisplay();
        
        yield return audioController.PlayReadRange();
        axisButton.SetActive(true);
        
        changeModeButton.SetActive(true);
        yield return WaitForScanMode(Display.ScanMode.Orientation);
        changeModeButton.SetActive(false);

        StartCoroutine(EnableGameobjectDelayed(orientationArrow1, 6f, 19f));
        StartCoroutine(EnableGameobjectDelayed(orientationArrow2, 25f, 5f));
        StartCoroutine(EnableGameobjectDelayed(orientationArrow3, 30f, 10f));
        yield return audioController.PlayOrientationDescription();
        startButton.SetActive(true);
        yield return WaitStartClicked();
        startButton.SetActive(false);
        yield return audioController.PlayOrientationStarted();
        
        StartCoroutine(EnableGameobjectDelayed(orientationResultArrow1, 15f, 7f));
        StartCoroutine(EnableGameobjectDelayed(orientationResultArrow2, 23f, 5f));
        yield return audioController.PlayOrientationFinished();
        
        yield return audioController.PlayCongratulation();

        yield return WaitRFIDUnMounted();
        yield return audioController.PlayWellDone();
        
        startButton.SetActive(true);
        stopButton.SetActive(true);
        changeModeButton.SetActive(true);
        axisButton.SetActive(false); // todo: enable if all screenshots are ready

        SetActiveAllRFIDChips(true);

    }

    private IEnumerator EnableGameobjectDelayed(GameObject go, float delay, float duration = 0f)
    {
        yield return new WaitForSeconds(delay);
        go.SetActive(true);

        if (duration > 0)
        {
            yield return new WaitForSeconds(duration);
            go.SetActive(false);
        }
    }
    
    private IEnumerator MoveAvatarTo(string waypoint, AvatarBehaviour.AnimationState movementState)
    {
        yield return avatar.UpdateAnimationState(movementState);
        avatar.TargetWayPoint = waypoints.GetWayPoint(waypoint);
        yield return new WaitUntil(() => avatar.HasReached(waypoints.GetWayPoint(waypoint)));
    }
    
    private IEnumerator MoveAvatarTo(string waypoint, AvatarBehaviour.AnimationState movementState, AvatarBehaviour.AnimationState reachedState)
    {
        yield return avatar.UpdateAnimationState(movementState);
        avatar.TargetWayPoint = waypoints.GetWayPoint(waypoint);
        yield return new WaitUntil(() => avatar.HasReached(waypoints.GetWayPoint(waypoint)));
        yield return avatar.UpdateAnimationState(reachedState);
    }
    private IEnumerator WaitAvatarClicked()
    {
        audioController.DontRepeat = true;
        yield return new WaitUntil(() => avatarClicked);
        avatarClicked = false;
        audioController.DontRepeat = false;
    }
    
    private IEnumerator WaitSpaceClicked()
    {
        yield return new WaitUntil(() => spaceClicked);
        spaceClicked = false;
    }
    
    private IEnumerator WaitStartClicked()
    {
        yield return new WaitUntil(() => startClicked);
        startClicked = false;
    }
    
    private IEnumerator WaitChamberOpened()
    {
        yield return new WaitUntil(() => rfidChamberDoor.IsChamberDoorOpen);
    }
    
    private IEnumerator WaitMagnifyingGlass()
    {
        yield return new WaitUntil(() => hologram.activeSelf);
    }
    
    private IEnumerator WaitForScanMode(Display.ScanMode mode)
    {
        yield return new WaitUntil(() => rfidDisplay.CurrentScanMode == mode);
    }
    
    private IEnumerator WaitTagAttached()
    {
        yield return new WaitUntil(() => tagAttached);
    }
    
    private IEnumerator WaitChamberClosed()
    {
        yield return new WaitUntil(() => (rfidChamberDoor.IsChamberLocked));
    }
    
    private IEnumerator WaitRFIDMounted()
    {
        yield return new WaitUntil(() => rfidMount.IsMounted);
    }
    
    private IEnumerator WaitRFIDUnMounted()
    {
        yield return new WaitUntil(() => !rfidMount.IsMounted);
    }

    private void SetActiveAllRFIDChips(bool newState)
    {
        foreach (Transform chip in rfidChips.transform)
        {
            chip.gameObject.SetActive(newState);
        }
    }

    private void SetActiveRFIDChip(RFID_Tag tag, Substrate substrate, bool newstate)
    {
        foreach (Transform chip in rfidChips.transform)
        {
            RFIDSample sample = chip.GetComponent<RFIDSample>();

            if (sample.rfid_tag == tag && sample.substrate == substrate)
                chip.gameObject.SetActive(newstate);
        }
    }
    
    public void Update()
    {
        // todo: just for testing. remove for VR scenario
        spaceClicked = Input.GetKeyDown("space");

        if (Input.GetKeyDown("backspace"))
            RFIDTutorialChip.transform.position = RFIDSpawnPoint.position;
    }

    public void StartExperiment()
    {
        startClicked = true;
    }
    public void StopExperiment()
    {
        startClicked = false;
    }

    public bool AvatarClicked
    {
        get => avatarClicked;
        set => avatarClicked = value;
    }

    public bool TagAttached
    {
        get => tagAttached;
        set => tagAttached = value;
    }

    private void LoadExperimentData()
    {
        LoadTransponderInformation();
    }

    private void LoadTransponderInformation() // todo: load from xml file
    {
        availableTransponders = new Dictionary<RFID_Tag, RFID_Transponder>();
        availableTransponders.Add(RFID_Tag.TBN, new RFID_Transponder());
        availableTransponders.Add(RFID_Tag.A02, new RFID_Transponder());
        availableTransponders.Add(RFID_Tag.Inline, new RFID_Transponder());
        
        availableTransponders[RFID_Tag.TBN].Type = "TBN UHF RFID \"Delta\" Transponder passiv";
        availableTransponders[RFID_Tag.TBN].Manufacturer = "TBN";
        availableTransponders[RFID_Tag.TBN].ChipType = "NXP UCODE G2XM";
        
        availableTransponders[RFID_Tag.A02].Type = "A02 UHF RFID Transponder passiv";
        availableTransponders[RFID_Tag.A02].Manufacturer = "NXP Semiconductors N.V.";
        availableTransponders[RFID_Tag.A02].ChipType = "NXP UCODE G2XM";
        
        availableTransponders[RFID_Tag.Inline].Type = "RF Link UHF RFID Transponder passiv";
        availableTransponders[RFID_Tag.Inline].Manufacturer = "NXP Semiconductors N.V.";
        availableTransponders[RFID_Tag.Inline].ChipType = "SL3 ICS 10";
    }
}
