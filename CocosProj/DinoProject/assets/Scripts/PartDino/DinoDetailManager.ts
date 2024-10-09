
import { _decorator, Component, Node, SpriteComponent, Sprite, LabelComponent, Label, CompactValueTypeArray, Quat, Vec3, instantiate, ScrollViewComponent, ScrollView, ButtonComponent, Button } from 'cc';
import { Common } from '../common/Common';
import { DinoBodyDefine } from '../common/DinoData';
import { MidasCard } from './MidasCard';
import { PartDinoController } from './PartDinoController';
const { ccclass, property } = _decorator;

@ccclass('DinoDetailManager')
export class DinoDetailManager extends Component {
    private static singleton: DinoDetailManager;
    public static getInstance(): DinoDetailManager {
        return DinoDetailManager.singleton;
    }


    // [1]
    // dummy = '';
    @property({ type: Boolean })
    public isDinoDetailInPVE: boolean = false;
    @property({ type: PartDinoController })
    public partDino: PartDinoController;
    @property({ type: SpriteComponent })
    public rarityBG: Sprite;
    @property({ type: SpriteComponent })
    public rarityFrame: Sprite;

    @property({ type: PartDinoController })
    public momDino: PartDinoController;
    @property({ type: PartDinoController })
    public dadDino: PartDinoController;
    @property({ type: SpriteComponent })
    public momRarityBG: Sprite;
    @property({ type: SpriteComponent })
    public dadRarityBG: Sprite;
    @property({ type: Node })
    public momUI: Node;
    @property({ type: Node })
    public dadUI: Node;
    @property({ type: Node })
    public partUI: Node;
    @property({ type: Node })
    public envolve: Node;
    @property({ type: Node })
    public envolveDNLBtn: Node;
    @property({ type: Node })
    public envolveBookBtn: Node;
    @property({ type: LabelComponent })
    public envolveDNLFeeLbl: Label;
    @property({ type: LabelComponent })
    public totalBookLbl: Label;
    @property({ type: LabelComponent })
    public envolveBookFeeLbl: Label;

    @property({ type: LabelComponent })
    public totalKnowledgeBookLbl: Label;
    @property({ type: Node })
    public learningSkill: Node;
    @property({ type: ButtonComponent })
    public learningSkillBtn: Button;

    ////UI
    @property({ type: Node })
    public rightScrollView: Node;
    @property({ type: Node })
    public syncingData: Node;

    @property({ type: LabelComponent })
    public nameLbl: Label;
    @property({ type: LabelComponent })
    public idLbl: Label;
    @property({ type: LabelComponent })
    public generationLbl: Label;
    @property({ type: LabelComponent })
    public midasRangeLbl: Label;
    @property({ type: LabelComponent })
    public classLbl: Label;
    @property({ type: SpriteComponent })
    public classIcon: Sprite;
    @property({ type: LabelComponent })
    public rarityLbl: Label;
    @property({ type: SpriteComponent })
    public rarityIcon: Sprite;
    @property({ type: LabelComponent })
    public genderLbl: Label;
    @property({ type: SpriteComponent })
    public genderIcon: Sprite;
    @property({ type: LabelComponent })
    public momIdLbl: Label;
    @property({ type: LabelComponent })
    public dadIdLbl: Label;
    @property({ type: LabelComponent })
    public levelLbl: Label;
    @property({ type: LabelComponent })
    public expLbl: Label;
    @property({ type: LabelComponent })
    public ageLbl: Label;
    @property({ type: LabelComponent })
    public breedLbl: Label;
    @property({ type: LabelComponent })
    public hpLbl: Label;
    @property({ type: LabelComponent })
    public atkLbl: Label;
    @property({ type: LabelComponent })
    public defenseLbl: Label;
    @property({ type: LabelComponent })
    public speedLbl: Label;

    @property({ type: LabelComponent })
    public wingNameLbl: Label;
    @property({ type: LabelComponent })
    public wingRarityLbl: Label;
    @property({ type: LabelComponent })
    public hornNameLbl: Label;
    @property({ type: LabelComponent })
    public hornRarityLbl: Label;
    @property({ type: LabelComponent })
    public plateNameLbl: Label;
    @property({ type: LabelComponent })
    public plateRarityLbl: Label;
    @property({ type: LabelComponent })
    public teethNameLbl: Label;
    @property({ type: LabelComponent })
    public teethRarityLbl: Label;
    @property({ type: LabelComponent })
    public eyeNameLbl: Label;
    @property({ type: LabelComponent })
    public bodyNameLbl: Label;

