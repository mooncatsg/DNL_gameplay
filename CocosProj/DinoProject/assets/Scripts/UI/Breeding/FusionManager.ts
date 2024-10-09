
import { _decorator, Component, Node,log, Button, Color,Sprite,Label,SpriteFrame, director, resources, InstancedBuffer, LabelComponent, color } from 'cc';
import { Common } from '../../common/Common';
import { BreadingDinoCardController } from './BreadingDinoCardController';
import { StoneItemController } from './StoneItemController';
const { ccclass, property } = _decorator;

@ccclass('FusionManager')
export class FusionManager extends Component {
    private static singleton: FusionManager;
    public static getInstance(): FusionManager {
        return FusionManager.singleton;
    }
    // STONE    
    @property({ type: LabelComponent })
    public totalRock: Label = null;
    // End STONE

    // DINO
    @property({ type: Node })
    public camera3D: Node = null;

    @property({ type: Node })
    public listDinoPopup: Node = null;

    @property({ type: Node })
    public nodeAskMale: Node = null;

    @property({ type: Node })
    public maleDinoCard: Node = null;

    @property({ type: Node })
    public nodeAskFemale: Node = null;

    @property({ type: Node })
    public femaleDinoCard: Node = null;
    
    @property({ type: BreadingDinoCardController })
    public listBreedingDinoCardControllers: BreadingDinoCardController []= [];
    @property({ type: Button })
    public dinoNextBtn: Button = null;
    @property({ type: Button })
    public dinoPrevBtn: Button = null;
    @property({ type: Label })
    public dinoPageLabel: Label = null;
    @property({ type: Node })
    public noDataDinoNode: Node = null;
    // End DINO

    @property({ type: Label })
    public breedingFeeLabel: Label = null;
    @property({ type: Button })
    public breedingButton: Button = null;
    @property({ type: Button })
    public approveButton: Button = null;
    @property({ type: Button })
    public chooseMaleBtn: Button = null;
    @property({ type: Button })
    public chooseFemaleBtn: Button = null;
    @property({ type: Button })
    public resetButton: Button = null;

    @property({ type: Node })
    public failPanel;
    @property({ type: Node })
    public fusionPanel: Node = null;

    private choosingMale:any;
    private choosingFemale:any;


    private dinoDatas = [];
    private stoneDatas = [];
    private choosingDinoDatas:any;
    private doingChooseMale:boolean = false;
    private fusionRockNeeded:number = -1;

    private maxDinoPage:number = 1;
    private currentDinoPage:number = 1;

    private feeChecking:boolean = false;

    private currentBalance:number = 0;

    start () {
        FusionManager.singleton = this;
        this.dinoDatas = Common.currentData["dinoDatas"];
        this.stoneDatas = Common.currentData["stoneDatas"];
        this.breedingButton.node.active = !Common.currentData["isApprove"];
        this.approveButton.node.active = Common.currentData["isApprove"];
        this.currentBalance = +Common.currentData["currentBalance"];
        this.totalRock.string = "Total Fusion Rock: " + this.stoneDatas.filter(x=>x["stoneType"] == 6).length;
        this.totalRock.color = Color.WHITE;
	    window.onFusionSuccess = function(success, newDinoData)
        {
            if(success){
                Common.currentHatchingData = newDinoData;
                Common.currentHatchingData["eggType"] = "1" + (+Common.currentHatchingData["expressTraits"]["Class"]+1) + "12";
                director.loadScene("Hatching");
            }else{
                let fusionMng = FusionManager.getInstance();
                fusionMng.fusionPanel.active = false;
                fusionMng.failPanel.active = true;
            }
        };
        window.onApproveCallbackFusion = this.onApproveCallbackFusion;
        window.onGetDataPreFusion = this.onGetDataPreFusion;
    }

    private currentFee = 0;
    public onGetDataPreFusion(fee, successRate, rockQuantity)
    {
        let fusionMng = FusionManager.getInstance();
        fusionMng.currentFee = +fee;
        fusionMng.fusionRockNeeded = +rockQuantity;
        fusionMng.feeChecking = false;
        fusionMng.breedingFeeLabel.string = "Fusion Fee : " + Common.numberWithCommas(+fee) +" BUSD and success rate is " + successRate + "%";
        fusionMng.chooseFemaleBtn.interactable =  true;
        fusionMng.chooseMaleBtn.interactable =  true;
        fusionMng.ResetBreedButton();
        let totalRock : number = fusionMng.stoneDatas.filter(x=>x["stoneType"] == 6).length;
        fusionMng.totalRock.string = "Fusion Rock: " + fusionMng.fusionRockNeeded + "/" + totalRock;
        fusionMng.totalRock.color = totalRock >= rockQuantity ? Color.GREEN : Color.RED;
    }

    public onApproveCallbackFusion(success)
    {
        let fusionMng = FusionManager.getInstance();
        if(success){
            fusionMng.approveButton.node.active = false;
            fusionMng.breedingButton.node.active = true;
            fusionMng.chooseMaleBtn.enabled = false;
            fusionMng.chooseFemaleBtn.enabled = false;
        }else{
            fusionMng.approveButton.interactable = true;
        }
    }

