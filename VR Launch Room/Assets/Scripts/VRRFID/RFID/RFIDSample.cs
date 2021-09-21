using UnityEngine;

public class RFIDSample : MonoBehaviour
{
    public RFID_Tag rfid_tag;
    public Substrate substrate;
    public RFID_Transponder transponder;
    
    private Scenario scenario;

    public void Start()
    {
        scenario = GameObject.Find("VR-RFID-Scenario").GetComponent<Scenario>();
        loadTransponders();
    }

    private void loadTransponders()
    {
        //transponder = scenario.AvailableTransponders[rfid_tag];
    }
}
