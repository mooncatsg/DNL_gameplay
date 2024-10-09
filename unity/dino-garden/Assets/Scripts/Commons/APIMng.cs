using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Text;
using SimpleHTTP;

public delegate void APICallback(bool isSuccess, object response, string errMsg = null, int code = -1);

public class APIMng : APIBase
{
    private static APIMng instance = null;
    #region Get-Set
    public static APIMng Instance
    {
        get { return instance; }
    }

    #endregion
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void OnBeforeSceneLoadRuntimeMethod()
    {
        GameObject go = new GameObject("APIMng");
        APIMng instance = go.AddComponent<APIMng>();
        DontDestroyOnLoad(go);
        if (PlayerPrefs.HasKey("loginData"))
        {
            string loginData = PlayerPrefs.GetString("loginData", string.Empty);
            if (!string.IsNullOrEmpty(loginData))
            {
                try
                {
                    AuthQRResponse login = JsonConvert.DeserializeObject<AuthQRResponse>(loginData);
                    instance.accessToken = login.accessToken;
                    instance.landid = login.landId;
                }
                catch(Exception e)
                {

                }
            }
        }
    }
    //private void Start()
    //{
    //    TestAPI();

    //}
    //int testCount = 0;
    //public void TestAPI()
    //{
    //    testCount++;
    //    switch (testCount)
    //    {
    //        case 1:
    //            APIAuthQR("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6MTQsIndhbGxldEFkZHJlc3MiOiIweGRlMGIxNjU0MzY0NGFlNmYwMDVjOWIwOTE2OWZjN2JkN2M0ODdkMTciLCJpYXQiOjE2NTI2MDY2MDEsImV4cCI6MTY1MjkwNjYwMX0.hmJwxic6NaF2-b1R6oL1fTrgHT7qvQcQVU81WIP9r-M",
    //                "0xDE0B16543644AE6F005c9b09169Fc7bD7C487d17",
    //                (s, d, e, c) =>
    //                {
    //                    TestAPI();
    //                });
    //            break;
    //            //Warehouse
    //            //   case 2:
    //            //       APIItemList(( e, d) => { TestAPI(); });
    //            //       break;
    //            //   case 3:
    //            //       APIProductsList((e, d) => { TestAPI(); });
    //            //       break;
    //            //   case 4:
    //            //       APIWarehouseDetail((e, d) => { TestAPI(); });
    //            //       break;
    //            //   case 5:
    //            //       APIWarehouseUpgrade((e, d) => { TestAPI(); });
    //            //       break;
    //            //// Farming
    //            //   case 6:
    //            //       APIPlantDetail(1, (e, d) => { TestAPI(); });
    //            //       break;
    //            //   case 7:
    //            //       APIPlantFertilize(1, (e, d) => { TestAPI(); });
    //            //       break;
    //            //   case 8:
    //            //       APIPlantRemove(1, (e, d) => { TestAPI(); });
    //            //       break;
    //            //   case 9:
    //            //       APIPlantSeeding(4, PlantType.wheat, (e, d) => { TestAPI(); });
    //            //       break;
    //            //   case 10:
    //            //       APIPlantKillBug(1, (e, d) => { TestAPI(); });
    //            //       break;
    //            //   case 11:
    //            //       APIPlantHarvest(1, (e, d) => { TestAPI(); });
    //            //       break;
    //            //   case 12:
    //            //       APICropWithdraw(1, (e, d) => { TestAPI(); });
    //            //       break;
    //            //   case 13:
    //            //       APICropList((e, d) => { TestAPI(); });
    //            //       break;
    //            //   case 14:
    //            //       APICropDetail(1, (e, d) => { TestAPI(); });
    //            //       break;
    //            //   case 15:
    //            //       APICropDeposit(1, 1, (e, d) => { TestAPI(); });
    //            //       break;
    //            //   case 16:
    //            //       APIPlantList((e, d) => { TestAPI(); });
    //            //       break;
    //            //// Map
    //            //   case 17:
    //            //       APILandList((e, d) => { TestAPI(); });
    //            //       break;