    public ResetBreedingState()
    {
        this.breedingButton.interactable = false;
        this.approveButton.interactable = false;
    }

    public ChooseMale(_chooseIdx:number)
    {
        if(this.nodeAskFemale.active == false)
            this.femaleDinoCard.active = true;      

        this.choosingMale = this.choosingDinoDatas[(this.currentDinoPage-1) * 10 + _chooseIdx];
        this.nodeAskMale.active = false;
        this.maleDinoCard.active = true;
        this.maleDinoCard.getComponent(BreadingDinoCardController).initialize(this.choosingMale);
        this.listDinoPopup.active = false;
        this.ResetBreedButton();
        this.resetButton.node.active = true;
        if(this.choosingFemale)
            this.CheckFee();
    }

    public ChooseFemale(_chooseIdx:number)
    {
        if(this.nodeAskMale.active == false)
            this.maleDinoCard.active = true;

        this.choosingFemale = this.choosingDinoDatas[(this.currentDinoPage-1) * 10 + _chooseIdx];
        this.nodeAskFemale.active = false;
        this.femaleDinoCard.active = true;
        this.femaleDinoCard.getComponent(BreadingDinoCardController).initialize(this.choosingFemale);
        this.listDinoPopup.active = false;
        this.ResetBreedButton();
        this.resetButton.node.active = true;
        if(this.choosingMale)
            this.CheckFee();
    }

    // Region DINO
    public OnChooseMaleButtonClicked()
    {
        if(this.listDinoPopup.active == true)
        {
            return;
        }
        this.maleDinoCard.active = false;
        this.femaleDinoCard.active = false;
        this.listDinoPopup.active = true;
        if(this.choosingFemale)
            this.choosingDinoDatas = this.dinoDatas.filter(x => x["geneRarity"] == this.choosingFemale["geneRarity"] && x["nftId"] != this.choosingFemale["nftId"]);
        else
            this.choosingDinoDatas = this.dinoDatas;
        this.doingChooseMale = true;

        log("MALE : "+this.choosingDinoDatas.length);
        this.currentDinoPage = 1;
        this.maxDinoPage = Math.ceil(this.choosingDinoDatas.length/10);
        this.noDataDinoNode.active = (this.choosingDinoDatas.length <= 0);
        if(this.noDataDinoNode.active == true)
        {
            this.dinoNextBtn.node.active = false;
            this.dinoPrevBtn.node.active = false;
            this.dinoPageLabel.node.active = false;
        }
        this.LoadDinoPage();
    }

    public OnChooseFemaleButtonClicked()
    {
        if(this.listDinoPopup.active == true)
        {
            return;
        }
        this.maleDinoCard.active = false;
        this.femaleDinoCard.active = false; 
        this.listDinoPopup.active = true;
        if(this.choosingMale)
            this.choosingDinoDatas = this.dinoDatas.filter(x => x["geneRarity"] == this.choosingMale["geneRarity"] && x["nftId"] != this.choosingMale["nftId"]);
        else
            this.choosingDinoDatas = this.dinoDatas;
        this.doingChooseMale = false;

        log("FEMALE : "+this.choosingDinoDatas.length);
        this.currentDinoPage = 1;
        this.maxDinoPage = Math.ceil(this.choosingDinoDatas.length/10); 
        this.noDataDinoNode.active = (this.choosingDinoDatas.length <= 0);
        if(this.noDataDinoNode.active == true)
        {
            this.dinoNextBtn.node.active = false;
            this.dinoPrevBtn.node.active = false;
            this.dinoPageLabel.node.active = false;
        }
        this.LoadDinoPage();
    }

    LoadDinoPage()
    {
        let otherDino:any = this.doingChooseMale? this.choosingFemale : this.choosingMale
        for (let index = 0; index < this.listBreedingDinoCardControllers.length; index++) 
        {
            let currentIndex = (this.currentDinoPage-1) * 10 + index;           

            const element = this.listBreedingDinoCardControllers[index];
            if(currentIndex < this.choosingDinoDatas.length){
                element.node.active = true;
                element.initialize(this.choosingDinoDatas[currentIndex]);
                element.validateDino(+Common.currentData["minLifeDurationToBreed"], otherDino, false);
            }else{
                element.node.active = false;
            }
        }
        this.resetButton.node.active = false;
        this.ResetDinoBtnState();

    }

    public OnChooseDino(event:any, idx:any) {
        if(this.doingChooseMale)
            this.ChooseMale(+idx);
        else
            this.ChooseFemale(+idx);
    }

    ResetDinoBtnState()
    {
        this.dinoPageLabel.string = "" + this.currentDinoPage + "/" + this.maxDinoPage;
        this.dinoNextBtn.interactable = (this.maxDinoPage > this.currentDinoPage);
        this.dinoPrevBtn.interactable = (this.currentDinoPage > 1);
    }

