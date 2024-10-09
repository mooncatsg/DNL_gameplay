using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoardSprite : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera theCam;
    public bool useStaticBillboard;  
    void Start()
    {
        theCam = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!useStaticBillboard)
        {
            transform.LookAt(theCam.transform);
        }
        else
        {
            transform.rotation=(theCam.transform.rotation);
        }
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
