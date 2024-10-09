
import { _decorator, Component, Node, Label, Sprite, SpriteFrame,log, Color } from 'cc';
import { Common } from '../../common/Common';
const { ccclass, property } = _decorator;

@ccclass('StoneItemController')
export class StoneItemController extends Component {
    // [1]
    // dummy = '';

    // [2]
    // @property
    // serializableDummy = 0;
    @property({ type: Label })
    public nftIdLabel: Label = null;

    @property({ type: Label })
    public itemNameLabel: Label = null;

    @property({ type: Sprite })
    public itemSprite: Sprite = null;

    @property({ type: Node })
    public highlightNode: Node = null;

    stoneNFT :number= 0;
    stoneType :number= 0;

    start () {
        // [3]
    }

    public loadData(_nftId:number,_itemName:string,_itemType:number)
    {
        this.stoneNFT = _nftId;
        this.stoneType = _itemType;

        this.nftIdLabel.string = _nftId + "";
        this.itemNameLabel.string = _itemName;   
        Common.GetStoneIcon(_itemType, (err,spr) =>{
            this.itemSprite.spriteFrame = spr;
        });     
    }

    public Highlight(isHighlighted :boolean)
    {
        this.highlightNode.active = isHighlighted;
    }


    // update (deltaTime: number) {
    //     // [4]
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