    OnClickDinoNextBtn()
    {
        if(this.currentDinoPage <this.maxDinoPage)
        {
            log("currentDinoPage : "+this.currentDinoPage);

            this.currentDinoPage ++;
            this.LoadDinoPage();
        }
    }

    OnClickDinoPrevBtn()
    {
        if(this.currentDinoPage > 0)
        {
            this.currentDinoPage --;
            this.LoadDinoPage();
        }
    }
    
    public OnCloseDinoPanelButtonClicked()
    {
        if(this.nodeAskMale.active == false)
            this.maleDinoCard.active = true;
        if(this.nodeAskFemale.active == false)
            this.femaleDinoCard.active = true;
        this.listDinoPopup.active = false;      
        this.camera3D.active = true;
        this.resetButton.node.active = true;
    }


    // END CHOOSE DINO

    public OnOkButtonClicked()
    {
        console.log("OnOkButtonClicked");
        if(window.closeCocosFusion)
            window.closeCocosFusion();
    }

    public OnBreedButtonClicked()
    {
        let rockArr = this.stoneDatas.filter(x=>x["stoneType"] == 6).map(y => y["nftId"]);
        if(rockArr.length >= this.fusionRockNeeded)
        {
            rockArr = rockArr.slice(0, this.fusionRockNeeded);
            console.log("OnFusionButtonClicked","Male:", this.choosingMale["nftId"], "Female:", this.choosingFemale["nftId"], "Stone:", rockArr);
            if(window.startFusion )
                window.startFusion (this.choosingMale["nftId"], this.choosingFemale["nftId"], rockArr);
        }
    }

    public OnApproveButtonClicked()
    {
        let rockArr = this.stoneDatas.filter(x=>x["stoneType"] == 6).map(y => y["nftId"]);
        if(rockArr.length >= this.fusionRockNeeded)
        {
            rockArr = rockArr.slice(0, this.fusionRockNeeded-1);
            console.log("OnApproveButtonClicked","Male:", this.choosingMale["nftId"], "Female:", this.choosingFemale["nftId"], "Stone:", rockArr );
            this.approveButton.interactable = false;
            if(window.approveClickFusion )
                window.approveClickFusion (this.choosingMale["nftId"], this.choosingFemale["nftId"], rockArr);
        }
    }

    public CheckFee()
    {
        this.feeChecking = true;
        this.DisableAllButton();
        console.log("CheckFee", this.choosingMale["nftId"], this.choosingFemale["nftId"]);
        if(window.checkDataPreFusion)
            window.checkDataPreFusion(this.choosingMale["nftId"], this.choosingFemale["nftId"], this.choosingMale["geneRarity"]);
        //bien update code here
    }

    public CalculatingFee(){
        if(this.feeChecking)
        {
            let num = this.breedingFeeLabel.string.length - "Calculating Fee".length;
            num ++;
            if(num > 3)
                num = 0;
            let tempStr = "Calculating Fee";
            if(num == 1)
                tempStr += ".";
            if(num == 2)
                tempStr += "..";
            if(num == 3)
                tempStr += "...";
            this.breedingFeeLabel.string  = tempStr;
        }
    }

    private timeUpdateCalculate = 0;
    update (deltaTime: number) {
        if(this.feeChecking){
            this.timeUpdateCalculate += deltaTime;
            if(this.timeUpdateCalculate > 0.5){
                this.timeUpdateCalculate = 0;
                this.CalculatingFee();
            }
        }
    }
    
    DisableAllButton()
    {
        this.chooseFemaleBtn.interactable =  false;
        this.chooseMaleBtn.interactable =  false;
    }

    ResetBreedButton()
    {
        this.approveButton.interactable =  (this.choosingMale != null && this.choosingFemale != null && !this.feeChecking 
            && this.fusionRockNeeded > 0 && this.stoneDatas.filter(x=>x["stoneType"] == 6).length >= this.fusionRockNeeded);
        this.breedingButton.interactable = (this.choosingMale != null && this.choosingFemale != null 
            && !this.feeChecking && this.currentBalance >= this.currentFee
            && this.fusionRockNeeded > 0 && this.stoneDatas.filter(x=>x["stoneType"] == 6).length >= this.fusionRockNeeded );        
    }
    resetDino(){
        if(this.maleDinoCard.active || this.femaleDinoCard.active){
            this.maleDinoCard.active = false;
            this.femaleDinoCard.active = false;
            this.nodeAskMale.active = true;
            this.nodeAskFemale.active = true;
            this.chooseFemaleBtn.interactable =  true;
            this.chooseMaleBtn.interactable =  true;
            this.choosingMale = null;
            this.choosingFemale = null;
            this.breedingFeeLabel.string  = "";
            this.feeChecking = false;
            this.totalRock.string = "Total Fusion Rock: " + this.stoneDatas.filter(x=>x["stoneType"] == 6).length;
            this.totalRock.color = Color.WHITE;
            console.log("---------clicked button reset dino");
        }
    }

    // End Region
}