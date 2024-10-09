
import { _decorator, Component, Node,Prefab, Vec3, tween, director, Label, JsonAsset,LabelComponent, SpriteFrame, resources, Sprite, Animation } from 'cc';
import { Common } from '../common/Common';
import { PVPHeroController } from './PVPHeroController';
const { ccclass, property } = _decorator;


enum PVPGameState {
    PREPARE,
    PLAYING,
    RESULT,
}
export { PVPGameState }


@ccclass('PVPManager')
export class PVPManager extends Component {
    private static singleton: PVPManager;
    public static getInstance(): PVPManager {
        return PVPManager.singleton;
    }

    @property({ type: PVPHeroController })
    public ourDinos: PVPHeroController[] = [];
    @property({ type: PVPHeroController })
    public enemyDinos: PVPHeroController [] = [];

    @property({ type: Node })
    public ourDinoPos: Node[] = [];
    @property({ type: Node })
    public enemyDinoPos: Node[] = [];

    @property({ type: JsonAsset })
    public sampleData: JsonAsset;

    @property({ type: Prefab })
    public explosionPrefab1: Prefab = null;
    @property({ type: Prefab })
    public explosionPrefab2: Prefab = null;
    @property({ type: Prefab })
    public explosionPrefab3: Prefab = null;
    @property({ type: Prefab })
    public healPrefab: Prefab = null;

    @property({ type: Node })
    public result: Node;
    @property({ type: Sprite })
    public victory: Sprite;
    @property({ type: LabelComponent })
    public resultLbl: Label;
    @property({ type: LabelComponent })
    public dinoAliveEnemy: Label;
    @property({ type: LabelComponent })
    public dinoAliveYourTeam: Label;
    @property({ type: LabelComponent })
    public totalHPEnemy: Label;
    @property({ type: LabelComponent })
    public totalHPYourTeam: Label;
    @property({ type: Sprite })
    public EnemyResult: Sprite;
    @property({ type: Animation })
    public amiShowResult: Animation;
    private isShow: boolean = false;

    public apiResponse: any;
    endGame:boolean = false;

    start () {
        PVPManager.singleton = this;
        //this.apiResponse = this.sampleData.json;
        this.apiResponse = Common.currentData;
        this.initTeam(this.apiResponse["myTeam"],this.ourDinos,this.ourDinoPos);
        this.initTeam(this.apiResponse["enemyTeam"],this.enemyDinos,this.enemyDinoPos);
        this.alldino = this.ourDinos.concat(this.enemyDinos);
        this.currentTurn = 0;
        this.totalTurn = +this.apiResponse["totalTurn"];
    }

    public initTeam(dinoListData:any, dinoControllerList:PVPHeroController[], posList:Node[]){
        for (let index = 0; index < dinoControllerList.length; index++) {
            const element = dinoControllerList[index];
            element.IconSkill(dinoListData[index]["skill"]);
            element.initialize(posList[+dinoListData[index]["position"] - 1], dinoListData[index]["decodeGenes"], +dinoListData[index]["rarity"], +dinoListData[index]["nftId"]);
        }
    }

    fixedTime:number = 2;
    currentTurn:number = 100000;
    totalTurn:number;
    update (deltaTime: number) {
        // [4]\
        //console.log("update -------------------- "+this.fixedTime + "-------" +deltaTime);
        if(this.currentTurn < this.totalTurn){
            this.fixedTime -= deltaTime;
            //console.log("update -------------------- "+this.fixedTime);
            if(this.fixedTime <= 0){
                this.doTurn(this.currentTurn);
                this.fixedTime = this.currentTurn < this.totalTurn? 1 : 3;
            }
        }else{
            if(this.fixedTime > 0){
                this.fixedTime -= deltaTime;
            }else{
                if(!this.endGame){
                    this.endGame = true;
                    this.showResult();
                }
            }
        }
    }
    alldino:PVPHeroController [];
    doTurn(turnIdx:number){
        console.log("doTurn -------------------- "+turnIdx);
        let turnData = this.apiResponse["fightDetails"][turnIdx];
        if(!this.alldino){
            this.alldino = this.ourDinos.concat(this.enemyDinos);
        }
        let striker = this.alldino.find(x=>x.nftId == +turnData["striker"]["nftId"]);
        
        if(striker.isBlockingNextTurn())
            return;
        let targets = new Array();
        for (let index = 0; index < turnData["targets"].length; index++) {
            const element = turnData["targets"][index];
            targets.push(this.alldino.find(x=>x.nftId == +element["nftId"]));
        }
        for (let index = 0; index < targets.length; index++) {
            const element = targets[index];
            if(element.isBlockingNextTurn())
                return;
        }
        let mana; turnData["striker"]["mana"]>=100 ? mana = 100 : mana = turnData["striker"]["mana"];
        this.currentTurn++;
        striker.Attack(turnData["strikeType"], targets,turnData["targets"], mana/100);
    }

    showResult()
    {
        this.resultLbl.string =""
        console.log('showResult');
        if (this.apiResponse["reward"])
            this.resultLbl.string = this.apiResponse["reward"] + (this.apiResponse["rewardType"] == 1?" W-DNL":" W-DNG");
        this.dinoAliveEnemy.string = this.apiResponse["survivors"]["enemyTeam"];
        this.dinoAliveYourTeam.string = this.apiResponse["survivors"]["myTeam"];
        this.totalHPEnemy.string = this.apiResponse["remainHp"]["enemyTeam"];
        this.totalHPYourTeam.string = this.apiResponse["remainHp"]["myTeam"];
        if(this.apiResponse["result"] == "win"){
            resources.load("Victory/BattleWinPvp/mainWin/spriteFrame", SpriteFrame, (err, spriteFrame) => {
                this.victory.spriteFrame = spriteFrame;
            });
            resources.load("Victory/BattleWinPvp/fame_right/spriteFrame", SpriteFrame, (err, spriteFrame) => {
                this.EnemyResult.spriteFrame = spriteFrame;
            });
        }
        else if(this.apiResponse["result"] == "draw"){
            resources.load("Victory/BattleDrawPvp/main/spriteFrame", SpriteFrame, (err, spriteFrame) => {
                this.victory.spriteFrame = spriteFrame;
            });
            resources.load("Victory/BattleDrawPvp/frame_right/spriteFrame", SpriteFrame, (err, spriteFrame) => {
                this.EnemyResult.spriteFrame = spriteFrame;
            });
        }
        else{
            resources.load("Victory/BattleLosePvp/main/spriteFrame", SpriteFrame, (err, spriteFrame) => {
                this.victory.spriteFrame = spriteFrame;
            });
            resources.load("Victory/BattleLosePvp/frame_right/spriteFrame", SpriteFrame, (err, spriteFrame) => {
                this.EnemyResult.spriteFrame = spriteFrame;
            });
        }
        this.result.active = true;
    }
    public onShowEnemyTeam(){
        if(this.isShow == false){
            this.amiShowResult.play("showEnemyResult");
            this.isShow = true;
        }
        else{
            this.amiShowResult.play("hideEnemyResult");
            this.isShow = false;
        }
    }
    public OnClickClose()
    {
        console.log('confirmResult');
        if(window.confirmResult)
            window.confirmResult(this.apiResponse["matchId"]);
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
