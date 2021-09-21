using UnityEngine;
using Valve.VR.InteractionSystem;

public class RFIDMount : MonoBehaviour
{
    [SerializeField] private RFIDSample mountedSample = null;
    [SerializeField] private bool isMounted = false;
    [SerializeField] private bool isInside = false;
    [SerializeField] private GameObject hoveringGO;
    
    public bool IsMounted => isMounted;
    public RFIDSample MountedSample => mountedSample;

    private bool rotate = false;
    private float currentRotation = 0f;
    private float totalTime;
    public void RotateMount(float tickTime, int stepCount)
    {
        totalTime = stepCount * tickTime;
        rotate = true;
    }
    
    private void MountSample()
    {
        RFIDSample sample = hoveringGO.GetComponent<RFIDSample>();

        if (sample != null)
        {
            mountedSample = sample;
            
            // disable collider to make sure we do not accidentally trigger the exit while moving the mounted chip...
            GetComponent<BoxCollider>().enabled = false;
            
            // disable physics
            Rigidbody physics = hoveringGO.GetComponent<Rigidbody>();
            physics.isKinematic = true;
             
            PlaceMountedChipOnTable();
            
            GetComponent<BoxCollider>().enabled = true;
            
            isMounted = true;

            hoveringGO.transform.parent = transform;
        }
    }

    private void UnmountSample()
    {
        Rigidbody physics = mountedSample.gameObject.GetComponent<Rigidbody>();
        
        Hand hand = mountedSample.gameObject.transform.parent.gameObject.GetComponent<Hand>();
        
        hand.DetachObject(mountedSample.gameObject);
        physics.isKinematic = false;
        hand.AttachObject(mountedSample.gameObject, hand.GetBestGrabbingType()); //TODO: polish grabbing type!
        
        mountedSample.gameObject.transform.parent = null;
        
        mountedSample = null;
        isMounted = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (isMounted)
            return;
        
        GameObject go = other.gameObject.transform.parent.gameObject;
        if (go == null)
        {
            return;
        }
            
        if(!go.CompareTag("RFID_Sample"))
            return;
        
        isInside = true;
        hoveringGO = go;
    }

    private void OnTriggerExit(Collider other)
    {
        // make sure the system does not break if the users adds multiple rfid chips...
        if(other.gameObject.transform.parent.gameObject != hoveringGO)
            return;

        isInside = false;
        hoveringGO = null;
        
        if(isMounted)
            UnmountSample();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMounted && isInside)
        {
            if (!IsInHand())
                MountSample();
        }

        if (rotate)
        {
            float step = Time.deltaTime / totalTime;
            currentRotation += step * 360f;
            transform.Rotate(new Vector3(0,step * 360f,0));

            if (currentRotation >= 360f)
            {
                currentRotation = 0f;
                rotate = false;
            }
        }
    }

    private void PlaceMountedChipOnTable()
    {
        hoveringGO.transform.position = transform.position;
        hoveringGO.transform.rotation = Quaternion.identity;
        hoveringGO.transform.Translate(0,0.08f,0);
        hoveringGO.transform.Rotate(0,0,90);
    }

    bool IsInHand()
    {
        Transform handGO = hoveringGO.transform.parent;
        return handGO != null;
    }
}
