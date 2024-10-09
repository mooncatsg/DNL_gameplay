using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DinoExtensions;
using UnityEngine.SceneManagement;
using System;
using System.Globalization;

public class MainMenuUI : MonoBehaviour
{
    [Header("USER INFO")]
    [SerializeField] Text dinoGardenTokenText;
    [SerializeField] Button dinoGardenTokenButton;

    [SerializeField] Text dinoLandTokenText;
    [SerializeField] Button dinoLandTokenButton;

    [Space]
    [SerializeField] Button btnBuild;
    [SerializeField] Button btnDinoCare;
    [SerializeField] Button btnLand;
    [SerializeField] Button btnShop;
    [SerializeField] Button btnLogout;
    [SerializeField] Button btnSetting;
    [SerializeField] Button btnMusic;
    [SerializeField] Button btnSound;
    [SerializeField] Button btnTele;
    [SerializeField] Button btnDiscord;
    [SerializeField] Button btnTwitter;
    [SerializeField] Button btnTerms;
    [SerializeField] Button btnCamera;
    [SerializeField] Button btnRewards;
    int isPlayEffectSound = 1;
    int isPlayBgSound = 1;
    [SerializeField] Image btnMusicSprite;
    [SerializeField] Image btnEffectSprite;
    [SerializeField] GameObject guiPopupUI;
    [SerializeField] GameObject guiBuilding;
    [SerializeField] GameObject guiSetting;
    [SerializeField] GameObject guiStore;
    [SerializeField] GameObject guiHenHouse;
    [SerializeField] GameObject guiPlantTreeUI;
    [SerializeField] GameObject guiAlertUI;
    [SerializeField] GameObject guiBlockInput;
    [SerializeField] GameObject guiAction;
    [SerializeField] GameObject guiStorage;
    [SerializeField] GameObject guiOrderList;
    [SerializeField] GameObject guiLandList;
    [SerializeField] GameObject alertFarmPurchasePrefab;
    [SerializeField] GameObject alertMessagePrefab;
    [SerializeField] public List<Sprite> listSeedIcons;
    [SerializeField] public List<Sprite> listAnimalIcons;
    [SerializeField] public List<Sprite> listAnimalProductIcons;
    [SerializeField] public List<Sprite> listDecorIcons;
    [SerializeField] public List<Sprite> listCropBoxIcons;
    [SerializeField] public List<Sprite> listCropIcons;
    [SerializeField] public List<Sprite> listFertilizerIcons;
    [SerializeField] public List<Sprite> listCurrencyIcons;
    public List<int> listCropsPurchased = new List<int>();
    public static MainMenuUI Instance;
    private void Start()
    {
        Instance = this;
        btnBuild.onClick.AddListener(OnBtnBuildClicked);
        btnDinoCare.onClick.AddListener(OnBtnDinoCareClicked);
        btnLand.onClick.AddListener(OnBtnLandClicked);
        btnShop.onClick.AddListener(OnBtnShopClicked);
        btnLogout.onClick.AddListener(OnBtnLogoutClicked);
        btnCamera.onClick.AddListener(OnBtnCameraClicked);
        btnRewards.onClick.AddListener(OnBtnRewardClicked);
        btnSetting.onClick.AddListener(OnBtnSetting);
        btnMusic.onClick.AddListener(OnBtnMusic);
        btnSound.onClick.AddListener(OnBtnSound);
        btnTele.onClick.AddListener(OnbtnTele);
        btnDiscord.onClick.AddListener(OnbtnDiscord);
        btnTwitter.onClick.AddListener(OnbtnTwitter);
        btnTerms.onClick.AddListener(OnbtnTerms);
        isPlayBgSound = PlayerPrefs.GetInt("isPlayBgSound", 1);
        isPlayEffectSound = PlayerPrefs.GetInt("isPlayEffectSound", 1);

        if (isPlayEffectSound == 1)
        {
            btnEffectSprite.sprite = Resources.Load<Sprite>("icon_Plant/Sound");
            SoundManager.instance.setEffectSound(true);
        }
        else
        {
            btnEffectSprite.sprite = Resources.Load<Sprite>("icon_Plant/Sound_disable");
            SoundManager.instance.setEffectSound(false);
        }
        if (isPlayBgSound == 1)
        {
            btnMusicSprite.sprite = Resources.Load<Sprite>("icon_Plant/Music");
            SoundManager.instance.setBgSound(true);
        }
        else
        {
            btnMusicSprite.sprite = Resources.Load<Sprite>("icon_Plant/Music_disable");
            SoundManager.instance.setBgSound(false);
        }
    }

