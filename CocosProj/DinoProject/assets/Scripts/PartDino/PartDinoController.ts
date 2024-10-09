
import { _decorator, Component, Node, SkinnedMeshRenderer, Material, SkeletalAnimationComponent, resources, log, randomRangeInt} from 'cc';
import { Common } from '../common/Common';
import { DinoBodyDefine, DinoData } from '../common/DinoData';
import { DinoController } from '../DinoController';
import { WBHeroController } from '../WorldBoss/WB_HeroController';
const { ccclass, property } = _decorator;

const playerAnim = {
    idleA: 'Idle',
    // attack: 'Dino_Attack',
    die: 'Die',
    // happy: 'Dino_Happy',
    Bite: 'Bite',
    HeadButt: 'HeadButt',
    HitBack: 'Hit',
    // HitBackFar: 'HitBack_Ver3',
    Shot: 'shot',
    Stun: 'Stun',
    // Run: 'Dino_Run',
    Jump: 'Jump',
    JumpBack: 'JumpBack',
    Evolve:"Evolve",
    JumpShot:"JumpShot",
    Shot2Times:"Shot2Times",
    Bitting2Times:"Bitting2Times",
}

@ccclass('PartDinoController')
export class PartDinoController extends Component {

    @property({ type: SkinnedMeshRenderer })
    public bodySkinnedMesh: SkinnedMeshRenderer;
    @property({ type: SkinnedMeshRenderer })
    public faceSkinnedMesh: SkinnedMeshRenderer;
    @property({ type: SkinnedMeshRenderer })
    public defaultBackSkinnedMesh: SkinnedMeshRenderer;
    @property({ type: SkinnedMeshRenderer })
    public defaultTeethSkinnedMesh: SkinnedMeshRenderer;
    @property({ type: SkeletalAnimationComponent })
    public playerAnimComp: SkeletalAnimationComponent = null;

    @property({ type: Node })    
    public wingNodeList: Node[] = [];
    @property({ type: Node })
    public teethNodeList: Node[] = [];
    @property({ type: Node })
    public backNodeList: Node[] = [];
    @property({ type: Node })
    public hornNodeList: Node[] = [];
    @property({ type: Node })
    public glassNode10: Node;
    
    @property({ type: Boolean })
    public randomDino: boolean = false;

    // @property({ type: Material })
    // public faceMaterialList:Material[] = [];

    // @property({ type: Material })
    // public bodyMaterialList:Material[] = [];

    public dinoController : DinoController = null;
    public wbHeroController : WBHeroController = null;

    public partNumber : number = 10;
    public bodyNumber : number = 12;
  
    // [1]
    // dummy = '';

    // [2]
    // @property
    // serializableDummy = 0;
    
    start () {
        this.dinoController = this.node.parent.getComponent(DinoController);
        this.wbHeroController = this.node.parent.getComponent(WBHeroController);
        if(this.randomDino)
        {
            let ran = randomRangeInt(0,2);
            if(ran == 0)
                this.randomNewDino();
            else
                this.randomOldDino();
        }
    }

    randomOldDino(){
        let rarity = randomRangeInt(1,5);
        let dino_class = randomRangeInt(1,3);
        this.loadDataVer1(dino_class,rarity);
    }

    randomNewDino(){
        let newDino = new DinoData();
        newDino.body = randomRangeInt(0,this.bodyNumber);
        newDino.wing = randomRangeInt(0,this.partNumber);
        newDino.face = randomRangeInt(0,this.partNumber);
        newDino.teeth = randomRangeInt(0,this.partNumber);
        newDino.back = randomRangeInt(0,this.partNumber);
        newDino.horn = randomRangeInt(0,this.partNumber);
        this.loadDinoData(newDino, 5, 1);
    }



    //#region Dino version 1
    loadClass(dino_class: any){
        this.loadDataVer1(dino_class,this.currentRare);
    }

    loadRare(dino_rarity: any){
        this.loadDataVer1(this.currentClass,dino_rarity);
    }
    currentClass:any;
    currentRare:any ;
    loadDataVer1ByGenes(genes: any){
        let dino_class = Number(genes.split(0,2)) % 10;
        let dino_rarity = Number(genes.split(2,2)) % 10;
        console.log(genes,dino_class,dino_rarity);
        this.loadDataVer1(dino_class, dino_rarity);
    }

