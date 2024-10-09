
import { _decorator, Component, Node, SkeletalAnimationComponent, log, Vec2, Vec3, CCFloat, systemEvent, SystemEvent, tween, AnimationComponent, Touch, ColliderComponent, ITriggerEvent, AudioClip, AudioSourceComponent, CCInteger, Material, DebugMode, SkinnedMeshRenderer, Label, Prefab, instantiate } from 'cc';
import { DEBUG } from 'cc/env';
import { GameManager } from './GameManager';
import { GameplayController, GameState } from './GameplayController';
import { BattleUI } from './UI/BattleUI';
import { MonsterController } from './MonsterController';

const { ccclass, property } = _decorator;


@ccclass('MonsterParentController')
export class MonsterParentController extends Component {
    @property({ type: MonsterController })
    public monsterGameobject: MonsterController = null;

    @property({ type: MonsterController })
    public miniBossGameobject: MonsterController = null;

    public currentMonsterObject: MonsterController = null;

    start() {
        if (GameManager.getInstance().monsterType == 1) {
            this.currentMonsterObject = this.monsterGameobject;
        }
        else {
            this.currentMonsterObject = this.miniBossGameobject;
        }
        this.currentMonsterObject.node.active = true;
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
