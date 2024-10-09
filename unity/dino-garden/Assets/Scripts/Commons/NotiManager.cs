using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotiManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject testObj;

    public GameObject boar1;
    void Start()
    {
        
    }
 
    // Update is called once per frame
    void Update()
    {
        Vector3 cvPos = Camera.main.WorldToScreenPoint(testObj.transform.position);
        cvPos.y += 60;
        boar1.transform.position = cvPos;
    }
}
