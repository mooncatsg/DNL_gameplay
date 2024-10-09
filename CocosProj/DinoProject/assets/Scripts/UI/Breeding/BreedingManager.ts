
import { _decorator, Component, Node,log, Button, Color,Sprite,Label,SpriteFrame, director, resources, InstancedBuffer } from 'cc';
import { Common } from '../../common/Common';
import { BreadingDinoCardController } from './BreadingDinoCardController';
import { StoneItemController } from './StoneItemController';
const { ccclass, property } = _decorator;

@ccclass('BreedingManager')
export class BreedingManager extends Component {
    private static singleton: BreedingManager;
    public static getInstance(): BreedingManager {
        return BreedingManager.singleton;
    }
    // STONE    
    @property({ type: Sprite })
    public stoneSprite: Sprite = null;
    @property({ type: Node })
    public stoneAskNode: Node = null;
    @property({ type: Node })
    public listStonePopup: Node = null;
    @property({ type: StoneItemController })
    public listStoneItemController: StoneItemController []= [];
    @property({ type: Button })
    public stoneNextBtn: Button = null;
    @property({ type: Button })
    public stonePrevBtn: Button = null;
    @property({ type: Label })
    public pageLabel: Label = null;
    @property({ type: Node })
    public noDataStoneNode: Node = null;
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
    public chooseStoneBtn: Button = null;
    @property({ type: Button })
    public chooseMaleBtn: Button = null;
    @property({ type: Button })
    public chooseFemaleBtn: Button = null;
    
    private choosingMale:any;
    private choosingFemale:any;
    private choosingStoneData:any;


    private dinoDatas = [];
    private stoneDatas = [];
    private choosingDinoDatas:any;
    private filteredStoneList:any;
    private doingChooseMale:boolean = false;

    private maxStonePage:number = 1;
    private currentStonePage:number = 1;

    private maxDinoPage:number = 1;
    private currentDinoPage:number = 1;

    private feeChecking:boolean = false;

    private currentBalance:number = 0;

    start () {
        BreedingManager.singleton = this;
        this.dinoDatas = Common.currentData["dinoDatas"];
        this.stoneDatas = Common.currentData["stoneDatas"];
        this.breedingButton.node.active = !Common.currentData["isApprove"];
        this.approveButton.node.active = Common.currentData["isApprove"];
        this.currentBalance = +Common.currentData["currentBalance"];

	    window.onBreedSuccess = function(newDinoData){
            console.log("onBreedSuccess")
            console.log(newDinoData)
            Common.currentHatchingData = newDinoData;
            Common.currentHatchingData["eggType"] = "1" + (+Common.currentHatchingData["expressTraits"]["Class"]+1) + "12";
            director.loadScene("Hatching");
        };
        window.onApproveCallback = this.onApproveCallback;
        window.onGetFee = this.OnGetFee;
        this.chooseStoneBtn.enabled = false;
        this.stoneAskNode.active = false;
    }

    private currentFee = 0;
    public OnGetFee(fee)
    {
        BreedingManager.getInstance().currentFee = +fee;
        BreedingManager.getInstance().feeChecking = false;
        BreedingManager.getInstance().breedingFeeLabel.string = "Breeding Fee : " + Common.numberWithCommas(+fee) +" DNL";
        BreedingManager.getInstance().chooseFemaleBtn.interactable =  true;
        BreedingManager.getInstance().chooseMaleBtn.interactable =  true;
        BreedingManager.getInstance().ResetBreedButton();
    }

    public onApproveCallback(success)
    {
        if(success){
            BreedingManager.getInstance().approveButton.node.active = false;
            BreedingManager.getInstance().breedingButton.node.active = true;
            BreedingManager.getInstance().chooseStoneBtn.enabled = false;
            BreedingManager.getInstance().chooseMaleBtn.enabled = false;
            BreedingManager.getInstance().chooseFemaleBtn.enabled = false;
        }else{
            BreedingManager.getInstance().approveButton.interactable = true;
        }
    }

    public ResetBreedingState()
    {
        this.breedingButton.interactable = false;
        this.approveButton.interactable = false;
    }

