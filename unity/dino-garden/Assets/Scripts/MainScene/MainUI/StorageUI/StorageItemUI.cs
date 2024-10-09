using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DinoExtensions;
public class StorageItemUI : MonoBehaviour
{
    [SerializeField] Text textName;
    [SerializeField] Image icon;
    [SerializeField] Text textAmount;
    public System.Action<StorageInfo, Vector3> acCallback;
    StorageInfo _info;
    float _amount = 1;

    public void SetData(StorageInfo info)
    {
        this._info = info;
        this._amount = info.Quantity;
        List<Sprite> listIcons = MainMenuUI.Instance.GetListStoreItemIcon(info.type);
        if (info.type == STORE_ITEM_TYPE.animal)
            listIcons = MainMenuUI.Instance.listAnimalProductIcons;
        Sprite spr = listIcons.Find(x => x.name == info.name);
        if (spr != null)
        {
            this.icon.sprite = spr;
            this.icon.SetNativeSize();
        }
        
        if (info.storeType == STORE_TYPE.product)
            this.textName.text = GameDefine.ParseProduct(this._info.name).ToUpper();
        else if (info.storeType == STORE_TYPE.item)
            this.textName.text = this._info.name.ToUpper()  + " SEED";

        string unit = this._amount <= 1 ? "Ton" : "Tons";
        this.textAmount.text = String.Format("{0:#,##0.###}", this._amount) +" "+ unit;
    }

    public void OnClicked(Button btn)
    {
        return;
        if (this.acCallback != null)
        {
            this.acCallback(this._info, this.transform.position);
        }
    }
}
