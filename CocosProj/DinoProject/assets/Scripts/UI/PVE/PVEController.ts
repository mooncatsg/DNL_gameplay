
import { _decorator, Component, Node, Label, Sprite, Button, SkinnedMeshRenderer, Material ,resources,log} from 'cc';
import { DinoDetailManager } from '../../PartDino/DinoDetailManager';
import { PartDinoController } from '../../PartDino/PartDinoController';
import { PVEHistoryLineController } from './PVEHistoryLineController';
const { ccclass, property } = _decorator;

@ccclass('PVEController')
export class PVEController extends Component {
    private static singleton: PVEController;
    public static getInstance(): PVEController {
        return PVEController.singleton;
    }
    
    // DINO Start
    @property({ type: PartDinoController })
    public partDinoController: PartDinoController = null;
    @property({ type: Label })
    public dinoNumberLabel: Label = null;
    @property({ type: Label })
    public dinoNameLabel: Label = null;
    @property({ type: Sprite })
    public dinoRaritySprite: Sprite = null;
    @property({ type: Label })
    public dinoLevelLabel: Label = null;
    @property({ type: Label })
    public dinoEXPLabel: Label = null;
    @property({ type: Label })
    public dinoTurnPlayLabel: Label = null;
    @property({ type: Button })
    public dinoNextBtn: Button = null;
    @property({ type: Button })
    public dinoPrevBtn: Button = null;
    @property({ type: Button })
    public dinoDetailBtn: Button = null;
    @property({ type: Button })
    public dinoFeedBtn: Button = null;
    // DINO End

    // DINO DETAIL
    @property({ type: DinoDetailManager })

    public dinoDetailManager: DinoDetailManager = null;
    @property({ type: Node })
    public offCamera2D: Node = null;
    @property({ type: Node })
    public offCamera3D: Node = null;
    // DINO DETAIL End

    // MONSTER Start
    @property({ type: Node })
    public monsterDetailPopup: Node = null;
    @property({ type: Label })
    public monsterWinRate: Label = null;
    @property({ type: Node })
    public batModel: Node = null;
    @property({ type: SkinnedMeshRenderer })
    public batMeshRender: SkinnedMeshRenderer = null;
    @property({ type: Node })
    public spiderKingModel: Node = null;
    @property({ type: Node })
    public monsterNodes: Node[] = [];
    // MONSTER End

    // ANOTHER Start
    @property({ type: Node })
    public pveGuidePopup: Node = null;
    @property({ type: Node })
    public pveHistoryPopup: Node = null;
    @property({ type: PVEHistoryLineController })
    public pveHistoryLineList: PVEHistoryLineController[] = [];
    @property({ type: Button })
    public pveHistoryPrevBtn: Button = null;
    @property({ type: Button })
    public pveHistoryNextBtn: Button = null;
    @property({ type: Label })
    public pveHistoryCurrentPage: Label = null;
    @property({ type: Label })
    public pveHistoryMaxPage: Label = null;
    // ANOTHER End


    private dinoChoosingIndex = 0;
    private dinoListMaxCount = 4;
    private monsterChoosingIndex = 0;

    private historyCurrentPage = 1;
    private historyMaxPage = 10;

    start () {
        this.LoadDino();
    }
    
    // Region DINO
    public LoadDino()
    {
        this.dinoNumberLabel.string = "You have " + this.dinoListMaxCount +" dinos";
        this.partDinoController.randomNewDino();
        this.NextPrevButtonStatus();
    }
    public NextPrevButtonStatus()
    {
        this.dinoNextBtn.interactable = (this.dinoChoosingIndex<(this.dinoListMaxCount-1));
        this.dinoPrevBtn.interactable = (this.dinoChoosingIndex > 0);
    }
    public OnClickDinoNextBtn()
    {
        if(this.dinoChoosingIndex < (this.dinoListMaxCount-1))
        {
            this.dinoChoosingIndex ++;
            this.LoadDino();
        }
    }
    public OnClickDinoPrevBtn()
    {
        if(this.dinoChoosingIndex > 0)
        {
            this.dinoChoosingIndex --;
            this.LoadDino();
        }
    }
    public OnClickDinoDetailBtn()
    {
        
    }
    public OnClickDinoFeedBtn()
    {
        
    }
    // End region DINO

    // Region DINO DETAIL
    public LoadDinoDetail()
    {
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
    // End Region DINO DETAIL

    // Region MONSTER
    public LoadMonsterDetail(event:any, _monsterIndex:any)
    {
        this.monsterChoosingIndex = +_monsterIndex;

        log("monsterIndex : "+this.monsterChoosingIndex);

        this.monsterNodes.forEach(element => {
            element.active = false;
        });
        this.monsterDetailPopup.active = true;
        this.monsterWinRate.string = this.monsterChoosingIndex + " %";
        this.batModel.active = (this.monsterChoosingIndex >=0 && this.monsterChoosingIndex < 5);
        this.spiderKingModel.active =  (this.monsterChoosingIndex == 5);
        if(this.batModel.active == true)
        {
            this.getBatMaterial(this.monsterChoosingIndex,(batMat)=>{
                this.batMeshRender.material = batMat;
            });
        }
    }
    public CloseMonsterDetail()
    {
        this.monsterDetailPopup.active = false;
        this.monsterNodes.forEach(element => {
            element.active = true;
        });
    }
    getBatMaterial(batIndex: any, callbacks: any): void {
        resources.load("Batty/M_Batty_A-00"+batIndex, Material, (err, mat) => {
            callbacks(mat);
        });
    }
    public Fight(monsterIndex:number)
    {
        if(monsterIndex < 0) // Fight in monster detail
        {

        }
        else
        {

        }
    }
    // End Region MONSTER

    // Region ANOTHER : PVE GUIDE, PVE HISTORY
    public LoadPVEGuide()
    {
        this.offCamera3D.active = false;
        this.pveGuidePopup.active = true;
    }

    public ClosePVEGuide()
    {
        this.offCamera3D.active = true;
        this.pveGuidePopup.active = false;
    }

    public LoadPVEHistory()
    {
        this.offCamera3D.active = false;
        this.pveHistoryPopup.active = true;
        this.historyCurrentPage = 1;
        this.historyMaxPage = 10;
        this.LoadPageHistory();
    }

    public LoadPageHistory()
    {
        this.HistoryButtonStatus();
    }

    public ClosePVEHistory()
    {
        this.offCamera3D.active = true;
        this.pveHistoryPopup.active = false;
    }

    public OnClickHistoryPrevPage()
    {
        if(this.historyCurrentPage > 1)
        {
            this.historyCurrentPage --;
            this.LoadPageHistory();
        }
    }

    public OnClickHistoryNextPage()
    {
        if(this.historyCurrentPage<this.historyMaxPage)
        {
            this.historyCurrentPage ++;
            this.LoadPageHistory();
        }
    }

    public HistoryButtonStatus()
    {
        this.pveHistoryNextBtn.interactable = (this.historyCurrentPage<this.historyMaxPage);
        this.pveHistoryPrevBtn.interactable = (this.historyCurrentPage > 1);
    }

    // End Region
}
