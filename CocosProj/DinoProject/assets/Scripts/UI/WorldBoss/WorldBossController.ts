
import { _decorator, Component, Node,log, Button, Color,Sprite,Label,SpriteFrame, director, resources, InstancedBuffer } from 'cc';
import { Common } from '../../common/Common';
import { WBChoosingCard } from './WBChoosingCard';
import { WBDinoCardController } from './WBDinoCardController';
const { ccclass, property } = _decorator;

@ccclass('WorldBossController')
export class WorldBossController extends Component {
    private static singleton: WorldBossController;
    public static getInstance(): WorldBossController {
        return WorldBossController.singleton;
    }

    @property({ type: WBChoosingCard })
    public listChoosingCard: WBChoosingCard []= [];
    @property({ type: WBDinoCardController })
    public listDinoCard: WBDinoCardController []= [];

    @property({ type: Node })
    public listBoss: Node []= [];
    @property({ type: Label })
    public healthText: Label = null;
    @property({ type: Sprite })
    public healthSprite: Sprite = null;   
    @property({ type: Node })
    public increaseDamageNode: Node = null; 
    @property({ type: Label })
    public increaseDamageLabel: Label = null;
    @property({ type: Label })
    public choosenDinoCount: Label = null;
    @property({ type: Label })
    public resultDinoLegion: Label = null;

    @property({ type: Button })
    public nextBtn: Button = null;
    @property({ type: Button })
    public prevBtn: Button = null;
    @property({ type: Button })
    public fightBtn: Button = null;

    private dinoDatas = [];
    private allDinoDatas = [];
    private firstPositionDino = [];
    private bossMaxHealthData = 0;
    private bossCurrentHealthData = 0;
    private currentDinoPage = 1;
    private maxDinoPage = 1;
    private typeboss:number;

    private choosingCardIndex = 0;

    MAX_DINO :number = 9;
    private choosingDinoDatas:any = [];

    start () {
        WorldBossController.singleton = this;       
        this.typeboss = Common.GetRarityBossNumber(Common.currentDinoData["typeBoss"])
        this.dinoDatas = Common.currentDinoData["dinoDatas"].filter(item => item.rarity == this.typeboss);
        this.allDinoDatas = Common.currentDinoData["dinoDatas"];
        this.firstPositionDino = Common.currentDinoData["team"]; 
        this.bossMaxHealthData = Common.currentDinoData["bossMaxHealth"];
        this.bossCurrentHealthData = Common.currentDinoData["bossCurrentHealth"];
        for(var i=0; i < this.listBoss.length; i++){
            if(i == this.typeboss){
                this.listBoss[i].active = true;
            }
            else{
                this.listBoss[i].active = false;
            }
            
        }

        this.choosingCardIndex = 0;
        this.currentDinoPage = 1;
        this.maxDinoPage = Math.ceil(this.dinoDatas.length/5);
        log("DINO LENGTH : "+this.dinoDatas.length);
        this.LoadDinoPage();
        this.healthText.string = this.bossCurrentHealthData.toLocaleString() +  " / " + this.bossMaxHealthData.toLocaleString();
        this.healthSprite.fillRange = this.bossCurrentHealthData / this.bossMaxHealthData;
        this.SetFirstDinoPosition();

        window.onCheckFight = this.OnGetBattleSuccess;

    }

    public SetFirstDinoPosition()
    {
        if(this.firstPositionDino != null && this.firstPositionDino.length > 0)
        {
            for(let i=0;i<this.firstPositionDino.length;i++)
            {
                for(let j=0;j<this.dinoDatas.length;j++)
                {
                    if(this.firstPositionDino[i]["nftId"] == this.dinoDatas[j]["nftId"])
                    {
                        this.choosingDinoDatas[i] = this.dinoDatas[j];
                        this.listChoosingCard[this.firstPositionDino[i]["position"] - 1].ChooseDino(this.dinoDatas[j]);

                        log(this.firstPositionDino[i]["position"] + " - " + j);
                    }
                }
            }
    
            this.ResetDinoCardState();
            this.ResetFightButton();
        }
    }

    public OnGetBattleSuccess(result)
    {    
        console.log("OnGetBattleSuccess", result);
        WorldBossController.getInstance().fightChecking = false;        

        Common.currentWBDinoResult = result;
        //this.healthText.string = Common.currentWBDinoResult["remainHp"] +  " / " + this.bossMaxHealthData;
        //this.healthSprite.fillRange = Common.currentWBDinoResult["remainHp"] / this.bossMaxHealthData;

        director.loadScene("GameplayWorldBoss");
    }

