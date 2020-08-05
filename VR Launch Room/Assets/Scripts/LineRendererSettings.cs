using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineRendererSettings : MonoBehaviour
{
    [SerializeField] private LineRenderer rend; // Store component attached to the game component
    private Vector3[] points; // Setting for LineRenderer are stored in a vec3
    public LayerMask layerMask; // Store alignment of the LineRenderer with Raycast
    
    // Just for demonstration: set the color of the panel to the selected color by button
    public GameObject panel;
    public Image img;
    public Button btn;

    void Start()
    {
        rend = gameObject.GetComponent<LineRenderer>();
        points = new Vector3[2]; // init LineRenderer

        points[0] = Vector3.zero; // Set origin of LineRenderer to position of the gameObject
        points[1] = transform.position + new Vector3(0, 0, 20); // Set endpoint 20 units away from origin

        rend.SetPositions(points); // set position of LineRenderer
        rend.enabled = true; // render
        
        // Just for demonstration
        img = panel.GetComponent<Image>();
    }
    
    void Update()
    {
        if (AlignLineRenderer(rend) && Input.GetAxis("Submit") > 0)
        {
            btn.onClick.Invoke();
        }
    }
    
    // Align the LineRenderer with a physically casted ray from the controller
    public bool AlignLineRenderer(LineRenderer rend)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit; // store when ever a Ray hits a Raycast Target (Button, Text, etc...)
        bool hitBtn = false;
        
        if (Physics.Raycast(ray, out hit, layerMask))
        {
            points[1] = transform.forward + new Vector3(0,0,hit.distance); // get points of ray
            rend.startColor = Color.red;
            rend.endColor = Color.red;
            
            // Just for demonstration
            btn = hit.collider.gameObject.GetComponent<Button>();
            hitBtn = true;
        }
        else
        {
            points[1] = transform.forward + new Vector3(0, 0, 20); // if to fare away, render shorter ray
            rend.startColor = Color.green;
            rend.endColor = Color.green;
            hitBtn = false;
        }
        rend.SetPositions(points);
        rend.material.color = rend.startColor;
        return hitBtn;
    }
    
    // Just for demonstarion
    public void ColorChangeOnClick()
    {
        if (btn != null)
        {
            if (btn.name == "red_btn")
            {
                img.color = Color.red;
            }
            
            else if (btn.name == "blue_btn")
            {
                img.color = Color.blue;
            }
            
            else if (btn.name == "green_btn")
            {
                img.color = Color.green;
            }
        }
    }
}
