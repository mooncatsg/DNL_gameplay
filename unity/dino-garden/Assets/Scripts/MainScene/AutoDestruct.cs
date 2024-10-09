using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestruct : MonoBehaviour
{
    public float timeDestruct = 5f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Destruct",timeDestruct);
    }

    // Update is called once per frame
    void Destruct()
    {
        
    }
}
