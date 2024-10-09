

import { _decorator, Component, Node, SkeletalAnimationComponent, log, Vec2, Vec3, CCFloat, systemEvent, SystemEvent, tween, AnimationComponent, Touch, ColliderComponent, ITriggerEvent, AudioClip, AudioSourceComponent, CCInteger, Material, DebugMode, SkinnedMeshRenderer, Label, Camera, Quat } from 'cc';
import { DEBUG } from 'cc/env';
import { DinoController } from './DinoController';
import { MonsterParentController } from './MonsterParentController';
import { GameManager } from './GameManager';

const { ccclass, property } = _decorator;

enum GameState {
    PREPARE,
    PLAYING,
    RESULT,
}

enum StepState {
    START,
    ATTACKING,
    FINISH,
}

export { GameState }

@ccclass('GameplayController')
export class GameplayController extends Component {
    public static getInstance(): GameplayController {
        return GameplayController._instance;
    }

    private static _instance: GameplayController;

    @property({ type: Label })
    public statusLabel: Label = null;

    @property({ type: MonsterParentController })
    public monsterParentGameobject: MonsterParentController = null;

    @property({ type: Node })
    public fightingTextLayerNode: Node = null;

    @property({ type: Node })
    public mainCameraTarget: Node = null;

    @property({ type: Node })
    public cameraNode: Node = null;

    @property({ type: Node })
    public prepareLayerNode: Node = null;

    @property({ type: Node })
    public prepareCountList: Node[] = [];

    @property({ type: Node })
    public resultLayerNode: Node = null;

    @property({ type: Node })
    public victoryNode: Node = null;

    @property({ type: Node })
    public defeatNode: Node = null;

    @property({ type: Node })
    public critNode: Node = null;

    @property({ type: Node })
    public missNode: Node = null;

    public currentStepIndex: number;
    public stepState: StepState = StepState.START;
    public stepData: Step;
    public gameState: GameState = GameState.PREPARE;
    private StartCamRotation: Vec3 = new Vec3(0, -45, 0);
    private EndCamRotation: Vec3 = new Vec3(0, 30, 0);
    cameraLocked: boolean = false;


    @property({ type: Vec3 })
    InitialPosition = new Vec3(2.25, 0, 0);



    start() {
        GameplayController._instance = this;
        this.gameState = GameState.PREPARE;
        systemEvent.on(SystemEvent.EventType.TOUCH_MOVE, this.onViewTouchMove, this);

        this.PreparePanelOpen();
    }

    update(deltaTime: number) {

        if (GameplayController.getInstance().gameState == GameState.PLAYING) {
            if (this.stepState == StepState.START) {
                this.executeStep();
            }
        }
    }

    executeStep() {
        this.stepData = new Step(this.currentStepIndex, GameManager.getInstance().getCurrentAttackId(), GameManager.getInstance().getCurrentAttackType(), GameManager.getInstance().getCurrentAttackEffect(), GameManager.getInstance().getCurrentAttackDamage(), GameManager.getInstance().getCurrentAttackHealthLeft(), GameManager.getInstance().getCurrentAttackFallback());
        this.stepState = StepState.ATTACKING;
        if (this.stepData.attackId == 1) // Dino
        {
            if ((this.currentStepIndex + 1) < GameManager.getInstance().getStepCount())
                DinoController.getInstance().Attack();
            else {
                this.RotateCameraToFinishPos();
                this.scheduleOnce(() => {
                    DinoController.getInstance().Attack();
                }, 0.5);

            }
        }
        else // Monster
        {
            this.monsterParentGameobject.currentMonsterObject.Attack();
        }

    }

    CallEffect() {
        if (this.stepData.attackEffectStatus == 2) {
            if (this.stepData.attackId != 1)
                this.critNode.setWorldPosition(DinoController.getInstance().node.getWorldPosition());//.add(Vec3.UP.multiplyScalar(0.5)));
            else
                this.critNode.setWorldPosition(this.monsterParentGameobject.currentMonsterObject.node.getWorldPosition());//.add(Vec3.UP.multiplyScalar(0.5)));
            this.critNode.active = true;
        }

        if (this.stepData.attackEffectStatus == 3) {
            if (this.stepData.attackId != 1)
                this.missNode.setWorldPosition(DinoController.getInstance().node.getWorldPosition());
            else
                this.missNode.setWorldPosition(this.monsterParentGameobject.currentMonsterObject.node.getWorldPosition());
            this.missNode.active = true;
        }
    }

    PreparePanelOpen() {
        let delay = 0.7;
        this.prepareLayerNode.active = true;
        let prepareCount = 0;
        this.PrepareCountStatus(prepareCount);
        this.scheduleOnce(() => {
            prepareCount++;
            this.PrepareCountStatus(prepareCount);
            this.scheduleOnce(() => {
                prepareCount++;
                this.PrepareCountStatus(prepareCount);
                this.scheduleOnce(() => {
                    prepareCount++;
                    this.PrepareCountStatus(prepareCount);
                    this.scheduleOnce(() => {
                        prepareCount++;
                        this.PrepareCountStatus(prepareCount);
                        this.prepareLayerNode.active = false;
                        this.GameStart();
                    }, delay);
                }, delay);
            }, delay);
        }, delay);
    }

