
import { _decorator, Component, Node, log, sys, SkinnedMeshRenderer, Material, RichText, Label, Game, resources, random, randomRange, director,randomRangeInt, DebugMode, math, TextureCube, Sprite, SpriteFrame} from 'cc';
import { GameplayController } from './GameplayController';
import { BattleUI } from './UI/BattleUI';
import { DinoController } from './DinoController';
import { PartDinoController } from './PartDino/PartDinoController';
import { DinoData } from './common/DinoData';
import { Common } from './common/Common';
const { ccclass, property } = _decorator;

@ccclass('GameManager')
export class GameManager extends Component {
    private static singleton: GameManager;
    public static getInstance(): GameManager {
        return GameManager.singleton;
    }

    @property({type: Node })
    public dinoPartController: Node;

    // @property({ type: SkinnedMeshRenderer })
    // public dinoMeshRender: SkinnedMeshRenderer[] = [];

    // @property({ type: SkinnedMeshRenderer })
    // public dinofaceMeshRender: SkinnedMeshRenderer;

    @property({ type: SkinnedMeshRenderer })
    public monsterMeshRender: SkinnedMeshRenderer[] = [];

    // @property({ type: Material })
    // public dinoFaceMaterialArray: Material[] = [];

    // @property({ type: Material })
    // public dinoMaterialArray: Material[] = [];

    // @property({ type: Material })
    // public monstersMaterialArray: Material[] = [];

    @property({ type: Node })
    public gameplayNodes: Node[] = [];

    @property({ type: Node })
    public pveMap_1: Node;

    @property({ type: Node })
    public pveMap_2: Node;

    @property({ type: Node })
    public mainLight: Node;

    @property({ type: TextureCube})
    public skyBox: Node;

    @property({ type: TextureCube})
    public skyBoxNight: Node;

    @property({ type: Sprite })
    public VictorySprite: Sprite = null;

    @property({ type: BattleUI })
    public dinoBattleUI: BattleUI;

    @property({ type: BattleUI })
    public monsterBattleUI: BattleUI;
    @property({ type: Label })
    public battleIdLabel: Label = null;

    start() {
        GameManager.singleton = this;
        GameManager.singleton.loadData();    

        if (Common.currentData) {
            let battleId = Common.currentData["battleId"];
            GameManager.singleton.battleIdLabel.string = battleId + "";
        }
    }

    getRandomInt(min, max): number { // min to max -1
        min = Math.ceil(min);
        max = Math.floor(max);
        return Math.floor(Math.random() * (max - min)) + min;
    }

    public monsterType: number;
    public monsterID: number;
    public monsterName: string;

    public dinoName: string;
    public dinoClass: string;
    public dinoRarity: string;
    public dinoLevel: string;

    public result: number;// 1 is player win, except is monster win