    loadDataVer1(dino_class: any, dino_rarity: any){
        this.currentClass = dino_class;
        this.currentRare = dino_rarity;
        this.clearMesh();
        this.bodySkinnedMesh.node.active = true;
        this.faceSkinnedMesh.node.active = true;
        this.defaultBackSkinnedMesh.node.active = true;
        this.defaultTeethSkinnedMesh.node.active = true;
        this.getDinoV1Mat(dino_class, dino_rarity,(dinoMat)=>{
            this.bodySkinnedMesh.material = dinoMat;
            this.defaultBackSkinnedMesh.material = dinoMat;
            this.defaultTeethSkinnedMesh.material = dinoMat;
        });

        this.getDinoV1FaceMat(dino_class, dino_rarity,(faceMat)=>{
            this.faceSkinnedMesh.material = faceMat;
        });
    }
    getDinoV1Mat(dino_class: any, dino_rarity: any, callbacks: any): void {
        resources.load("MaterialDino/dino_"+dino_class+"_"+dino_rarity, Material, (err, mat) => {
            callbacks(mat);
        });
    }

    getDinoV1FaceMat(dino_class: any, dino_rarity: any, callbacks: any): void {
        resources.load("MaterialDino/dino_face_"+dino_class+"_"+dino_rarity, Material, (err, mat) => {
            callbacks(mat);
        });
    }
    //#endregion
    //#region Dino version 2
    public ShowPartDino(part:string, idx:number){
        this.bodySkinnedMesh.node.active = false;
        this.wingNodeList.forEach(element => {
            element.active = false;  
        });
        this.faceSkinnedMesh.node.active = false;
        this.teethNodeList.forEach(element => {
            element.active = false;  
        });
        this.backNodeList.forEach(element => {
            element.active = false;  
        });
        this.hornNodeList.forEach(element => {
            element.active = false;  
        });
        if(part == "body"){
            this.bodySkinnedMesh.node.active = true;
            this.loadBoby(idx);
        }else       
        if(part == "face"){
            this.faceSkinnedMesh.node.active = true;
            this.loadFace(idx);
        }else       
        if(part == "wing"){
            this.loadWingByIdx(idx);
        }else       
        if(part == "teeth"){
            this.loadTeeth(idx);
        }else       
        if(part == "back"){
            this.loadBack(idx);
        }else        
        if(part == "horn"){
            this.loadHorn(idx);
        }
    }
    loadDataByTrait(expressTraits:any, rarity: number, nftId: number)
    {    
        let expTraits = (expressTraits);
        let data:DinoData = new DinoData();
        data.wing = expTraits["Wing"];
        data.back = expTraits["Back"];
        //data.body =    expressTraits["Texture\"];
        data.face = expTraits["Eye"];
        data.horn = expTraits["Horn"];
        data.teeth = expTraits["Teeth"];
        let bodyClass = DinoBodyDefine.BodyArray[expTraits["Class"]];
        data.body = bodyClass[expTraits["Texture"]%bodyClass.length];
        if(nftId == 5813)
        {
            data.wing = 7;
            data.back = 7;
            data.body = 0;
            data.face = 6;
            data.horn = 7;
            data.teeth = 0;
        }
        this.loadDinoData(data, rarity, nftId)
    }

    loadBoby(bodyId:number){
        this.getBodyMat(bodyId,(bodyMat) =>{
            this.bodySkinnedMesh.material = bodyMat;
        });
    }

    loadWing(wingId:number, rarity:number, nftId:number){
        this.wingNodeList.forEach(element => {
            element.active = false;  
        });
        if(nftId%2 === 0 && wingId >=4 ){
            wingId -= 4;
        }
        if(rarity >= 4 && wingId >=0 && wingId <this.partNumber)
        {
            this.loadWingByIdx(wingId);
        }
    }
    loadWingByIdx(wingId:number){
        this.getBodyMat(wingId,(wingMat) =>{
            this.wingNodeList[wingId].getComponents(SkinnedMeshRenderer).forEach(element => {
                element.material = wingMat;
            });
        });
        this.wingNodeList[wingId].active = true;  
    }

