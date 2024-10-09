using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public enum HEN_HOUSE_UI_STATE
{
    NORMAL,
    UPGRADE,    
    ANIMAL_LIST,
}
public class HenHouse : UIPopupBase
{
    public const string UpgradeText = "A HOUSE FOR {0} THE NUMBER OF {0} THAR CAN BE RAISED WILL INCREASE WITH THE LEVEL OF THE HEN HOUSE";
    [Header ("UPGRADE")]
    [SerializeField] GameObject upgradePanel;
    [SerializeField] Text titleLabel;
    [SerializeField] Text priceLabel;
    [SerializeField] Image icon;
    [SerializeField] HeartProgress reliabilityProgress;
    [SerializeField] HeartProgress capacityProgress;
    [SerializeField] Text txtCurLevel;
    [SerializeField] Text txtNextLevel;
    [SerializeField] Text txtUpgradeDetail;
    [SerializeField] Button UpgradeBtn;
    [SerializeField] Button AnimalBtn;

    [SerializeField] HenHouseUnlock guiUnlock;
    [SerializeField] HenHouseUpgrade guiUpgrade;
    [Space]
    [Header("NORMAL")]
    [SerializeField] GameObject normalPanel;
    [SerializeField] Image animalIcon;
    [Space]
    [Header("ANIMAL LIST")]
    [SerializeField] GameObject animalListPanel;
    [SerializeField] GameObject notHaveAnimalObject;
    [SerializeField] Transform animalListParent;
    [SerializeField] Text animalListTitle;

    public GameObject animalHenhousePrefab;

    public HEN_HOUSE_UI_STATE henHouseState = HEN_HOUSE_UI_STATE.NORMAL;
    public static HenHouse Instance;
    public Cage cageData;
    //private int _level = 0;
    //private int _type = 0;
    List<HHLevelInfo> listHHLevelInfo = new List<HHLevelInfo>();
    List<Cage> listCages;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowHenHouseByInfo(Cage _cageData)
    {
        cageData = _cageData;
        this.SetData(cageData);        
        this.Show();

        Sprite iconSprite = MainMenuUI.Instance.listAnimalIcons.Find(x => x.name == cageData.type);
        if (iconSprite)
        {
            animalIcon.sprite = iconSprite;
            animalIcon.rectTransform.sizeDelta = new Vector2(96f * iconSprite.rect.width / iconSprite.rect.height, 96f);
        }

        if (cageData.status == "locked")//Locked
        {
            ShowPanelFollowState(HEN_HOUSE_UI_STATE.UPGRADE);
        }
        else
        {
            ShowPanelFollowState(HEN_HOUSE_UI_STATE.NORMAL);
            UpgradeBtn.interactable = true;
            AnimalBtn.interactable = true;
            if (cageData.durability <= 0)
            {
                UpgradeBtn.interactable = false;
                AnimalBtn.interactable = false;
                string content = String.Format("YOUR CAGE IS BROKEN ! \n DO YOU WANT TO FIX THIS WITH {0} {1}", cageData.fix.Find(x=>x.level == cageData.level).amount, cageData.fix.Find(x => x.level == cageData.level).Currency);
                MainMenuUI.Instance.ShowAlertMessage(content,"CAGE BROKEN",true,null,()=> { 
                    APIMng.Instance.APICageFix(cageData.id, (isSuccess, fixCageData, errMsg, code) =>
                    {
                        if(isSuccess)
                        {
                            MainMenuUI.Instance.ShowAlertMessage("SUCCESS TO FIX THIS CAGE !!!");
                            MapController.instance.UpdateNewAnimal();
                            MapController.instance.UpdateUserInfo();

                        }
                        else
                        {
                            MainMenuUI.Instance.ShowAlertMessage(errMsg);
                        }
                    });
                });
            }    
        }
    }

    public void ShowPanelFollowState(HEN_HOUSE_UI_STATE state)
    {
        henHouseState = state;
        upgradePanel.SetActive(henHouseState == HEN_HOUSE_UI_STATE.UPGRADE);
        normalPanel.SetActive(true);
        animalListPanel.SetActive(henHouseState == HEN_HOUSE_UI_STATE.ANIMAL_LIST);
    }

