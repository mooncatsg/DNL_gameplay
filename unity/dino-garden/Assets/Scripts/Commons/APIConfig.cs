using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APIConfig
{
    public static string API_HOST = "https://apimobile.networkin9.com/"; 

    

    // UserInfor
    public static string API_GET_USER_INFOR = "farm/user-info";
    public static string API_PUSH_TOKEN = "user/token-notification";
    public static string API_CHECK_VERSION = "farm/check-version";

    // Breeding
    public static string API_GET_CAGE_LIST = "land/cage/list";
    public static string API_GET_CAGE_DETAIL = "land/cage/{0}";
    public static string API_PUT_CAGE_UNLOCK = "land/cage/{0}/unlock";
    public static string API_PUT_CAGE_FIX = "land/cage/{0}/fix";
    public static string API_PUT_CAGE_UPGRADE = "land/cage/{0}/upgrade";
    public static string API_GET_LAND_ANIMAL = "land/animal/{0}";
    public static string API_PUT_ANIMAL_HARVEST = "land/animal/{0}/harvest";
    public static string API_PUT_ANIMAL_FEED = "land/animal/{0}/feed";

    // Map
    public static string API_GET_LAND_LIST = "land/list";
    public static string API_GET_LAND_DETAIL = "land/get/{0}";

    // Farming
    public static string API_GET_CROP_LIST = "land/crop/list";
    public static string API_GET_CROP_DETAIL = "land/crop/{0}";
    public static string API_PUT_CROP_DEPOSIT = "land/crop/{0}/deposit?position={1}";
    public static string API_PUT_CROP_WITHDRAW = "land/crop/{0}/withdraw";
    public static string API_PUT_UPDATE_POSITION = "land/crop/{0}/change-position?position={1}";
    public static string API_GET_PLANT_LIST = "land/plant/list";
    public static string API_GET_PLANT_DETAIL = "land/plant/{0}";
    public static string API_PUT_PLANT_FERTILIZE = "land/plant/{0}/fertilize?level={1}";
    public static string API_PUT_PLANT_WATERING = "land/plant/{0}/watering";
    public static string API_POST_PLANT_REMOVE = "land/plant/{0}/remove";
    public static string API_PUT_PLANT_SEEDING = "land/plant/{0}/seeding?cropId={1}";
    public static string API_PUT_PLANT_KILL_BUG = "land/plant/{0}/kill-bug";
    public static string API_PUT_PLANT_HARVEST = "land/plant/{0}/harvest";

    //  Market
    public static string API_GET_SHOP_LIST_ALL = "land/shop/list";
    public static string API_GET_SHOP_LIST = "land/shop/list?classification={0}&type={1}";
    public static string API_POST_SHOP_BUY = "land/shop/buy?classification={0}&type={1}&quantity={2}";
    public static string API_POST_SHOP_BUY_MULTIPLE = "land/shop/buy-multi";
    public static string API_GET_ORDER_LIST = "land/order/list";
    public static string API_GET_ORDER_SELL = "land/order/fulfill?market_id={0}";

    // Warehouse
    public static string API_GET_ITEM_LIST = "land/items/list";
    public static string API_GET_PRODUCTS_LIST = "land/products/list";
    public static string API_GET_WAREHOUSE_DETAIL = "warehouse/detail";
    public static string API_PUT_WAREHOUSE_UPGRADE = "warehouse/upgrade";

    // Dino care
    public static string API_GET_DINO_LIST = "land/care/dino"; 
    public static string API_GET_STAKED_DINO_LIST = "land/care/staking";
    public static string API_POST_STAKE_DINO = "land/care/staking";
    public static string API_PUT_UNSTAKE_DINO = "land/care/staking";

}
