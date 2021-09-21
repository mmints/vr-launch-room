using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Display : MonoBehaviour
{
    public enum ScanMode
    {
        Threshold,
        Orientation
    }

    private bool isScanning = false;
    [SerializeField] private TransmissionPowerIndicator powerIndicator;
    
    [SerializeField] private RFIDMount rfidMount;
    [SerializeField] private DoorController chamberDoor;
    [SerializeField] private TextMeshProUGUI modeText;
    [SerializeField] private TextMeshProUGUI stateText;

    [SerializeField] private TextMeshProUGUI firstText;
    [SerializeField] private TextMeshProUGUI firstValue;
    [SerializeField] private TextMeshProUGUI firstUnit;
    
    [SerializeField] private TextMeshProUGUI secondText;
    [SerializeField] private TextMeshProUGUI secondValue;
    [SerializeField] private TextMeshProUGUI secondUnit;
    
    [SerializeField] private TextMeshProUGUI thirdText;
    [SerializeField] private TextMeshProUGUI thirdValue;
    [SerializeField] private TextMeshProUGUI thirdUnit;
    
    [SerializeField] private TextMeshProUGUI fourthText;
    [SerializeField] private TextMeshProUGUI fourthValue;
    [SerializeField] private TextMeshProUGUI fourthUnit;

    [SerializeField] private GameObject toggleYAxis;
    
    private ScanMode currentScanMode = ScanMode.Threshold;
    public ScanMode CurrentScanMode => currentScanMode;
    
    [SerializeField] private Texture idleThreshold;
    [SerializeField] private Texture idleOrientation;
    [SerializeField] private List<Texture> a02AluThresholdList;
    [SerializeField] private List<Texture> a02AluOrientationList;
    [SerializeField] private List<Texture> a02PvcThresholdList;
    [SerializeField] private List<Texture> a02PvcOrientation;
    [SerializeField] private List<Texture> a02CardboardThresholdList;
    [SerializeField] private List<Texture> a02CardboardOrientation;

    [SerializeField] private List<Texture> inlineSteelThresholdList;
    [SerializeField] private List<Texture> inlineSteelOrientationList;
    [SerializeField] private List<Texture> inlinePvcThresholdList;
    [SerializeField] private List<Texture> inlinePvcOrientation;
    [SerializeField] private List<Texture> inlineWoodThresholdList;
    [SerializeField] private List<Texture> inlineWoodOrientation;
    
    [SerializeField] private List<Texture> tbnAluThresholdList;
    [SerializeField] private List<Texture> tbnAluOrientationList;
    [SerializeField] private List<Texture> tbnPvcThresholdList;
    [SerializeField] private List<Texture> tbnPvcOrientation;
    [SerializeField] private List<Texture> tbnWoodThresholdList;
    [SerializeField] private List<Texture> tbnWoodOrientation;
    [SerializeField] private Texture tbnTheoreticalReadRange;
    
    [SerializeField] private List<float> a02AluOrientationValues;
    [SerializeField] private List<float> a02PvcOrientationValues;
    [SerializeField] private List<float> a02CardboardOrientationValues;
    
    [SerializeField] private List<float> inlineSteelOrientationValues;
    [SerializeField] private List<float> inlinePvcOrientationValues;
    [SerializeField] private List<float> inlineWoodOrientationValues;
    
    [SerializeField] private List<float> tbnAluOrientationValues;
    [SerializeField] private List<float> tbnPvcOrientationValues;
    [SerializeField] private List<float> tbnWoodOrientationValues;

    private List<Texture> currentTextureList = new List<Texture>();
    
    [SerializeField] private float measurementTickThres = 2.25f;
    [SerializeField] private float measurementTickOri = 4f;
    [SerializeField] private ChamberTransparencyController chamberTransparencyController;
    
    private float measurementTick = 2.25f;
    
    private Material displayMaterial;
    private bool yAxisIsTRR = true;

    public void ToggleResultDisplay()
    {
        if (currentTextureList.Count <= 0 || isScanning)
            return;
        
        if(yAxisIsTRR)
            displayMaterial.mainTexture = tbnTheoreticalReadRange;
        else
            displayMaterial.mainTexture = currentTextureList[currentTextureList.Count-1];

        yAxisIsTRR = !yAxisIsTRR;
    }

    IEnumerator PlayTextureList(List<Texture> texList)
    {
        //chamberTransparencyController.SetTransparency(0f);
        currentTextureList = texList;
        foreach (Texture tex in texList)
        {
            if (!isScanning)
            {
                displayMaterial.mainTexture = texList[0];
                break;
            }
                
            displayMaterial.mainTexture = tex;
            yield return new WaitForSeconds(measurementTick);
        }

        isScanning = false;
        SetIdleText();
        //chamberTransparencyController.SetTransparency(1f);
    }

    IEnumerator PlayTextureList(List<Texture> texList, List<float> measurementValues)
    {
        chamberTransparencyController.SetTransparency(0f);
        int counter = 0;
        currentTextureList = texList;
        powerIndicator.SetActive(true);
        rfidMount.RotateMount(measurementTick,texList.Count);
        
        foreach (Texture tex in texList)
        {
            if (!isScanning)
            {
                displayMaterial.mainTexture = texList[0];
                break;
            }
                
            displayMaterial.mainTexture = tex;
            
            if(counter + 1 < measurementValues.Count)
                powerIndicator.SetPower(measurementValues[counter+1]);
            yield return new WaitForSeconds(measurementTick);
            counter++;
        }
        
        powerIndicator.SetActive(false);
        isScanning = false;
        SetIdleText();
        chamberTransparencyController.SetTransparency(1f);
    }

    
    void Start()
    {
        displayMaterial = GetComponent<Renderer>().material;
        
        currentScanMode = ScanMode.Threshold;
        measurementTick = measurementTickThres;

        powerIndicator.TickTime = measurementTickOri;
        powerIndicator.SetActive(false);
    }
    void SetIdleText()
    {
        stateText.color = Color.white;
        stateText.text = "CURRENT STATE:\n-IDLE-";
    }
    
    void SetScanningText()
    {
        stateText.color = Color.green;
        stateText.text = "CURRENT STATE:\n-SCANNING-";
    }
    
    void SetErrorText(string error)
    {
        stateText.color = Color.red;
        stateText.text = "ERROR:\n-"+error+"-";
    }
    
    public void PlayRFIDSample()
    {
        if (chamberDoor.IsChamberDoorOpen)
        {
            SetErrorText("DOOR OPEN");
            Invoke("SetIdleText", 2f);
            return;
        }
        
        if (!chamberDoor.IsChamberLocked)
        {
            SetErrorText("LOCK DOOR");
            Invoke("SetIdleText", 2f);
            return;
        }
        
        if (!rfidMount.IsMounted)
        {
            SetErrorText("NO CHIP");
            Invoke("SetIdleText", 2f);
            return;
        }

        if (isScanning)
        {
            SetErrorText("BUSY");
            
            if(isScanning)
                Invoke("SetScanningText", 2f);
            else
                Invoke("SetIdleText", 2f);
            
            // todo: vibrate controller?
            return;
        }

        SetScanningText();
        isScanning = true;
        
        switch (rfidMount.MountedSample.rfid_tag)
        {
            case RFID_Tag.TBN:
                PlayTbn();
                break;
            case RFID_Tag.A02:
                PlayA02();
                break;
            case RFID_Tag.Inline:
                PlayInline();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void StopPlayingRFIDSample()
    {
        isScanning = false;
    }

    private void PlayTbn()
    {
        switch (rfidMount.MountedSample.substrate)
        {
            case Substrate.Wood:
                StartCoroutine(currentScanMode == ScanMode.Threshold
                    ? PlayTextureList(tbnWoodThresholdList)
                    : PlayTextureList(tbnWoodOrientation, tbnWoodOrientationValues));
                break;
            case Substrate.Aluminium:
                StartCoroutine(currentScanMode == ScanMode.Threshold
                    ? PlayTextureList(tbnAluThresholdList)
                    : PlayTextureList(tbnAluOrientationList, tbnAluOrientationValues));
                break;
            case Substrate.PVC:
                StartCoroutine(currentScanMode == ScanMode.Threshold
                    ? PlayTextureList(tbnPvcThresholdList)
                    : PlayTextureList(tbnPvcOrientation, tbnPvcOrientationValues));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void PlayInline()
    {
        switch (rfidMount.MountedSample.substrate)
        {
            case Substrate.Wood:
                StartCoroutine(currentScanMode == ScanMode.Threshold
                    ? PlayTextureList(inlineWoodThresholdList)
                    : PlayTextureList(inlineWoodOrientation, inlineWoodOrientationValues));
                break;
            case Substrate.PVC:
                StartCoroutine(currentScanMode == ScanMode.Threshold
                    ? PlayTextureList(inlinePvcThresholdList)
                    : PlayTextureList(inlinePvcOrientation, inlinePvcOrientationValues));
                break;
            case Substrate.Steel:
                StartCoroutine(currentScanMode == ScanMode.Threshold
                    ? PlayTextureList(inlineSteelThresholdList)
                    : PlayTextureList(inlineSteelOrientationList, inlineSteelOrientationValues));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private void PlayA02()
    {
        switch (rfidMount.MountedSample.substrate)
        {
            case Substrate.Aluminium:
                if(currentScanMode == ScanMode.Threshold)
                    StartCoroutine(PlayTextureList(a02AluThresholdList));
                else
                    StartCoroutine(PlayTextureList(a02AluOrientationList, a02AluOrientationValues));
                break;
            case Substrate.PVC:
                if(currentScanMode == ScanMode.Threshold)
                    StartCoroutine(PlayTextureList(a02PvcThresholdList));
                else
                    StartCoroutine(PlayTextureList(a02PvcOrientation, a02PvcOrientationValues));
                break;
            case Substrate.Cardboard:
                if(currentScanMode == ScanMode.Threshold)
                    StartCoroutine(PlayTextureList(a02CardboardThresholdList));
                else
                    StartCoroutine(PlayTextureList(a02CardboardOrientation, a02CardboardOrientationValues));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void ChangeScanMode()
    {
        if(isScanning)
            return;

        if (currentScanMode == ScanMode.Orientation)
            currentScanMode = ScanMode.Threshold;
        else
            currentScanMode = ScanMode.Orientation;
        
        if (currentScanMode == ScanMode.Threshold)
        {
            measurementTick = measurementTickThres;
            modeText.text = "CURRENT MODE:\n-THRESHOLD-";

            firstText.text = "Start frequency";
            firstValue.text = "800";
            firstUnit.text = "MHz";
            
            secondText.text = "Stop frequency";
            secondValue.text = "100";
            secondUnit.text = "MHz";
            
            thirdText.text = "Frequency step";
            thirdValue.text = "5";
            thirdUnit.text = "MHz";
            
            fourthText.text = "Power step";
            fourthValue.text = "0,1";
            fourthUnit.text = "dB";

            //toggleYAxis.SetActive(true); todo: enable if screenshots are ready...
            displayMaterial.mainTexture = idleThreshold;
        }
        else
        {
            measurementTick = measurementTickOri;
            modeText.text = "CURRENT MODE:\n-ORIENTATION-";
            
            firstText.text = "Frequency";
            firstValue.text = "866";
            firstUnit.text = "MHz";
            
            secondText.text = "Power step";
            secondValue.text = "0,5";
            secondUnit.text = "dB";
            
            thirdText.text = "Angle step";
            thirdValue.text = "10";
            thirdUnit.text = "Deg";
            
            fourthText.text = "unused";
            fourthValue.text = "";
            fourthUnit.text = "";
            toggleYAxis.SetActive(false);
            displayMaterial.mainTexture = idleOrientation;
        }
    }
}