    public ResetChoosingDinoData(_nftId:any)
    {
        let nftId = +_nftId;
        let tempData = [];
        tempData = this.choosingDinoDatas;
        this.choosingDinoDatas = [];
        log(this.choosingDinoDatas.length + " - "+nftId);
        let i=0;
        let j=0;
        for(i=0;i<tempData.length;i++)
        {
            if(nftId != tempData[i].nftId)
            {
                this.choosingDinoDatas[j] = tempData[i];
                j++;
            }            
        }
        this.ResetDinoCardState();
        this.ResetFightButton();
    }

    public OnChooseDino(event:any, idx:any)
    {
        let index = +idx;

        if(this.listChoosingCard[this.choosingCardIndex].dinoData != null)
        {
            let i=0;
            for(i=0;i<this.choosingDinoDatas.length; i++)
            {
                if(this.choosingDinoDatas[i].nftId === this.listChoosingCard[this.choosingCardIndex].dinoData.nftId)
                {
                    this.choosingDinoDatas[i] = this.dinoDatas[(this.currentDinoPage-1 )*5+index];
                }
            }
        }
        else
        {
            this.choosingDinoDatas[this.choosingDinoDatas.length] = this.dinoDatas[(this.currentDinoPage-1 )*5+index];
        }

        log(index + " - "+this.currentDinoPage+ " - " + this.dinoDatas.length + " - choosingDinoDatas :" + this.choosingDinoDatas.length);

        this.listChoosingCard[this.choosingCardIndex].ChooseDino(this.dinoDatas[(this.currentDinoPage-1 )*5+index]);

        this.ResetDinoCardState();
        this.ResetFightButton();
    }

    public OnChooseCard(event:any, idx:any)
    {
        let index = +idx;
        if(this.choosingCardIndex === index && this.listChoosingCard[this.choosingCardIndex].dinoData != null)
        {
            this.ResetChoosingDinoData(this.listChoosingCard[this.choosingCardIndex].dinoData.nftId);
            this.listChoosingCard[this.choosingCardIndex].ResetDino();
            log("ResetChoosingDinoData : "+this.dinoDatas.length);
        }
        this.listChoosingCard.forEach(element => {
            element.OnReset();
        });
        this.choosingCardIndex = index;
        this.listChoosingCard[this.choosingCardIndex].OnDinoChoosing();
    }

    public LoadDinoPage()
    {
        for (let index = 0; index < this.listDinoCard.length; index++) 
        {
            let currentIndex = (this.currentDinoPage-1) * 5 + index;           

            const element = this.listDinoCard[index];
            if(currentIndex < this.dinoDatas.length){
                element.node.active = true;
                element.initialize(this.dinoDatas[currentIndex]);
            }else{
                element.node.active = false;
            }
        }
        
        this.ResetDinoCardState();
        this.ResetButtonNextPrevState();
        this.ResetFightButton();
    }    

    public OnBtnNext()
    {
        if(this.currentDinoPage <this.maxDinoPage)
        {
            this.currentDinoPage ++;
            this.LoadDinoPage();
        }
    }

    public OnBtnPrev()
    {
        if(this.currentDinoPage > 0)
        {
            this.currentDinoPage --;
            this.LoadDinoPage();
        }
    }

    public ResetButtonNextPrevState()
    {
        this.nextBtn.interactable = (this.maxDinoPage > this.currentDinoPage);
        this.prevBtn.interactable = (this.currentDinoPage > 1);
    }

    public ResetFightButton()
    {
        this.fightBtn.interactable = (this.choosingDinoDatas.length >0 && this.choosingDinoDatas.length <= this.MAX_DINO);
        this.choosenDinoCount.string = "Dinos : " + this.choosingDinoDatas.length + "/" + this.MAX_DINO;

        if(this.choosingDinoDatas.length >= this.MAX_DINO)
        {
            this.listChoosingCard.forEach(element => {
                element.node.active = (element.dinoData != null);
            });
        }
        else
        {
            this.listChoosingCard.forEach(element => {
                element.node.active = true;
            });
        }

        this.CheckIncreaseNodeState();
    }

    public ResetDinoCardState()
    {
        this.listDinoCard.forEach(element => {
            let i=0;
            element.UpdateDinoCardState(false);
            for(i=0;i<this.choosingDinoDatas.length;i++)
            {
                if(element.dinoData != null && element.dinoData.nftId === this.choosingDinoDatas[i].nftId)
                {
                    element.UpdateDinoCardState(true);
                    break;
                }
            }
        });
    }