    //            //   case 18:
    //            //       APILandDetail((e, d) => { TestAPI(); });
    //            //       break;
    //            //// Breeding
    //            //   case 19:
    //            //       APIListCage((e, d) => { TestAPI(); });
    //            //       break;
    //            //   case 20:
    //            //       APICageUnlock(1, (e, d) => { TestAPI(); });
    //            //       break;
    //            //   case 21:
    //            //       APICageDetail(1, (e, d) => { TestAPI(); });
    //            //       break;
    //            //   case 22:
    //            //       APICageFix(1, (e, d) => { TestAPI(); });
    //            //       break;
    //            //   case 23:
    //            //       APICageUpgrade(1, (e, d) => { TestAPI(); });
    //            //       break;
    //            //   case 24:
    //            //       APILandAnimal(1, (e, d) => { TestAPI(); });
    //            //       break;
    //            //   case 25:
    //            //       APIAnimalHarvest(1, (e, d) => { TestAPI(); });
    //            //       break;
    //            //   case 26:
    //            //       APIAnimalFeed(1, (e, d) => { TestAPI(); });
    //            //       break;
    //            //   // Market
    //            //   case 27:
    //            //       APIShopList("crop", "", (e, d) => { TestAPI(); });
    //            //       break;
    //            //   case 28:
    //            //       APIShopList("crop", "common", (e, d) => { TestAPI(); });
    //            //       break;
    //            //   case 29:
    //            //       APIShopList("animal", "", (e, d) => { TestAPI(); });
    //            //       break;
    //            //   case 30:
    //            //       APIShopList("animal", "pig", (e, d) => { TestAPI(); });
    //            //       break;
    //            //   case 31:
    //            //       APIBuyerList( (e, d) => { TestAPI(); });
    //            //       break;
    //            //   case 32:
    //            //       APIShopSell(1, (e, d) => { TestAPI(); });
    //            //       break;
    //            //   case 33:
    //            //       APIShopBuy("animal", "pig", 10, (e, d) => { TestAPI(); });
    //            //       break;
    //    }
    //}