    PrepareCountStatus(count: number) {
        for (let i = 0; i < this.prepareCountList.length; i++) {
            this.prepareCountList[i].active = (i == count);
        }
    }

    public AttackEnemy(delay: number) {
        if (DinoController.getInstance() != null && this.monsterParentGameobject.currentMonsterObject != null) {
            this.monsterParentGameobject.currentMonsterObject.Hit(this.stepData.damage);
            this.CallEffect();

            this.scheduleOnce(() => {
                this.currentStepIndex++;

                this.stepState = StepState.START;
            }, delay);
        }
    }

    public AttackPlayer(delay: number) {
        if (DinoController.getInstance() != null && this.monsterParentGameobject.currentMonsterObject != null) {
            DinoController.getInstance().Hit(this.stepData.damage);
            this.CallEffect();

            this.scheduleOnce(() => {
                this.currentStepIndex++;

                this.stepState = StepState.START;
            }, delay);
        }
    }

    public GameStart() {

        tween(this.mainCameraTarget)
            // Delay 1s
            .to(0.4, { eulerAngles: this.StartCamRotation }, { easing: 'linear' })
            .start();

        this.scheduleOnce(() => {
            this.currentStepIndex = 1;

            this.gameState = GameState.PLAYING;
            this.stepState = StepState.START;
        }, 0.5);
    }

    public RotateCameraToFinishPos() {
        this.cameraLocked = true;
        tween(this.mainCameraTarget)
            .to(0.4, { eulerAngles: this.EndCamRotation }, { easing: 'linear' })
            .start();
    }

    public GameFinish(isPlayerWin: boolean) {
        if (this.gameState == GameState.PLAYING) {

            this.gameState = GameState.RESULT;

            this.scheduleOnce(() => {
                // SHOW RESULT

                if (isPlayerWin) {
                    DinoController.getInstance().OnPlayAnimHappy();
                    tween(this.mainCameraTarget)
                        .to(0.4, { eulerAngles: new Vec3(0, 0, 0) }, { easing: 'linear' })
                        .start();
                    tween(this.cameraNode)
                        .delay(0.4)
                        .to(0.4, { position: new Vec3(2.5, 2.5, 0), eulerAngles: new Vec3(-20, 90, 0) }, { easing: 'linear' })
                        .start();
                }
                else {
                    this.monsterParentGameobject.currentMonsterObject.OnPlayAnimHappy();
                    tween(this.mainCameraTarget)
                        .to(0.4, { eulerAngles: new Vec3(0, 0, 0) }, { easing: 'linear' })
                        .start();
                    tween(this.cameraNode)
                        .delay(0.4)
                        .to(0.4, { position: new Vec3(-2.5, 2.5, 0), eulerAngles: new Vec3(-20, -90, 0) }, { easing: 'linear' })
                        .start();
                }

                this.scheduleOnce(() => {
                    this.resultLayerNode.active = true;
                    if (isPlayerWin)
                        this.victoryNode.active = true;
                    else this.defeatNode.active = true;
                }, 1);
            }, 1);
        }
    }

    onViewTouchMove(event: Touch) {
        if (this.gameState != GameState.PLAYING || this.cameraLocked == true)
            return;

        let touchInfo = event.getDelta();
        //this.mainCameraTarget.eulerAngles = new Vec3(0, this.mainCameraTarget.eulerAngles.y + touchInfo.x * 5, 0);

        this.InitialPosition = new Vec3(0, touchInfo.x * 0.3, 0);

        if (this.mainCameraTarget != null) {
            this.mainCameraTarget.setRotationFromEuler(this.mainCameraTarget.eulerAngles.add(this.InitialPosition));
        }
        else {
        }
    }

    onDestroy() {
        //systemEvent.off(SystemEvent.EventType.TOUCH_START, this.onViewTouchStart, this);
        systemEvent.off(SystemEvent.EventType.TOUCH_MOVE, this.onViewTouchMove, this);

    }

    public StatusText(text: string) {
        this.statusLabel.string = text;
    }
}

class Step {
    stepId: number;
    attackId: number;//1 là dino, 2 là monster
    attackType: number;//1 là đánh xa,2 là đánh gần, 3 là đánh kết thúc(chỉ dành cho Dino)
    attackEffectStatus: number;//1 là bình thường, 2 là chí mạng, 3 là miss, 4 là bị choáng (mất lượt tiếp theo)
    damage: number;//lượng máu mất đi
    healthLeft: number;//lượng máu còn lại
    fallbackDistance: number;//con bị đánh lùi lại bao nhiêu ô?

    constructor(_stepId: number, _attackId: number, _attackType: number, _attackEffectStatus: number, _damage: number, _healthLeft: number, _fallback: number) {
        this.stepId = _stepId;
        this.attackId = _attackId;
        this.attackType = _attackType;
        this.attackEffectStatus = _attackEffectStatus;
        this.damage = _damage;
        this.healthLeft = _healthLeft;
        this.fallbackDistance = _fallback;
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
