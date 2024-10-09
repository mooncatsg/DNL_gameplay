using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Globalization;

public class HenHouseAnimalItem : MonoBehaviour
{
    public Image animalIcon;
    public Text animalHarvestCount;

    public Text animalNextHarvest;
    public Button animalNextHarvestBtn;

    public Text animalNextFeeding;
    public Button animalNextFeedingBtn;


    public Animal animalData = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckTimeLeftHarvest();
        //CheckTimeLeftFeeding();
    }
    float currentTimeToHarvest = 0f;
    List<EventCycle> listHungryCycle = new List<EventCycle>();
    EventCycle currentHungryCycle = null;
    AnimalLifecycle harvestLifeCycle = null;
    public void SetData(Animal _data)
    {
        animalData = _data;
        animalIcon.sprite = MainMenuUI.Instance.GetListStoreItemIcon(STORE_ITEM_TYPE.animal).Find(x=>x.name == animalData.type);
        animalIcon.SetNativeSize();
        animalHarvestCount.text = animalData.havestCnt + "/" + animalData.maxHarvestCount;

        // FEED
        animalNextFeedingBtn.gameObject.SetActive(false);
        animalNextHarvestBtn.gameObject.SetActive(false);

        listHungryCycle = animalData.events.FindAll(x=> x.type == "hungry" && x.fixTime == null);
        listHungryCycle = listHungryCycle.OrderBy(x => x.StartTime).ToList();

        if(listHungryCycle.Count > 0)
        {
            currentHungryCycle = listHungryCycle[0];
            Debug.LogError("listHungryCycle : " + listHungryCycle.Count + "  -  current :" + currentHungryCycle.StartTime);
        }

        harvestLifeCycle = animalData.lifecycles.Find(x => x.name == "adult");
        if (harvestLifeCycle != null)
        {
            DateTime dt = DateTime.ParseExact(harvestLifeCycle.endTime, "M/d/yyyy, h:mm:ss tt", CultureInfo.InvariantCulture);
            currentTimeToHarvest = (float)((dt - DateTime.UtcNow).TotalSeconds);
            Debug.LogError("timeToHarvest : " + currentTimeToHarvest);
            if (currentTimeToHarvest <= 0)
            {
                animalNextHarvestBtn.gameObject.SetActive(true);
            }
        }
        else
        {

        }
    }

    public void CheckTimeLeftHarvest()
    {
        if (MapController.instance.IsDinoCaring(StakingClassification.animal, animalData.StakeType))
        {
            animalNextHarvestBtn.gameObject.SetActive(false);
            animalNextHarvest.text = "DINO CARING";
            CheckTimeLeftFeeding();
            return;
        }

        if (currentTimeToHarvest > 0f)
        {
            currentTimeToHarvest -= Time.deltaTime;
            TimeSpan t = TimeSpan.FromSeconds(currentTimeToHarvest);
            animalNextHarvest.text = "" + string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
            if (currentTimeToHarvest <= 0f)
            {
                // HARVEST 
                animalNextHarvestBtn.gameObject.SetActive(true);
            }
            else
            {
                CheckTimeLeftFeeding();
            }
        }
        else
        {
            CheckTimeLeftFeeding();
        }    
    }

    public void CheckTimeLeftFeeding()
    {
        if (MapController.instance.IsDinoCaring(StakingClassification.animal, animalData.StakeType))
        {
            animalNextFeedingBtn.gameObject.SetActive(false);
            animalNextFeeding.text = "DINO CARING";
            return;
        }

        if (currentHungryCycle == null || currentTimeToHarvest <= 0)
        {
            animalNextFeedingBtn.gameObject.SetActive(false);
            animalNextFeeding.text = "FULL";
            return;
        }

        DateTime dt = DateTime.ParseExact(currentHungryCycle.startTime, "M/d/yyyy, h:mm:ss tt", CultureInfo.InvariantCulture);
        float timeToFeed = (float)((dt - DateTime.UtcNow).TotalSeconds);
        timeToFeed -= Time.deltaTime;
        TimeSpan t = TimeSpan.FromSeconds(timeToFeed);
        animalNextFeeding.text = "" + string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
        if (timeToFeed <= 0f)
        {
            // FEED 
            animalNextFeedingBtn.gameObject.SetActive(true);
        }
    }

    public void OnClickFeed()
    {
        Debug.LogError("ANIMAL ==> feeding");
        animalNextFeedingBtn.gameObject.SetActive(false);
        float price = 0; int typeCurrency = 0;
        List<Animal> listAnimals = HenHouse.Instance.getCage(animalData.type);
        for(int i=0; i<listAnimals.Count; i++)
            if(listAnimals[i].id == animalData.id)
            {
                if (listAnimals[i].fixEventPrice != null)
                {
                    List<fixEventPrice> fixEventPrices = listAnimals[i].fixEventPrice;
                    Debug.Log(fixEventPrices.Count);
                    for (int j = 0; j < fixEventPrices.Count; j++)
                        if (fixEventPrices[j].type == "hungry")
                        {
                            price = fixEventPrices[j].price;
                            typeCurrency = fixEventPrices[j].typeCurrency;
                        }
                }
            }
        MainMenuUI.Instance.ShowAlertMessage("DO YOU WANT TO FEED " + animalData.type.ToUpper() + " WITH "+ price + (typeCurrency==2?" W-DNG":" W-DNL"), "FEEDING ANIMAL", true, null, () =>
        {
            APIMng.Instance.APIAnimalFeed(animalData.id, (isSuccess2, animalFeeding, errMsg2, code2) =>
            {
                if (isSuccess2)
                {
                    MapController.instance.UpdateNewAnimal(() => {
                        Animal newData = MapController.instance.currentlandDetail.cages.Find(x => x.Type == animalData.Type).animalList.Find(x => x.id == animalData.id);
                        if (newData != null)
                            this.SetData(newData);
                    });
                    MapController.instance.UpdateUserInfo();
                }
                else
                {
                    animalNextFeedingBtn.gameObject.SetActive(true);
                    MainMenuUI.Instance.ShowAlertMessage(errMsg2);
                }
            });
        });
    }

    bool harvesting = false;
    public void OnClickHarvest()
    {
        if (harvesting == true)
            return;
        Debug.LogError("ANIMAL ==> havest");
        harvesting = true;
        animalNextHarvestBtn.gameObject.SetActive(false);
        APIMng.Instance.APIAnimalHarvest(animalData.id, (isSuccess2, animalHarvest, errMsg2, code2) =>
        {
            harvesting = false;
            if (isSuccess2)
            {
                MainMenuUI.Instance.ShowAlertMessage("SUCCESS HARVEST : " + " " + animalData.type.ToUpper() + "\nQUANTITY : " + ((Animal)animalHarvest).harvest, "HARVEST");
                MapController.instance.UpdateNewAnimal(() => {                    
                    if (MapController.instance.currentlandDetail.cages.Find(x => x.Type == animalData.Type).animalList.Find(x => x.id == animalData.id) != null)
                    {
                        MapController.instance.currentlandDetail.cages.Find(x => x.Type == animalData.Type).animalList.Find(x => x.id == animalData.id).havestCnt = ((Animal)animalHarvest).havestCnt;

                        this.SetData(MapController.instance.currentlandDetail.cages.Find(x => x.Type == animalData.Type).animalList.Find(x => x.id == animalData.id));
                    }    
                        
                    else
                    {
                        GameObject.Destroy(this.gameObject);
                    }

                });
            }
            else
                MainMenuUI.Instance.ShowAlertMessage(errMsg2);

        });
    }
}
