
import { _decorator, Component, Node, director, view } from 'cc';
import { Common } from './common/Common';
const { ccclass, property } = _decorator;

@ccclass('LoadingController')
export class LoadingController extends Component {
    // [1]
    // dummy = '';

    // [2]
    // @property
    // serializableDummy = 0;

    start () {
        if(Common.mobileAndTabletCheck())
            view.setDesignResolutionSize(1080, 1920, 4);
        else
            view.setDesignResolutionSize(1920, 1080, 4);

        if (Common.currentLoadScene)
        {
            if(Common.currentLoadScene === "pve")
                this.LoadAScene("Gameplay");
            if(Common.currentLoadScene === "hatching")
                this.LoadAScene("Hatching");
            if(Common.currentLoadScene === "worldboss")
            {
                if(Common.mobileAndTabletCheck())
                    this.LoadAScene("GameplayWorldBossMobile");
                else
                    this.LoadAScene("GameplayWorldBoss");
            }
            if(Common.currentLoadScene === "market")
            {
                if(Common.mobileAndTabletCheck())
                    this.LoadAScene("DinoMarketMobile");
                else
                    this.LoadAScene("DinoMarket");
            }
            if(Common.currentLoadScene === "mydino")
            {                
                if(Common.mobileAndTabletCheck())
                    this.LoadAScene("DinoMarketMobile");
                else
                    this.LoadAScene("DinoMarket");
            }
            if(Common.currentLoadScene === "mydino-onsale")
            {                
                if(Common.mobileAndTabletCheck())
                    this.LoadAScene("DinoMarketMobile");
                else
                    this.LoadAScene("DinoMarket");
            }
            if(Common.currentLoadScene === "envolve")
                this.LoadAScene("EvolveScene");
            if(Common.currentLoadScene === "dino-detail")
            {
                if(Common.mobileAndTabletCheck())
                    this.LoadAScene("DinoDetailMobile");
                else
                    this.LoadAScene("DinoDetail");
            }
            if(Common.currentLoadScene === "breeding")
            {
                if(Common.mobileAndTabletCheck())
                    this.LoadAScene("BreadingMobile");
                else
                    this.LoadAScene("Breading");
            }
            if(Common.currentLoadScene === "worldBoss")
            {
                if(Common.mobileAndTabletCheck())
                    this.LoadAScene("WorldBossMobile");
                else
                    this.LoadAScene("WorldBoss");
            }
            if(Common.currentLoadScene === "fusion")
            {
                if(Common.mobileAndTabletCheck())
                    this.LoadAScene("FusionMobile");
                else
                    this.LoadAScene("Fusion");
            }
            if(Common.currentLoadScene === "pvpChooseBattle")
            {
                if(Common.mobileAndTabletCheck())
                    this.LoadAScene("PVPChooseBattleMobile");
                else
                    this.LoadAScene("PVPChooseBattle");
            }
            if(Common.currentLoadScene === "pvp")
            {
                /*if(Common.mobileAndTabletCheck())
                    this.LoadAScene("GameplayPVPMobile");
                else*/
                    this.LoadAScene("GameplayPVP");
            }
            if(Common.currentLoadScene === "ArenaMode")
            {
                /*if(Common.mobileAndTabletCheck())
                    this.LoadAScene("GameplayPVPMobile");
                else*/
                    this.LoadAScene("GameplayArenaMode");
            }
            if(Common.currentLoadScene === "ArenaModeChooseBattle")
            {
                // if(Common.mobileAndTabletCheck())
                //     this.LoadAScene("ArenaModeChooseBattleMobile");
                // else
                    this.LoadAScene("ArenaModeChooseBattle");
            }
        }

        window.cocosReload = function(scene, data) {
            Common.currentLoadScene = scene;
            Common.currentData = data;
            if(Common.currentLoadScene === "market"){
                Common.walletAddress = data["wallet"];
                Common.currentData = data["data"];
            }
            if(Common.currentLoadScene === "mydino"){
                Common.isShowEvolve = data["isShowEvolve"];
                Common.isApprovedEvolve = data["isApprovedEvolve"];
                Common.evolveDNLFee = data["evolveDNLFee"];
                Common.totalEvolveBook = data["totalEvolveBook"];
                Common.evolveDNLFlag = data["evolveDNLFlag"];
                Common.isTransferDino = data["isTransferDino"];
                Common.currentData = data["data"];
            }
            if(Common.currentLoadScene === "dino-detail" || Common.currentLoadScene === "worldBoss")
                Common.currentDinoData = data;
            if(Common.currentLoadScene === "hatching")
                Common.currentHatchingData = data;
            director.loadScene("Loading");
            localStorage.setItem("logScene",scene);
            localStorage.setItem("logData",btoa(JSON.stringify(data)));
        }
        
        window.cocosReset = function() {
            Common.currentData = null;
            Common.currentLoadScene = null;
            director.loadScene("Reset");
        }
    }


    /**
     * LoadScene
     */
    LoadAScene(sceneName:string) {
        director.preloadScene(sceneName, function (err, scene) {
            // if(err)
                director.loadScene(sceneName);
            // else
            //     director.runScene(scene);
        });
    }
    // update (deltaTime: number) {
    //     // [4]
    // }
}

/**
 * [1] Class member could be defined like this.
 * [2] Use `property` decorator if your want the member to be serializable.
 * [3] Your initialization goes here.
 * [4] Your update function goes here.
 *
 * Learn more about scripting: https://docs.cocos.com/creator/3.0/manual/en/scripting/
 * Learn more about CCClass: https://docs.cocos.com/creator/3.0/manual/en/scripting/ccclass.html
 * Learn more about life-cycle callbacks: https://docs.cocos.com/creator/3.0/manual/en/scripting/life-cycle-callbacks.html
 */
