
import { _decorator, Component, Node, tween, Vec3,Label, SkeletalAnimation } from 'cc';
import { Common } from '../common/Common';
import { DinoData, OldDinoData } from '../common/DinoData';
import { PartDinoController } from '../PartDino/PartDinoController';
const { ccclass, property } = _decorator;

@ccclass('EvolveManager')
export class EvolveManager extends Component {

    @property({ type: PartDinoController })
    public partDino: PartDinoController;

    @property({ type: Node })
    public effectNode: Node;

    // @property({ type: Node })
    // public buttonNode: Node;

    @property({ type: Label })
    public speedLabel: Label = null;

    @property({ type: Node })
    public closeBtn: Node = null;

    speed:number = 0;
    isStartEvolve : boolean = false;

    public vectorScale : Vec3 = new Vec3(4.5,4.5,4.5);
    // [1]
    // dummy = '';

    // [2]
    // @property
    // serializableDummy = 0;
    oldDino:any;
    newDino:any;
    start () {
        // [3]
        let data = Common.currentData
        this.newDino = data["newDino"];
        this.oldDino = data["oldDino"];
        if(this.oldDino)
            this.partDino.loadDataVer1(this.oldDino["class"],this.oldDino["rarity"]);
        else
            this.partDino.loadDataVer1(1,1);    
        this.scheduleOnce(() => {
            this.onButtonEvolveClicked();
        },1);
    }

    update(deltaTime: number)
    {
        if(this.isStartEvolve == true)
        {
            this.speed += deltaTime * 5;
            this.speedLabel.string = this.speed.toString();
            this.partDino.node.setRotationFromEuler(new Vec3(this.partDino.node.eulerAngles.x,this.partDino.node.eulerAngles.y + this.speed,this.partDino.node.eulerAngles.z));

        }
        
    }

    evolve(){

        tween(this.node)
        .to(1, { worldScale: this.vectorScale }, {
            easing: 'cubicOut', onComplete: () => {  

            }
        }).start();    
    }

    onButtonEvolveClicked()
    {
        //this.buttonNode.active = false;
        this.isStartEvolve = true;
        this.scheduleOnce(() => {
            this.effectNode.active = true;
            tween(this.partDino.node)
                .to(1, { eulerAngles: new Vec3(0,0,0)})
                .delay(0.5).start();  
            this.scheduleOnce(() => {
                this.isStartEvolve = false;
                this.partDino.node.active = true;
                if(this.newDino){
                    this.partDino.loadDataByTrait(this.newDino["expressTraits"], this.newDino["geneRarity"], this.newDino["nftId"]);
                }
                this.partDino.OnPlayAnimEvolve();
                this.scheduleOnce(() => {
                    this.closeBtn.active = true;
                },1.5);
            },1);
        },3);
    }
    
    public Close(){
        console.log('closeCocos');
        if(window.closeCocos)
            window.closeCocos();
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
