
import { _decorator, Component, Node, Quat, randomRangeInt, Toggle, ToggleComponent, director } from 'cc';
import { DinoBodyDefine, DinoData, OldDinoData } from '../common/DinoData';
import DropDown from '../DropDown/DropDown';
import { PartDinoController } from '../PartDino/PartDinoController';
const { ccclass, property } = _decorator;


@ccclass('DemoManager')
export class DemoManager extends Component {
    private static singleton: DemoManager;
    public static getInstance(): DemoManager {
        return DemoManager.singleton;
    }
    @property({ type: ToggleComponent })
    public toogleRotate: Toggle;

    @property({ type: DropDown })
    public dropDownPart: DropDown;

    @property({ type: DropDown })
    public dropDownBody: DropDown;
    @property({ type: DropDown })
    public dropDownWing: DropDown;
    @property({ type: DropDown })
    public dropDownFace: DropDown;
    @property({ type: DropDown })
    public dropDownTeeth: DropDown;
    @property({ type: DropDown })
    public dropDownBack: DropDown;
    @property({ type: DropDown })
    public dropDownHorn: DropDown;


    @property({ type: DropDown })
    public dropDownClass: DropDown;
    @property({ type: DropDown })
    public dropDownRare: DropDown;
    @property({ type: Node })
    public newDinoUI: Node;
    @property({ type: Node })
    public oldDinoUI: Node;

    @property({ type: PartDinoController })
    public dino: PartDinoController;
    
    public autoRotate:boolean = true;
    // private newDino:DinoData;
    private oldDino:OldDinoData;
    start () {
        DemoManager.singleton = this;
        
        var wingNames = new Array();
        var faceNames = new Array();
        for (let index = 0; index < 10; index++) {
            let id = index + 1;
            wingNames[index] = 'wing_' + id;
            faceNames[index] = 'face_' + id;
        }

        this.dropDownWing.addOptionStrings(wingNames);
        this.dropDownFace.addOptionStrings(faceNames);
        this.newDinoUI.active = true;
        this.oldDinoUI.active = false;
    }

    // randomDino(){
    //     this.newDino = new DinoData();
    //     this.newDino.body = randomRangeInt(0,this.dino.bodyNumber);
    //     this.newDino.wing = randomRangeInt(0,this.dino.partNumber);
    //     this.newDino.face = randomRangeInt(0,this.dino.partNumber);
    //     this.newDino.teeth = randomRangeInt(0,this.dino.partNumber);
    //     this.newDino.back = randomRangeInt(0,this.dino.partNumber);
    //     this.newDino.horn = randomRangeInt(0,this.dino.partNumber);
     
    //     this.dino.loadDinoData(this.newDino , 5, 1);
    //     this.dropDownBody.selectedIndex =  this.newDino.body;
    //     this.dropDownWing.selectedIndex =  this.newDino.wing;
    //     this.dropDownFace.selectedIndex =  this.newDino.face;
    //     this.dropDownTeeth.selectedIndex =  this.newDino.teeth;
    //     this.dropDownBack.selectedIndex =  this.newDino.back;
    //     this.dropDownHorn.selectedIndex =  this.newDino.horn;
    //   //  loadData(bodyId:number,wingId:number,faceId:number,teethId:number,backId:number,hornId:number)
    // }

    randomOldDino(){
        // this.oldDino = new OldDinoData();
        // this.oldDino.rarity = randomRangeInt(1,5);
        // this.oldDino.dino_class = randomRangeInt(1,3);
        // this.dino.loadDataVer1(this.oldDino.dino_class,this.oldDino.rarity);
        // this.dropDownClass.selectedIndex =  this.oldDino.dino_class;
        // this.dropDownRare.selectedIndex =  this.oldDino.rarity;
      //  loadData(bodyId:number,wingId:number,faceId:number,teethId:number,backId:number,hornId:number)
    }

    OnClickPrepareEvolve(){
        // this.newDinoUI.active = false;
        // this.oldDinoUI.active = true;
        // this.randomOldDino();
    }

    OnClickStartEvolve(){
        // this.oldDinoUI.active = false;
        // localStorage.setItem("newDino", JSON.stringify( this.newDino));
        // localStorage.setItem("oldDino", JSON.stringify( this.oldDino));
        // director.preloadScene("EvolveScene", function (err, scene) {
        //     // if(err)
        //         director.loadScene("EvolveScene");
        //     // else
        //     //     director.runScene(scene);
        // });
    }

    hideOtherDropdown(){
        this.dropDownBody.node.active = false;
        this.dropDownWing.node.active = false;
        this.dropDownFace.node.active = false;
        this.dropDownTeeth.node.active = false;
        this.dropDownBack.node.active = false;
        this.dropDownHorn.node.active = false;
        this.dropDownClass.node.active = false;
        this.dropDownRare.node.active = false;
    }
    onPartSeclect(id:number){
        this.hideOtherDropdown();
        let part = this.dropDownPart.getOptionString(id);
        if(part == "body"){
            this.dropDownClass.node.active = true;
        }else       
        if(part == "face"){
            this.dropDownFace.node.active = true;
        }else       
        if(part == "wing"){
            this.dropDownWing.node.active = true;
        }else       
        if(part == "teeth"){
            this.dropDownRare.node.active = true;
        }else       
        if(part == "back"){
            this.dropDownRare.node.active = true;
        }else        
        if(part == "horn"){
            this.dropDownRare.node.active = true;
        }
    }