    @property({ type: LabelComponent })
    public skillTitle: Label;
    @property({ type: LabelComponent })
    public skillRarityLbl: Label;
    @property({ type: LabelComponent })
    public skillDescription: Label;
    @property({ type: Node })
    public skill: Node;
    @property({ type: SpriteComponent })
    public skillIcon: Sprite;
    @property({ type: SpriteComponent })
    public skillRarityIcon: Sprite;


    @property({ type: LabelComponent })
    public priceLbl: Label;
    @property({ type: Node })
    public buyNode: Node;
    @property({ type: SpriteComponent })
    public coinIcon: Sprite;


    @property({ type: LabelComponent })
    public sellPriceLbl: Label;
    @property({ type: Node })
    public cancelNode: Node;
    @property({ type: SpriteComponent })
    public coinSellIcon: Sprite;

    @property({ type: Node })
    public midasPopup: Node;
    @property({ type: Node })
    public midasDetail: Node;
    @property({ type: ScrollViewComponent })
    public midasScrollView: ScrollView;
    @property({ type: MidasCard })
    public midasCardList: MidasCard[] = [];
    @property({ type: Node })
    public midasNodata: Node;
    @property({ type: Node })
    public leftDetailPanel: Node;
    // [2]
    // @property
    // serializableDummy = 0;
    private dinoData:any;
    start () 
    {
        DinoDetailManager.singleton = this;
        if(this.isDinoDetailInPVE)
            return;

        this.Initialize(Common.currentDinoData);
        if(window.getDetailStatus){
            window.getDetailStatus(this.dinoData["nftId"]);
        }

        window.getDetailStatusCallback = function(isEvolve, currentMidas, midasList, totalKnowledgebook, bookToLearningSkill, canLearingSkill){
            console.log("getDetailStatusCallback");
            console.log(midasList);
            let manager = DinoDetailManager.getInstance();
            let sceneData = Common.currentLoadScene;
            manager.onGetDetailStatus(isEvolve, currentMidas, midasList);
            manager.totalKnowledgeBookLbl.string = bookToLearningSkill + "/" + totalKnowledgebook;
            sceneData === "mydino-onsale" ? manager.learningSkill.active = false : manager.learningSkill.active = canLearingSkill;
            //manager.learningSkillBtn.interactable = bookToLearningSkill <= totalKnowledgebook;
        }
        
        window.leaningSkillCallback = function(isSuccess, totalKnowledgebook, bookToLearningSkill, canLearingSkill, dinoData){
            console.log("leaningSkillCallback");
            console.log(dinoData);
            if(isSuccess){
                for(var i=0; i<Common.currentData.length; i++){
                    if(Common.currentData[i]["nftId"]==dinoData["nftId"]){
                        Common.currentData[i] = dinoData;
                    }
                }
                let manager = DinoDetailManager.getInstance();
                manager.Initialize(dinoData);
                manager.totalKnowledgeBookLbl.string = bookToLearningSkill + "/" + totalKnowledgebook;
                manager.learningSkill.active = canLearingSkill;
                //manager.learningSkillBtn.interactable = bookToLearningSkill <= totalKnowledgebook;
            }
        }

        window.useMidasCallback = function(isSuccess, currentMidas, midasList){
            console.log("useMidasCallback");
            console.log(midasList);
            DinoDetailManager.getInstance().onUseMidasCallback(currentMidas, midasList);
        }
        window.onApproveEvolve = function(success:boolean){
            if(success){
                Common.isApprovedEvolve = true;
                DinoDetailManager.getInstance().envolveDNLFeeLbl.string = ""+Common.evolveDNLFee;
            }
        };
    }

