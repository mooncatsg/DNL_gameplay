using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class BuildingCropsItemUI : MonoBehaviour
{
    [SerializeField] GameObject itemBg;
    [SerializeField] GameObject iconSelect0;
    [SerializeField] GameObject iconSelect1;
    [SerializeField] Image itemIcon;
    [SerializeField] Text txtRarity;

    public Action<BuildingCropsItemUI> acFarmItemClicked;
    public int index = 0;
    private bool _isEnableSelect = false;
    private bool _isSelected = false;
    public int cropsType;
    public int cropId = -1;


    public void SetData(int index, int cropsType,int _cropId, Action<BuildingCropsItemUI> _acFarmItemClicked)
    {
        this.index = index;
        this.cropsType = cropsType;
        this.cropId = _cropId;
        if(MainMenuUI.Instance.GetListStoreItemIcon(STORE_ITEM_TYPE.crop)[cropsType - 1])
            this.itemIcon.sprite = MainMenuUI.Instance.GetListStoreItemIcon(STORE_ITEM_TYPE.crop)[cropsType - 1];
        this.acFarmItemClicked = _acFarmItemClicked;
        this.txtRarity.text = GameDefine.getRarity(cropsType);
    }
    public void OnSelectClicked()
    {
        if (!this._isEnableSelect)
            return;

        if (this.acFarmItemClicked != null)
        {
            this.acFarmItemClicked(this);
        }
        this._isSelected = !this._isSelected;
        this.UpdateFarmStatus();
    }

    public void DeselectedStatus()
    {
        this._isSelected = false;
        this.UpdateFarmStatus();
    }
    public void UpdateFarmStatus()
    {
        this.itemBg.SetActive(this._isEnableSelect);
        this.iconSelect0.SetActive(this._isEnableSelect);
        this.iconSelect1.SetActive(this._isSelected);
    }

    public void SetSelectEnable(bool isEnable)
    {
        this._isEnableSelect = isEnable;
        this._isSelected = false;
        this.UpdateFarmStatus();
    }

    public bool isSelect()
    {
        return this._isSelected;
    }
}