    loadFace(faceId:number){
        this.getFaceMat(faceId,(faceMat) =>{
            this.faceSkinnedMesh.material = faceMat;
        });
    }
    loadTeeth(teethId:number){
        this.teethNodeList.forEach(element => {
            element.active = false;  
        });
        if(teethId >=0 && teethId <this.partNumber)
        {
            this.getBodyMat(teethId,(teethMat) =>{
                this.teethNodeList[teethId].getComponent(SkinnedMeshRenderer).material = teethMat;
            });
            this.teethNodeList[teethId].active = true;
        }
    }
    loadBack(backId:number){
        this.backNodeList.forEach(element => {
            element.active = false;  
        });
        if(backId >=0 && backId <this.partNumber)
        {
            this.getBodyMat(backId,(backMat) =>{
                this.backNodeList[backId].getComponent(SkinnedMeshRenderer).material = backMat;
            });
            this.backNodeList[backId].active = true;
        }
    }
    loadHorn(hornId:number){
        this.hornNodeList.forEach(element => {
            element.active = false;  
        });
        this.glassNode10.active = false;

        if(hornId >=0 && hornId <this.partNumber)
        {
            this.glassNode10.active = (hornId == 9);
            this.getBodyMat(hornId,(hornMat) =>{
                this.hornNodeList[hornId].getComponent(SkinnedMeshRenderer).material = hornMat;
            });
            this.hornNodeList[hornId].active = true;
        }
    }
    loadDinoData(data:DinoData, rarity:number, nftId:number){
        this.loadData(data.body,data.face,data.wing,data.teeth,data.back,data.horn, rarity, nftId);
    }
    loadData(bodyId:number,faceId:number,wingId:number,teethId:number,backId:number,hornId:number, rarity: number, nftId:number)
    {
        this.getFaceMat(faceId,(faceMat) =>{
            this.faceSkinnedMesh.material = faceMat;
        });
        this.getBodyMat(bodyId,(bodyMat) =>{
            this.bodySkinnedMesh.material = bodyMat;
        });

        this.loadWing(wingId, rarity, nftId);
        this.loadTeeth(teethId);
        this.loadBack(backId);
        this.loadHorn(hornId);
    }

    getBodyMat(id: number, callbacks: any): void {
        resources.load("DinoPart/Body/part_dino_body_" + (id+1), Material, (err, mat) => {
            callbacks(mat);
        });
    }

    getFaceMat(id: number, callbacks: any): void {
        resources.load("DinoPart/Face/part_dino_face_" + (id+1), Material, (err, mat) => {
            callbacks(mat);
        });
    }

    //#endregion
    clearMesh()
    {
        this.wingNodeList.forEach(element => {
            element.active = false;  
        });
        this.teethNodeList.forEach(element => {
            element.active = false;  
        });
        this.backNodeList.forEach(element => {
            element.active = false;  
        });
        this.hornNodeList.forEach(element => {
            element.active = false;  
        });
        this.defaultBackSkinnedMesh.node.active = false;
        this.defaultTeethSkinnedMesh.node.active = false;
    }
    //#region Dino Animation
    OnPlayAnimDie() {
        if(this.playerAnimComp != null)
            this.playerAnimComp.play(playerAnim.die);
    }


    OnPlayAnimIdle() {
        if(this.playerAnimComp != null)
            this.playerAnimComp.play(playerAnim.idleA);
    }

    OnPlayAnimBite() {
        if(this.playerAnimComp != null)
            this.playerAnimComp.play(playerAnim.Bite);
    }

    OnPlayAnimShot() {
        if(this.playerAnimComp != null)
            this.playerAnimComp.play(playerAnim.Shot);
    }

    OnPlayAnimHeadButt() {
        if(this.playerAnimComp != null)
            this.playerAnimComp.play(playerAnim.HeadButt);
    }

    OnPlayAnimHitBack() {
        if(this.playerAnimComp != null)
            this.playerAnimComp.play(playerAnim.HitBack);
    }

    OnPlayAnimStun() {
        if(this.playerAnimComp != null)
            this.playerAnimComp.play(playerAnim.Stun);
    }

    OnPlayAnimJump() {
        if(this.playerAnimComp != null)
            this.playerAnimComp.play(playerAnim.Jump);
    }

    OnPlayAnimJumpBack() {
        if(this.playerAnimComp != null)
            this.playerAnimComp.play(playerAnim.JumpBack);
    }

    OnPlayAnimEvolve() {
        if(this.playerAnimComp != null)
            this.playerAnimComp.play(playerAnim.Evolve);
    }
    //#endregion

    //#region DINO CONTROLLER ANIMATION CALLBACK
    OnGoToIdleState()
    {
        if(this.dinoController != null)
        {
            this.dinoController.OnPlayAnimIdle();
        }
    }
    //#endregion

    //#region WB HERO CONTROLLER ANIMATION CALLBACK
    OnSpawnEnd()
    {
        if(this.playerAnimComp != null)
            this.playerAnimComp.play("HatchingIdle");
    }
    

    //#endregion

    //#region WB HATCHING

    

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