    public Initialize(_dinoData:any):void{
        this.dinoData = _dinoData;
        

        this.loadUI();
        let dinoRarity =  this.dinoData["isEvolved"] ? this.dinoData["geneRarity"] : this.dinoData["rarity"];
        if(this.dinoData["isEvolved"]){
            if(!this.partDino)
                this.partDino = this.getComponentInChildren<PartDinoController>(PartDinoController);

            if(this.dinoData["mom"] && this.dinoData["dad"]){
                this.momUI.active = true;
                this.dadUI.active = true;
                this.momDino.node.active = true;
                this.dadDino.node.active = true;
                this.momRarityBG.node.active = true;
                this.dadRarityBG.node.active = true;
                Common.getRarityDinoBG(this.dinoData["mom"]["geneRarity"],(err, spr)=>{this.momRarityBG.spriteFrame = spr; });
                Common.getRarityDinoBG(this.dinoData["dad"]["geneRarity"],(err, spr)=>{this.dadRarityBG.spriteFrame = spr; });
                this.momDino.loadDataByTrait(this.dinoData["mom"]["expressTraits"],this.dinoData["mom"]["geneRarity"],this.dinoData["mom"]["nftId"]);
                this.dadDino.loadDataByTrait(this.dinoData["dad"]["expressTraits"],this.dinoData["dad"]["geneRarity"],this.dinoData["dad"]["nftId"]);
                this.momIdLbl.string = "#" + this.dinoData["mom"]["nftId"];
                this.dadIdLbl.string = "#" + this.dinoData["dad"]["nftId"];
            }else{
                this.momDino.node.active = false;
                this.dadDino.node.active = false;
                this.momRarityBG.node.active = false;
                this.dadRarityBG.node.active = false;
                this.momUI.active = false;
                this.dadUI.active = false;
            }
            this.partUI.active = true;
            this.loadPartUI(this.dinoData["expressTraits"]);
            this.partDino.loadDataByTrait(this.dinoData["expressTraits"],this.dinoData["geneRarity"],this.dinoData["nftId"]);
        }
        else
        {
            this.rarityBG.node.setPosition(Vec3.ZERO);
            this.partDino.loadDataVer1(this.dinoData["class"], this.dinoData["rarity"]);
            this.partUI.active = false;
            this.momDino.node.active = false;
            this.dadDino.node.active = false;
            this.momRarityBG.node.active = false;
            this.dadRarityBG.node.active = false;
            this.momUI.active = false;
            this.dadUI.active = false;
            this.hideMidasDetail();
        }
        
        
        let sceneData = Common.currentLoadScene;
        
        let isMyDino = false;
        if(this.dinoData["prevOwner"] && Common.walletAddress)
        isMyDino = ((""+this.dinoData["prevOwner"]).toLowerCase() === Common.walletAddress.toLowerCase());
        
        this.buyNode.active = sceneData ==="market" && !isMyDino;
        this.cancelNode.active = (sceneData ==="market" && isMyDino) || sceneData === "mydino-onsale";
        
        
        Common.getCoinIcon(+this.dinoData["cashType"], (err, spriteFrame) => { 
            this.coinIcon.spriteFrame = spriteFrame;
            this.coinSellIcon.spriteFrame = spriteFrame;
        });
        if(sceneData ==="market"){
            this.priceLbl.string = Common.numberWithCommas(+this.dinoData["endingPrice"]) + " " + Common.getCoinName(+this.dinoData["cashType"]);
            this.sellPriceLbl.string = Common.numberWithCommas(+this.dinoData["endingPrice"]) + " " + Common.getCoinName(+this.dinoData["cashType"]);
        }
        if(sceneData ==="mydino-onsale"){
            this.sellPriceLbl.string = Common.numberWithCommas(+this.dinoData["endingPrice"]) + " " + Common.getCoinName(+this.dinoData["cashType"]);
        }
        Common.getRarityDinoBG(dinoRarity,(err, spr)=>{this.rarityBG.spriteFrame = spr; });
        Common.getRarityDinoDetail(dinoRarity,(err, spr)=>{this.rarityFrame.spriteFrame = spr; });
        this.loadDinoSkillData();
    }


    public loadDinoSkillData(){
        if(+this.dinoData["skill"] > 0){
            this.skill.active = true;
            let skillDes = "Skill description here";
            if(this.dinoData["skillDetail"]["note"])
                skillDes = "Description: " + this.dinoData["skillDetail"]["note"]+"";
            this.skillDescription.string = skillDes;

            let skillTit = "";
            if(this.dinoData["skillDetail"]["name"])
                skillTit = this.dinoData["skillDetail"]["name"]+"";
            this.skillTitle.string = skillTit;
            this.skillRarityLbl.string =  "Skill Rarity: " + Common.getSkillRarityString(+this.dinoData["skill"]);
            Common.getSkillRarityIcon(+this.dinoData["skill"],(err, spr)=>{this.skillRarityIcon.spriteFrame = spr; });
            Common.getSkillIcon(+this.dinoData["skill"],(err, spr)=>{this.skillRarityIcon.spriteFrame = spr; });

        }else{
            this.skill.active = false;
        }
    }


