using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DinoGarden;
using DinoExtensions;
using UnityEngine.UI;
using System;
public class StorageUI : UIPopupBase
{
    [SerializeField] Tabbar tabbar;
    [SerializeField] GameObject content;
    [SerializeField] GameObject storageItemPrefab;
    [SerializeField] StorageSellUI guiSell;
    [SerializeField] Text txtCapacity;
    [SerializeField] GameObject guiBtnStore;
    [SerializeField] Button upgradeBtn;


    public static StorageUI Instance;
    List<StorageInfo> listInfo = new List<StorageInfo>();

    public Warehouse warehouseData;
    int _nextCapacity = 20;
    int _pricePurchase = 500;
    bool _isShowed = false;

    private void Awake()
    {
        Instance = this;
        this.tabbar.acTabClick = this.OnTabbarClicked;
        upgradeBtn.onClick.AddListener(OnUpgradeButtonClick);
    }

    public void ShowStorage()
    {
        this._isShowed = false;
        this.tabbar.setTabIndex(0);
        this.Show();
    }

    public void OnUpgradeButtonClick()
    {
        if (warehouseData != null && warehouseData.upgradeDetail.Find(x => x.level == (warehouseData.level + 1)) != null)
        {
            UpgradeWarehouse currentUpgrade = warehouseData.upgradeDetail.Find(x => x.level == (warehouseData.level + 1));
            MainMenuUI.Instance.ShowAlertMessage("DO YOU WANT TO UPGRADE STORAGE WITH " + currentUpgrade.upgrade_price.amount + " " + ((CURRENCY_TYPE)currentUpgrade.upgrade_price.typeCurrency) + " ?", "UPGRADE", true, null, () => {
                APIMng.Instance.APIWarehouseUpgrade((isSuccess, _warehouseData, errMsg, code) => 
                {
                    if (isSuccess)
                    {
                        Warehouse tempWarehouseData = (Warehouse)_warehouseData;
                        warehouseData.capacity = tempWarehouseData.capacity;
                        warehouseData.level = tempWarehouseData.level;

                        MainMenuUI.Instance.ShowAlertMessage("SUCCESS TO UPGRADE");
                        this.txtCapacity.text = String.Format("CAPACITY: {0}/{1}", WarehouseCurrentCounting(), warehouseData.capacity);
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
    public void OnTabbarClicked(int index)
    {
        // this._isShowed = true;
        Debug.Log("==> OnTabbarClicked: " + index);
        this.content.DestroyAllChild();

        if (!this._isShowed)
        {            
            this.GetDataAndShow(index);
        }
        else
        {
            this.ShowData(index);
        }
    }

    void GetDataAndShow(int indexType)
    {
        this.listInfo.Clear();

        APIMng.Instance.APIWarehouseDetail((isSuccess, _warehouseData, errMsg, code) =>
        {
            if(isSuccess)
            {
                // Destroy all old object
                this.content.DestroyAllChild();

                warehouseData = (Warehouse)_warehouseData;
                foreach (WarehouseItem item in warehouseData.items)
                {
                    if (item.Quantity > 0)
                        this.listInfo.Add(new StorageInfo(storeType: STORE_TYPE.item, type: item.Classification, name: item.type, 100, item.quantity));
                }

                foreach (WarehouseItem product in warehouseData.products)
                {
                    if (product.Quantity > 0)
                        this.listInfo.Add(new StorageInfo(storeType: STORE_TYPE.product, type: product.Classification, name: product.type, 100, product.quantity));
                }

                this.ShowData(indexType);
                this._isShowed = true;
                this.txtCapacity.text = String.Format("CAPACITY: {0}/{1}", WarehouseCurrentCounting(), warehouseData.capacity);
            }
            else
            {
                MainMenuUI.Instance.ShowAlertMessage(errMsg);
            }
        });
    }

    public float WarehouseCurrentCounting()
    {
        float counting = 0;
        foreach (WarehouseItem item in warehouseData.items)
        {
            counting += item.Quantity;
        }
        foreach (WarehouseItem product in warehouseData.products)
        {
            counting += product.Quantity;
        }

        return counting;
    }

    public int tabbarIndex = -1;
    void ShowData(int indexType)
    {
        //TODO ==> GET DATA FROM API
        List<StorageInfo> listFilter = new List<StorageInfo>();
        tabbarIndex = indexType;
        switch (indexType)
        {
            case 0:
                listFilter = this.listInfo;
                break;

            case 1:
                listFilter = this.listInfo.FindAll(x => x.type == STORE_ITEM_TYPE.seed);
                break;

            case 2:
                listFilter = this.listInfo.FindAll(x => x.storeType == STORE_TYPE.product);
                break;

            case 3:
                listFilter = this.listInfo.FindAll(x => x.type == STORE_ITEM_TYPE.fertilizer);
                break;

            default:
                break;
        }

        this.AddItems(listFilter);
    }


    void AddItems(List<StorageInfo> listInfo)
    {
        if (listInfo.Count > 0)
        {
            this.guiBtnStore.SetActive(false);
            for (int i = 0; i < listInfo.Count; i++)
            {
                var item = Instantiate(this.storageItemPrefab, this.content.transform);
                var comp = item.GetComponent<StorageItemUI>();
                comp.SetData(listInfo[i]);
                comp.acCallback = this.OnItemClicked;
            }
        }
        else
        {
            this.guiBtnStore.SetActive(true);
        }

    }

    public void OnItemClicked(StorageInfo info, Vector3 pointItem)
    {
        this.guiSell.gameObject.SetActive(true);
        this.guiSell.ShowSellPopup(info, pointItem);
    }

    public void OnBtnShowStore()
    {
        this.OnClose(null, () =>
        {
            switch(tabbarIndex)
            {
                case 0:
                    MainMenuUI.Instance.ShowUIStore();
                    break;
                case 1:
                    MainMenuUI.Instance.ShowUIStore_Seed();
                    break;
                case 2:
                    MainMenuUI.Instance.ShowUIStore();
                    break;
                case 3:
                    MainMenuUI.Instance.ShowUIStore_Fertilizer();
                    break;
            }
            
        });
    }
}