    Upgrade upgradeModel = null;
    void SetData(Cage cageData)
    {

        if (cageData.status == "locked")
        {
            cageData.level = 0;
        }

        titleLabel.text = cageData.type.ToUpper() + " HOUSE";

        this.icon.sprite = MainMenuUI.Instance.GetListStoreItemIcon(STORE_ITEM_TYPE.animal)[(int)cageData.Type - 1];
        this.icon.SetNativeSize();

        int level = cageData.level;

        this.txtCurLevel.text = (cageData.status != "locked") ? String.Format("LVL {0}", level) : "UNLOCK";
        //this.txtUpgradeDetail.text = string.Format(cageData.type.ToUpper());


        int next_level = level + 1;
        if (cageData.status != "locked")
        {            
            if (next_level > GameDefine.MAX_ANIMAL_IN_CAGE)
            {
                next_level = GameDefine.MAX_ANIMAL_IN_CAGE;
            }
            this.txtNextLevel.text = String.Format("LVL {0}", next_level);
        }
        else
        {
            this.txtNextLevel.text = String.Format("LVL {0}", 1);
        }

        this.listHHLevelInfo.Clear();
        if (this.listHHLevelInfo.Count == 0)
        {
            this.listHHLevelInfo.Add(new HHLevelInfo(0, 0, 0, 0, CURRENCY_TYPE.WDNG));
            this.listHHLevelInfo.Add(new HHLevelInfo(1, cageData.upgradeDetail[0].durability, cageData.upgradeDetail[0].capacity, cageData.upgrade.Find(x => x.level == 1).amount, (CURRENCY_TYPE)cageData.upgrade.Find(x => x.level == 1).currency));
            this.listHHLevelInfo.Add(new HHLevelInfo(2, cageData.upgradeDetail[1].durability, cageData.upgradeDetail[1].capacity, cageData.upgrade.Find(x =>x.level == 2).amount, (CURRENCY_TYPE)cageData.upgrade.Find(x => x.level == 2).currency));
            this.listHHLevelInfo.Add(new HHLevelInfo(3, cageData.upgradeDetail[2].durability, cageData.upgradeDetail[2].capacity, cageData.upgrade.Find(x => x.level == 3).amount, (CURRENCY_TYPE)cageData.upgrade.Find(x => x.level == 3).currency));
            this.listHHLevelInfo.Add(new HHLevelInfo(4, cageData.upgradeDetail[3].durability, cageData.upgradeDetail[3].capacity, cageData.upgrade.Find(x => x.level == 4).amount, (CURRENCY_TYPE)cageData.upgrade.Find(x => x.level == 4).currency));
        }

        var curInfo = this.listHHLevelInfo[level];
        var nextInfo = this.listHHLevelInfo[next_level];

        this.reliabilityProgress.SetData(curInfo.durability, nextInfo.durability, this.listHHLevelInfo[this.listHHLevelInfo.Count-1].durability);
        this.capacityProgress.SetData(curInfo.capacity, nextInfo.capacity, 4);
        bool isEnableAc = cageData.level < GameDefine.MAX_ANIMAL_IN_CAGE;
        if (cageData.status != "locked")
        {
            //UPGRADE
            this.guiUpgrade.gameObject.SetActive(true);
            this.guiUnlock.gameObject.SetActive(false);

            this.guiUpgrade.Show(this.listHHLevelInfo[next_level].price, this.listHHLevelInfo[next_level].CurrencyType, isEnableAc, (price) =>
            {
                //Confirm Callback ==> UPGRADE
                this.ShowAlertConfirm(price, true, this.listHHLevelInfo[next_level].CurrencyType, (isConfirm) =>
                {
                    if (isConfirm)
                    {
                        //TODO: BienVT UPGRADE to LEVEL
                        APIMng.Instance.APICageUpgrade(cageData.id, (isSuccess, upgradedCageData, errMsg, code) => {
                            if(isSuccess && (Cage)upgradedCageData != null)
                            {
                                MapController.instance.UpdateUserInfo();

                                int idx = MapController.instance.currentlandDetail.cages.FindIndex(x => x.id == cageData.id);
                                Cage _tempCage = (Cage)upgradedCageData;
                                MapController.instance.currentlandDetail.cages[idx].level = _tempCage.level;
                                MapController.instance.currentlandDetail.cages[idx].capacity = _tempCage.capacity;
                                MapController.instance.currentlandDetail.cages[idx].status = _tempCage.status;
                                this.SetData(MapController.instance.currentlandDetail.cages[idx]);
                                if (MapController.instance.itemChoosing != null && MapController.instance.itemChoosing.GetComponent<AnimalHouseCtrl>() != null)
                                {
                                    MapController.instance.itemChoosing.GetComponent<AnimalHouseCtrl>().InitData(MapController.instance.currentlandDetail.cages[idx]);
                                }
                            }
                            else
                            {
                                MainMenuUI.Instance.ShowAlertMessage(errMsg);
                            }
                        });
                    }
                });

            });
        }
        else
        {
            //UNLOCK
            this.guiUnlock.gameObject.SetActive(true);
            this.guiUpgrade.gameObject.SetActive(false);
            this.guiUnlock.Show(this.listHHLevelInfo[next_level].price, this.listHHLevelInfo[next_level].CurrencyType, isEnableAc, (price) =>
            {
                //Confirm Callback ==> UNLOCK
                this.ShowAlertConfirm(price, false, this.listHHLevelInfo[next_level].CurrencyType, (isConfirm) =>
                {
                    if (isConfirm)
                    {
                        //TODO: BienVT UNLOCK to LEVEL
                        APIMng.Instance.APICageUnlock(cageData.id, (isSuccess, upgradedCageData, errMsg, code) => {
                            if(isSuccess)
                            {
                                MapController.instance.UpdateUserInfo();

                                int idx = MapController.instance.currentlandDetail.cages.FindIndex(x => x.id == cageData.id);
                                Cage _tempCage = (Cage)upgradedCageData;
                                MapController.instance.currentlandDetail.cages[idx].level = _tempCage.level;
                                MapController.instance.currentlandDetail.cages[idx].capacity = _tempCage.capacity;
                                MapController.instance.currentlandDetail.cages[idx].status = _tempCage.status;
                                this.SetData(MapController.instance.currentlandDetail.cages[idx]);
                                if (MapController.instance.itemChoosing != null && MapController.instance.itemChoosing.GetComponent<AnimalHouseCtrl>() != null)
                                {
                                    MapController.instance.itemChoosing.GetComponent<AnimalHouseCtrl>().InitData(MapController.instance.currentlandDetail.cages[idx]);
                                }
                            }    
                            else
                            {
                                MainMenuUI.Instance.ShowAlertMessage(errMsg);                                
                            }
                        });
                    }
                });
            });
        }
    }

