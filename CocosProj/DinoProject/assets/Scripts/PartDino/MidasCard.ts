
import { _decorator, Component, Node, Sprite, LabelComponent, Label, SpriteComponent, Button, ButtonComponent, EventHandler } from 'cc';
import { Common } from '../common/Common';
const { ccclass, property } = _decorator;

@ccclass('MidasCard')
export class MidasCard extends Component {
    @property({ type: ButtonComponent })
    public useBtn: Button;

    @property({ type: LabelComponent })
    public nameLbl: Label;

    @property({ type: LabelComponent })
    public idLbl: Label;

    @property({ type: LabelComponent })
    public amountLbl: Label;

    @property({ type: LabelComponent })
    public xrateLbl: Label;

    @property({ type: LabelComponent })
    public speedLbl: Label;
    @property({ type: SpriteComponent })
    public midasBoxSprite: Sprite;
    @property({ type: SpriteComponent })
    public midasBGSprite: Sprite;

    public data:any;

    public Initialize(_data:any): void
    {
        this.data = _data;
        this.useBtn.clickEvents[0].customEventData = this.data["id"];
        this.nameLbl.string = this.data["name"] + "";
        this.idLbl.string = "#" + this.data["id"];
        this.amountLbl.string = this.data["price"] + " ";
        this.xrateLbl.string = "x" + this.data["xRateMin"] + " - " + "x" + this.data["xRateMax"];
        this.speedLbl.string = this.data["speedEarnPercentMin"] + " - " + this.data["speedEarnPercentMax"] + "%";
        Common.getMidasBox(_data["rarity"],(err, spr)=>{this.midasBoxSprite.spriteFrame = spr; });
        Common.getMidasBG(_data["rarity"],(err, spr)=>{this.midasBGSprite.spriteFrame = spr; });
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
