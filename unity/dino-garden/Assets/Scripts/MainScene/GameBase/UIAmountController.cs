using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DinoExtensions;
public class UIAmountController : MonoBehaviour
{
    [SerializeField] Button btnMinus;
    [SerializeField] Button btnPlus;
    [SerializeField] Text inputText;
    [SerializeField] Text textPrice;
    public Action<int> acSetAmountCalback;
    bool _isConfirmed = false;
    private int _tag = 0;
    private int _amount = 0;
    private float _rate = 1;
    private int maxCageCapacity = 1;

    public void Start()
    {
        this.btnMinus.onClick.AddListener(this.OnBtnMinusClicked);
        this.btnPlus.onClick.AddListener(this.OnBtnPlusClicked);
    }

    public void setRate(int rate)
    {
        this._rate = rate;
    }

    public void OnBtnMinusClicked()
    {
        if (this._amount <= 0) return;
        this.SetAmount(this._amount - 1);
    }

    public void OnBtnPlusClicked()
    {
        // if (this._amount <= 0) return;
        Debug.Log("OnBtnPlusClicked");

        this.SetAmount(this._amount + 1);

    }

    public int GetAmount()
    {
        return this._amount;
    }

    public void SetAmount(int amount)
    {
        Debug.Log("SetAmount = " + amount.ToString());
        this._amount = amount;
        this.inputText.text = amount.FormatMoney();

        StoreItemUI storeItemUI = this.transform.parent.GetComponent<StoreItemUI>();                
        if (storeItemUI != null && storeItemUI._itemInfo.type == STORE_ITEM_TYPE.animal)
        {
            Cage thisCage = MapController.instance.currentlandDetail.cages.Find(x => x.type == storeItemUI._itemInfo.name);
            if(thisCage != null)
            {
                maxCageCapacity = thisCage.capacity - thisCage.animalList.Count;
                if (maxCageCapacity < 0 || thisCage.Status == CageStatus.locked)
                    maxCageCapacity = 0;

                this.inputText.text = amount.FormatMoney() + "/" + maxCageCapacity;
            }    
        }    
            
            
        var price = this._amount * this._rate;
        if (this.textPrice)
        {
            this.textPrice.text = price.FormatMoney();
        }

        if (this.acSetAmountCalback != null)
        {
            this.acSetAmountCalback(amount);
        }
    }


    public void SetStartAmountAndRate(int amount, float rate)
    {
        this._rate = rate;
        this.SetAmount(amount);
    }

    //public void OnInputFieldEndEdit()
    //{
    //    Debug.Log("OnInputFieldEditEnd => " + this.inputField.text);
    //    var amount = this.inputField.text.ToFloat(1);
    //    this.SetAmount((int)amount);
    //}

    public void SetActionAmountCallback(Action<int> callback)
    {
        this.acSetAmountCalback = callback;
    }
}