    public void ShowAlertConfirm(int price, bool unlocked, CURRENCY_TYPE currencyType ,Action<bool> acConfirm)
    {
        var alert = MainMenuUI.Instance.AddAlertMessageConfirm();
        alert.SetBtnLeftActive(true);
        string currencyString = currencyType  == CURRENCY_TYPE.WDNG? "W-DNG" : "W-DNL";
        alert.showAlert("CONFIRM", String.Format("DO YOU CONFIRM SPEND {0} {1} TO {2}", price, currencyString, !unlocked ? "UNLOCK" : "UPGRADE"));
        alert.SetRightCallback((value) =>
        {
            if (acConfirm != null)
            {
                acConfirm(true);
            }
        });

        alert.SetLeftCallback((value) =>
        {
            if (acConfirm != null)
            {
                acConfirm(false);
            }
        });
    }

    public void OnClickAnimalCloseButton()
    {
        ShowPanelFollowState(HEN_HOUSE_UI_STATE.NORMAL);
    }

    public void OnClickUpgradeCloseButton()
    {
        ShowPanelFollowState(HEN_HOUSE_UI_STATE.NORMAL);
    }

    public void OnClickUpgradeButton()
    {
        ShowPanelFollowState(HEN_HOUSE_UI_STATE.UPGRADE);
    }

    public void OnClickGoToAnimalStoreBtn()
    {
        MainMenuUI.Instance.ShowUIStore_Animal();
        this.OnBtnCloseClicked(null);
    }
    public void OnClickAnimalListButton()
    {
        ShowPanelFollowState(HEN_HOUSE_UI_STATE.ANIMAL_LIST);
        animalListTitle.text = cageData.Type.ToString().ToUpper() + " RAISE INFO";
        notHaveAnimalObject.SetActive(false);
        // Delete all animal first
        List<HenHouseAnimalItem> listOldAnimal = animalListParent.GetComponentsInChildren<HenHouseAnimalItem>().ToList<HenHouseAnimalItem>();
        for (int i = listOldAnimal.Count - 1; i >= 0; i--)
        {
            Destroy(listOldAnimal[i].gameObject);
        }

        APIMng.Instance.APIListCage((isSuccess, listCage, errMsg, code) => {
            if (isSuccess && (List<Cage>)listCage != null)
            {
                List<Animal> listAnimal = ((List<Cage>)listCage).Find(x => x.id == cageData.id).animalList;
                listCages = (List<Cage>)listCage;
                cageData.animalList = new List<Animal>(listAnimal);

                if (cageData != null && cageData.animalList != null)
                {
                    for (int i = 0; i < cageData.animalList.Count; i++)
                    {
                        GameObject obj = GameObject.Instantiate(animalHenhousePrefab, animalListParent);
                        obj.GetComponent<HenHouseAnimalItem>().SetData(cageData.animalList[i]);
                    }
                }

                notHaveAnimalObject.SetActive(listAnimal.Count <= 0);
            }
            else
            {
                MainMenuUI.Instance.ShowAlertMessage(errMsg);
            }
        });
    }
    public List<Animal> getCage(string type)
    {
        List<Animal> getCage = (listCages.Find(x => x.type == type)).animalList;
        return getCage;
    }
}
public class HHLevelInfo
{
    public int level;
    public int durability;
    public int capacity;
    public int price;
    public CURRENCY_TYPE CurrencyType;
    public HHLevelInfo(int level, int reli, int capacity, int price, CURRENCY_TYPE cType)
    {
        this.level = level;
        this.durability = reli;
        this.capacity = capacity;
        this.price = price;
        this.CurrencyType = cType;
    }
}


// public class HenHouseInfo
// {
//     public int type;
//     public int capacity;
//     public HenHouseInfo(int type, int capacity)
//     {
//         this.type = type;
//         this.capacity = capacity;
//     }
// }
