using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DinoExtensions;
using System.Linq;
using System;
public class StorePurchaseConfirm : UIPopupBase
{
    [SerializeField] public Text txtGValue;
    [SerializeField] public Text txtDnlValue;
    [SerializeField] public Button btnPurchase;
    [SerializeField] public VerticalLayoutGroup layoutGCoin;
    [SerializeField] public VerticalLayoutGroup layoutDnlCoin;
    [SerializeField] public HorizontalLayoutGroup layoutTotalCash;
    [SerializeField] public GameObject content;
    [SerializeField] public GameObject purchaseItemPrefab;
    List<ItemPurchaseInfo> dataPurchaseList = new List<ItemPurchaseInfo>();
    private int _cropsBoxAmout = 0;
    private int _gCoin = 0;
    private int _dnlCoin = 0;

    public override void Start()
    {
        base.Start();
        this.btnPurchase.onClick.AddListener(this.OnBtnPurchaseClicked);
    }

    public void OnBtnPurchaseClicked()
    {
        this.OnBtnCloseClicked(this.btnPurchase);

        if (this._cropsBoxAmout > 0)
        {
            APIMng.Instance.APIBuyBox(_cropsBoxAmout, (isSuccess, cropDetail, errMsg, code) =>
            {
                if (isSuccess && ((List<BuyCrop>)cropDetail) != null)
                {
                    MapController.instance.UpdateUserInfo();

                    StoreUI.Instance.ShowCropsResultPurchase(this._cropsBoxAmout, (List<BuyCrop>)cropDetail);
                }
                else
                {
                    StoreUI.Instance.ShowAlertPurchaseFail(errMsg);
                }
            });
        }
        else
        {
            // CALL Buy Animal,seeds and decor
            if (dataPurchaseList.Count > 0)
            {
                PurchaseMultiple();
                //StartCoroutine(IEPurchase());
            }
        }
    }

    void PurchaseMultiple()
    {
        List<BuyAll> buyAllData = new List<BuyAll>();
        // Get data
        foreach (var f in dataPurchaseList)
        {
            if(f.amount > 0)
                buyAllData.Add(new BuyAll(f.type.ToString(), f.name, f.amount));
        }

        if (buyAllData.Count > 0)
        {
            APIMng.Instance.APIBuyMultiple(buyAllData, (isSuccess, buyAllReturnList, errMsg, code) =>
            {
                if (isSuccess && ((List<BuyAll>)buyAllReturnList) != null)
                {
                    MapController.instance.UpdateUserInfo();

                    foreach (BuyAll buyallData in (List<BuyAll>)buyAllReturnList)
                    {
                        if(buyallData.classification == "animal")
                        {
                            MapController.instance.UpdateNewAnimal();
                        }
                        else if (buyallData.Type == STORE_ITEM_TYPE.seed)
                        {

                        }
                    }

                    StoreUI.Instance.ShowAlertPurchaseSuccess(this._dnlCoin, this._gCoin, (List<BuyAll>)buyAllReturnList);
                }
                else
                {
                    StoreUI.Instance.ShowAlertPurchaseFail(errMsg);
                }
            });
        }
    }

    public void ShowStorePurchaseConfirm(Dictionary<int, ItemPurchaseInfo> dataPurchase, int dnlCoin, int gCoin)
    {
        dataPurchaseList.Clear();
        dataPurchaseList = dataPurchase.Values.ToList();

        this._dnlCoin = dnlCoin;
        this._gCoin = gCoin;
        foreach (Transform child in this.content.transform)
        {
            Destroy(child.gameObject);
        }

        //Check has Crops Gacha Box
        this._cropsBoxAmout = 0;

        foreach (KeyValuePair<int, ItemPurchaseInfo> kvp in dataPurchase)
        {
            Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            
            if (kvp.Value.type == STORE_ITEM_TYPE.crop_box)
            {
                this._cropsBoxAmout = kvp.Value.amount;
                this.AddPurchaseItem(kvp.Value);
                continue;
            }
            else
            {
                if (kvp.Value.amount > 0)
                {
                    this.AddPurchaseItem(kvp.Value);
                }
            }
        }

        this.txtDnlValue.text = dnlCoin.FormatMoney();
        this.txtGValue.text = gCoin.FormatMoney();

        this.layoutDnlCoin.gameObject.ForceRebuildLayoutImmediate();
        this.layoutGCoin.gameObject.ForceRebuildLayoutImmediate();
        this.layoutTotalCash.gameObject.ForceRebuildLayoutImmediate();

        this.Show();
    }

    void AddPurchaseItem(ItemPurchaseInfo itemInfo)
    {
        var item = Instantiate(this.purchaseItemPrefab, this.content.transform);
        item.GetComponent<StorePurchaseConfirmItem>().SetData(itemInfo);
    }
}
