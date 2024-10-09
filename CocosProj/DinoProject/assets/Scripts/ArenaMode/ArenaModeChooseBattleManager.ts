
import { _decorator, Component, Node,log, Button, Color,Sprite,Label,SpriteFrame, director, resources, InstancedBuffer } from 'cc';

import { Common } from '../common/Common';
import { WBChoosingCard } from '../UI/WorldBoss/WBChoosingCard';
import { WBDinoCardController } from '../UI/WorldBoss/WBDinoCardController';
const { ccclass, property } = _decorator;
//
@ccclass('ArenaModeChooseBattleManager')
export class ArenaModeChooseBattleManager extends Component {
    private static singleton: ArenaModeChooseBattleManager;
    public static getInstance(): ArenaModeChooseBattleManager {
        return ArenaModeChooseBattleManager.singleton;
    }

    @property({ type: WBChoosingCard })
    public listChoosingCard: WBChoosingCard []= [];
    @property({ type: WBDinoCardController })
    public listDinoCard: WBDinoCardController []= [];

    @property({ type: Label })
    public choosenDinoCount: Label = null;

    @property({ type: Button })
    public nextBtn: Button = null;
    @property({ type: Button })
    public prevBtn: Button = null;
    @property({ type: Button })
    public matchingBtn: Button = null;
    @property({ type: Label })
    public matchingBtnLabel: Label = null;
    @property({ type: Node })
    public matchingNode: Node = null;
    @property({ type: Node })
    public findingNode: Node = null;
    @property({ type: Node })
    public foundNode: Node = null;
    @property({ type: Label })
    public countDownTxt: Label;
    @property({ type: Label })
    public countDownFound: Label;

    private dinoDatas = [];
    private firstPositionDino = [];
    private currentDinoPage = 1;
    private maxDinoPage = 1;

    private choosingCardIndex = 0;
    private matchingGame:boolean = false;
    MAX_DINO :number = 4;
    MAX_DINO_IN_PAGE :number = 12;
    private choosingDinoDatas = [];
    private countDownNum:number = 0;

