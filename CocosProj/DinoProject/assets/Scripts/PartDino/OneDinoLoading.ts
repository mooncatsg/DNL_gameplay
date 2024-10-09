
import { _decorator, Component, Node, director } from 'cc';
const { ccclass, property } = _decorator;

@ccclass('OneDinoLoading')
export class OneDinoLoading extends Component {
    start () {
        window.cocosDinoReload = function() {director.loadScene("DinoLoading");}
        window.cocosDinoReset = function() {director.loadScene("DinoReset");}
        this.LoadAScene("DinoPart");
    }
    

    /**
     * LoadScene
     */
    LoadAScene(sceneName:string) {
        director.preloadScene(sceneName, function (err, scene) {
            // if(err)
                director.loadScene(sceneName);
            // else
            //     director.runScene(scene);
        });
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
