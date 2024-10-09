using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DinoGarden
{
    using UnityEngine.UI;
    public class TabbarItem : MonoBehaviour
    {
        [SerializeField] Image imgBg;
        [SerializeField] Image imgIcon;
        [SerializeField] Text textTitle;
        [SerializeField] Sprite sprNormal;
        [SerializeField] Sprite sprChoose;
        [SerializeField] Sprite sprIconNormal;
        [SerializeField] Sprite sprIconChoose;
        public int index;
        public bool isChangeTextColor = false;
        public Color colorTextNormal = Color.white;
        public Color colorTextChoose = Color.black;

        public Action<int> acTabClick;
        public void OnTabClicked(Button button)
        {
            if (this.acTabClick != null)
            {
                this.acTabClick(this.index);
            }
        }

        virtual public void SetChoose(bool isChoose)
        {
            // Debug.Log("SetChoose Index = " + index + " ==> " + isChoose);
            if (isChoose)
            {
                if (this.imgIcon && sprIconChoose)
                {
                    this.imgIcon.sprite = sprIconChoose;
                    this.imgIcon.SetNativeSize();
                }

                if (this.imgBg && sprChoose)
                {
                    this.imgBg.sprite = sprChoose;
                }

            }
            else
            {
                if (this.imgIcon && sprIconNormal)
                {
                    this.imgIcon.sprite = sprIconNormal;
                    this.imgIcon.SetNativeSize();
                }

                if (this.imgBg && sprNormal)
                {
                    this.imgBg.sprite = sprNormal;
                }
            }

            if (this.isChangeTextColor)
            {
                this.textTitle.color = isChoose ? colorTextChoose : colorTextNormal;
            }
        }
    }
}

