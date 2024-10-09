
import { _decorator, Component, Node, Vec3, tween, director, Label } from 'cc';
import { Common } from '../common/Common';
import { WBBossController } from './WB_BossController';
import { WBHeroController } from './WB_HeroController';
const { ccclass, property } = _decorator;

enum WBGameState {
    PREPARE,
    PLAYING,
    RESULT,
}
export { WBGameState }

@ccclass('WBManager')
export class WBManager extends Component {
    private static singleton: WBManager;
    public static getInstance(): WBManager {
        return WBManager.singleton;
    }

    @property({ type: Node })
    public heroes: Node[] =[];

    @property({ type: Node })
    public listBoss: Node[] = [];

    @property({ type: Node })
    public victoryNode: Node = null;

    @property({ type: Node })
    public defeatNode: Node = null;

    @property({ type:Label })
    public resultLabel: Label = null;

    @property({ type: Node })
    public retryButtonNode: Node = null;

    @property({ type: Node })
    public closeButtonNode: Node = null;
    
    public isBossDie:boolean = false;
    public remainHP:number = 0;
    public dmg:number = 0;
    public rank:number = 10000;
    public gameState: WBGameState = WBGameState.PREPARE;
    public heroAmountLeft:number = 0;
    public boss:Node = null;
    private typeboss:number;

    start () {
        WBManager.singleton = this;
        this.gameState = WBGameState.PREPARE;
        this.typeboss = Common.GetRarityBossNumber(Common.currentDinoData["typeBoss"]);
        for(var i=0; i < this.listBoss.length; i++){
            if(i == this.typeboss){
                this.listBoss[i].active = true;
                this.boss = this.listBoss[i];
            }
            else{
                this.listBoss[i].active = false;
            }
            
        }
        this.scheduleOnce(()=>{
            this.gameState = WBGameState.PLAYING;
        },1);
        this.SetData((Common.currentWBDinoResult["result"] === 2),Common.currentWBDinoResult["remainHp"],Common.currentWBDinoResult["damaged"],Common.currentWBDinoResult["rank"]);
        this.heroAmountLeft = JSON.parse(Common.currentWBDinoData).length;
        console.log("heroAmountLeft" + this.heroAmountLeft);
        
    }

    /**
     * SetData
     */
    public SetData(_isBossDie:boolean,_remainHP:number, _dmg:number, _rank:number) {
        this.isBossDie = _isBossDie;
        this.remainHP = _remainHP;
        this.dmg = _dmg;
        this.rank = _rank;
        console.log("this.isBossDie : "+this.isBossDie + " - "+this.remainHP + " - "+this.dmg + " - "+this.rank);

    }

    // update (deltaTime: number) {
    //     // [4]
    // }

    /**
     * FinishGame     * 
     */

    /**
     * GetTargetForBoss
     */
    public GetTargetForBoss() {
        for(let i=0;i<this.heroes.length; i++)
        {
            if(this.heroes[i].getComponent(WBHeroController).isDead == false)
                return this.heroes[i];
        }
        return null;
    }    

    public CheckFinishGame() {
        if(this.boss.getComponent(WBBossController).isDead )        
        {
            this.scheduleOnce(()=>{
                this.victoryNode.parent.active = true;
                this.victoryNode.active = true;
                this.defeatNode.active = false;
                
                this.victoryNode.scale = new Vec3(0,0,0);
                
                tween(this.victoryNode)
                .to(0.5, { scale: new Vec3(1,1,1) }, {
                    easing: 'linear', onComplete: () => {
                        // this.retryButtonNode.parent.active = true;
                        // let dmg =300;
                        // let rank = 40;
                        this.resultLabel.node.active = true;
                        this.resultLabel.string = "CONGRATULATION !!! "+ "\n You are the BOSS KILLER";
                        this.closeButtonNode.active = true;
                    }
                }).start();
            },1); 

            return;
        }
        if(this.heroAmountLeft <= 0)
        {            
            this.scheduleOnce(()=>{
                this.victoryNode.parent.active = true;
                this.victoryNode.active = false;
                this.defeatNode.active = true;
                
                this.defeatNode.scale = new Vec3(0,0,0);
                
                tween(this.defeatNode)
                .to(0.5, { scale: new Vec3(1,1,1) }, {
                    easing: 'linear', onComplete: () => {
                        // this.retryButtonNode.parent.active = true;
                        // let dmg =300;
                        // let rank = 40;
                        this.resultLabel.node.active = true;
                        this.resultLabel.string ="Boss HP remaining : "+this.formatPrice(this.remainHP) + "\nYour total damage : "+this.formatPrice(this.dmg) + "\nYour current rank : "+this.rank;
                        this.closeButtonNode.active = true;

                    }
                }).start();
            },1);          
        }
    }
    public formatPrice(number){
        const transfernumber = parseFloat(number).toFixed(2).replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,');
        return transfernumber.replace(/\.00/g, '');
      }
    /**
     * OnClickRetry
     */
    public OnClickRetry() {
        director.loadScene("GameplayWorldBoss");
    }

    public OnClickClose()
    {
        console.log('closeCocos');
        if(window.closeCocos)
            window.closeCocos();
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
