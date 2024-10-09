using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerViewCtrl : MonoBehaviour
{

    [SerializeField]
    float speed_M;
    [SerializeField]
    float speed_E;
    Rigidbody rb;
   
    bool mouse_ss = false;
    Vector3 lastTouch;
 
    void Start()
    {
        this.rb = this.gameObject.GetComponent<Rigidbody>();
        
       // this.rb.velocity = this.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer)
        {

            HandleTouch();
        }
        else
        {

            HandleMouse();
        }
        if (mouse_ss)
        {
            this.rb.velocity = this.transform.forward * this.speed_M;
        }
        
    }
    private void OnEnable()
    {
        if (this.rb != null)
        {
            this.rb.velocity = new Vector3();
        }
        
    }
    private void OnDisable()
    {
        if (this.rb != null)
        {
            this.rb.velocity = new Vector3();
        }

    }
   
   
    
    void HandleTouch()
    {
        
        if(Input.touchCount==1)
        {

            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lastTouch = touch.position;
                this.mouse_ss = true;
            }
            
            else if (touch.phase == TouchPhase.Moved)
            {

                Vector3 crPos = touch.position;
                Vector3 vtdt = crPos - lastTouch;
                if (vtdt.magnitude > 30)
                {
                    //Debug.Log("-----vtdt" + vtdt);
                    Vector3 vtE = this.gameObject.transform.eulerAngles;
                    vtE.y += vtdt.x * this.speed_E*0.1f;
                    this.gameObject.transform.eulerAngles = vtE;
                    this.rb.velocity = this.transform.forward * this.speed_M;
                    lastTouch = crPos;
                }
                this.mouse_ss = true;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                this.rb.velocity = new Vector3(0, 0, 0);
                this.mouse_ss = false;
            }
        }
    }

    void HandleMouse()
    {

        if (Input.GetMouseButtonDown(0))
        {
            mouse_ss = true;
            lastTouch = Input.mousePosition;
            Debug.Log("----touch begin---");
        }
        else if (Input.GetMouseButton(0))
        {

            Vector3 crPos = Input.mousePosition;
            Vector3 vtdt = crPos - lastTouch;
            if (vtdt.magnitude > 3)
            {
                //Debug.Log("-----vtdt" + vtdt);
                Vector3 vtE=this.gameObject.transform.eulerAngles;
                vtE.y += vtdt.x * this.speed_E;
                this.gameObject.transform.eulerAngles = vtE;
                this.rb.velocity = this.transform.forward * this.speed_M;
                lastTouch = crPos;
            }
            
        }
        else
        {
            if (mouse_ss)
            {
                mouse_ss = false;
                this.rb.velocity = new Vector3(0, 0, 0);
                Debug.Log("----touch ended---");
            }
            
        }
        
    }
}