    loadData() {

        GameManager.singleton.result = +Common.currentData["result"];
        GameManager.singleton.monsterType = +Common.currentData["rightMonster"]["type"];

        // SET DATA
        let dataID = 1;
        if (GameManager.singleton.result == 1) // DINO WIN
        {
            if (GameManager.singleton.monsterType == 1) { // MONSTER
                dataID = this.getRandomInt(1, 8);
            }
            else if (GameManager.singleton.monsterType == 2) { // MINI BOSS
                dataID = this.getRandomInt(8, 11);
            }
        }
        else if (GameManager.singleton.result == 2) // MONSTER WIN
        {
            if (GameManager.singleton.monsterType == 1) { // MONSTER                
                dataID = this.getRandomInt(11, 18);

            }
            else if (GameManager.singleton.monsterType == 2) { // MINI BOSS
                dataID = this.getRandomInt(18, 21);
            }
        }

        if (Common.currentData) {
            let forceDataContent = Common.currentData["content"];
            if (forceDataContent) {
                if (this.ValidContent(GameManager.singleton.result, GameManager.singleton.monsterType, +forceDataContent))
                    dataID = forceDataContent;
            }
        }
        this.prepareData(dataID);

        this.dinoBattleUI.SetNameAndLevel(Common.currentData["leftNft"]["title"], Common.currentData["leftNft"]["level"]);
        let dino_rarity = Common.currentData["leftNft"]["isEvolved"]? Common.currentData["leftNft"]["geneRarity"] : Common.currentData["leftNft"]["rarity"];
        let dino_class = Common.currentData["leftNft"]["isEvolved"]? Common.currentData["leftNft"]["expressTraits"]["Class"] + 1 : Common.currentData["leftNft"]["class"];
        this.dinoBattleUI.SetRarity(dino_rarity);
        this.dinoBattleUI.SetIconDino(dino_class, dino_rarity);


        this.monsterBattleUI.SetNameAndLevel(Common.currentData["rightMonster"]["title"], null);
        this.monsterBattleUI.SetIconMonster(Common.currentData["rightMonster"]["type"], Common.currentData["rightMonster"]["kindId"]);

        if (GameManager.singleton.monsterType == 1) {
            GameManager.singleton.getMonsterMat(Common.currentData["rightMonster"]["kindId"], (monsterMat) => {
                GameManager.singleton.monsterMeshRender.forEach(element => {
                    element.material = monsterMat;
                });
            });
        }

        if(Common.currentData["leftNft"]["isEvolved"])
        {
            this.dinoPartController.getComponent(PartDinoController).loadDataByTrait(Common.currentData["leftNft"]["expressTraits"], Common.currentData["leftNft"]["geneRarity"], Common.currentData["leftNft"]["nftId"]);
        }
        else
        {
            this.dinoPartController.getComponent(PartDinoController).loadDataVer1(Common.currentData["leftNft"]["class"], Common.currentData["leftNft"]["rarity"]);
        }

        switch(Common.currentData["randomMap"])
        {
            case 0:{
                console.log("random map: 1");
                director.getScene().globals.skybox.envmap = this.skyBox;
                this.mainLight.setRotation(-110);
                resources.load("Victory/UI_Victory/spriteFrame", SpriteFrame, (err, spriteFrame) => {
                    this.VictorySprite.spriteFrame = spriteFrame;
                });
                this.pveMap_1.active = true;
                this.pveMap_2.active = false;
                break;
            }
            case 1:{
                console.log("random map: 2");
                director.getScene().globals.skybox.envmap = this.skyBoxNight;
                this.mainLight.rotate = (65);
                resources.load("Victory/UI_Victory_MidAutumn/spriteFrame", SpriteFrame, (err, spriteFrame) => {
                    this.VictorySprite.spriteFrame = spriteFrame;
                });
                this.pveMap_1.active = false;
                this.pveMap_2.active = true;
                break;
            }
            default:{
                console.log("random map: 1");
                director.getScene().globals.skybox.envmap = this.skyBox;
                this.mainLight.setRotation(-110);
                resources.load("Victory/UI_Victory/spriteFrame", SpriteFrame, (err, spriteFrame) => {
                    this.VictorySprite.spriteFrame = spriteFrame;
                });
                this.pveMap_1.active = true;
                this.pveMap_2.active = false;
            }
        }

        // GameManager.singleton.getDinoFaceMat(dino_class, dino_rarity, (faceMat) => {
        //     GameManager.singleton.dinofaceMeshRender.material = faceMat;
        // });

        // GameManager.singleton.getDinoMat(dino_class, dino_rarity, (dinoMat) => {
        //     GameManager.singleton.dinoMeshRender.forEach(element => {
        //         element.material = dinoMat;
        //     });
        // });
        

        GameManager.singleton.gameplayNodes.forEach(element => {
            element.active = true;
        });

        this.scheduleOnce(() => {
            GameplayController.getInstance().StatusText(Common.currentData["rightMonster"]["title"]);
        }, 0.5);

    }

    ValidContent(result: number, monsterType: number, content: number) {
        if (result == 1) // DINO WIN
        {
            if (monsterType == 1) { // MONSTER
                if (content >= 1 && content < 8)
                    return true;
            }
            else if (monsterType == 2) { // MINI BOSS
                if (content >= 8 && content < 11)
                    return true;
            }
        }
        else if (result == 2) // MONSTER WIN
        {
            if (monsterType == 1) { // MONSTER
                if (content >= 11 && content < 18)
                    return true;
            }
            else if (monsterType == 2) { // MINI BOSS
                if (content >= 18 && content < 21)
                    return true;
            }
        }
        return false;
    }

