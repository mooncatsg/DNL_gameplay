
import { _decorator, Component, Node, log, sys, SkinnedMeshRenderer, Material, CCInteger, RichText, resources, Label, Sprite, SpriteFrame, director,randomRangeInt } from 'cc';
import { PartDinoController } from '../PartDino/PartDinoController';
import { DinoData } from '../common/DinoData';
import { Common } from '../common/Common';
const { ccclass, property } = _decorator;

@ccclass('DataManager')
export class DataManager extends Component {
    private static singleton: DataManager;
    public static getInstance(): DataManager {
        return DataManager.singleton;
    }

    public eggPKDict: any = [];

    // @property({ type: SkinnedMeshRenderer })
    // public dinoMeshRender: SkinnedMeshRenderer[] = [];

    // @property({ type: SkinnedMeshRenderer })
    // public dinofaceMeshRender: SkinnedMeshRenderer;

    @property({ type: SkinnedMeshRenderer })
    public eggMeshRender: SkinnedMeshRenderer[] = [];

    @property({ type: Node })
    public eggCustom: Node[] = [];

    @property({ type: Node })
    public hatchingController: Node;

    @property({ type: Label })
    public dinoNameText: Label;

    @property({ type: Label })
    public dinoRareText: Label;

    @property({ type: Label })
    public dinoClassText: Label;

    @property({ type: Label })
    public dinoIdText: Label;

    @property({ type: SpriteFrame })
    public starList: SpriteFrame[] = [];

    @property({ type: String })
    public rareList: string[] = [];

    @property({ type: Sprite })
    public star: Sprite;

    @property({ type: Sprite })
    public iconClass: Sprite;

    @property({ type: Node })
    public particles: Node[] = [];

    @property({ type: PartDinoController })
    public partDino: PartDinoController = null;

    @property({ type: Node })
    public closeButton: Node;

    public isElvoved :boolean = false;

    start() {
        DataManager.singleton = this;

        DataManager.singleton.loadData();

        //-1= none, 0 = crown, 1 = leaf, 2 = horn, 3 =  Wing, 4 =  Spike
        DataManager.singleton.eggPKDict["1111"] = -1;
        DataManager.singleton.eggPKDict["1112"] = -1;
        DataManager.singleton.eggPKDict["1113"] = 0;
        DataManager.singleton.eggPKDict["1114"] = 1;
        DataManager.singleton.eggPKDict["1115"] = 2;
        DataManager.singleton.eggPKDict["1116"] = 4;
  
        DataManager.singleton.eggPKDict["1211"] = -1;
        DataManager.singleton.eggPKDict["1212"] = 4;
        DataManager.singleton.eggPKDict["1213"] = 0;
        DataManager.singleton.eggPKDict["1214"] = 2;
        DataManager.singleton.eggPKDict["1215"] = 0;
        DataManager.singleton.eggPKDict["1216"] = -1;
        
        DataManager.singleton.eggPKDict["1311"] = -1;
        DataManager.singleton.eggPKDict["1312"] = 1;
        DataManager.singleton.eggPKDict["1313"] = 2;
        DataManager.singleton.eggPKDict["1314"] = 4;
        DataManager.singleton.eggPKDict["1315"] = 4;
        DataManager.singleton.eggPKDict["1316"] = 2;

        DataManager.singleton.eggPKDict["1411"] = 2;
        DataManager.singleton.eggPKDict["1412"] = 4;
        DataManager.singleton.eggPKDict["1413"] = 0;
        DataManager.singleton.eggPKDict["1414"] = 3;
        DataManager.singleton.eggPKDict["1415"] = 3;
        DataManager.singleton.eggPKDict["1416"] = 3;
        
        DataManager.singleton.eggPKDict["1511"] = 3;
        if(Common.currentLoadScene != "hatching")
            this.scheduleOnce(()=>{ this.closeButton.active = true; } , 8);
    }

    loadData() {
        let dino_class = Common.currentHatchingData["isEvolved"]? Common.currentHatchingData["expressTraits"]["Class"] + 1 : Common.currentHatchingData["class"];
        let dino_id = Common.currentHatchingData["nftId"];
        if(DataManager.singleton.particles[dino_class-1])
            DataManager.singleton.particles[dino_class-1].active = true; 
        
        console.log(Common.currentHatchingData["eggType"]);
        DataManager.singleton.getEggMat(Common.currentHatchingData["eggType"],(eggMat)=>{
            DataManager.singleton.eggMeshRender.forEach(element => {
                element.material = eggMat;
            });
        });

        // DINO PART HERE
        if(Common.currentHatchingData["isEvolved"])
        {
            this.partDino.getComponent(PartDinoController).loadDataByTrait(Common.currentHatchingData["expressTraits"],Common.currentHatchingData["geneRarity"],Common.currentHatchingData["nftId"]);
        }
        else
        {
            this.partDino.getComponent(PartDinoController).loadDataVer1(Common.currentHatchingData["class"], Common.currentHatchingData["rarity"]);
        }

        DataManager.singleton.dinoNameText.string = Common.currentHatchingData["title"];
        let dino_rarity = Common.currentHatchingData["isEvolved"]? Common.currentHatchingData["geneRarity"] : Common.currentHatchingData["rarity"];
        this.star.spriteFrame = this.starList[dino_rarity - 1];
        this.dinoRareText.string = this.rareList[dino_rarity - 1];
        this.dinoClassText.string = Common.GetClassString(dino_class);
        Common.GetClassIcon(+dino_class, (err,spr)=>{ this.iconClass.spriteFrame = spr; });
        this.dinoIdText.string = "#" + dino_id

        DataManager.singleton.hatchingController.active = true;
    }

    getEggMat(egg_name: number, callbacks: any): void {
        // let egg_id = (((Math.floor(egg_name / 100)) - 11) * 6) + ((egg_name % 100) - 11);
        // if (egg_id < 0 || egg_id >= DataManager.singleton.eggsMaterialArray.length)
        //     egg_id = 0;
        // return DataManager.singleton.eggsMaterialArray[egg_id];
        resources.load("Materials_Egg/" + egg_name, Material, (err, mat) => {
            callbacks(mat);
        });
    }

    getDinoMat(dino_class: any, dino_rarity: any, callbacks: any): void {
        // let dino_id = ((dino_class - 1) * 5) + (dino_rarity - 1);
        // if (dino_id < 0 || dino_id >= DataManager.singleton.dinoMaterialArray.length)
        //     dino_id = 0;
        // return DataManager.singleton.dinoMaterialArray[dino_id];
        resources.load("MaterialDino/dino_"+dino_class+"_"+dino_rarity, Material, (err, mat) => {
            callbacks(mat);
        });
    }

    getDinoFaceMat(dino_class: any, dino_rarity: any, callbacks: any): void {
        // let dino_id = ((dino_class - 11) * 5) + (dino_rarity - 11);
        // if (dino_id < 0 || dino_id >= DataManager.singleton.dinoFaceMaterialArray.length)
        //     dino_id = 0;
        // return DataManager.singleton.dinoFaceMaterialArray[dino_id];
        resources.load("MaterialDino/dino_face_"+dino_class+"_"+dino_rarity, Material, (err, mat) => {
            callbacks(mat);
        });
    }

    public Close(){
        console.log('closeCocos');
        if(window.closeCocos)
            window.closeCocos();
    }
}
