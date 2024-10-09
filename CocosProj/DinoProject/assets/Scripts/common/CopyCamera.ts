
import { _decorator, Component, Node, CameraComponent, Camera } from 'cc';
const { ccclass, property } = _decorator;

@ccclass('CopyCamera')
export class CopyCamera extends Component {
    // [1]
    // dummy = '';
    @property({ type: CameraComponent })
    public target_cam: Camera;
    @property({ type: CameraComponent })
    public cam: Camera;
    // [2]
    // @property
    // serializableDummy = 0;

    start () {
        // [3]

    }

    update (deltaTime: number) {
        if(this.cam.fov != this.target_cam.fov)
        {
            this.cam.fov = this.target_cam.fov;
        }

        if(this.cam.orthoHeight != this.target_cam.orthoHeight)
        {
            this.cam.orthoHeight = this.target_cam.orthoHeight;
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
