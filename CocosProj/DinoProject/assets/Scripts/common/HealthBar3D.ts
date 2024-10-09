
import { _decorator, Component, Node, Vec3 } from 'cc';
const { ccclass, property } = _decorator;

@ccclass('HealthBar3D')
export class HealthBar3D extends Component {
    // [1]
    // dummy = '';

    // [2]
    // @property
    // serializableDummy = 0;

    @property({ type: Node })
    public healthProgress: Node = null;

    @property({ type: Node })
    public target: Node = null;

    @property({ type: Number })
    public addVec3X: number = 0;

    @property({ type: Number })
    public addVec3Y: number = 1;

    @property({ type: Number })
    public addVec3Z: number = 0;

    private addVec3:Vec3 = Vec3.UP;

    start() {
        this.addVec3 = new Vec3(this.addVec3X,this.addVec3Y,this.addVec3Z);
    }

    update(deltaTime: number) {
        if(this.target)
            this.node.setWorldPosition(this.target.getWorldPosition().add(this.addVec3));
    }
    
    /**
     * setTarget
     */
    public setTarget(_target:Node) {
        this.target = _target;
    }
    /**
     * setHealthValue
     */
    public setHealthValue(value:number) {
        if(value < 0){
            this.node.active = false;
            value = 0;
        }
        this.healthProgress.setScale(new Vec3(1,1,value));
        let pos = new Vec3(0,0.001,(value - 1)/(2*this.node.scale.z));
        this.healthProgress.position = pos;
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