    getMonsterMat(monster_id: number, callbacks: any): void {
        // if (!monster_id)
        //     monster_id = 1001;
        // monster_id = monster_id % 1000;
        // if (monster_id <= 0 || monster_id > GameManager.singleton.monstersMaterialArray.length)
        //     monster_id = 1;
        // return GameManager.singleton.monstersMaterialArray[monster_id - 1];
        resources.load("Material_Monster/monster_" + monster_id, Material, (err, mat) => {
            callbacks(mat);
        });
    }

    getDinoMat(dino_class: any, dino_rarity: any, callbacks: any): void {
        // if (!dino_class)
        //     dino_class = 1;
        // if (!dino_rarity)
        //     dino_rarity = 1;
        // let dino_id = ((dino_class - 1) * 5) + (dino_rarity - 1);
        // if (dino_id < 0 || dino_id >= GameManager.singleton.dinoMaterialArray.length)
        //     dino_id = 0;
        // return GameManager.singleton.dinoMaterialArray[dino_id];

        resources.load("MaterialDinoFixLight/dino_" + dino_class + "_" + dino_rarity, Material, (err, mat) => {
            callbacks(mat);
        });
    }

    getDinoFaceMat(dino_class: any, dino_rarity: any, callbacks: any): void {
        // if (!dino_class)
        //     dino_class = 1;
        // if (!dino_rarity)
        //     dino_rarity = 1;
        // let dino_id = ((dino_class - 11) * 5) + (dino_rarity - 11);
        // if (dino_id < 0 || dino_id >= GameManager.singleton.dinoFaceMaterialArray.length)
        //     dino_id = 0;
        // return GameManager.singleton.dinoFaceMaterialArray[dino_id];
        resources.load("MaterialDinoFixLight/dino_face_" + dino_class + "_" + dino_rarity, Material, (err, mat) => {
            callbacks(mat);
        });
    }

