using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedStoreUI : UIPopupBase
{
    [SerializeField] GameObject seedItemPrefab;

    public override void Start()
    {
        var colume = 5;
        var row = 2;
        var space = 100;
        var item_size = 100;
        var margin = 50;

        var totalItem = 12;
        var numItemInPage = 10;
        int totalPage = numItemInPage / totalItem;

        int pageIndex = 1;
        int numItem = pageIndex < totalPage ? numItemInPage : totalItem % numItemInPage;
        for (int i = 0; i < numItem; i++)
        {
            var item = Instantiate(seedItemPrefab, this.transform);
            // var x_index = i % num

            var pos_x = margin + i * space + item_size / 2 * i;

        }

    }

}