    /**
     * onGetDetailStatus
     */
    public onGetDetailStatus(isEvolve: boolean, currentMidas: number, midasList:any) {
        if(isEvolve)
        {      
            if(!this.dinoData["isEvolved"])              
                this.showSyncingData();
        }
        else 
        {
            this.showEvolve();
        }
        this.onUseMidasCallback( currentMidas, midasList);
    }

    /**
     * onGetDetailStatus
     */
    public onUseMidasCallback( currentMidas: any, midasList:any) {
        if(midasList){
            this.midasScrollView.node.active = true;
            this.midasNodata.active = false;
            let tmpLength = this.midasCardList.length > midasList.length ? this.midasCardList.length : midasList.length;
            for (let index = 0; index < tmpLength; index++) 
            {
                let cur:MidasCard = null;
                
                if(this.midasCardList.length <= index){
                    let newItem = instantiate(this.midasCardList[0].node);
                    newItem.setParent(this.midasScrollView.content);
                    cur = newItem.getComponent<MidasCard>(MidasCard);
                    this.midasCardList.push(cur);
                }else{
                    cur = this.midasCardList[index];
                }
                if(midasList.length > index){
                    cur.node.active = true;
                    cur.Initialize(midasList[index]);
                }else{
                    cur.node.active = false;
                }
            }
            this.midasScrollView.scrollToTopLeft();
        }else{
            for (let index = 0; index < this.midasCardList.length; index++) {
                this.midasCardList[index].node.active = false;
            }
            this.midasScrollView.node.active = false;
            this.midasNodata.active = true;
        }
        this.midasRangeLbl.string = currentMidas+"";
    }

    /**
     * loadUI
     */
    public loadUI() 
    {
        this.nameLbl.string = this.dinoData["title"];
        this.idLbl.string = "#" + this.dinoData["nftId"];
        this.generationLbl.string = Common.ConvertGetGenerationString(+this.dinoData["generation"]) ;
        if (!('midasRemain' in  this.dinoData))
            this.dinoData["midasRemain"] = 0;
        this.midasRangeLbl.string = this.dinoData["midasRemain"];

        this.classLbl.string = Common.GetClassString(+this.dinoData["class"]);
        Common.GetClassIcon(+this.dinoData["class"], (err,spr)=>{ this.classIcon.spriteFrame = spr; });
        this.rarityLbl.string = Common.GetRarityString(+this.dinoData["rarity"]);
        Common.getRarityIcon(+this.dinoData["rarity"], (err,spr)=>{ this.rarityIcon.spriteFrame = spr; });
    
        this.levelLbl.string = this.dinoData["level"];
        this.expLbl.string = this.dinoData["exp"] + "/" + this.dinoData["expLevelUp"] ;
        
        this.ageLbl.string = Math.floor((Date.now()/1000 - this.dinoData["bornAt"])/86400) + "d";
        if(this.dinoData.hasOwnProperty("breedCount") && this.dinoData["isEvolved"])
        {
            this.breedLbl.string =  this.dinoData["breedCount"] + "/" + this.dinoData["maxBreedCount"];
        }
        else  {
            console.log('this.dinoData["breedCount"]', false);  
            this.breedLbl.string =  "-";
        }

        if(this.dinoData["isEvolved"]){
            this.genderLbl.string = Common.getGenderString(+this.dinoData["gender"]);
            Common.getGenderIcon(+this.dinoData["gender"], (err,spr)=>{ this.genderIcon.spriteFrame = spr; });
        }else{
            this.genderLbl.string = "?";
        }



        this.hpLbl.string = this.dinoData["statsHp"];
        this.atkLbl.string = this.dinoData["statsAtk"];
        this.defenseLbl.string = this.dinoData["statsDef"];
        this.speedLbl.string = this.dinoData["statsSpd"];
    }

