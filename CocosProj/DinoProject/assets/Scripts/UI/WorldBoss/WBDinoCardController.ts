
import { _decorator, Component, Node, Sprite, resources, SpriteFrame, director, Label, LabelComponent, color, Color,Quat } from 'cc';
import { Common } from '../../common/Common';
import { DinoData } from '../../common/DinoData';
import { PartDinoController } from '../../PartDino/PartDinoController';
import { DinoDetailManager } from '../../PartDino/DinoDetailManager';

const { ccclass, property } = _decorator;

@ccclass('WBDinoCardController')
export class WBDinoCardController extends Component {

    private partDino: PartDinoController = null;
    // DINO DETAIL
    @property({ type: DinoDetailManager })

    public dinoDetailManager: DinoDetailManager = null;
    @property({ type: Node })
    public offCamera2D: Node = null;
    @property({ type: Node })
    public offCamera3D: Node = null;
    // DINO DETAIL End

    @property({ type: Sprite })
    public cardBG: Sprite;

    @property({ type: Sprite })
    public dinoBG: Sprite;

    @property({ type: Sprite })
    public dinoGender: Sprite;

    @property({ type: Sprite })
    public dinoClass: Sprite;

    @property({ type: Label })
    public dinoLevelLabel: Label;

    @property({ type: Node })
    public dinoSelectButtonNode: Node;

    @property({ type: Node })
    public dinoSelectedNode: Node;

    @property({ type: LabelComponent })
    public nftIdLbl: Label;

    public dinoData:any = null;
    private selected :boolean= false;
    start()
    {
        this.partDino = this.getComponentInChildren(PartDinoController);
    }

    public initialize( data:any): void{
        this.dinoData = data;
        Common.getRarityCard(+data["rarity"], (err, spriteFrame) => { this.cardBG.spriteFrame = spriteFrame;});
        Common.getRarityDinoBG(+data["rarity"], (err, spriteFrame) => { this.dinoBG.spriteFrame = spriteFrame;});
        Common.getGenderIcon(+data["gender"], (err, spriteFrame) => { this.dinoGender.spriteFrame = spriteFrame;});
        Common.GetClassIcon(+data["class"], (err, spriteFrame) => { this.dinoClass.spriteFrame = spriteFrame;});
        if(this.nftIdLbl)
            this.nftIdLbl.string = "#" + data["nftId"];
        if(!this.partDino)
            this.partDino = this.getComponentInChildren<PartDinoController>(PartDinoController);
        if(data["isEvolved"]){
            this.partDino.loadDataByTrait(data["expressTraits"],data["geneRarity"],data["nftId"]);
        }else{
            this.partDino.loadDataVer1(data["class"], data["rarity"]);
        }
        if(this.dinoLevelLabel)
        {
            this.dinoLevelLabel.string = "Level: "+ data["level"]
        }
    }

    public LoadDinoDetail()
    {
        Common.currentDinoData = this.dinoData;
        this.offCamera2D.active = false;
        this.offCamera3D.active = false;
        this.dinoDetailManager.node.active = true;
    }

    public CloseDinoDetail()
    {
        this.offCamera2D.active = true;
        this.offCamera3D.active = true;
        this.dinoDetailManager.node.active = false;
    }   

    public UpdateDinoCardState(_selected:boolean)
    {
        this.selected = _selected;
        this.dinoSelectButtonNode.active = !this.selected;
        this.dinoSelectedNode.active = this.selected;
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