    content: Content = new Content(1, 1);
    prepareData(dataId: number) {

        switch (dataId) {
            case 1:
                this.content = new Content(dataId, 1);
                this.content.steps[0] = new Step(1, 1, 1, 1, 0, 0, 2); // Xuat hien
                this.content.steps[1] = new Step(2, 1, 1, 1, 30, 70, 2);
                this.content.steps[2] = new Step(3, 2, 2, 1, 15, 85, 1);
                this.content.steps[3] = new Step(4, 1, 2, 1, 20, 50, 1);
                this.content.steps[4] = new Step(5, 2, 1, 1, 50, 35, 2);
                this.content.steps[5] = new Step(6, 1, 2, 1, 10, 4, 1);
                this.content.steps[6] = new Step(7, 2, 3, 1, 30, 5, 2);
                this.content.steps[7] = new Step(8, 1, 3, 2, 40, 0, 2);
                break;
            case 2:
                this.content = new Content(dataId, 1);
                this.content.steps[0] = new Step(1, 1, 1, 1, 0, 0, 2); // Xuat hien
                this.content.steps[1] = new Step(2, 1, 1, 2, 50, 50, 2);
                this.content.steps[2] = new Step(3, 2, 2, 1, 30, 70, 2);
                this.content.steps[3] = new Step(4, 1, 2, 1, 20, 30, 1);
                this.content.steps[4] = new Step(5, 2, 1, 2, 50, 20, 2);
                this.content.steps[5] = new Step(6, 1, 2, 1, 30, 0, 1);
                break;
            case 3:
                this.content = new Content(dataId, 1);
                this.content.steps[0] = new Step(1, 1, 1, 1, 0, 0, 2); // Xuat hien
                this.content.steps[1] = new Step(2, 2, 1, 1, 20, 80, 1);
                this.content.steps[2] = new Step(3, 1, 2, 1, 30, 70, 1);
                this.content.steps[3] = new Step(4, 2, 3, 2, 50, 30, 2);
                this.content.steps[4] = new Step(5, 1, 2, 1, 30, 40, 2);
                this.content.steps[5] = new Step(6, 2, 1, 3, 0, 30, 0);
                this.content.steps[6] = new Step(7, 1, 3, 2, 40, 0, 2);
                break;
            case 4: // DANG CHO DATA
                this.content = new Content(dataId, 1);
                this.content.steps[0] = new Step(1, 1, 1, 1, 0, 0, 2); // Xuat hien
                this.content.steps[1] = new Step(2, 1, 2, 1, 20, 80, 1);
                this.content.steps[2] = new Step(3, 2, 2, 1, 35, 65, 1);
                this.content.steps[3] = new Step(4, 1, 2, 1, 30, 50, 1);
                this.content.steps[4] = new Step(5, 2, 3, 2, 30, 25, 2);
                this.content.steps[5] = new Step(6, 1, 2, 1, 30, 20, 1);
                this.content.steps[6] = new Step(7, 2, 1, 1, 20, 5, 1);
                this.content.steps[7] = new Step(8, 1, 3, 2, 20, 0, 0);
                break;
            case 5:
                this.content = new Content(dataId, 1);
                this.content.steps[0] = new Step(1, 1, 1, 1, 0, 0, 2); // Xuat hien
                this.content.steps[1] = new Step(2, 1, 2, 1, 30, 70, 1);
                this.content.steps[2] = new Step(3, 2, 1, 1, 15, 85, 1);
                this.content.steps[3] = new Step(4, 1, 2, 1, 20, 50, 1);
                this.content.steps[4] = new Step(5, 2, 3, 2, 50, 35, 2);
                this.content.steps[5] = new Step(6, 1, 1, 1, 20, 30, 1);
                this.content.steps[6] = new Step(7, 2, 1, 1, 30, 5, 2);
                this.content.steps[7] = new Step(8, 1, 1, 2, 30, 0, 0);
                break;
            case 6:
                this.content = new Content(dataId, 1);
                this.content.steps[0] = new Step(1, 1, 1, 1, 0, 0, 2); // Xuat hien
                this.content.steps[1] = new Step(2, 2, 2, 1, 30, 70, 2);
                this.content.steps[2] = new Step(3, 1, 2, 1, 15, 85, 1);
                this.content.steps[3] = new Step(4, 2, 3, 2, 50, 20, 2);
                this.content.steps[4] = new Step(5, 1, 2, 4, 25, 60, 1);
                this.content.steps[5] = new Step(6, 1, 3, 2, 60, 0, 0);
                break;
            case 7:
                this.content = new Content(dataId, 1);
                this.content.steps[0] = new Step(1, 1, 1, 1, 0, 0, 2); // Xuat hien
                this.content.steps[1] = new Step(2, 2, 1, 1, 20, 80, 1);
                this.content.steps[2] = new Step(3, 1, 2, 1, 30, 70, 1);
                this.content.steps[3] = new Step(4, 2, 3, 2, 50, 30, 2);
                this.content.steps[4] = new Step(5, 1, 2, 1, 30, 40, 2);
                this.content.steps[5] = new Step(6, 2, 1, 3, 0, 30, 0);
                this.content.steps[6] = new Step(7, 1, 3, 2, 40, 0, 2);
                break;
            case 8:
                this.content = new Content(dataId, 1);
                this.content.steps[0] = new Step(1, 1, 1, 1, 0, 0, 2); // Xuat hien
                this.content.steps[1] = new Step(2, 2, 3, 2, 40, 60, 2);
                this.content.steps[2] = new Step(3, 1, 2, 1, 15, 85, 1);
                this.content.steps[3] = new Step(4, 2, 1, 2, 55, 5, 1);
                this.content.steps[4] = new Step(5, 1, 1, 1, 35, 50, 2);
                this.content.steps[5] = new Step(6, 2, 1, 3, 0, 30, 0);
                this.content.steps[6] = new Step(7, 1, 3, 2, 50, 0, 0);
                break;
            case 9:
                this.content = new Content(dataId, 1);
                this.content.steps[0] = new Step(1, 1, 1, 1, 0, 0, 2); // Xuat hien
                this.content.steps[1] = new Step(2, 1, 2, 1, 20, 80, 1);
                this.content.steps[2] = new Step(3, 2, 2, 1, 30, 70, 1);
                this.content.steps[3] = new Step(4, 1, 1, 1, 30, 50, 1);
                this.content.steps[4] = new Step(5, 2, 3, 1, 40, 30, 2);
                this.content.steps[5] = new Step(6, 1, 2, 4, 20, 30, 1);
                this.content.steps[6] = new Step(7, 1, 1, 2, 30, 0, 0);
                break;
            case 10:
                this.content = new Content(dataId, 1);
                this.content.steps[0] = new Step(1, 1, 1, 1, 0, 0, 2); // Xuat hien
                this.content.steps[1] = new Step(2, 1, 2, 1, 20, 80, 1);
                this.content.steps[2] = new Step(3, 2, 3, 1, 30, 70, 1);
                this.content.steps[3] = new Step(4, 1, 2, 1, 30, 50, 1);
                this.content.steps[4] = new Step(5, 2, 2, 1, 40, 30, 2);
                this.content.steps[5] = new Step(6, 1, 1, 1, 30, 20, 2);
                this.content.steps[6] = new Step(7, 2, 1, 1, 25, 5, 1);
                this.content.steps[7] = new Step(8, 1, 2, 1, 20, 0, 0);
                break;
            case 11:
                this.content = new Content(dataId, 1);
                this.content.steps[0] = new Step(1, 1, 1, 1, 0, 0, 2); // Xuat hien
                this.content.steps[1] = new Step(2, 1, 1, 1, 30, 70, 2);
                this.content.steps[2] = new Step(3, 2, 2, 1, 15, 85, 1);
                this.content.steps[3] = new Step(4, 1, 2, 1, 20, 50, 1);
                this.content.steps[4] = new Step(5, 2, 2, 1, 50, 35, 2);
                this.content.steps[5] = new Step(6, 1, 2, 1, 10, 40, 1);
                this.content.steps[6] = new Step(7, 2, 1, 1, 35, 0, 1);
                //this.content.steps[7] = new Step(8, 1, 2, 1, 20, 0, 0); //CAMERA
                break;
            case 12:
                this.content = new Content(dataId, 1);
                this.content.steps[0] = new Step(1, 1, 1, 1, 0, 0, 2); // Xuat hien
                this.content.steps[1] = new Step(2, 1, 1, 2, 50, 50, 2);
                this.content.steps[2] = new Step(3, 2, 2, 1, 30, 70, 2);
                this.content.steps[3] = new Step(4, 1, 2, 1, 20, 30, 1);
                this.content.steps[4] = new Step(5, 2, 1, 2, 50, 20, 2);
                this.content.steps[5] = new Step(6, 1, 2, 1, 25, 5, 1);
                this.content.steps[6] = new Step(7, 2, 3, 1, 20, 0, 1);
                //this.content.steps[7] = new Step(8, 1, 2, 1, 20, 0, 0); //CAMERA
                break;
            case 13:
                this.content = new Content(dataId, 1);
                this.content.steps[0] = new Step(1, 1, 1, 1, 0, 0, 2); // Xuat hien
                this.content.steps[1] = new Step(2, 2, 1, 1, 20, 80, 1);
                this.content.steps[2] = new Step(3, 1, 2, 1, 30, 70, 1);
                this.content.steps[3] = new Step(4, 2, 3, 2, 50, 30, 2);
                this.content.steps[4] = new Step(5, 1, 2, 2, 60, 10, 2);
                this.content.steps[5] = new Step(6, 2, 2, 1, 30, 0, 1);
                //this.content.steps[7] = new Step(8, 1, 2, 1, 20, 0, 0); //CAMERA
                break;
            case 14:
                this.content = new Content(dataId, 1);
                this.content.steps[0] = new Step(1, 1, 1, 1, 0, 0, 2); // Xuat hien
                this.content.steps[1] = new Step(2, 1, 2, 1, 20, 80, 1);
                this.content.steps[2] = new Step(3, 2, 3, 1, 20, 80, 2);
                this.content.steps[3] = new Step(4, 1, 2, 1, 30, 50, 1);
                this.content.steps[4] = new Step(5, 2, 3, 2, 50, 30, 2);
                this.content.steps[5] = new Step(6, 1, 2, 1, 20, 30, 1);
                this.content.steps[6] = new Step(7, 2, 1, 1, 25, 5, 1);
                this.content.steps[7] = new Step(8, 1, 1, 1, 25, 5, 1);
                this.content.steps[8] = new Step(9, 2, 2, 1, 5, 0, 1);
                //this.content.steps[7] = new Step(8, 1, 2, 1, 20, 0, 0); //CAMERA
                break;
            case 15:
                this.content = new Content(dataId, 1);
                this.content.steps[0] = new Step(1, 1, 1, 1, 0, 0, 2); // Xuat hien
                this.content.steps[1] = new Step(2, 1, 1, 1, 30, 70, 1);
                this.content.steps[2] = new Step(3, 2, 2, 1, 30, 70, 1);
                this.content.steps[3] = new Step(4, 1, 2, 1, 20, 50, 1);
                this.content.steps[4] = new Step(5, 2, 2, 2, 50, 20, 2);
                this.content.steps[5] = new Step(6, 1, 1, 2, 40, 10, 1);
                this.content.steps[6] = new Step(7, 2, 1, 1, 20, 0, 1);
                //this.content.steps[7] = new Step(8, 1, 2, 1, 20, 0, 0); //CAMERA
                break;
            case 16:
                this.content = new Content(dataId, 1);
                this.content.steps[0] = new Step(1, 1, 1, 1, 0, 0, 2); // Xuat hien
                this.content.steps[1] = new Step(2, 2, 2, 1, 30, 70, 1);
                this.content.steps[2] = new Step(3, 1, 2, 1, 30, 70, 1);
                this.content.steps[3] = new Step(4, 2, 3, 2, 50, 30, 2);
                this.content.steps[4] = new Step(5, 1, 2, 2, 50, 20, 2);
                this.content.steps[5] = new Step(6, 2, 2, 4, 15, 5, 1);
                this.content.steps[6] = new Step(7, 2, 1, 1, 5, 0, 1);
                //this.content.steps[7] = new Step(8, 1, 2, 1, 20, 0, 0); //CAMERA
                break;
            case 17:
                this.content = new Content(dataId, 1);
                this.content.steps[0] = new Step(1, 1, 1, 1, 0, 0, 2); // Xuat hien
                this.content.steps[1] = new Step(2, 2, 3, 2, 50, 50, 2);
                this.content.steps[2] = new Step(3, 1, 2, 1, 20, 80, 1);
                this.content.steps[3] = new Step(4, 2, 1, 2, 40, 10, 2);
                this.content.steps[4] = new Step(5, 1, 1, 2, 50, 30, 2);
                this.content.steps[5] = new Step(6, 2, 1, 3, 0, 10, 0);
                this.content.steps[6] = new Step(7, 1, 2, 3, 0, 30, 0);
                this.content.steps[7] = new Step(8, 2, 2, 1, 10, 0, 0);
                //this.content.steps[7] = new Step(8, 1, 2, 1, 20, 0, 0); //CAMERA
                break;
            case 18:
                this.content = new Content(dataId, 1);
                this.content.steps[0] = new Step(1, 1, 1, 1, 0, 0, 2); // Xuat hien
                this.content.steps[1] = new Step(2, 2, 3, 2, 40, 60, 2);
                this.content.steps[2] = new Step(3, 1, 2, 1, 15, 85, 1);
                this.content.steps[3] = new Step(4, 2, 1, 2, 55, 5, 2);
                this.content.steps[4] = new Step(5, 1, 1, 1, 35, 50, 2);
                this.content.steps[5] = new Step(6, 2, 1, 3, 0, 5, 0);
                this.content.steps[6] = new Step(7, 1, 2, 1, 30, 20, 1);
                this.content.steps[7] = new Step(8, 2, 2, 1, 5, 0, 0);
                //this.content.steps[7] = new Step(8, 1, 2, 1, 20, 0, 0); //CAMERA
                break;
            case 19:
                this.content = new Content(dataId, 1);
                this.content.steps[0] = new Step(1, 1, 1, 1, 0, 0, 2); // Xuat hien
                this.content.steps[1] = new Step(2, 1, 2, 1, 20, 80, 1);
                this.content.steps[2] = new Step(3, 2, 2, 1, 30, 70, 1);
                this.content.steps[3] = new Step(4, 1, 1, 1, 30, 50, 1);
                this.content.steps[4] = new Step(5, 2, 3, 4, 40, 30, 2);
                this.content.steps[5] = new Step(6, 2, 1, 1, 30, 0, 1);
                //this.content.steps[7] = new Step(8, 1, 2, 1, 20, 0, 0); //CAMERA
                break;
            case 20:
                this.content = new Content(dataId, 1);
                this.content.steps[0] = new Step(1, 1, 1, 1, 0, 0, 2); // Xuat hien
                this.content.steps[1] = new Step(2, 1, 2, 1, 20, 80, 1);
                this.content.steps[2] = new Step(3, 2, 1, 2, 50, 50, 2);
                this.content.steps[3] = new Step(4, 1, 2, 1, 30, 50, 1);
                this.content.steps[4] = new Step(5, 2, 2, 3, 0, 50, 0);
                this.content.steps[5] = new Step(6, 1, 1, 1, 30, 20, 1);
                this.content.steps[6] = new Step(7, 2, 3, 2, 50, 0, 1);
                //this.content.steps[7] = new Step(8, 1, 2, 1, 20, 0, 0); //CAMERA
                break;
        }
    }

