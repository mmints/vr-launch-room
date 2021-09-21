using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttentionArrow : MonoBehaviour
{
    [SerializeField] private GameObject arrow;
    [SerializeField] private Transform pointAtTarget;
    [SerializeField] private Transform attachmentPoint;
    [SerializeField] private Canvas targetCanvas;
    [SerializeField] private Transform headTransform;
    
    public Collider targetCollider;
    private Camera cam;

    private Plane[] planes;
    
    void Start()
    {
        cam = Camera.main;
        //myTransform.parent = attachmentPoint;
        targetCollider =  pointAtTarget.GetComponentInChildren<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        planes = GeometryUtility.CalculateFrustumPlanes(cam);
        if (GeometryUtility.TestPlanesAABB(planes, targetCollider.bounds))
        {
            arrow.SetActive(false);
            targetCanvas.enabled = false;
        }
        else
        { 
            transform.GetChild(0).transform.LookAt(pointAtTarget);
            targetCanvas.transform.GetChild(0).transform.LookAt(headTransform);

            targetCanvas.enabled = true;
            arrow.SetActive(true);
        }
    }
}
