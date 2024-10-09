
import { _decorator, Component, Node, resources, Material, sys, log, SkinnedMeshRenderer, director, Vec3 } from 'cc';
import { Common } from '../common/Common';
import { HealthBar3D } from '../common/HealthBar3D';
import { WBDinoRender } from './WB_DinoRender';
import { WBHeroController } from './WB_HeroController';
import { WBManager } from './WB_Manager';
const { ccclass, property } = _decorator;



@ccclass('WBDataManager')
export class WBDataManager extends Component {
    private static singleton: WBDataManager;
    public static getInstance(): WBDataManager {
        return WBDataManager.singleton;
    }
    public apiResponse: any;
    public isBossDead: boolean;

    @property({ type: WBHeroController })
    public dinoHeroes: WBHeroController[] = [];

    @property({ type: Node })
    public positionNodes: Node[] = [];

    @property({ type: HealthBar3D })
    public bossHealthBar: HealthBar3D = null;

    public damageRate:number = 1;
    start() {
        WBDataManager.singleton = this;
        WBDataManager.singleton.apiResponse = Common.currentData;
        WBDataManager.singleton.loadData(JSON.parse(Common.currentWBDinoData),Common.currentWBDinoPosition);
        this.SetBossHealthBar(WBDataManager.singleton.apiResponse["remainHp"],WBDataManager.singleton.apiResponse["maxHealth"]);
    }

    public SetBossHealthBar(_current:number,_max:number)
    {
        WBDataManager.singleton.bossHealthBar.setHealthValue(_current/_max);
    }

    loadData(dinos:any,positions:any) {
        this.damageRate = dinos.length/4;
        log("dinos : "+dinos.length+ " - damageRate : "+this.damageRate);
        for (let index = 0; index < dinos.length; index++) {
            this.dinoHeroes[index].node.active = true;
            this.dinoHeroes[index].node.parent.position = new Vec3(this.positionNodes[positions[index]].position.x,0,this.positionNodes[positions[index]].position.z);
            if(dinos[index]["isEvolved"]){
                //this.partDino.loadDataByTrait(dinos["expressTraits"],dinos["geneRarity"],dinos["nftId"]);
                this.dinoHeroes[index].LoadData(dinos[index]["expressTraits"],dinos[index]["geneRarity"],dinos[index]["nftId"]);
            }
        }

        // Set data for the rest of dino
        for (let index = dinos.length; index < this.dinoHeroes.length; index++) {
            this.dinoHeroes[index].currentHealth = 0;
        }

        this.scheduleOnce(()=>{
            this.positionNodes.forEach(element => {
                element.active = false;
            });
        },1);
    }
    // getDinoMat(dino_class: any, dino_rarity: any, callbacks: any): void {
    //     resources.load("MaterialDinoFixLight/dino_" + dino_class + "_" + dino_rarity, Material, (err, mat) => {
    //         callbacks(mat);
    //     });
    // }

    // getDinoFaceMat(dino_class: any, dino_rarity: any, callbacks: any): void {
    //     resources.load("MaterialDinoFixLight/dino_face_" + dino_class + "_" + dino_rarity, Material, (err, mat) => {
    //         callbacks(mat);
    //     });
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
