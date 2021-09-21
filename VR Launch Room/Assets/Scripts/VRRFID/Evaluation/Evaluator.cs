using System;
using System.IO;
using UnityEngine;
using Valve.VR;


public class Evaluator : MonoBehaviour
{
    private string path;
    private float timer = 0f;
    private bool recordTime = false;
    private int stampNumber = 0;
    
    public SteamVR_Action_Boolean triggerClick;
    private const SteamVR_Input_Sources InputRightHand = SteamVR_Input_Sources.RightHand;
    private const SteamVR_Input_Sources InputLeftHand = SteamVR_Input_Sources.LeftHand;

    private System.IO.StreamWriter evaluationFile;

    public Transform headPosition;

    private void OnEnable()
    {
        triggerClick.AddOnStateDownListener(ReactToTriggerInput, InputRightHand);
        triggerClick.AddOnStateDownListener(ReactToTriggerInput, InputLeftHand);
    }

    private void OnDisable()
    {
        triggerClick.RemoveOnStateDownListener(ReactToTriggerInput, InputRightHand);
        triggerClick.RemoveOnStateDownListener(ReactToTriggerInput, InputLeftHand);
    }

    private void ReactToTriggerInput(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        switch (fromSource)
        {
            case SteamVR_Input_Sources.LeftHand:
                RecordTimeStamp("Left");
                break;
            case SteamVR_Input_Sources.RightHand:
                RecordTimeStamp("Right");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(fromSource), fromSource, null);
        }
    }

    public void StartEvaluation()
    {
        stampNumber = 0;
        recordTime = true;

        evaluationFile = new System.IO.StreamWriter(path + "eval_" + DateTime.Now.ToString().Replace(":","_").Replace(" ", "_") + ".txt", false);
        //evaluationFile.WriteLine("Starting Evaluation Session: " + DateTime.Now);
        evaluationFile.WriteLine("Timestamp;PositionX;PositionY;PositionZ;LookX;LookY;LookZ");
        
        InvokeRepeating("StorePosition",0f,0.5f);
        
        Debug.Log("Adding Evaluation data to: " +path + "eval_" + DateTime.Now.ToString().Replace(":","_").Replace(" ", "_") + ".txt");
    }

    private void RecordTimeStamp(string hand)
    {
        if (!recordTime)
            return; 
        
        //evaluationFile.WriteLine("Stamp #" + stampNumber + " " + timer + " Seconds (" + hand +" Hand)");
        stampNumber++;
    }

    void OnDestroy()
    {
        if(evaluationFile != null)
            evaluationFile.Close();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        path = System.IO.Path.Combine(Application.dataPath,"Evaluation/");
        if (!Directory.Exists(System.IO.Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (recordTime)
        {
            timer += Time.deltaTime;
        }
    }

    void StorePosition()
    {
        Vector3 pos = headPosition.position;
        Ray ray = new Ray(pos, headPosition.forward);
 
        evaluationFile.WriteLine(timer + ";"+ pos.x + ";" + pos.y + ";" + pos.z + ";" + ray.direction.x + ";" + ray.direction.y +";" + ray.direction.z);
    }
}

