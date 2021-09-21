using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAnimation : MonoBehaviour
{
    private float startScale;

    [SerializeField] private float scalePercentageStart;
    [SerializeField] private float scalePercentageEnd;
    [SerializeField] private float animationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        startScale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        float range = (scalePercentageEnd - scalePercentageStart) / 2f;
        float offset = range + scalePercentageStart;
        
        float scale = offset + Mathf.Sin(Time.time * animationSpeed) * range;
        scale *= startScale;
        
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
