
import { _decorator, Component, Node, Vec3, tween } from 'cc';
const { ccclass, property } = _decorator;

@ccclass('RotateToTarget')
export class RotateToTarget extends Component {
    // [1]
    // dummy = '';

    // [2]
    // @property
    // serializableDummy = 0;
    @property({ type: Node })
    public target: Node;

    start () {
        // [3]
       
    }

    update (deltaTime: number) {
        this.node.lookAt(this.target.getWorldPosition());
    }

    onEnable(){
        tween(this.node)
        .delay(0.4)
        .to(0.4, { position: new Vec3(0, 1, 0)}, { easing: 'linear' })
        .start();

        this.scheduleOnce(()=>{
            this.node.active = false;
        }, 0.7);
    }

    onDisable(){

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
