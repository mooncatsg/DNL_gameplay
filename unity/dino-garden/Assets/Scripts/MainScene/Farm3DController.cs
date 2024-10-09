using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
using UnityEngine.UI;


public class Farm3DController : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("LAND")]
    public List<GameObject> listLand = new List<GameObject>();
    [Header("Trạng thái đất")]
    public FarmState farmStt = FarmState.seed;
    public FarmState lastFarmStt = FarmState.seed;
    public Transform notifyIcon;

    public Crop cropData;
    //public Plant plantData;
    private MeshRenderer farmRender;
    private MeshFilter farmFilter;
    //private FarmInfo aniData;
    private GameObject platingObj;
    private GameObject activeNode;
    private GameObject farmObject;
    void Start()
    {
        farmRender = this.GetComponentInChildren<MeshRenderer>();
        farmFilter = this.GetComponentInChildren<MeshFilter>();

        InvokeRepeating("UpdateModelByTimer",1f,1f);
    }
    public float totalTimePlant = 60f;
    public float currentTimePlant = 0f;
    private DateTime startTime;
    private DateTime endTime;

    private void Update()
    {
        
        if (cropData.plant != null && MapController.instance.IsDinoCaring(StakingClassification.plant, StakingType.garden) == false && (CanHavest() || NeedToCatchWorm() || NeedToWatering()))
        {
            notifyIcon.gameObject.SetActive(true);
            notifyIcon.LookAt(Camera.main.transform);
        }
        else
        {
            notifyIcon.gameObject.SetActive(false);
        }
    }
    void UpdateModelByTimer()
    {
        GetFarmState();
        if (lastFarmStt != farmStt)
        {
            Debug.LogError("SpawnPlantModel : " + lastFarmStt + " = " + farmStt);

            lastFarmStt = farmStt;
            SpawnPlantModel();
        }
    }   

    public void UpdateFarmData(Crop _landData)
    {
        notifyIcon.gameObject.SetActive(false);
        cropData = _landData;
        UpdateFarmDisplay();
        UpdateCropsDisplay();
    }

    public void UpdateFarmDisplay()
    {
        if((int)cropData.Rarity > 0)
        {
            listLand[(int)cropData.Rarity - 1].SetActive(true);
        }
        else
        {
            Debug.LogError("this cropdata is invalid");
            Debug.LogError(Newtonsoft.Json.JsonConvert.SerializeObject(cropData));
        }
    }

    public void UpdateCropsDisplay()
    {
        if (cropData != null && cropData.plant != null && cropData.plant.id != 0)
        {
            farmStt = GetFarmState();
            lastFarmStt = farmStt;
            startTime = DateTime.ParseExact(cropData.plant.createdAt, "M/d/yyyy, h:mm:ss tt", CultureInfo.InvariantCulture);
            endTime = DateTime.ParseExact(cropData.plant.lifecycles.Find(x=>x.name == "growth").endTime, "M/d/yyyy, h:mm:ss tt", CultureInfo.InvariantCulture);
            SpawnPlantModel();
        }
    }

    public void SpawnPlantModel()
    {
        if (!IsHavePlant())
            return;

        if (this.activeNode)
        {
            Destroy(this.activeNode);
        }
        this.activeNode = Instantiate(MapController.instance.arrPlant[(int)cropData.plant.Type - 1].prefabTime[(int)farmStt], this.transform);
        this.activeNode.transform.localEulerAngles = Vector3.zero;

        // Reload crop detail
        if (MapController.instance.IsDinoCaring(StakingClassification.plant, StakingType.garden) && farmStt == FarmState.canHarvest)
        {
            this.ActionWaitForSeconds(1f, () => {
                APIMng.Instance.APICropDetail(cropData.id, (isSuccess, cropDetail, errMsg, code) =>
                {
                    if (isSuccess)
                    {
                        this.ActionWaitForSeconds(3f, () => 
                        {
                            APIMng.Instance.APICropDetail(cropData.id, (isSuccess2, cropDetail2, errMsg2, code2) =>
                            {
                                if (isSuccess2)
                                {
                                    Crop crop = (Crop)cropDetail2;
                                    if (crop != null && (crop.plant == null || cropData.plant.id <= 0))
                                    {
                                        cropData.plant = crop.plant;
                                        HavestFarm();
                                    }
                                    else
                                    {
                                        MainMenuUI.Instance.ShowAlertMessage("Warehouse is full. Please upgrade warehouse!");
                                    }    
                                }
                                else
                                {
                                    MainMenuUI.Instance.ShowAlertMessage(errMsg2);
                                }
                            });
                        });
                    }
                    else
                    {
                        MainMenuUI.Instance.ShowAlertMessage(errMsg);
                    }
                });
            });
        }
    }

    public FarmState GetFarmState()
    {
        if (IsHavePlant())
        {
            farmStt = FarmState.canHarvest;
            if (cropData.plant.lifecycles.Count > 0)
            {
                for (int i = 0;i < cropData.plant.lifecycles.Count; i++)
                {
                    DateTime dt = DateTime.ParseExact(cropData.plant.lifecycles[i].endTime, "M/d/yyyy, h:mm:ss tt", CultureInfo.InvariantCulture);
                    int timer = (int)((dt.ToLocalTime() - DateTime.Now).TotalSeconds);
                    if (timer >= 0)
                    {
                        farmStt = (FarmState)i;
                        break;
                    }
                }
            }
        }
        return farmStt;
    }

    public bool IsHavePlant()
    {
        if (cropData == null)
            return false;
        if (cropData.plant == null)
            return false;
        if (cropData.plant.id <= 0)
            return false;

        return true;
    }

    public float SliderCountdown()
    {
        if (cropData != null && cropData.plant != null)
        {
            float totalSecond = (float)((endTime - startTime).TotalSeconds);
            float currentSecond = (float)((DateTime.Now - startTime.ToLocalTime()).TotalSeconds);
            
            return currentSecond / totalSecond;
        }
        return -1f;
    }    
    public int GetFarmTimer()
    {
        if(IsHavePlant() && cropData.plant != null)
        {
            LifeCycle lifeCycle = cropData.plant.lifecycles.Find(x => x.name == "growth");
            if(lifeCycle != null)
            {
                DateTime dt = DateTime.ParseExact(lifeCycle.endTime, "M/d/yyyy, h:mm:ss tt", CultureInfo.InvariantCulture);
                return (int)((dt.ToLocalTime() - DateTime.Now).TotalSeconds);
            }
        }
        return 0;
    }

    public void ActionSeed(Plant _plant) // phase 1
    {
        this.cropData.plant = _plant;
        UpdateFarmDisplay();
        UpdateCropsDisplay();
    }

    public void SelectedFarm()
    {
        EffectManager.instance.PlayEffect(EffectType.DROP, this.transform.position);
        if (cropData.plant == null || cropData.plant.id == 0)
        {
            MainMenuUI.Instance.ShowActionUI(ACTION_UI_TYPE.SPAWN_TREE);
        }
        else
        {
            MainMenuUI.Instance.ShowActionUI(ACTION_UI_TYPE.TAKE_CARE_PLANT);
        }
    }

    public void DeselectedFarm()
    {

    }

    public void WithdrawCrop()
    {
        GameObject.Destroy(this.gameObject);
    }    
    public void DropFarm()
    {
        this.cropData.plant = null;
        notifyIcon.gameObject.SetActive(false);
        farmStt = FarmState.seed;
        if (this.activeNode)
        {
            Destroy(this.activeNode);
        }
        this.activeNode = null;
    }

    public void HavestFarm()
    {
        notifyIcon.gameObject.SetActive(false);
        farmStt = FarmState.seed;
        if (this.activeNode)
        {
            Destroy(this.activeNode);
        }
        this.activeNode = null;
    }

    public bool CanHavest()
    {
        return (GetFarmState() == FarmState.canHarvest);
    }

    public bool NeedToCatchWorm()
    {
        if (IsHavePlant() && cropData.plant.events.Count > 0)
        {
            for(int i=0; i<cropData.plant.events.Count; i++)
            {
                if (cropData.plant.events[i].Type == EventCycleType.worm && cropData.plant.events[i].fixTime == null)
                {
                    DateTime dt = DateTime.ParseExact(cropData.plant.events[i].startTime, "M/d/yyyy, h:mm:ss tt", CultureInfo.InvariantCulture);
                    int remainTime = (int)((dt.ToLocalTime() - DateTime.Now).TotalSeconds);
                    return remainTime <= 0;
                }
            }
        }
        return false;
    }

    public bool NeedToWatering()
    {
        if (IsHavePlant() && cropData.plant.events.Count > 0)
        {
            for (int i = 0; i < cropData.plant.events.Count; i++)
            {
                if (cropData.plant.events[i].Type == EventCycleType.arid && cropData.plant.events[i].fixTime == null)
                {
                    DateTime dt = DateTime.ParseExact(cropData.plant.events[i].startTime, "M/d/yyyy, h:mm:ss tt", CultureInfo.InvariantCulture);
                    int remainTime = (int)((dt.ToLocalTime() - DateTime.Now).TotalSeconds);
                    return remainTime <= 0;
                }
            }
        }
        return false;
    }
}
