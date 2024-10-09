using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Mình hoàn toàn có thể sử dụng kiểm tra va chạm để sắp xếp các đối tượng trên
 * bản đồ, tuy nhiên sẽ chuyển sang việc sắp xếp dạng chia lưới để người dùng họ 
 * có thể trang trí bản đồ 1 cách đẹp hơn thay vì họ tự căn từng pixel 1
 * 
 * 
 * 
 */

[SerializeField]
public enum PLACE_TYPE
{
    NONE,
    BUTTON,
    MOVE,
}
public class ItemPlaceController : MonoBehaviour
{
    // Start is called before the first frame update
    // Start is called before the first frame update
    public Outline outline;
    public PositionUnit posUnit;
    public int USize = 1;
    public int item_id = 0;
    public PLACE_TYPE placeType = PLACE_TYPE.BUTTON;

    private bool isDrag = false;

    private Vector3 lastPos;
    bool activeClick = false;
    
    void Start()
    {
        //this.outline = this.gameObject.GetComponent<Outline>();
        if (this.outline != null && this.transform.parent.GetComponent<Farm3DController>() != null)
        {
            this.outline.enabled = false;
            this.outline.OutlineWidth = 1f;
            this.outline.OutlineColor = Color.white;
        }
        //Invoke("BeginTest", 1);
    }
    void BeginTest()
    {
        PositionUnit ptmp = Ultils.GetUnitPos(posUnit.ux, posUnit.uy, this.USize);;
        if (ptmp.active)
        {
            this.gameObject.transform.position = new Vector3(ptmp.x, 0, ptmp.z);
        }

    }
    public void SelectedItem()
    {
        this.lastPos = this.gameObject.transform.position;

        this.outline.enabled = true;
        
        this.outline.OutlineColor = Color.white;
        isDrag = true;
        Debug.Log("----pick up Object=" + this.gameObject.name);
        //Debug.Log("----this.posUnit=" + this.posUnit.ux + " y=" + this.posUnit.uy);
        Ultils.CreateCacheGrid(this.posUnit);
    }

    public void UpdateTouchPos(Vector3 vtUpos)
    {
        vtUpos.y = 0.5f;
        this.gameObject.transform.position = vtUpos;

        if (!MapController.instance.movePlace.activeSelf)
        {
            MapController.instance.movePlace.SetActive(true);
            float asc = 0.9f;
            if (this.USize == 2)
            {
                asc = 1.8f;
            }
            MapController.instance.movePlace.transform.localScale = new Vector3(asc, asc, asc);
        }

        PositionUnit tmpU = Ultils.ConvertVec3ToUnit(vtUpos, this.USize);
        if (!tmpU.active)
        {
            this.outline.OutlineColor = Color.cyan;
            return;
        }
        if (Ultils.CheckAvaiable(tmpU.ux, tmpU.uy, this.USize))
        {
            this.outline.OutlineWidth = 1.0f;
            this.outline.OutlineColor = Color.white;
        }
        else
        {
            this.outline.OutlineWidth = 1.0f;
            this.outline.OutlineColor = Color.red;
        }
        MapController.instance.movePlace.transform.position= new Vector3(tmpU.x, 0.1f, tmpU.z);
    }

    public void OutlineEnable(bool enable)
    {
        this.outline.enabled = enable;
    }    

    public void ShowClickActive()
    {
        //Debug.Log("ShowClickActive");
        if (this.outline == null)
        {            
            return;
        }
        this.CancelInvoke();
        this.activeClick = true;
        this.outline.enabled = true;
        this.outline.OutlineColor = Color.white;
        isDrag = false;
        if (MapController.instance.itemChoosing != null)
        {
            MapController.instance.itemChoosing.OutlineEnable(false);
        }
        MapController.instance.itemChoosing = this;
        if(this.GetComponent<Farm3DController>() != null)
        {
            this.GetComponent<Farm3DController>().SelectedFarm();
        }
        else if (this.GetComponent<AnimalHouseCtrl>() != null)
        {
            this.GetComponent<AnimalHouseCtrl>().SelectedAnimalHouse();
        }
        else if (this.GetComponent<HouseController>() != null)
        {
            this.GetComponent<HouseController>().SelectedHouse();
        }
        else if (this.GetComponent<AnimalController>() != null)
        {
            this.GetComponent<AnimalController>().SelectedAnimal();
        }
        //this.Invoke("HiddenClickActive", 0.5f);
    }
    private void HiddenClickActive()
    {
        if (this.outline == null)
        {
            return;
        }
        Debug.Log("ENALBE 3");
        this.outline.enabled = false;
        this.activeClick = false;
    }


