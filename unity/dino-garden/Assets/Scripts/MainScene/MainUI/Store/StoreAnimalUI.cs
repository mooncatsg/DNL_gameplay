using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreAnimalUI : StoreContentBase
{
    public override void SetupData()
    {
        if (this.listStoreInfo.Count > 0)
            return;

        List<ShopItem> listShopAll = MapController.instance.listShopAll.FindAll(x => x.classification == "animal");
        if (listShopAll.Count > 0)
        {
            for (int i = 0; i < listShopAll.Count; i++)
            {
                this.listStoreInfo.Add(new StoreItemInfo(STORE_ITEM_TYPE.animal, listShopAll[i].id, listShopAll[i].type,listShopAll[i].price, listShopAll[i].priceType,  listShopAll[i].harvestTime));
            }
        }
    }
}