    getCurrentAttackId(): number {
        return this.content.steps[GameplayController.getInstance().currentStepIndex].attackId;
    }
    getCurrentAttackType(): number {
        return this.content.steps[GameplayController.getInstance().currentStepIndex].attackType;
    }
    getCurrentAttackEffect(): number {
        return this.content.steps[GameplayController.getInstance().currentStepIndex].attackEffectStatus;
    }
    getCurrentAttackDamage(): number {
        return this.content.steps[GameplayController.getInstance().currentStepIndex].damage;
    }
    getCurrentAttackHealthLeft(): number {
        return this.content.steps[GameplayController.getInstance().currentStepIndex].healthLeft;
    }
    getCurrentAttackFallback(): number {
        return this.content.steps[GameplayController.getInstance().currentStepIndex].fallbackDistance;
    }
    getStepCount(): number {
        return this.content.steps.length;
    }
}

class Content {
    contentId: number;//ID của kịch bản
    winnerId: number;//con nào chiến thắng. 1 là dino, 2 là monster
    steps: Step[] = [];
    constructor(_contentId: number, _winnerId: number,) {
        this.contentId = _contentId;
        this.winnerId = _winnerId;
    }
}

class Step {
    stepId: number;
    attackId: number;//1 là dino, 2 là monster
    attackType: number;//1 là đánh xa,2 là đánh gần, 3 là đánh kết thúc(chỉ dành cho Dino)
    attackEffectStatus: number;//1 là bình thường, 2 là chí mạng, 3 là miss, 4 là bị choáng (mất lượt tiếp theo)
    damage: number;//lượng máu mất đi
    healthLeft: number;//lượng máu còn lại
    fallbackDistance: number;//con bị đánh lùi lại bao nhiêu ô?

    constructor(_stepId: number, _attackId: number, _attackType: number, _attackEffectStatus: number, _damage: number, _healthLeft: number, _fallback: number) {
        this.stepId = _stepId;
        this.attackId = _attackId;
        this.attackType = _attackType;
        this.attackEffectStatus = _attackEffectStatus;
        this.damage = _damage;
        this.healthLeft = _healthLeft;
        this.fallbackDistance = _fallback;
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