    public void DeSelectItem()
    {
        Debug.Log("DeSelectItem");

        if (this.activeClick)
        {
            return;
        }
        if (this.outline == null)
        {
            return;
        }
        if (this.outline.enabled)
        {
            this.outline.enabled = false;
        }
        if (MapController.instance.movePlace.activeSelf)
        {
            MapController.instance.movePlace.SetActive(false);
        }
        isDrag = false;
    }



    public void FinishDragItem()
    {
        if (!isDrag)
        {
            return;
        }
        isDrag = false;



        PositionUnit tmpU = Ultils.ConvertVec3ToUnit(this.gameObject.transform.position, this.USize);
        if (!tmpU.active)
        {
            this.transform.position = this.lastPos;
            return;
        }
        if (Ultils.CheckAvaiable(tmpU.ux, tmpU.uy, this.USize))
        {
            this.posUnit = tmpU;

            if (this.outline != null)
            {
                if (this.outline.enabled)
                {
                    Debug.Log("ENALBE 2");
                    this.outline.enabled = false;
                }
            }
            


            this.UpdatePosFromUnit();
        }
        else
        {
            this.transform.position = this.lastPos;
        }


    }
    void UpdatePosFromUnit()
    {
        this.gameObject.transform.position = new Vector3(this.posUnit.x, 0, this.posUnit.z);
    }
    public void UpdatePosFromConfig(ItemPos itp)
    {
        PositionUnit ptmp = Ultils.GetUnitPos(itp.px, itp.py, this.USize); ;
        if (ptmp.active)
        {
            this.posUnit = ptmp;
            this.USize = itp.usize;
            this.UpdatePosFromUnit();
        }
        else
        {
            this.BeginTest();
        }
    }

    public void ActionNotiClick(NotiBillBoard itemclick)
    {
        if (itemclick.notiType == NotiType.Animal_East)
        {
            AnimalHouseCtrl aniH = this.gameObject.GetComponent<AnimalHouseCtrl>();
            if (aniH != null)
            {
                aniH.PlayAnimationEat();
            }

        }else if (itemclick.notiType == NotiType.Animal_Finish)
        {
            AnimalHouseCtrl aniH = this.gameObject.GetComponent<AnimalHouseCtrl>();
            if (aniH != null)
            {
                Debug.Log("------PlayAnimationFinish");
                aniH.PlayAnimationFinish();
            }
        }
        TimeLineController timeLine = this.gameObject.GetComponent<TimeLineController>();
        if (timeLine != null)
        {
            timeLine.NextTimeLine();
        }
        
        Destroy(itemclick.gameObject);
    }


   


    /*
#if UNITY_EDITOR
    void OnValidate()
    {
        if (Application.isPlaying)
        {
            return;
        }
       // item_id = Ultils.autoID;
       // Ultils.autoID++;
       // this.gameObject.name = this.gameObject.name + "_" + item_id;
        if (checkValidLocation)
        {
            MapController mapcc = MapController.ins;
            if (mapcc == null)
            {
                mapcc = this.gameObject.transform.parent.parent.gameObject.GetComponent<MapController>();
            }
            if (mapcc != null)
            {
                if (mapcc.ValidLocation(this))
                {
                    Debug.Log("-----OK----");
                }
                else
                {
                    Debug.Log("-----ERROR----");
                }
            }
            return;
        }

        if (autoPlace)
        {
            if (updateUnit)
            {

                

                this.posUnit = Ultils.ConvertVec3ToUnit(this.transform.position);
                Vector3 vtpos = Ultils.ConvertUnitToVec3(this.posUnit);
                this.gameObject.transform.position = vtpos;
            }
            
        }
        else
        {
            Vector3 vtpos = Ultils.ConvertUnitToVec3(this.posUnit);
            this.gameObject.transform.position = vtpos;
        }
       

    }
#endif

    */
}
