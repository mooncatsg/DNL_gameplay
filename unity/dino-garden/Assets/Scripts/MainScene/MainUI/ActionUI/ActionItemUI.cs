using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DinoExtensions;
public class ActionItemUI : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Text txtAmount;
    [SerializeField] Text txtName;
    public Action<ActionUIInfo> acItemClicked;
    ActionUIInfo info;

    public void SetData(ActionUIInfo info, Sprite spIcon)
    {
        this.info = info;
        this.icon.sprite = spIcon;
        this.icon.SetNativeSize();

        if (info.acType == (int)ACTION_UI_TYPE.SPAWN_TREE && info.itemInfo != null)
        {
            this.txtAmount.gameObject.SetActive(true);
            this.txtAmount.text = String.Format("x{0}", info.itemInfo.Quantity.FormatMoney());
        }
        if(((ACTION_TYPE)info.acType) == ACTION_TYPE.TAKE_CARE_TREE)
        {
            this.txtName.text = ((TakeCarePlantType)info.acId).ToString().ToUpper();
        }    
    }

    public void OnBtnClickItem(Button btn)
    {
        if (this.acItemClicked != null)
        {
            btn.interactable = false;
            this.acItemClicked(this.info);
        }
    }
}