    void OnBtnBuildClicked()
    {
        Debug.Log("==> Click: BUILD");
        // this.ShowBuildStore();
        this.ShowUIBuilding();
        SoundManager.instance.PlaySound(SoundType.BUTCLICK);
    }
    void OnbtnTele()
    {
        Application.OpenURL("https://t.me/dinolandglobal");
    }
    void OnbtnDiscord()
    {
        Application.OpenURL("https://discord.com/invite/ujctynMMk3");
    }
    void OnbtnTwitter()
    {
        Application.OpenURL("https://twitter.com/dinolandgame");
    }
    void OnbtnTerms()
    {
        Application.OpenURL("https://dinoland.substack.com/");
    }
    
    void OnBtnMusic()
    {
        isPlayBgSound = PlayerPrefs.GetInt("isPlayBgSound", 1);
        if (isPlayBgSound == 1)
        {
            btnMusicSprite.sprite = Resources.Load<Sprite>("icon_Plant/Music_disable");
            PlayerPrefs.SetInt("isPlayBgSound", 0);
            SoundManager.instance.setBgSound(false);
        }
        else
        {
            btnMusicSprite.sprite = Resources.Load<Sprite>("icon_Plant/Music");
            PlayerPrefs.SetInt("isPlayBgSound", 1);
            SoundManager.instance.setBgSound(true);
        }
    }
    void OnBtnSound()
    {
        isPlayEffectSound = PlayerPrefs.GetInt("isPlayEffectSound", 1);
        if (isPlayEffectSound == 1)
        {
            btnEffectSprite.sprite = Resources.Load<Sprite>("icon_Plant/Sound_disable");
            PlayerPrefs.SetInt("isPlayEffectSound", 0);
            SoundManager.instance.setEffectSound(false);
        }
        else
        {
            btnEffectSprite.sprite = Resources.Load<Sprite>("icon_Plant/Sound");
            PlayerPrefs.SetInt("isPlayEffectSound", 1);
            SoundManager.instance.setEffectSound(true);
        }
    }
    void OnBtnSetting()
    {
        Debug.Log("==> Click: SETTING");
        // this.ShowBuildStore();
        this.ShowUISetting();
        SoundManager.instance.PlaySound(SoundType.BUTCLICK);
    }
    void OnBtnDinoCareClicked()
    {
        SoundManager.instance.PlaySound(SoundType.BUTCLICK);
        Debug.Log("==> Click: DINO CARE");
        //this.ShowUIHenHouse();
        // this.ShowActionUI(ACTION_UI_TYPE.TAKE_CARE_ANIMAL);
        //this.ShowStorageUI();
        DinoCareManager.instance.Show();

        //BienVT ==> SetTextMoneyWithAnimChange
        // MainMenuUI.SetTextMoneyWithAnimChange(this.dinoLandTokenText, 8529, 11000, 1.0f);
    }

    void OnBtnRewardClicked()
    {
        SoundManager.instance.PlaySound(SoundType.BUTCLICK);
        Debug.Log("==> Click: REWARD - ORDER LIST");
        //this.ShowUIHenHouse();
        // this.ShowActionUI(ACTION_UI_TYPE.TAKE_CARE_ANIMAL);
        this.ShowOrderListUI();
    }

    void OnBtnLandClicked()
    {
        SoundManager.instance.PlaySound(SoundType.BUTCLICK);
        Debug.Log("==> Click: LAND");
    }
    void OnBtnShopClicked()
    {
        SoundManager.instance.PlaySound(SoundType.BUTCLICK);
        Debug.Log("==> Click: Shop");
        // this.ShowBuildStore();
        this.ShowUIStore();
    }

    void OnBtnLogoutClicked()
    {
        SoundManager.instance.PlaySound(SoundType.BUTCLICK);
        Debug.Log("==> Click: Logout");
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Start");
    }


    void OnBtnCameraClicked()
    {
        Debug.Log("==> Click: CAMERA");
        SoundManager.instance.PlaySound(SoundType.BUTCLICK);
    }

