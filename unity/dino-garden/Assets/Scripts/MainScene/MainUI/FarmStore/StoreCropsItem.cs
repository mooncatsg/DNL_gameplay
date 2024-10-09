using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StoreCropsItem : MonoBehaviour
{
    [SerializeField] Image itemIcon;
    [SerializeField] Text txtRarity;

    public void SetData(int cropsType)
    {
        this.itemIcon.sprite = MainMenuUI.Instance.GetListStoreItemIcon(STORE_ITEM_TYPE.crop)[cropsType - 1];
        this.txtRarity.text = GameDefine.getRarity(cropsType);
    }
}
