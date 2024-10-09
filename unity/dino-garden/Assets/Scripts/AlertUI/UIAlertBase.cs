using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DinoExtensions;
public class UIAlertBase : UIPopupBase
{
    [SerializeField] Button btnLeft;
    [SerializeField] Button btnRight;
    [SerializeField] Button btnMinus;
    [SerializeField] Button btnPlus;
    [SerializeField] InputField inputField;
    [SerializeField] Text textPrice;
    [SerializeField] Text txtTitle;
    [SerializeField] Text txtContent;
    Action<int> _acLeftCalback;
    Action<int> _acRightCalback;

    bool _isConfirmed = false;
    private int _tag = 0;
    private int _amount = 1;
    private float _rate = 1;

    public override void Start()
    {
        base.Start();
        if (this.btnLeft) this.btnLeft.onClick.AddListener(this.OnBtnLeftClicked);
        if (this.btnRight) this.btnRight.onClick.AddListener(this.OnBtnRightClicked);
        if (this.btnMinus) this.btnMinus.onClick.AddListener(this.OnBtnMinusClicked);
        if (this.btnPlus) this.btnPlus.onClick.AddListener(this.OnBtnPlusClicked);
    }

    public void SetTitleAndContent(string title, string content)
    {
        if (this.txtTitle)
        {
            this.txtTitle.text = title;
        }

        if (this.txtTitle)
        {
            this.txtContent.text = content;
        }
    }

    public void OnBtnLeftClicked()
    {
        if (this._isConfirmed) return;
        this._isConfirmed = true;

        this.OnClose(null, () =>
        {
            if (this._acLeftCalback != null)
            {
                this._acLeftCalback(this._amount);
            }
        });
    }

    public void OnBtnRightClicked()
    {
        if (this._isConfirmed) return;
        this._isConfirmed = true;

        this.OnClose(null, () =>
        {
            if (this._acRightCalback != null)
            {
                this._acRightCalback(this._amount);
            }
        });
    }

    public void SetLeftCallback(Action<int> callback)
    {
        this._acLeftCalback = callback;
    }

    public void SetRightCallback(Action<int> callback)
    {
        this._acRightCalback = callback;
    }

    public void OnBtnMinusClicked()
    {
        if (this._amount <= 1) return;
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
        this.inputField.text = amount.FormatMoney();
        var price = this._amount * this._rate;
        this.textPrice.text = price.FormatMoney();
    }


    public void SetStartAmountAndRate(int amount, float rate)
    {
        this._rate = rate;
        this.SetAmount(amount);
    }

    public int alertTag
    {
        get
        {
            return this._tag;
        }

        set
        {
            this._tag = value;
        }
    }

    public void OnInputFieldEndEdit()
    {
        Debug.Log("OnInputFieldEditEnd => " + this.inputField.text);
        var amount = this.inputField.text.ToFloat(1);
        this.SetAmount((int)amount);
    }

    public void SetBtnLeftActive(bool isActive)
    {
        this.btnLeft.gameObject.SetActive(isActive);
    }

    public void SetBtnRightActive(bool isActive)
    {
        this.btnRight.gameObject.SetActive(isActive);
    }

}