    /**
     * loadPartUI
     */
    public loadPartUI(expressTraits:any) 
    {
        let expTraits = (expressTraits);
        if(+this.dinoData["geneRarity"] >= 4){
            this.wingNameLbl.string = "Wing" ;//+ Common.romanize(expTraits["Wing"] + 1);
            this.wingRarityLbl.string = Common.GetPartRarityString(expTraits["Wing"] + 1);
        }else{
            this.wingNameLbl.string = "Wing"
            this.wingRarityLbl.string = "-";
        }
        

        this.hornNameLbl.string = "Horn " + Common.romanize(expTraits["Horn"] + 1);
        this.hornRarityLbl.string = Common.GetPartRarityString(expTraits["Horn"] + 1);
        this.plateNameLbl.string = "Plate " + Common.romanize(expTraits["Back"] + 1);
        this.plateRarityLbl.string = Common.GetPartRarityString(expTraits["Back"] + 1);
        this.teethNameLbl.string = "Teeth " + Common.romanize(expTraits["Teeth"] + 1);
        this.teethRarityLbl.string = Common.GetPartRarityString(expTraits["Teeth"] + 1);
        this.eyeNameLbl.string = "Eye " + Common.romanize(expTraits["Eye"] + 1);
        let bobyClass = DinoBodyDefine.BodyArray[expTraits["Class"]];
        let body = bobyClass[expTraits["Texture"]%bobyClass.length];
        this.bodyNameLbl.string = "Body " + Common.romanize(body + 1);
    }

    public showSyncingData()
    {
        this.rightScrollView.active = false;
        this.syncingData.active = true;
        this.envolve.active = false; 
    }

    public showEvolve()
    {
        this.envolve.active = !this.dinoData["isEvolved"] && Common.isShowEvolve && Common.currentLoadScene === "mydino";
        this.totalBookLbl.string = Common.totalEvolveBook.toString();
        this.envolveDNLBtn.active = Common.evolveDNLFlag;
        this.envolveDNLFeeLbl.string = Common.isApprovedEvolve? ""+Common.evolveDNLFee : "Approve";
        this.envolveBookFeeLbl.string = Common.totalEvolveBook >= 1? "Evolve" : "Buy";
    }

    public closeDinoDetail()
    {
        if(window.closeDetail)
        {
            window.closeDetail();
        }
    }

    public envolveDNL()
    {
        console.log('envolveDNL ' + this.dinoData["nftId"]);
        if(!Common.isApprovedEvolve){
            if(window.approveEvolve)
                window.approveEvolve();
        }else{
            if(window.envolveWithDNL)
                window.envolveWithDNL(this.dinoData["nftId"]);
        }
    }

    public learnSkill()
    {
        console.log('learnSkill ' + this.dinoData["nftId"]);
        if(window.learningSkill)
            window.learningSkill(this.dinoData["nftId"]);
    }

    public envolveBook()
    {
        if(Common.totalEvolveBook >= 1){
            console.log('envolveBook ' + this.dinoData["nftId"]);
            if(window.envolveWithBook)
                window.envolveWithBook(this.dinoData["nftId"]);
        }else{
            console.log('go to book market');
            if(window.goMarket)
                window.goMarket();
        }

    }

    public buyDino(){
        console.log('buyDino ' + this.dinoData["nftId"]);
        if(window.buyDino)
            window.buyDino(this.dinoData["nftId"], this.dinoData["endingPrice"], this.dinoData["cashType"],);
    }

    public cancelSellDino(){
        console.log('cancelSell ' + this.dinoData["nftId"]);
        if(window.cancelSell)
            window.cancelSell(this.dinoData["nftId"]);
    }

    private _temp_quat: Quat = new Quat();
    update (deltaTime: number) {
        //if(this.autoRotate){
            Quat.fromEuler(this._temp_quat,0, -50 * deltaTime, 0);
            this.partDino.node.rotate(this._temp_quat);
        //}
        // [4]
    }

    public OnUseMidasBox(event:any, midasId:any)
    {
        if(window.useMidas){
            window.useMidas(this.dinoData["nftId"], midasId);
        }
        this.hideMidasPopup();
    }

    public showMidasPopup(){
        // this.leftDetailPanel.active = false;
        // this.midasPopup.active = true;
        let sceneData = Common.currentLoadScene;
        if(sceneData ==="market" || sceneData ==="mydino-onsale"){
            console.log(sceneData);
        }
        else{
            this.leftDetailPanel.active = false;
            this.midasPopup.active = true;
        }
    }

    public hideMidasPopup(){
        this.leftDetailPanel.active = true;
        this.midasPopup.active = false;
    }
    public hideMidasDetail(){
        this.midasDetail.active = false;
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
