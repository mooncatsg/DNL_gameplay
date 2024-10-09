
import { _decorator, Component, Node, Label } from 'cc';
const { ccclass, property } = _decorator;

@ccclass('PVEHistoryLineController')
export class PVEHistoryLineController extends Component {
    @property({ type: Label })
    public dateTime: Label = null;

    @property({ type: Label })
    public battleId: Label = null;

    @property({ type: Label })
    public dinoId: Label = null;

    @property({ type: Label })
    public monster: Label = null;

    @property({ type: Label })
    public result: Label = null;

    @property({ type: Label })
    public rewardWDNL: Label = null;

    @property({ type: Label })
    public exp: Label = null;

    @property({ type: Label })
    public breedStone: Label = null;
    // [1]
    // dummy = '';

    // [2]
    // @property
    // serializableDummy = 0;

    start () {
        // [3]
    }

    public LoadHistoryLine(_date:string,_battle:string,_dino:string,_monster:string,_result:string,_rewardWDNL:string,_exp:string,_breedStone:string,)
    {
        this.dateTime.string = _date;
        this.battleId.string = _battle;
        this.dinoId.string = _dino;
        this.monster.string = _monster;
        this.result.string = _result;
        this.rewardWDNL.string = _rewardWDNL;
        this.exp.string = _exp;
        this.breedStone.string = _breedStone;
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
