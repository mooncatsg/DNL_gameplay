
import { _decorator, Component, Node, Sprite, resources, SpriteFrame, director, LabelComponent, Label, sys, Quat } from 'cc';
import { Common } from '../common/Common';
import { PartDinoController } from '../PartDino/PartDinoController';
const { ccclass, property } = _decorator;

@ccclass('DinoCardController')
export class DinoCardController extends Component {

    @property({ type: PartDinoController })
    public partDino: PartDinoController;

    @property({ type: Sprite })
    public cardBG: Sprite;

    @property({ type: Sprite })
    public dinoBG: Sprite;

    @property({ type: Sprite })
    public dinoGender: Sprite;

    @property({ type: Sprite })
    public dinoClass: Sprite;

    @property({ type: Node })
    public buyBtn: Node;

    @property({ type: LabelComponent })
    public priceLbl: Label;

    @property({ type: Node })
    public sellBtn: Node;

    @property({ type: Node })
    public transferBtn: Node;

    @property({ type: LabelComponent })
    public sellpriceLbl: Label;

    @property({ type: LabelComponent })
    public sellPriceInUSDLbl: Label;

    @property({ type: Node })
    public cancelBtn: Node;

    @property({ type: LabelComponent })
    public levelLbl: Label;

    @property({ type: LabelComponent })
    public nftIdLbl: Label;

    @property({ type: Sprite })
    public coinIcon: Sprite;

    @property({ type: Sprite })
    public coinCancelIcon: Sprite;

    private dinoData:any;


    public initialize(data:any): void{
        if(!this.partDino)
            this.partDino = this.getComponentInChildren<PartDinoController>(PartDinoController);
        let isMyDino = false;
        if(data["prevOwner"] && Common.walletAddress)
            isMyDino = ((""+data["prevOwner"]).toLowerCase() === Common.walletAddress.toLowerCase());
        
        let sceneData = Common.currentLoadScene;
        this.buyBtn.active = sceneData === "market" && !isMyDino;
        this.cancelBtn.active = (sceneData === "market" && isMyDino) || (sceneData === "mydino-onsale");
        this.sellBtn.active = sceneData === "mydino";
        this.transferBtn.active = sceneData === "mydino" && Common.isTransferDino;
        
        this.dinoData = data;
        if(data)
        {
            Common.getRarityCard(+data["rarity"], (err, spriteFrame) => { this.cardBG.spriteFrame = spriteFrame;});
            Common.getRarityDinoBG(+data["rarity"], (err, spriteFrame) => { this.dinoBG.spriteFrame = spriteFrame;});
            Common.GetClassIcon(+data["class"], (err, spriteFrame) => { this.dinoClass.spriteFrame = spriteFrame;});
            if(data["isEvolved"])
            {
                Common.getGenderIcon(+data["gender"], (err, spriteFrame) => { this.dinoGender.spriteFrame = spriteFrame;});
                this.partDino.loadDataByTrait(data["expressTraits"],data["geneRarity"], data["nftId"]);
            }
            else
            {
                this.partDino.loadDataVer1(data["class"], data["rarity"]);
            }
            this.priceLbl.string = Common.numberWithCommas(+data["endingPrice"]) +" " + Common.getCoinName(+data["cashType"]);
            this.sellpriceLbl.string = Common.numberWithCommas(+data["endingPrice"]) + " " + Common.getCoinName(+data["cashType"]);
            
            if (!('midasRemain' in  this.dinoData))
                this.dinoData["midasRemain"] = 0;
            
            // if(data["isEvolved"]){
            //     this.levelLbl.string = "Midas: " + data["midasRemain"];//"Level "+ data["level"];
            // }
            // else{
            //     this.levelLbl.string = "";
            // }
            this.levelLbl.string = "Level "+ data["level"];
            
            this.nftIdLbl.string = "#" + data["nftId"] + " ";
            this.sellPriceInUSDLbl.string = "";
            Common.getCoinIcon(+data["cashType"], (err, spriteFrame) => { 
                this.coinIcon.spriteFrame = spriteFrame;
                this.coinCancelIcon.spriteFrame = spriteFrame;
            });
        }
    }

    public UpdateUSDPrice(exchangeRate:number){
        if(+this.dinoData["cashType"] == 2)
            this.sellPriceInUSDLbl.string = "~$" + Common.numberWithCommas(Math.ceil(+this.dinoData["endingPrice"]*exchangeRate));
    }

    public OnCardClicked():void{
        Common.currentDinoData = this.dinoData;
        window.closeDetail = function() { 
            if(Common.mobileAndTabletCheck())
                director.loadScene("DinoMarketMobile");
            else
                director.loadScene("DinoMarket");
        }
        if(Common.mobileAndTabletCheck())
            director.loadScene("DinoDetailMobile");
        else
            director.loadScene("DinoDetail");
    }

    public sellDino(){
        console.log('sellDino ' + this.dinoData["nftId"]);
        if(window.sellDino)
            window.sellDino(this.dinoData["nftId"]);
    }

    public tranferDino(){
        console.log('tranferDino ' + this.dinoData["nftId"]);
        if(window.transferDino)
            window.transferDino(this.dinoData["nftId"]);
    }

    public buyDino(){
        console.log('buyDino ' + this.dinoData["nftId"]);
        if(window.buyDino)
            window.buyDino(this.dinoData["nftId"], this.dinoData["endingPrice"], this.dinoData["cashType"],);
    }

    public cancelSellDino(){
        console.log('cancelSell ' + this.dinoData["nftId"]);
        if(window.cancelSell)
            window.cancelSell(this.dinoData["nftId"]);
    }

    private _temp_quat: Quat = new Quat();
    update (deltaTime: number) {
        if(!this.partDino)
            this.partDino = this.getComponentInChildren<PartDinoController>(PartDinoController);
        //if(this.autoRotate){
            Quat.fromEuler(this._temp_quat,0, -50 * deltaTime, 0);
            this.partDino.node.rotate(this._temp_quat);
        //}
        // [4]
    }
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
