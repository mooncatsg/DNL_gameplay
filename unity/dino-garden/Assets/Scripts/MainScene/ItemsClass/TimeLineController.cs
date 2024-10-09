using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimeEvent
{
    public string EventName;
    public float time;
    //[System.NonSerialized]
    public bool active=false;
    //[System.NonSerialized]
    public bool selected = false;
}
public class TimeLineController : MonoBehaviour
{
    // Start is called before the first frame update
    public TimeEvent[] listTimeItem;
    public float time_life;
    private float step;
    private bool couting;
    public bool pause;
    private const float STEPDELAY = 0.5f;
    private GameObject activeNoti;
    void Start()
    {
        
    }
    public void StopCountingTimeAnimal()
    {
        this.couting = false;
        this.pause = true;
    }
    public void StartCountingTimeAnimal()
    {
        this.couting = true;
        this.pause = false;
        this.step = 0;
        this.time_life = 0;
        Debug.Log("---AAAA-----");
        for (int a= 0;a < this.listTimeItem.Length;a++)
        {

            this.listTimeItem[a].active = false;
            this.listTimeItem[a].selected = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!couting)
        {
            return;
        }
        if (this.pause)
        {
            return;
        }
        this.step += Time.deltaTime;
        if(this.step > STEPDELAY)
        {
            this.time_life += this.step;
            this.step = 0;
            this.CheckEvent();
        }
    }
    public void NextTimeLine()
    {
        this.pause = false;
        for (int a = 0; a < this.listTimeItem.Length; a++)
        {
            if(this.listTimeItem[a].active)
                this.listTimeItem[a].selected = true;
        }
        
    }
    void checkActiveTime()
    {
        bool actived = false;
        foreach(TimeEvent tme in this.listTimeItem)
        {
            if (tme.selected)
            {
                continue;
            }
            if (tme.active)
            {
                continue;
            }
            actived = true;
        }

        if (!actived)
        {
            this.couting = false;
        }
    }
    void CheckEvent()
    {
        
        for (int b = 0; b < this.listTimeItem.Length; b++)
        {
            TimeEvent tmE = this.listTimeItem[b];
            if (tmE.selected)
            {
                continue;
            }
            if (tmE.active)
            {
                continue;
            }
            if (tmE.time<=this.time_life)
            {
                Debug.Log("---Add Event----:"+tmE.EventName);
                this.pause = true;
                tmE.active = true;
                this.FireEvent(tmE);
                break;
            }
        }
        this.checkActiveTime();


    }
    void FireEvent(TimeEvent tmE)
    {
        NotiType nty = NotiType.NONE;
        if (tmE.EventName == "finish")
        {
            nty = NotiType.Animal_Finish;
        }
        if (tmE.EventName == "eat")
        {
            nty = NotiType.Animal_East;
        }
        if (tmE.EventName == "farm_growup")
        {
            nty = NotiType.NONE;
        }
        
        if (tmE.EventName == "worm")
        {
            nty = NotiType.Farm_Worm;
        }
        if (this.activeNoti != null)
        {
            Destroy(this.activeNoti);
        }



        if (nty == NotiType.NONE)
        {
            
            if(tmE.EventName == "growup")
            {
                //this.transform.gameObject.GetComponent<AnimalHouseCtrl>().AnimalGrowUp();
                this.NextTimeLine();
            }
            else if (tmE.EventName == "farm_growup")
            {
                //this.transform.gameObject.GetComponent<Farm3DController>().FarmGrowUp();
                this.NextTimeLine();
            }
        }
        else
        {
            int idFrm = 0;
            int cvID = (int)nty;
            if (cvID > 50)
            {
                idFrm = 1;
            }
            this.activeNoti = Instantiate(MapController.instance.listBoard[idFrm]);
            activeNoti.transform.parent = this.transform;
            activeNoti.transform.localPosition = new Vector3();
            activeNoti.transform.localScale = new Vector3(1, 1, 1);
            NotiBillBoard boardname = activeNoti.GetComponent<NotiBillBoard>();
            boardname.notiType = nty;
            boardname.UpdateUIMesh();
        }
        
    }

}
