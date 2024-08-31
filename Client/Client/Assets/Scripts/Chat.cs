using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Chat : MonoBehaviour
{
    private TMP_InputField inputObject;
    [SerializeField] private ColyseusHandler colyseusHandler;
    // Start is called before the first frame update
    void Start()
    {
        inputObject = gameObject.GetComponent<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log(inputObject.text);
            colyseusHandler.sendMessageTest(inputObject.text);
            inputObject.text = "";
        };

    }
}