    protected byte[] GetBytes(string str)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        return bytes;
    }
    #region Qr
    public void APIAuthQR(string logInToken, APICallback callback)
    {
        AuthQRRequest req = new AuthQRRequest() { signInToken = logInToken};
        string param = JsonConvert.SerializeObject(req);
        Debug.Log(param);
        PostRaw("auth-qr", param, (isOK, response) => {
            if (isOK)
            {
                try
                {
                    AuthQRResponse login = response.data.ToObject<AuthQRResponse>();
                    PlayerPrefs.SetString("loginData", response.data.ToString());
                    instance.accessToken = login.accessToken;
                    instance.landid = login.landId;
                    callback?.Invoke(isOK, login, response.message, response.code);
                }
                catch (Exception e)
                {
                    callback?.Invoke(false, null, response.message, response.code);
                }
            }
            else
                callback?.Invoke(false, null, response.message, response.code);
        }, false);
    }
    #endregion

    #region user-infor
    public void APIGetUserInfor(APICallback callback)
    {
        Get(APIConfig.API_GET_USER_INFOR, (isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data.ToObject<UserInfor>() : null, response.message, response.code);
        });
    }

    public void APIPushToken(string token, APICallback callback)
    {
        PushToken rawData = new PushToken() { tokenNotification = token };
        PostRaw(APIConfig.API_PUSH_TOKEN, JsonConvert.SerializeObject(rawData),(isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data : null, response.message, response.code);
        });
    }
    #endregion

    #region breeding
    public void APIListCage(APICallback callback)
    {
        Get(APIConfig.API_GET_CAGE_LIST, (isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data.ToObject<List<Cage>>() : null, response.message, response.code);
        });
    }
    public void APICageUnlock(int cageId, APICallback callback)
    {
        Put(string.Format(APIConfig.API_PUT_CAGE_UNLOCK, cageId), null, (isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data.ToObject<Cage>() : null, response.message, response.code);
        });
    }
    public void APICageDetail(int cageId, APICallback callback)
    {
        Get(string.Format(APIConfig.API_GET_CAGE_DETAIL, cageId), (isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data.ToObject<Cage>() : null, response.message, response.code);
        });
    }
    public void APICageFix(int cageId, APICallback callback)
    {
        Put(string.Format(APIConfig.API_PUT_CAGE_FIX, cageId), null, (isOK, response) => {
            callback?.Invoke(isOK, null, response.message, response.code);
        });
    }
    public void APICageUpgrade(int cageId, APICallback callback)
    {
        Put(string.Format(APIConfig.API_PUT_CAGE_UPGRADE, cageId), null, (isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data.ToObject<Cage>() : null, response.message, response.code);
        });
    }
    public void APILandAnimal(int cageId, APICallback callback)
    {
        Get(string.Format(APIConfig.API_GET_LAND_ANIMAL, cageId), (isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data.ToObject<Animal>() : null, response.message, response.code);
        });
    }
    public void APIAnimalHarvest(int animalId, APICallback callback)
    {
        Put(string.Format(APIConfig.API_PUT_ANIMAL_HARVEST, animalId), null, (isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data.ToObject<Animal>() : null, response.message, response.code);
        });
    }
    public void APIAnimalFeed(int animalId, APICallback callback)
    {
        Put(string.Format(APIConfig.API_PUT_ANIMAL_FEED, animalId), null, (isOK, response) => {
            callback?.Invoke(isOK, null, response.message, response.code);
        });
    }

    #endregion

    #region Map
    //public void APILandList(Action<string, LandList> callback)
    public void APILandList(APICallback callback)
    {
        Get(APIConfig.API_GET_LAND_LIST, (isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data.ToObject<List<LandDataDetail>>() : null, response.message, response.code);
        });
    }
    public void APILandDetail(APICallback callback)
    {
        Get(string.Format(APIConfig.API_GET_LAND_DETAIL, landid), (isOK, response) => {
            if (isOK)
            {
                var data = response.data.ToObject<LandDetail>();
                callback?.Invoke(isOK, isOK ? data : null, response.message, response.code);
            }
        });
    }
    #endregion

    #region Farming
    public void APICropList(APICallback callback)
    {
        Get(APIConfig.API_GET_CROP_LIST, (isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data.ToObject<List<Crop>>() : null, response.message, response.code);
        });
    }
    public void APICropDetail(int cropId, APICallback callback)
    {
        Get(string.Format(APIConfig.API_GET_CROP_DETAIL, cropId), (isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data.ToObject<Crop>() : null, response.message, response.code);
        });
    }
    public void APICropDeposit(int cropId, int position, APICallback callback)
    {
        Put(string.Format(APIConfig.API_PUT_CROP_DEPOSIT, cropId, position), null, (isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data.ToObject<Crop>() : null, response.message, response.code);
        });
    }
    public void APICropWithdraw(int cropId, APICallback callback)
    {
        Put(string.Format(APIConfig.API_PUT_CROP_WITHDRAW, cropId), null, (isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data.ToObject<Crop>() : null, response.message, response.code);
        });
    }
    public void APIUpdatePositon(int cropId, int position, APICallback callback)
    {
        Put(string.Format(APIConfig.API_PUT_UPDATE_POSITION, cropId, position), null, (isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data.ToObject<Crop>() : null, response.message, response.code);
        });
    }
    public void APIPlantList(APICallback callback)
    {
        Get(APIConfig.API_GET_PLANT_LIST, (isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data.ToObject<List<Plant>>() : null, response.message, response.code);
        });
    }
    public void APIPlantDetail(int plantID, APICallback callback)
    {
        Get(string.Format(APIConfig.API_GET_PLANT_DETAIL, plantID), (isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data.ToObject<Plant>() : null, response.message, response.code);
        });
    }
    public void APIPlantFertilize(int plantId, int fertilizerLevel ,APICallback callback)
    {
        Put(string.Format(APIConfig.API_PUT_PLANT_FERTILIZE, plantId, fertilizerLevel), null, (isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data.ToObject<PlantFertilize>() : null, response.message, response.code);
        });
    }
    public void APIPlantWatering(int plantId, APICallback callback)
    {
        Put(string.Format(APIConfig.API_PUT_PLANT_WATERING, plantId), null, (isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data.ToObject<PlantWatering>() : null, response.message, response.code);
        });
    }
    public void APIPlantRemove(int plantId, APICallback callback)
    {
        Post(string.Format(APIConfig.API_POST_PLANT_REMOVE, plantId), null, (isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data.ToObject<PlantFertilize>() : null, response.message, response.code);
        });
    }
    public void APIPlantSeeding(int plantId,int cropId ,PlantType type, APICallback callback)
    {
        PlantSeedRequest plantSeedRequest = new PlantSeedRequest() { type = type.ToString() };
        string param = JsonConvert.SerializeObject(plantSeedRequest);
        PutRaw(string.Format(APIConfig.API_PUT_PLANT_SEEDING, plantId, cropId), param, (isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data.ToObject<Plant>() : null, response.message, response.code);
        });
    }
    public void APIPlantKillBug(int plantId, APICallback callback)
    {
        Put(string.Format(APIConfig.API_PUT_PLANT_KILL_BUG, plantId), null, (isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data.ToObject<PlantWatering>() : null, response.message, response.code);
        });
    }
    public void APIPlantHarvest(int plantId, APICallback callback)
    {
        Put(string.Format(APIConfig.API_PUT_PLANT_HARVEST, plantId), null, (isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data.ToObject<WarehouseItem>() : null, response.message, response.code);
        });
    }

    #endregion

    #region Market
    public void APIGetShopListAll( APICallback callback)
    {
        Get(APIConfig.API_GET_SHOP_LIST_ALL, (isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data.ToObject<List<ShopItem>>() : null, response.message, response.code);
        });
    }
    public void APIGetOrderList(APICallback callback)
    {
        Get(APIConfig.API_GET_ORDER_LIST, (isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data.ToObject<List<OrderData>>() : null, response.message, response.code);
        });
    }
    public void APISellOrderList(int orderId, APICallback callback)
    {
        Post(string.Format(APIConfig.API_GET_ORDER_SELL, orderId), null, (isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data.ToObject<ItemOrderData>() : null, response.message, response.code);
        });
    }
    public void APIShopList(string classification, string type, APICallback callback)
    {
        Get(string.Format(APIConfig.API_GET_SHOP_LIST, classification, type), (isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data.ToObject<List<ShopItem>>() : null, response.message, response.code);
        });
    }
    public void APIBuyMultiple(List<BuyAll> buyAllList, APICallback callback)
    {
        PostRaw(APIConfig.API_POST_SHOP_BUY_MULTIPLE, Newtonsoft.Json.JsonConvert.SerializeObject(buyAllList), (isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data.ToObject<List<BuyAll>>() : null, response.message, response.code);
        });
    }
    public void APIBuyBox(int quantity, APICallback callback)
    {
        Post(string.Format(APIConfig.API_POST_SHOP_BUY, "crop", "random", quantity), null, (isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data.ToObject<List<BuyCrop>>() : null, response.message, response.code);
        });
    }
    //public void APIShopBuy(string classification, string type, int quantity, APICallback callback)
    //{
    //    Post(string.Format(APIConfig.API_POST_SHOP_BUY, classification, type, quantity), null, (isOK, response) => {
    //        callback?.Invoke(isOK, isOK ? response.data.ToObject<List<Buy>>() : null, response.message, response.code);
    //    });
    //}

    //public void APIBuySeed(string classification, string type, int quantity, APICallback callback)
    //{
    //    Post(string.Format(APIConfig.API_POST_SHOP_BUY, classification, type, quantity), null, (isOK, response) => {
    //        callback?.Invoke(isOK, isOK ? response.data.ToObject<Plant>() : null, response.message, response.code);
    //    });
    //}

    //public void APIBuyAnimal(string classification, string type, int quantity, APICallback callback)
    //{
    //    Post(string.Format(APIConfig.API_POST_SHOP_BUY, classification, type, quantity), null, (isOK, response) => {
    //        callback?.Invoke(isOK, isOK ? response.data.ToObject<BuyAnimal>() : null, response.message, response.code);
    //    });
    //}

    //public void APIBuyerList(APICallback callback)
    //{
    //    Get(APIConfig.API_GET_BUYER_LIST, (isOK, response) => {
    //        callback?.Invoke(isOK, isOK ? response.data.ToObject<List<ListBuyer>>() : null, response.message, response.code);
    //    });
    //}
    //public void APIShopSell(int sellId, APICallback callback)
    //{
    //    Post(string.Format(APIConfig.API_GET_BUYER_SELL, sellId), null, (isOK, response) => {
    //        callback?.Invoke(isOK, isOK ? response.data.ToObject<BuyerSell>() : null, response.message, response.code);
    //    });
    //}
    #endregion

    #region Warehouse
    public void APIItemList(APICallback callback)
    {
        Get(APIConfig.API_GET_ITEM_LIST, (isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data.ToObject<List<WarehouseItem>>() : new List<WarehouseItem>(), response.message, response.code);
        });
    }
    public void APIProductsList(APICallback callback)
    {
        Get(APIConfig.API_GET_PRODUCTS_LIST, (isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data.ToObject<List<WarehouseItem>>() : null, response.message, response.code);
        });
    }
    public void APIWarehouseDetail(APICallback callback)
    {
        Get(APIConfig.API_GET_WAREHOUSE_DETAIL, (isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data.ToObject<Warehouse>() : null, response.message, response.code);
        });
    }
    public void APIWarehouseUpgrade(APICallback callback)
    {
        Put(APIConfig.API_PUT_WAREHOUSE_UPGRADE, null, (isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data.ToObject<Warehouse>() : null, response.message, response.code);
        });
    }
    #endregion

    #region Dino Care
    public void APIDinoList(APICallback callback)
    {
        Get(APIConfig.API_GET_DINO_LIST, (isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data.ToObject<List<DinoModel>>() : null, response.message, response.code);
        });
    }
    public void APIDinoCaring(APICallback callback)
    {
        Get(APIConfig.API_GET_STAKED_DINO_LIST, (isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data.ToObject<List<StakedFarm>>() : null, response.message, response.code);
        });
    }
    public void APIStakeDinoToGarden(int nftId, APICallback callback)
    {
        APIStakeDino(new StakeDinoRequest() { nftId = nftId, classification = StakingClassification.plant.ToString(), type = StakingType.garden.ToString() }, callback);
    }
    public void APIStakeDinoToChicken(int nftId, APICallback callback)
    {
        APIStakeDino(new StakeDinoRequest() { nftId = nftId, classification = StakingClassification.animal.ToString(), type = StakingType.chicken.ToString() }, callback);
    }
    public void APIStakeDinoToCow(int nftId, APICallback callback)
    {
        APIStakeDino(new StakeDinoRequest() { nftId = nftId, classification = StakingClassification.animal.ToString(), type = StakingType.cow.ToString() }, callback);
    }
    public void APIStakeDinoToPig(int nftId, APICallback callback)
    {
        APIStakeDino(new StakeDinoRequest() { nftId = nftId, classification = StakingClassification.animal.ToString(), type = StakingType.pig.ToString() }, callback);
    }
    public void APIStakeDinoToSheep(int nftId, APICallback callback)
    {
        APIStakeDino(new StakeDinoRequest() { nftId = nftId, classification = StakingClassification.animal.ToString(), type = StakingType.sheep.ToString() }, callback);
    }
    void APIStakeDino(StakeDinoRequest req, APICallback callback)
    {
        PostRaw(APIConfig.API_POST_STAKE_DINO, JsonConvert.SerializeObject(req), (isOK, response) => {
            callback?.Invoke(isOK, isOK ? response.data.ToObject<StakingFarm>() : null, response.message, response.code);
        });
    }
    public void APIUnstakeDino(int _stakingFarmId, APICallback callback)
    {
        UnstakeDinoRequest req = new UnstakeDinoRequest() { stakingFarmId = _stakingFarmId };
        PutRaw(APIConfig.API_PUT_UNSTAKE_DINO, JsonConvert.SerializeObject(req), (isOK, response) => {
            callback?.Invoke(isOK, null, response.message, response.code);
        });
    }
    #endregion

    #region config
    public void APIGetsystem(APICallback callback)
    {
        GetFullURL("https://din-config.s3.amazonaws.com/dg_cgfs.json", (isOK, response) => {
            callback?.Invoke(isOK, isOK ? JsonConvert.DeserializeObject<SystemConfig>(response) : null, "", 0);
        });
    }

    public void APICheckVersion(string _version, APICallback callback)
    {
        CheckVersion rawData = new CheckVersion() { version = _version };
        PostRaw(APIConfig.API_CHECK_VERSION, JsonConvert.SerializeObject(rawData), (isOK, response) => {
            callback?.Invoke(isOK, isOK ? (bool)response.data : null, response.message, response.code);
        });
    }
    #endregion
}
