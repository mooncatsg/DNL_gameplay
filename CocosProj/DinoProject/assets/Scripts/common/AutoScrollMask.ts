
import { _decorator, Component, Node, UITransformComponent, UITransform, Vec3 } from 'cc';
const { ccclass, property } = _decorator;

@ccclass('AutoScrollMask')
export class AutoScrollMask extends Component {
    @property({ type: UITransformComponent })
    public targetTrans: UITransform;
    @property({ type: UITransformComponent })
    public maskTrans: UITransform;
    @property({ type: Number })
    public movementX: number = 2;

    update (deltaTime: number) {
        if(this.targetTrans.contentSize.x > this.maskTrans.contentSize.x + 12)
        {
            let maxPosX = this.targetTrans.contentSize.x + 20;
            let targetPosX = this.targetTrans.node.position.x;
            targetPosX -=  (this.movementX * deltaTime);
            if(targetPosX < -maxPosX)
                targetPosX = maxPosX;
            this.targetTrans.node.setPosition(new Vec3(targetPosX,0,0));
        }
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
