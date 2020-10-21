using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableBehaviour : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("ENTER table area");
        EventSystem.current.OnEnterTableArea();
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("EXIT table area");
        EventSystem.current.OnExitTableArea();

    }
}