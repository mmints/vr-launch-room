using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class AvatarBehaviour : MonoBehaviour
{
    public enum AnimationState
    {
        Walking,
        Flying,
        Rotating
    }
    
    [SerializeField] private UnityEvent clickedEvent;
    // Steam VR Interaction
    public SteamVR_Action_Boolean triggerClick;
    private const SteamVR_Input_Sources InputRightHand = SteamVR_Input_Sources.RightHand;
    private const SteamVR_Input_Sources InputLeftHand = SteamVR_Input_Sources.LeftHand;
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    
    // animation behaviour and waypoints
    [SerializeField] private Transform followObject;
    [SerializeField] private float maxSpeed = 3f;
    [SerializeField] private float minSpeed = 1.5f;
    [SerializeField] private float rollTransitionThreshold = 10000f;
    [SerializeField] private float minDestinationDistance = 0.05f;
    [SerializeField] private float rotateSpeed = 3f;

    [SerializeField] private float animationTimeOpen = 5f;
    [SerializeField] private float animationTimeClose = 1.5f;
    
    private Animator animator;
    private NavMeshAgent navAgent;
    private ParticleSystem myParticleSystem;
    private SphereCollider avatarCollider;
    
    private bool isReady;
    private bool rotateTowards = false;

    [SerializeField] private GameObject currentWayPoint;
    [SerializeField] private GameObject targetWayPoint;

    private AnimationState currentState;
    
    private void Awake()
    {
        currentState = AnimationState.Rotating;
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        myParticleSystem = GetComponent<ParticleSystem>();
        avatarCollider = transform.GetComponentInChildren<SphereCollider>();

        isReady = false;

        Invoke("Walk", 3.5f);
    }

    public AnimationState CurrentState => currentState;

    public GameObject CurrentWayPoint => currentWayPoint;
    
    public GameObject TargetWayPoint
    {
        get => targetWayPoint;
        set
        {
            targetWayPoint = value;
            if(navAgent.enabled)
                navAgent.destination = targetWayPoint.transform.position;
        } 
    }

    public bool HasReached(GameObject wayPoint)
    {
        return wayPoint == currentWayPoint;
    }

    public void SetParticleFilterState(bool newstate)
    {
        if(newstate)
            myParticleSystem.Play();
        else
            myParticleSystem.Stop();
    }

    IEnumerator DelayedPointRight(float sec)
    {
        yield return new WaitForSeconds(sec);
        animator.SetBool("PointRight_Anim", true);
    }
    
    IEnumerator DelayedPointLeft(float sec)
    {
        yield return new WaitForSeconds(sec);
        animator.SetBool("PointLeft_Anim", true);
    }
    
    IEnumerator DelayedTap(float sec)
    {
        yield return new WaitForSeconds(sec);
        animator.SetBool("Tap_Anim", true);
    }
    
    public void TriggerTapChamber(float waitTime = 0f)
    {
        StartCoroutine("DelayedTap", waitTime);
    }

    public void TriggerPointRight(float waitTime = 0f)
    {
        StartCoroutine("DelayedPointRight", waitTime);
    }
    
    public void TriggerPointLeft(float waitTime = 0f)
    {
        StartCoroutine("DelayedPointLeft", waitTime);
    }

    public void SetRoboterCloseState(bool newState)
    {
        animator.SetBool("Open_Anim", newState);
    }

    public void SetWalkingState(bool newState)
    {
        animator.SetBool("Walk_Anim", newState);
    }

    public void SetRotationTarget(Transform target)
    {
        followObject = target;
    }

    public void StopRotateTowards()
    {
        rotateTowards = false;
    }

    public bool IsAnimationFinished(string name)
    {
        bool playing = animator.GetCurrentAnimatorStateInfo(0).length > animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).length);
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName(name));
        
        return !(playing);
    }
    
    private void UpdateSpeed()
    {
        Vector3 deltaPos = transform.position - navAgent.destination;
    }

    private bool DestinationReached(Transform wayPoint)
    {
        if ((transform.position - wayPoint.position).magnitude <= minDestinationDistance)
            return true;

        return false;
    }
    private void UpdateWayPointEvents()
    {
        if(targetWayPoint == currentWayPoint)
            return;
        
        if (DestinationReached(targetWayPoint.transform))
        {
            currentWayPoint = targetWayPoint;
            WayPointEvent e = currentWayPoint.GetComponent<WayPointEvent>();
            if(e != null)
                e.Execute();
        }
    }
    
    public IEnumerator UpdateAnimationState(AnimationState newState)
    {
        if (newState == currentState)
            yield return new WaitForSeconds(0f);

        switch (newState) // handle state changes
        {
            case AnimationState.Walking:
                rotateTowards = false;
                if (currentState == AnimationState.Flying)
                {
                    myParticleSystem.Stop();
                    animator.SetBool("Open_Anim", true);
                    yield return new WaitForSeconds(animationTimeOpen);
                }
                animator.SetBool("Walk_Anim", true);
                break;
            case AnimationState.Flying:
                rotateTowards = false;
                animator.SetBool("Walk_Anim", false);
                animator.SetBool("Open_Anim", false);
                yield return new WaitForSeconds(animationTimeClose);
                myParticleSystem.Play();
                break;
            case AnimationState.Rotating:
                if (currentState == AnimationState.Flying)
                {
                    myParticleSystem.Stop();
                    animator.SetBool("Open_Anim", true);
                    yield return new WaitForSeconds(animationTimeOpen);
                }
                animator.SetBool("Walk_Anim", false);
                rotateTowards = true;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        currentState = newState;

        yield return new WaitForSeconds(0f);
    }

    private void Walk()
    {
        isReady = true;
        navAgent.enabled = true;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!isReady)
            return;
       
        UpdateSpeed();
        UpdateWayPointEvents();

        if (rotateTowards)
        {
            followObject = GameObject.Find("VRCamera").transform;
            RotateTowardsUpdate(followObject.position);
        }
        
    }

    private void RotateTowardsUpdate(Vector3 target)
    {
        Vector3 targetDir = target - transform.position;

        float step = rotateSpeed * Time.deltaTime;

        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0f);
        Debug.DrawRay(transform.position, newDir, Color.red);

        transform.rotation = Quaternion.LookRotation(newDir);
    }
    
    private void OnEnable()
    {
        triggerClick.AddOnStateDownListener(HandleUserInput, InputRightHand);
        triggerClick.AddOnStateDownListener(HandleUserInput, InputLeftHand);
    }

    private void OnDisable()
    {
        triggerClick.RemoveOnStateDownListener(HandleUserInput, InputRightHand);
        triggerClick.RemoveOnStateDownListener(HandleUserInput, InputLeftHand);
    }
    
    private void HandleUserInput(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        switch (fromSource)
        {
            case SteamVR_Input_Sources.LeftHand:
                var lHand = leftHand.GetComponent<Hand>();
                if (avatarCollider.bounds.Contains(lHand.transform.position))
                {
                    clickedEvent.Invoke();
                }
                break;
            case SteamVR_Input_Sources.RightHand:
                var rHand = rightHand.GetComponent<Hand>();
                if (avatarCollider.bounds.Contains(rHand.transform.position))
                {
                    clickedEvent.Invoke();
                }
                break;
            case SteamVR_Input_Sources.Any:
                break;
            case SteamVR_Input_Sources.LeftFoot:
                break;
            case SteamVR_Input_Sources.RightFoot:
                break;
            case SteamVR_Input_Sources.LeftShoulder:
                break;
            case SteamVR_Input_Sources.RightShoulder:
                break;
            case SteamVR_Input_Sources.Waist:
                break;
            case SteamVR_Input_Sources.Chest:
                break;
            case SteamVR_Input_Sources.Head:
                break;
            case SteamVR_Input_Sources.Gamepad:
                break;
            case SteamVR_Input_Sources.Camera:
                break;
            case SteamVR_Input_Sources.Keyboard:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(fromSource), fromSource, null);
        }
    }
}
