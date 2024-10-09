
import { _decorator, Component, Node } from 'cc';
import { PartDinoController } from './PartDinoController';
const { ccclass, property } = _decorator;

@ccclass('PartDinoAnimation')
export class PartDinoAnimation extends Component {

    
    @property({ type: PartDinoController })
    public partDino: PartDinoController;
    // [1]
    // dummy = '';

    // [2]
    // @property
    // serializableDummy = 0;

    start () {
        // [3]
    }

    OnGoToIdleState()
    {
        if(this.partDino != null)
        {
            this.partDino.OnGoToIdleState();
        }
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