    public ResetChooseStone()
    {
         this.listStoneItemController.forEach(element => {
            element.Highlight(false);
        });
    }

    public ChooseStone(_chooseIdx:number)
    {
        this.ResetChooseStone();
        this.listStoneItemController[_chooseIdx].Highlight(true);
        this.choosingStoneData = this.filteredStoneList[(this.currentStonePage-1) * 10 + _chooseIdx] ;
        Common.GetStoneIcon(+this.choosingStoneData["stoneType"], (err,spr) =>{
            this.stoneSprite.spriteFrame = spr;
        });   
    }

    public resetChoosingStone(){
        this.choosingStoneData = null;
        resources.load("Stones/base/spriteFrame", SpriteFrame, (err, spriteFrame) => {
            this.stoneSprite.spriteFrame = spriteFrame;
        });
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
        this.resetChoosingStone();
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
        this.resetChoosingStone();
        if(this.choosingMale)
            this.CheckFee();
    }

    // Region DINO
    public OnChooseMaleButtonClicked()
    {
        if(this.listDinoPopup.active == true || this.listStonePopup.active == true)
        {
            return;
        }
        this.maleDinoCard.active = false;
        this.femaleDinoCard.active = false;
        this.listDinoPopup.active = true;
        this.choosingDinoDatas = this.dinoDatas.filter(x => x["gender"] == 1);
        if(this.choosingFemale){
            if(this.choosingFemale["class"] == 4)
                this.choosingDinoDatas = this.choosingDinoDatas.filter(x => x["class"] != 5);
            else if(this.choosingFemale["class"] == 5)
                this.choosingDinoDatas = this.choosingDinoDatas.filter(x => x["class"] != 4);
        }
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
        if(this.listDinoPopup.active == true || this.listStonePopup.active == true)
        {
            return;
        }
        this.maleDinoCard.active = false;
        this.femaleDinoCard.active = false; 
        this.listDinoPopup.active = true;
        this.choosingDinoDatas = this.dinoDatas.filter(x => x["gender"] != 1);
        if(this.choosingMale){
            if(this.choosingMale["class"] == 4)
                this.choosingDinoDatas = this.choosingDinoDatas.filter(x => x["class"] != 5);
            else if(this.choosingMale["class"] == 5)
                this.choosingDinoDatas = this.choosingDinoDatas.filter(x => x["class"] != 4);
        }
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
                element.validateDino(+Common.currentData["minLifeDurationToBreed"], otherDino, true);
            }else{
                element.node.active = false;
            }
        }
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
    }


    // END CHOOSE DINO

    // START STONE    
    public filterStone(){
        if(+this.choosingMale["class"] == 4 || +this.choosingFemale["class"] == 4) //one of them is Dark
        {
            this.filteredStoneList = this.stoneDatas.filter(x => x["stoneType"] == 4);
        }
        else if(+this.choosingMale["class"] == 5 || +this.choosingFemale["class"] == 5) //one of them is Light
        {
            this.filteredStoneList = this.stoneDatas.filter(x => x["stoneType"] == 8);
        }
        else if(+this.choosingMale["class"] != +this.choosingFemale["class"] )
        {
            this.filteredStoneList = this.stoneDatas.filter(x => x["stoneType"] == 3);
        }else if(+this.choosingMale["class"] == 1 )
        {
            this.filteredStoneList = this.stoneDatas.filter(x => x["stoneType"] == 3 || x["stoneType"] == 0);
        }else if(+this.choosingMale["class"] == 2 )
        {
            this.filteredStoneList = this.stoneDatas.filter(x => x["stoneType"] == 3 || x["stoneType"] == 1);
        }else if(+this.choosingMale["class"] == 3 )
        {
            this.filteredStoneList = this.stoneDatas.filter(x => x["stoneType"] == 3 || x["stoneType"] == 2);
        }
    }

    public OnChooseStoneListButtonClicked()
    {
        if(this.listDinoPopup.active == true || this.listStonePopup.active == true)
        {
            return;
        }
        this.filterStone();
        this.filteredStoneList.sort((a,b)=>{
            return a["stoneType"] - b["stoneType"];
        });
        this.maxStonePage = Math.ceil(this.filteredStoneList.length/10);
        this.noDataStoneNode.active = (this.filteredStoneList.length <= 0);
        if(this.noDataStoneNode.active == true)
        {
            this.stoneNextBtn.node.active = false;
            this.stonePrevBtn.node.active = false;
            this.pageLabel.node.active = false;
        }
        this.LoadStonePage();
    }

    public OnChooseStone(event:any, idx:number) {
        this.stoneAskNode.active = false;
        this.ChooseStone(+idx);
        this.ResetBreedButton();

    }

    LoadStonePage()
    {
        for(let i = 0;i<this.listStoneItemController.length;i++)
        {
            let currentIndex = (this.currentStonePage-1) * 10 + i;            
            this.listStoneItemController[i].node.active = (currentIndex<this.filteredStoneList.length);
            if(currentIndex <this.filteredStoneList.length)
            {
                this.listStoneItemController[i].loadData(this.filteredStoneList[currentIndex]["nftId"],this.filteredStoneList[currentIndex]["title"],this.filteredStoneList[currentIndex]["stoneType"]);
                if(this.choosingStoneData != null)
                    this.listStoneItemController[i].Highlight(this.listStoneItemController[i].stoneNFT == this.choosingStoneData.nftId);
            }
        }
        this.listStonePopup.active = true;
        this.camera3D.active = false;
        this.ResetStoneBtnState();
    }

    ResetStoneBtnState()
    {
        this.pageLabel.string = "" + this.currentStonePage + "/" + this.maxStonePage;
        this.stoneNextBtn.interactable = (this.maxStonePage > this.currentStonePage);
        this.stonePrevBtn.interactable = (this.currentStonePage > 1);
    }

    OnClickStoneNextBtn()
    {
        if(this.currentStonePage < Math.ceil((this.filteredStoneList.length+9)/10))
        {
            this.currentStonePage ++;
            this.LoadStonePage();
        }
    }
    OnClickStonePrevBtn()
    {
        if(this.currentStonePage > 0)
        {
            this.currentStonePage --;
            this.LoadStonePage();
        }
    }
    
    public OnCloseStonePanelButtonClicked()
    {
        this.listStonePopup.active = false;      
        this.camera3D.active = true;
    }
    // END STONE

    public OnBreedButtonClicked()
    {
        console.log("OnBreedButtonClicked","Male:", this.choosingMale["nftId"], "Female:", this.choosingFemale["nftId"], "Stone:", this.choosingStoneData["nftId"]);
        if(window.startBreed)
            window.startBreed(this.choosingMale["nftId"], this.choosingFemale["nftId"], this.choosingStoneData["nftId"]);
    }

    public OnApproveButtonClicked()
    {
        console.log("OnApproveButtonClicked","Male:", this.choosingMale["nftId"], "Female:", this.choosingFemale["nftId"], "Stone:", this.choosingStoneData["nftId"]);
        this.approveButton.interactable = false;
        if(window.onApproveClick)
            window.onApproveClick(this.choosingMale["nftId"], this.choosingFemale["nftId"], this.choosingStoneData["nftId"]);
    }

    public CheckFee()
    {
        this.feeChecking = true;
        this.DisableAllButton();
        console.log("CheckFee", this.choosingMale["nftId"], this.choosingFemale["nftId"]);
        if(window.checkFee)
            window.checkFee(this.choosingMale["nftId"], this.choosingFemale["nftId"]);
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
        this.stoneAskNode.active = (this.choosingMale != null && this.choosingFemale != null && this.choosingStoneData == null);
        this.chooseStoneBtn.enabled = (this.choosingMale != null && this.choosingFemale != null);
        this.approveButton.interactable =  (this.choosingMale != null && this.choosingFemale != null && this.choosingStoneData != null && !this.feeChecking);
        this.breedingButton.interactable = (this.choosingMale != null && this.choosingFemale != null && this.choosingStoneData != null && !this.feeChecking && this.currentBalance >= this.currentFee);        
    }

    // End Region
}