    public void ShowUserInfo()
    {
        if (MapController.instance.userInfor != null)
        {
            dinoGardenTokenText.text = "" + String.Format("{0:#,##0.###}", Convert.ToDouble(MapController.instance.userInfor.dngBalance));
            dinoLandTokenText.text = "" + String.Format("{0:#,##0.###}", Convert.ToDouble(MapController.instance.userInfor.money));
        }
    }

    public void ShowUIBuilding()
    {
        if (this.guiBuilding)
        {
            this.guiBuilding.SetActive(true);
            this.guiBuilding.GetComponent<BuildingUI>().Show(TRANSITION_TYPE.ZOOM, TRANSITION_TYPE.ZOOM, (() => { })); 
            this.guiBuilding.GetComponent<BuildingUI>().tabbar.clearDataAndActive();

        }
        else
        {
            Debug.LogWarning("WARN ==> guiBuildStore = NULL");
        }
    }
    public void ShowUISetting()
    {
        if (this.guiSetting)
        {
            this.guiSetting.SetActive(true);
            this.guiSetting.GetComponent<UIPopupBase>().Show(TRANSITION_TYPE.ZOOM, TRANSITION_TYPE.ZOOM, (() => { }));
            //this.guiSetting.GetComponent<UIPopupBase>().tabbar.clearDataAndActive();
        }
        else
        {
            Debug.LogWarning("WARN ==> guiBuildStore = NULL");
        }
    }

    public void ShowUIStore()
    {
        if (this.guiStore)
        {
            this.guiStore.SetActive(true);
            this.guiStore.GetComponent<StoreUI>().SetDataWithTabbar(3);
            this.guiStore.GetComponent<StoreUI>().Show(TRANSITION_TYPE.ZOOM, TRANSITION_TYPE.ZOOM, (() => { }));
        }
        else
        {
            Debug.LogWarning("WARN ==> guiBuildStore = NULL");
        }
    }

    public void ShowUIStore_Fertilizer()
    {
        if (this.guiStore)
        {
            this.guiStore.SetActive(true);
            this.guiStore.GetComponent<StoreUI>().SetDataWithTabbar(2);
            this.guiStore.GetComponent<StoreUI>().Show(TRANSITION_TYPE.ZOOM, TRANSITION_TYPE.ZOOM, (() => { }));
        }
        else
        {
            Debug.LogWarning("WARN ==> guiBuildStore = NULL");
        }
    }

    public void ShowUIStore_Animal()
    {
        if (this.guiStore)
        {
            this.guiStore.SetActive(true);
            this.guiStore.GetComponent<StoreUI>().SetDataWithTabbar(1);
            this.guiStore.GetComponent<StoreUI>().Show(TRANSITION_TYPE.ZOOM, TRANSITION_TYPE.ZOOM, (() => { }));
        }
        else
        {
            Debug.LogWarning("WARN ==> guiBuildStore = NULL");
        }
    }

    public void ShowUIStore_Seed()
    {
        if (this.guiStore)
        {
            this.guiStore.SetActive(true);
            this.guiStore.GetComponent<StoreUI>().SetDataWithTabbar(0);
            this.guiStore.GetComponent<StoreUI>().Show(TRANSITION_TYPE.ZOOM, TRANSITION_TYPE.ZOOM, (() => { }));
        }
        else
        {
            Debug.LogWarning("WARN ==> guiBuildStore = NULL");
        }
    }

    public void ShowUIHenHouse(Cage cageData)
    {
        this.guiHenHouse.SetActive(true);
        Debug.LogError(cageData.Type);
        this.guiHenHouse.GetComponent<HenHouse>().ShowHenHouseByInfo(cageData);
    }

    public void ShowActionUI(ACTION_UI_TYPE actionType)
    {
        this.guiAction.SetActive(true);
        this.guiAction.GetComponent<ActionUI>().ShowActionByType(actionType);
    }