    start () {
        ArenaModeChooseBattleManager.singleton = this;       

        this.dinoDatas = Common.currentData["dinoDatas"].sort((a, b) => {
            return (b.rarity - a.rarity) || (b.class - a.class);
        });
        this.firstPositionDino = Common.currentData["team"]; 
        this.nextBtn.interactable = false;
        this.prevBtn.interactable = false;
        this.choosingCardIndex = 0;
        this.currentDinoPage = 1;
        this.maxDinoPage = Math.ceil(this.dinoDatas.length/this.MAX_DINO_IN_PAGE);
        log("DINO LENGTH : "+this.dinoDatas.length);        

        this.LoadDinoPage();

        this.SetFirstDinoPosition();

     
        this.matchingNode.active = true;
        this.findingNode.active = false;
        this.foundNode.active = false;
        this.blockSelectDino(false);
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

    public OnChooseDinoWithData(idx:any,dataIdx:any)
    {
        this.listChoosingCard[idx].ChooseDino(this.dinoDatas[dataIdx]);

        // this.ResetDinoCardState();
        // this.ResetFightButton();
    }

    public OnGetBattleSuccess(result)
    {    
        console.log("OnGetBattleSuccess", result);
        ArenaModeChooseBattleManager.getInstance().matchingNode.active = false;
        ArenaModeChooseBattleManager.getInstance().findingNode.active = false;
        ArenaModeChooseBattleManager.getInstance().nextBtn.interactable = true;
        ArenaModeChooseBattleManager.getInstance().prevBtn.interactable = true;
        ArenaModeChooseBattleManager.getInstance().foundNode.active = true;
        let value = 5;
        if(result["code"] == 1){
            ArenaModeChooseBattleManager.getInstance().countDownFound.string = value+"s";
            ArenaModeChooseBattleManager.getInstance().schedule(()=>{
                if(value >= 1) {value--; ArenaModeChooseBattleManager.getInstance().countDownFound.string = value + "s";}
            },1)
            ArenaModeChooseBattleManager.getInstance().schedule(()=>{
                ArenaModeChooseBattleManager.getInstance().fightChecking = false;        
                Common.currentData = result["data"];
                director.loadScene("GameplayPVP");
            },5)
        }else{
            ArenaModeChooseBattleManager.getInstance().matchingNode.active = true;
            ArenaModeChooseBattleManager.getInstance().findingNode.active = false;
            ArenaModeChooseBattleManager.getInstance().foundNode.active = false;
            ArenaModeChooseBattleManager.getInstance().unschedule(this.countBySecond);
            window.foundMatch = null;
            ArenaModeChooseBattleManager.getInstance().blockSelectDino(false);
            ArenaModeChooseBattleManager.getInstance().ResetFightButton();
        }
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
                    this.choosingDinoDatas[i] = this.dinoDatas[(this.currentDinoPage-1 )*this.MAX_DINO_IN_PAGE+index];
                    this.listChoosingCard[this.choosingCardIndex].ChooseDino(this.dinoDatas[(this.currentDinoPage-1 )*this.MAX_DINO_IN_PAGE+index]);
                }
            }
        }
        else
        {
            if(this.choosingDinoDatas.length < 4) {
                this.choosingDinoDatas[this.choosingDinoDatas.length] = this.dinoDatas[(this.currentDinoPage-1 )*this.MAX_DINO_IN_PAGE+index];
                this.listChoosingCard[this.choosingCardIndex].ChooseDino(this.dinoDatas[(this.currentDinoPage-1 )*this.MAX_DINO_IN_PAGE+index]);
            }
        }
        log(index + " - "+this.currentDinoPage+ " - " + this.dinoDatas.length + " - choosingDinoDatas :" + this.choosingDinoDatas.length);
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
            let currentIndex = (this.currentDinoPage-1) * this.MAX_DINO_IN_PAGE + index;           

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
        this.matchingBtn.interactable = (this.choosingDinoDatas.length == 4);
        this.choosenDinoCount.string = this.choosingDinoDatas.length + "/" + this.MAX_DINO;

        if(this.choosingDinoDatas.length >= 4)
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
        this.matchingBtn.interactable = false;
        this.nextBtn.interactable = false;
        this.prevBtn.interactable = false;
        let finalChoosenData:object = new Object();
        finalChoosenData["dinoList"] = new Array();

        for (let index = 0; index < ArenaModeChooseBattleManager.getInstance().listChoosingCard.length; index++) {
            const element = ArenaModeChooseBattleManager.getInstance().listChoosingCard[index];
            if(element.dinoData != null)
            {
                let pvpDino:object = new Object();
                pvpDino["position"] = element.node.getSiblingIndex() + 1;
                pvpDino["nftId"] = element.dinoData.nftId;
                finalChoosenData["dinoList"].push(pvpDino);
            }
        }
        this.matchingNode.active = false;
        this.findingNode.active = true;
        this.foundNode.active = false;
        this.countDownNum = 0;
        this.countDownTxt.string = "0";
        this.schedule(this.countBySecond, 1);
        this.blockSelectDino(true);
        window.foundMatch = this.OnGetBattleSuccess;
        log(finalChoosenData);
        log("window.findMatch");
        if(window.findMatch)
        {
            window.findMatch(finalChoosenData);
        }
    }
    public countBySecond(){
        this.countDownNum++;
        this.countDownTxt.string = this.countDownNum.toString();
    }
    public OnCancelFindClicked(){
        let finalChoosenData:object = new Object();
        finalChoosenData["dinoList"] = new Array();
        if(window.cancelMatch)
        {
            window.cancelMatch(finalChoosenData);
            //window.foundMatch = null;
        }
        // this.matchingNode.active = true;
        // this.findingNode.active = false;
        // this.blockSelectDino(false);
        // this.ResetFightButton();
    }

    public CheckIncreaseNodeState()
    {
        let classCount = 0;
        let i=0;
        let j=0;

        for(i=1;i<this.MAX_DINO_IN_PAGE;i++)
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
    }

    blockSelectDino(isBlock:boolean): void{
        this.listDinoCard.forEach(element => {
            element.dinoSelectButtonNode.active = !isBlock && !element.dinoSelectedNode.activeInHierarchy;
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
