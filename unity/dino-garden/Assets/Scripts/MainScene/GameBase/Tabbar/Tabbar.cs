using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace DinoGarden
{
    public class Tabbar : MonoBehaviour
    {
        public int startIndex = 0;
        public Action<int> acTabClick;
        public bool isSetStartIndex = true;
        private int _currentIndex = -1;
        private List<TabbarItem> listTabItems = new List<TabbarItem>();
        private void Awake()
        {
            foreach (Transform child in transform)
            {
                var comp = child.GetComponent<TabbarItem>();
                if (comp)
                {
                    listTabItems.Add(comp);
                    comp.acTabClick = this.OnTabItemClicked;
                }
            }

            if (this.isSetStartIndex)
            {
                this.OnTabItemClicked(startIndex);
            }
        }

        public void OnTabItemClicked(int index)
        {
            // Debug.Log("OnTabItemClicked = " + index + " | " + this._currentIndex);
            if (this._currentIndex == index) return;
            for (int i = 0; i < listTabItems.Count; i++)
            {
                var item = listTabItems[i];
                if (item.index == index)
                {
                    item.SetChoose(true);
                }
                else
                {
                    item.SetChoose(false);
                }
            }
            this._currentIndex = index;

            if (this.acTabClick != null)
            {
                this.acTabClick(index);
            }
        }

        public void clearDataAndActive()
        {
            this._currentIndex = -1;
            this.OnTabItemClicked(startIndex);
        }

        public void setTabIndex(int index)
        {
            this._currentIndex = -1;
            this.OnTabItemClicked(index);
        }

    }
}