    public PlantTreeUI GetPlanTreeUI()
    {
        return guiPlantTreeUI.GetComponent<PlantTreeUI>();
    }
    public void ShowPlantTreeUI(ACTION_TYPE actionType)
    {
        //this.ShowActionUI();
        if (this.guiPlantTreeUI)
        {
            this.guiPlantTreeUI.SetActive(true);
            var plantTreeUI = this.guiPlantTreeUI.GetComponent<PlantTreeUI>();
            if (plantTreeUI)
            {
                plantTreeUI.SetData(actionType);
                plantTreeUI.Show(TRANSITION_TYPE.ZOOM, TRANSITION_TYPE.ZOOM, (() => { }));
            }
        }
        else
        {
            Debug.LogWarning("WARN ==> guiPlantTreeUI = NULL");
        }
    }

    public void ShowStorageUI()
    {
        this.guiStorage.SetActive(true);
        this.guiStorage.GetComponent<StorageUI>().ShowStorage();
    }

    public void ShowOrderListUI()
    {
        this.guiOrderList.SetActive(true);
        this.guiOrderList.GetComponent<OrderUIManager>().ShowOrderList();
    }

    public void ShowLandListUI()
    {
        this.guiLandList.SetActive(true);
    }

    public bool IsUIActive()
    {
        // bool result = false;
        // foreach (Transform child in this.guiPopupUI.transform)
        // {
        //     if (child.gameObject.activeSelf)
        //     {
        //         result = true;
        //         break;
        //     }
        // }
        // return result;
        return this.guiBuilding.activeSelf || this.guiPlantTreeUI.activeSelf || this.guiBlockInput.activeSelf || this.guiStore.activeSelf
         || this.guiHenHouse.activeSelf || this.guiAction.activeSelf || this.guiStorage.activeSelf || this.guiOrderList.activeSelf || this.guiSetting.activeSelf || this.guiLandList.activeSelf || this.guiAlertUI.transform.childCount > 0 || DinoCareManager.IsInstantiated;
    }

    public UIAlertFarmPurchase AddAlertFarmPurchase()
    {
        var alert_item = Instantiate(alertFarmPurchasePrefab, this.guiAlertUI.transform);
        return alert_item.GetComponent<UIAlertFarmPurchase>();
    }

    public UIAlertMessage AddAlertMessageConfirm()
    {
        var alert_item = Instantiate(alertMessagePrefab, this.guiAlertUI.transform);
        return alert_item.GetComponent<UIAlertMessage>();
    }
    public UIAlertMessage alert = null;
    public void ShowAlertMessage(string stringAlert, string title = "MESSAGE",bool isConfirm = false ,Action callbackLeft = null, Action callbackRight = null)
    {
        if (alert != null)
            return;
        alert = AddAlertMessageConfirm();
        alert.SetBtnLeftActive(isConfirm);
        alert.showAlert(title, stringAlert);
        alert.SetRightCallback((amount) =>
        {
            if (callbackRight != null)
                callbackRight.Invoke();
        });
        alert.SetLeftCallback((amount) =>
        {
            if (callbackLeft != null)
                callbackLeft.Invoke();
        });
    }

    public Sprite GetCurrencySprite(CURRENCY_TYPE currencyType)
    {
        return listCurrencyIcons[(int)currencyType-1];
    }

    public List<Sprite> GetListStoreItemIcon(STORE_ITEM_TYPE type)
    {
        var results = new List<Sprite>();
        switch (type)
        {
            case STORE_ITEM_TYPE.seed:
            case STORE_ITEM_TYPE.plant:
                results = this.listSeedIcons;
                break;

            case STORE_ITEM_TYPE.animal:
                results = this.listAnimalIcons;
                break;

            case STORE_ITEM_TYPE.decor:
                results = this.listDecorIcons;
                break;

            case STORE_ITEM_TYPE.crop_box:
                results = this.listCropBoxIcons;
                break;

            case STORE_ITEM_TYPE.crop:
                results = this.listCropIcons;
                break;

            case STORE_ITEM_TYPE.fertilizer:
                results = this.listFertilizerIcons;
                break;
        }

        return results;
    }

    public void OnCloseStore()
    {
        // this.close
    }

    public static void SetTextMoneyWithAnimChange(Text text, int fromVl, int toVl, float duration)
    {
        float value = fromVl;
        text.text = value.FormatMoney();

        DOTween.To(() => value, x => value = x, toVl, duration).OnUpdate(() =>
            {
                Debug.Log(value);
                text.text = value.FormatMoney();
            }).OnComplete(() =>
            {
                text.text = toVl.FormatMoney();
            });
    }
}
