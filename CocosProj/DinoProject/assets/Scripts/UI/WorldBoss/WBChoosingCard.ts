
import { _decorator, Component, Node, Sprite ,log} from 'cc';
import { Common } from '../../common/Common';
import { PartDinoController } from '../../PartDino/PartDinoController';
const { ccclass, property } = _decorator;

@ccclass('WBChoosingCard')
export class WBChoosingCard extends Component {

    @property({ type: Node })
    public bgChoosing: Node = null;

    @property({ type: Sprite })
    public bgDino: Sprite = null;

    @property({ type: Sprite })
    public rarity: Sprite = null;

    @property({ type: Sprite })
    public class: Sprite = null;

    @property({ type: PartDinoController })
    public dinoPart: PartDinoController = null;

    public dinoData:any;

    start () {
        // [3]
        this.dinoPart = this.getComponentInChildren(PartDinoController);

    }    

    public ChooseDino(data:any)
    {
        this.dinoData = data;
        this.bgDino.node.active = true;
        log(this.dinoData["rarity"] + " + " + this.dinoData.toString());
        // DATA HERE     
        Common.getDinoBGByRarity(+this.dinoData["rarity"], (err, spriteFrame) => { this.bgDino.spriteFrame = spriteFrame;});
        Common.getRarityIcon(+this.dinoData["rarity"], (err, spriteFrame) => { this.rarity.spriteFrame = spriteFrame;});
        Common.GetClassIcon(+this.dinoData["class"], (err, spriteFrame) => { this.class.spriteFrame = spriteFrame;});
        if(this.dinoData["isEvolved"]){
            this.dinoPart.loadDataByTrait(this.dinoData["expressTraits"],this.dinoData["geneRarity"],this.dinoData["nftId"]);
        }
    }

    public ResetDino()
    {
        this.dinoData = null;
        this.bgDino.node.active = false;
    }


    public OnDinoChoosing()
    {
        this.bgChoosing.active = true;
    }

    public OnReset()
    {
        this.bgChoosing.active = false;
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