    private fightChecking:boolean = false;
    public OnFightButtonClicked()
    {
        this.fightChecking = true;
        this.fightBtn.interactable = false;

        let nftDatas:any = new Array();
        let finalChoosenData = [];
        Common.currentWBDinoPosition = [];
        let pos = 0;
        let posIndex = 0;
        WorldBossController.getInstance().listChoosingCard.forEach(element => {
            pos++;
            if(element.dinoData != null)
            {
                let nftData:any = new Object();
                nftData["position"] = pos;
                nftData["nftId"] = element.dinoData.nftId;
                nftDatas.push(nftData);
                Common.currentWBDinoPosition[posIndex] = (element.node.getSiblingIndex());
                finalChoosenData[posIndex] = element.dinoData;
                posIndex++;
            }
        });

        log("currentWBDinoPosition : "+Common.currentWBDinoPosition.length + " - "+ Common.currentWBDinoPosition.toString());
        Common.currentWBDinoData = JSON.stringify(finalChoosenData);
        
        //let str = '"nftIds":'+JSON.stringify(nftIds);
        log(nftDatas);
        if(window.checkFight)
        {
            window.checkFight(nftDatas);
        }
        return;

        // let finalChoosenData = [];
        // Common.currentWBDinoPosition = [];
        // let posIndex = 0;
        // this.listChoosingCard.forEach(element => {
        //     if(element.dinoData != null)
        //     {
        //         Common.currentWBDinoPosition[posIndex] = element.node.getSiblingIndex();
        //         finalChoosenData[posIndex] = element.dinoData;
        //         posIndex++;
        //     }
        // });

        // log("currentWBDinoPosition : "+Common.currentWBDinoPosition.length + " - "+ Common.currentWBDinoPosition.toString());
        // Common.currentWBDinoData = JSON.stringify(finalChoosenData);
        
        // director.loadScene("GameplayWorldBoss");

    }

    public CheckIncreaseNodeState()
    {
        let classCount = 0;
        let i=0;
        let j=0;

        for(i=1;i<6;i++)
        {
            for(j=0;j<this.choosingDinoDatas.length;j++)
            {
                if(i == this.choosingDinoDatas[j].class)
                {
                    classCount++;
                    break;
                }
            }                       
        }
        this.increaseDamageNode.active = (classCount >= 3);
        if(classCount == 3)
            this.increaseDamageLabel.string = "+10% Damage";
        else if(classCount == 4)
            this.increaseDamageLabel.string = "+20% Damage";
        else
            this.increaseDamageLabel.string = "+30% Damage";

        let listDino = this.allDinoDatas.filter((x) => !this.choosingDinoDatas.map(y => y.nftId).includes(x.nftId));
        //filter dino by rarity
        let listDinoMystic:any = [];
        let listDinoLegendary:any = [];
        let listDinoSP:any = [];
        let listDinoRare:any = [];
        let listDinoNormal:any = [];
    
        listDino.map((x: any) => {
            if (x.rarity == 5) {
            listDinoMystic.push(x);
            }
            if (x.rarity == 4) {
            listDinoLegendary.push(x);
            }
            if (x.rarity == 3) {
            listDinoSP.push(x);
            }
            if (x.rarity == 2) {
            listDinoRare.push(x);
            }
            if (x.rarity == 1) {
            listDinoNormal.push(x);
            }
        });
        const n =  3;
        //handle arr dino legion
        const countLegionDino = (listDino:any, listUpperDino:any) => {
            if(listUpperDino.length > 0 && listDino.length > listUpperDino.length*n){
            listDino = listDino.slice(0,listUpperDino.length*n);
            return listDino;
            }
            else if(listUpperDino.length == 0 && listDino.length > 0) {
            listDino = listDino.slice(0,n);
            return listDino;
            }
            else return listDino;
        }
    
        const dinoLegendary = countLegionDino(listDinoLegendary, listDinoMystic);
        const dinoSuperRare = countLegionDino(listDinoSP, dinoLegendary);
        const dinoRare = countLegionDino(listDinoRare, dinoSuperRare);
        const dinoNormal = countLegionDino(listDinoNormal, dinoRare);
        this.resultDinoLegion.string = "Dino Legion: "+ dinoNormal.length+" Normal, "+dinoRare.length+" Rare, "+dinoSuperRare.length+" Super Rare, "+dinoLegendary.length+" Legendary, "+listDinoMystic.length+" Mystic";
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