    public onRaritySeclected(id:number){
        let part = this.dropDownPart.getSelectedString();


        let Normal =[0,1,2];
        let Rare =[3,4,5];
        let Epic =[6,7];
        let Legendary = [8];
        let Super = [9];
        let rarityArr = [Normal,Rare,Epic,Legendary,Super];
        let dropDown:DropDown;
        let names = new Array();
        if(part == "teeth"){
            this.dropDownTeeth.clearOptionDatas();
            names = rarityArr[id].map(x => 'teeth_' + x);
            this.dropDownTeeth.optionDatas = new Array();
            this.dropDownTeeth.addOptionStrings(names);
            this.dropDownTeeth.node.active = true;
        }else       
        if(part == "back"){
            this.dropDownBack.clearOptionDatas();
            names = rarityArr[id].map(x => 'back_' + x);
            this.dropDownBack.optionDatas = new Array();
            this.dropDownBack.addOptionStrings(names);
            this.dropDownBack.node.active = true;
        }else        
        if(part == "horn"){
            this.dropDownHorn.clearOptionDatas();
            names = rarityArr[id].map(x => 'horn_' + x);
            this.dropDownHorn.optionDatas = new Array();
            this.dropDownHorn.addOptionStrings(names);
            this.dropDownHorn.node.active = true;
        }


    }

    public onClassSelected(id:number){
        this.dropDownBody.clearOptionDatas();
        let bodyNames: string[] = DinoBodyDefine.BodyArray[id].map(x => 'body_' + x);
        this.dropDownBody.node.active = false;
        this.dropDownBody.optionDatas = new Array();
        this.dropDownBody.addOptionStrings(bodyNames);
        this.dropDownBody.node.active = true;
    }

    checkBox(){
        this.autoRotate = this.toogleRotate.isChecked;
    }

    loadClass(dinoClass:number){
        this.oldDino.dino_class = dinoClass + 1;
        this.dino.loadClass(this.oldDino.dino_class);
    }

    loadRare(rare:number){
        this.oldDino.rarity = rare + 1;
        this.dino.loadRare(this.oldDino.rarity);
    }

    loadBoby(bodyId:number){
        // this.newDino.body = bodyId;
        let bodyIdx = Number(this.dropDownBody.getOptionString(bodyId).replace('body_',''))
        this.dino.ShowPartDino(this.dropDownPart.getSelectedString(), bodyIdx);
    }
    loadWing(wingId:number){
        // this.newDino.wing = wingId;
        let idx = Number(this.dropDownWing.getOptionString(wingId).replace('wing_',''))
        
        this.dino.ShowPartDino(this.dropDownPart.getSelectedString(), idx);
        // this.dino.loadWing(wingId, 5, 1);
    }
    loadFace(faceId:number){
        // this.newDino.face = faceId;
        let idx = Number(this.dropDownFace.getOptionString(faceId).replace('face_',''))
        this.dino.ShowPartDino(this.dropDownPart.getSelectedString(), idx);
        // this.dino.loadFace(faceId);
    }
    loadTeeth(teethId:number){
        // this.newDino.teeth = teethId;
        let idx = Number(this.dropDownTeeth.getOptionString(teethId).replace('teeth_',''))
        this.dino.ShowPartDino(this.dropDownPart.getSelectedString(), idx);
        // this.dino.loadTeeth(teethId);
    }
    loadBack(backId:number){
        // this.newDino.back = backId;
        let idx = Number(this.dropDownBack.getOptionString(backId).replace('back_',''))
        this.dino.ShowPartDino(this.dropDownPart.getSelectedString(), idx);
        // this.dino.loadBack(backId);
    }
    loadHorn(hornId:number){
        // this.newDino.horn = hornId;
        let idx = Number(this.dropDownHorn.getOptionString(hornId).replace('horn_',''))
        this.dino.ShowPartDino(this.dropDownPart.getSelectedString(), idx);
        // this.dino.loadHorn(hornId);
    }

    OnPlayAnimDie() {
        this.dino.OnPlayAnimDie();
    }

    OnPlayAnimIdle() {
        this.dino.OnPlayAnimIdle();
    }

    OnPlayAnimBite() {
        this.dino.OnPlayAnimBite();
    }

    OnPlayAnimShot() {
        this.dino.OnPlayAnimShot();
    }

    OnPlayAnimHeadButt() {
        this.dino.OnPlayAnimHeadButt();
    }

    OnPlayAnimHitBack() {
        this.dino.OnPlayAnimHitBack();
    }

    OnPlayAnimStun() {
        this.dino.OnPlayAnimStun();
    }

    OnPlayAnimJump() {
        this.dino.OnPlayAnimJump();
    }

    OnPlayAnimJumpBack() {
        this.dino.OnPlayAnimJumpBack();
    }

    private _temp_quat: Quat = new Quat();
    update (deltaTime: number) {
        if(this.autoRotate){
            Quat.fromEuler(this._temp_quat,0, -50 * deltaTime, 0);
            this.dino.node.rotate(this._temp_quat);
        }
        // [4]
    }

    //#region 

    //#endregion
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
