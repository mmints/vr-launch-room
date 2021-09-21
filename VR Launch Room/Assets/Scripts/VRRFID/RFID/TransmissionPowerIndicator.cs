using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class TransmissionPowerIndicator : MonoBehaviour
{
    private float tickTime;

    public float TickTime
    {
        get => tickTime;
        set => tickTime = value;
    }

    private float targetPower = 0f;
    private float stepPower = 0f;
    private float currentPower = 0f;

    private Gradient gradient;
    private GradientColorKey[] colorKey;
    private GradientAlphaKey[] alphaKey;
    [SerializeField] private MeshRenderer powerRenderer;
    
    private Transform myTransform;
    private float maxScale = 0.47f;
    // Start is called before the first frame update
    void Start()
    {
        myTransform = GetComponent<Transform>();
        //powerColor = GetComponentInChildren<Material>();
        
        gradient = new Gradient();
        
        colorKey = new GradientColorKey[2];
        colorKey[0].color = Color.red;
        colorKey[0].time = 0.0f;
        colorKey[1].color = Color.green;
        colorKey[1].time = 1.0f;
        
        // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
        alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 0.5f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 0.5f;
        alphaKey[1].time = 1.0f;

        gradient.SetKeys(colorKey, alphaKey);
    }

    // Update is called once per frame
    void Update()
    {
        if (Math.Abs(currentPower - targetPower) > Single.Epsilon)
        {
            float fraction = Time.deltaTime / tickTime;

            Vector3 scaleChange = myTransform.localScale;
            currentPower += stepPower * fraction;
            scaleChange.x = currentPower * maxScale;
            myTransform.localScale = scaleChange;

            if (currentPower >= 1f)
                currentPower = 1f;
            if (currentPower <= 0f)
                currentPower = 0;
            
            powerRenderer.material.color = gradient.Evaluate(currentPower);
            powerRenderer.material.SetColor("_EmissionColor",gradient.Evaluate(currentPower));
           
        }
    }

    public void SetPower(float power)
    {
        if (power >= 0f && power <= 1f)
        {
            targetPower = power;
            stepPower = targetPower - currentPower;
        }
        else
        {
            Debug.Log("Wrong length factor for the power indicator. The value has to be between 0 and 1");
        }
    }

    public void SetActive(bool enable)
    {
        targetPower = 0f;
        stepPower = 0f;
        currentPower = 0f;
        powerRenderer.enabled = enable;
    }
}
