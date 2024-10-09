
import { _decorator, Component, Node, Sprite, resources, SpriteFrame, director, Label, LabelComponent, color, Color,Quat } from 'cc';
import { Common } from '../../common/Common';
import { PartDinoController } from '../../PartDino/PartDinoController';
const { ccclass, property } = _decorator;

@ccclass('BreadingDinoCardController')
export class BreadingDinoCardController extends Component {

    private partDino: PartDinoController = null;

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

    @property({ type: LabelComponent })
    public nftIdLbl: Label;

    @property({ type: LabelComponent })
    public errorLbl: Label;

    private dinoData:any;
    public errorStr:string;
    
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
            this.dinoLevelLabel.string = "Love: "+ data["breedCount"] + "/" + data["maxBreedCount"]
        }
    }

    public validateDino(minLifeDurationToBreed:number, otherSelectedDino: any, isBreeding:boolean)
    {
        if(!this.errorLbl)
            return;
        this.errorStr = "";
        if(isBreeding){
            if(otherSelectedDino){
                if((''+otherSelectedDino["fatherNftId"]) === (''+this.dinoData["nftId"])){
                    this.errorStr += "Cannot breed with daughter."
                }
                else if((''+otherSelectedDino["motherNftId"]) === (''+this.dinoData["nftId"])){
                    if(!this.errorStr || this.errorStr.length > 0) this.errorStr += "\n";
                    this.errorStr += "Cannot breed with son."
                }
                else if((''+otherSelectedDino["nftId"]) === (''+this.dinoData["fatherNftId"])){
                    if(!this.errorStr || this.errorStr.length > 0) this.errorStr += "\n";
                    this.errorStr += "Cannot breed with father."
                }
                else if((''+otherSelectedDino["nftId"]) === (''+this.dinoData["motherNftId"])){
                    if(!this.errorStr || this.errorStr.length > 0) this.errorStr += "\n";
                    this.errorStr += "Cannot breed with mother."
                }
                else if( ((''+otherSelectedDino["fatherNftId"]) === (''+this.dinoData["fatherNftId"]) && (''+this.dinoData["fatherNftId"]) != "0")
                || ( (''+otherSelectedDino["motherNftId"]) === (''+this.dinoData["motherNftId"]) && (''+this.dinoData["fatherNftId"]) != "0") )
                {
                    if(!this.errorStr || this.errorStr.length > 0) this.errorStr += "\n";
                    this.errorStr += "Cannot breed siblings."
                }
            }
    
            if(Date.now() - +this.dinoData["bornAt"] < minLifeDurationToBreed){
                    if(!this.errorStr || this.errorStr.length > 0) this.errorStr += "\n";
                    this.errorStr += "No underage breeding."
            }
            if(+this.dinoData["breedCount"] >=  +this.dinoData["maxBreedCount"]){
                if(!this.errorStr || this.errorStr.length > 0) this.errorStr += "\n";
                this.errorStr += "This dino is over breed."
            }
            if(!this.dinoData["isEvolved"]){
                    if(!this.errorStr || this.errorStr.length > 0) this.errorStr += "\n";
                    this.errorStr += "Dino hasn\'t evolved,\ncan\'t breed."
            }
        }else{
            if(otherSelectedDino){
                console.log("BreadingDinoCardController --------------------------------- 1111111111111111111");
                console.log(otherSelectedDino);
                console.log("BreadingDinoCardController --------------------------------- 2222222222222222222");
                console.log(this.dinoData);
                if((''+otherSelectedDino["geneRarity"]) != (''+this.dinoData["geneRarity"]))
                    this.errorStr += "Cannot fusion with dino\nhave different rarity.";
            }
        }
        if(this.errorStr || this.errorStr.length > 0){
            this.errorLbl.string = this.errorStr;
            this.dinoSelectButtonNode.active = false;
            this.errorLbl.node.active = true;
            this.cardBG.color = Color.GRAY;
        }else{
            this.dinoSelectButtonNode.active = true;
            this.errorLbl.node.active = false;
            this.cardBG.color = Color.WHITE;
        }
    }

    public OnCardClicked():void{

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