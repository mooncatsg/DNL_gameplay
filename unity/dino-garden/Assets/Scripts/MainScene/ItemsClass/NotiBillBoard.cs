using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NotiType
{
    NONE=0,
    Animal_East=1,
    Animal_Finish=2,
    Farm_Step1=103,
    Farm_Step2=104,
    Farm_Step3=105,
    Farm_Worm = 108,
}
public class NotiBillBoard : MonoBehaviour
{
    private Camera mainCam;
    public bool staticBillboard;
    public  static string BOARDNAME = "NotiBillBoard";
    public ItemPlaceController itemParent;
    public NotiType notiType = NotiType.Animal_East;

    public MeshRenderer myMesh;
    public Material[] arrMat;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = MapController.instance.getActiveCamera();
        this.gameObject.name = BOARDNAME;
    }
    public void UpdateUIMesh()
    {
        this.itemParent = this.transform.parent.gameObject.GetComponent<ItemPlaceController>();

        if (notiType == NotiType.Animal_Finish)
        {
            myMesh.material = arrMat[1];
        }
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (!mainCam.enabled)
        {
            mainCam = MapController.instance.getActiveCamera();
        }
        if (!staticBillboard)
        {
            transform.LookAt(mainCam.transform);
        }
        else
        {
            transform.rotation = mainCam.transform.rotation;
        }
    }
}
