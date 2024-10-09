using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DinoExtensions;
public class StorePurchaseController : MonoBehaviour
{
    [SerializeField] public Text txtGValue;
    [SerializeField] public Text txtDnlValue;
    [SerializeField] public Button btnPurchase;
    [SerializeField] public VerticalLayoutGroup layoutGCoin;
    [SerializeField] public VerticalLayoutGroup layoutDnlCoin;
    [SerializeField] public HorizontalLayoutGroup layoutTotalCash;
    Dictionary<int, ItemPurchaseInfo> dataPurchase = new Dictionary<int, ItemPurchaseInfo>();
    public static StorePurchaseController Instance;

    private int _gCoin = 0;
    private int _dnlCoin = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        this.btnPurchase.onClick.AddListener(this.OnBtnPurchaseClicked);
        this.SetBtnPurchaseEnable(false);
    }

    public void OnBtnPurchaseClicked()
    {
        StoreUI.Instance.ShowPurchaseConfirm(this.dataPurchase, this._dnlCoin, this._gCoin);
    }

    public void SetOrderItem(StoreItemInfo itemInfo, int amount)
    {
        var key = (int)itemInfo.type * itemInfo.rate + itemInfo.itemID; //String.Format("{0}_", itemInfo.type);
        var purchaseInfo = new ItemPurchaseInfo(itemInfo, amount);
        if (dataPurchase.ContainsKey(key))
        {
            dataPurchase[key] = purchaseInfo;
        }
        else
        {
            dataPurchase.Add(key, purchaseInfo);
        }

        this.CalculatePrice();
    }

    public void CalculatePrice()
    {
        this._gCoin = 0;
        this._dnlCoin = 0;

        foreach (KeyValuePair<int, ItemPurchaseInfo> kvp in dataPurchase)
        {
            // Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            var item = kvp.Value;
            if (item.currencyType == (int)CURRENCY_TYPE.WDNL)
            {
                _dnlCoin += kvp.Value.rate * kvp.Value.amount;
            }
            else
            {
                _gCoin += kvp.Value.rate * kvp.Value.amount;
            }
        }

        this.SetBtnPurchaseEnable(this._gCoin > 0 || this._dnlCoin > 0);

        this.txtDnlValue.text = _dnlCoin.FormatMoney();
        this.txtGValue.text = _gCoin.FormatMoney();

        this.layoutDnlCoin.gameObject.ForceRebuildLayoutImmediate();
        this.layoutGCoin.gameObject.ForceRebuildLayoutImmediate();
        this.layoutTotalCash.gameObject.ForceRebuildLayoutImmediate();
    }

    void SetBtnPurchaseEnable(bool isEnable)
    {
        if (isEnable)
        {
            // this.btnPurchase.gameObject.SetObjectAlpha(1.0f);
            this.btnPurchase.interactable = true;
        }
        else
        {
            // this.btnPurchase.gameObject.SetObjectAlpha(0.85f);
            this.btnPurchase.interactable = false;
        }
    }

    public void ResetData()
    {
        this.dataPurchase.Clear();
        this._dnlCoin = 0;
        this._gCoin = 0;
        this.txtDnlValue.text = _dnlCoin.FormatMoney();
        this.txtGValue.text = _gCoin.FormatMoney();
        this.SetBtnPurchaseEnable(false);
    }
}
