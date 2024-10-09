
import { _decorator, Component, Node, SkeletalAnimationComponent, log, Vec2, Vec3, CCFloat, systemEvent, SystemEvent, tween, AnimationComponent, Touch, ColliderComponent, ITriggerEvent, AudioClip, AudioSourceComponent, CCInteger, Material, DebugMode, SkinnedMeshRenderer, Label, Prefab, instantiate, Animation } from 'cc';
import { DEBUG } from 'cc/env';
import { GameManager } from './GameManager';
import { GameplayController, GameState } from './GameplayController';
import { PartDinoController } from './PartDino/PartDinoController';

import { BattleUI } from './UI/BattleUI';

const { ccclass, property } = _decorator;

const playerAnim = {
    idleA: 'Dino_IdleA',
    attack: 'Dino_Attack',
    die: 'Dino_Die',
    happy: 'Dino_Happy',
    Bite: 'Bite',
    HeadButt: 'HeadButt',
    HitBack: 'HitBack_Ver1',
    HitBackFar: 'HitBack_Ver3',
    Shot: 'Shot',
    Stun: 'Stun',
    Run: 'Dino_Run',
    Jump: 'Jump',
    JumpBack: 'JumpBack'

}



@ccclass('DinoController')
export class DinoController extends Component {

    private static singleton: DinoController;
    public static getInstance(): DinoController {
        return DinoController.singleton;
    }
    
    @property({ type: Node })
    public partDino: Node;

    @property({ type: CCInteger })
    public health: number = 0;

    @property({ type: CCInteger })
    public attackDamage: number = 0;

    @property({ type: CCInteger })
    public defense: number = 0;

    @property({ type: CCFloat })
    public attackSpeed: number = 0;

    @property({ type: CCInteger })
    public materialId: number = 0;

    // @property({ type: SkeletalAnimationComponent })
    // public playerAnimComp: SkeletalAnimationComponent = null;

    // @property({ type: Vec3 })
    // InitialPosition = new Vec3(0, 0, 0);

    // @property({ type: Material })
    // public dinoFaceMaterialArray: Material[] = [];

    // @property({ type: Material })
    // public dinoMaterialArray: Material[] = [];

    // @property({ type: SkinnedMeshRenderer })
    // public faceMeshRender: SkinnedMeshRenderer;
    // @property({ type: SkinnedMeshRenderer })
    // public teethMeshRender: SkinnedMeshRenderer;
    // @property({ type: SkinnedMeshRenderer })
    // public backMeshRender: SkinnedMeshRenderer;
    // @property({ type: SkinnedMeshRenderer })
    // public bodyMeshRender: SkinnedMeshRenderer;

    @property({ type: BattleUI })
    public battleUI: BattleUI = null;

    @property({ type: Node })
    public attackPosNode: Node = null;

    @property({ type: Prefab })
    public attackBullet: Prefab = null;

    @property({ type: Prefab })
    public attackEffect: Prefab = null;

    @property({ type: CCInteger })
    smoothness: number = 1;

    private _attackSpeed: number = 0;
    private _currentHealth: number = 0;
    private damaged: number = 1;
    private isDie: boolean = false;

    private InitialPosition: Vec3 = new Vec3(0, 0, 0);
    private AttackPosition: Vec3 = new Vec3(3, 0, 0);
    private BulletTargetPosition: Vec3 = new Vec3(4, 0.5, 0);


    //private _times = 1;
    // [1]
    // dummy = '';

    // [2]
    // @property
    // serializableDummy = 0;

    start() {
        DinoController.singleton = this;
        this.InitialPosition = new Vec3(this.node.position.x, this.node.position.y, this.node.position.z);

        //systemEvent.on(SystemEvent.EventType.TOUCH_START, this.onViewTouchStart, this);
        systemEvent.on(SystemEvent.EventType.TOUCH_MOVE, this.onViewTouchMove, this);

        this.health = 100;
        this._attackSpeed = this.attackSpeed / 2;
        this._currentHealth = this.health;
        this.battleUI.SetHealth(this.health);
        this.OnPlayAnimIdle();

    }

    update(deltaTime: number) {

        // if (GameplayController.getInstance().gameState == GameState.PLAYING) {
        //     this._attackSpeed -= deltaTime;

        //     if (this._attackSpeed <= 0) {
        //         this._attackSpeed = this.attackSpeed;
        //         this.Attack();
        //     }

        // }
    }

    initDinoMaterial(dinoId: number) {

        switch (dinoId.toString()) {
            case "0001":
                this.materialId = 9;
                break;
            case "0002":
                this.materialId = 14;
                break;
            case "0003":
                this.materialId = 0;
                break;
            case "0004":
                this.materialId = 21;
                break;
            case "0005":
                this.materialId = 23;
                break;

            case "1001":
                this.materialId = 1;
                break;
            case "1002":
                this.materialId = 19;
                break;
            case "1003":
                this.materialId = 10;
                break;
            case "1004":
                this.materialId = 5;
                break;
            case "1005":
                this.materialId = 15;
                break;

            case "2001":
                this.materialId = 24;
                break;
            case "2002":
                this.materialId = 16;
                break;
            case "2003":
                this.materialId = 8;
                break;
            case "2004":
                this.materialId = 11;
                break;
            case "2005":
                this.materialId = 20;
                break;
        }


        // if (this.materialId < this.dinoMaterialArray.length) {
        //     this.faceMeshRender.material = this.dinoFaceMaterialArray[this.materialId];

        //     this.teethMeshRender.material = this.dinoMaterialArray[this.materialId];
        //     this.backMeshRender.material = this.dinoMaterialArray[this.materialId];
        //     this.bodyMeshRender.material = this.dinoMaterialArray[this.materialId];
        // }
        // else {
        //     this.materialId = 0;
        // }
    }

    onViewTouchMove(event: Touch) {


    }
    onDestroy() {
        //systemEvent.off(SystemEvent.EventType.TOUCH_START, this.onViewTouchStart, this);
        systemEvent.off(SystemEvent.EventType.TOUCH_MOVE, this.onViewTouchMove, this);

    }

    //#region  ANIMATION
    OnPlayAnimDie() {
        //this.playerAnimComp.play(playerAnim.die);
        this.partDino.getComponent(PartDinoController).OnPlayAnimDie();
    }

    OnPlayAnimAttack() {
        //this.playerAnimComp.play(playerAnim.attack);
        this.partDino.getComponent(PartDinoController).OnPlayAnimBite();
    }

    OnPlayAnimIdle() {
        //this.playerAnimComp.play(playerAnim.idleA);
        this.partDino.getComponent(PartDinoController).OnPlayAnimIdle();
    }

    OnPlayAnimHappy() {
        //this.playerAnimComp.play(playerAnim.happy);
        this.partDino.getComponent(PartDinoController).OnPlayAnimIdle();
    }

    OnPlayAnimBite() {
        //this.playerAnimComp.play(playerAnim.Bite);
        this.partDino.getComponent(PartDinoController).OnPlayAnimBite();

    }

    OnPlayAnimShot() {
        //this.playerAnimComp.play(playerAnim.Shot);
        this.partDino.getComponent(PartDinoController).OnPlayAnimShot();
    }

    OnPlayAnimHeadButt() {
        //this.playerAnimComp.play(playerAnim.HeadButt);
        this.partDino.getComponent(PartDinoController).OnPlayAnimHeadButt();
    }

    OnPlayAnimHitBack() {
        //this.playerAnimComp.play(playerAnim.HitBack);
        this.partDino.getComponent(PartDinoController).OnPlayAnimHitBack();
    }

    OnPlayAnimHitBackFar() {
        //this.playerAnimComp.play(playerAnim.HitBackFar);
        this.partDino.getComponent(PartDinoController).OnPlayAnimHitBack();
    }

    OnPlayAnimStun() {
        //this.playerAnimComp.play(playerAnim.Stun);
        this.partDino.getComponent(PartDinoController).OnPlayAnimStun();
    }

    OnPlayAnimRun() {
        //this.playerAnimComp.play(playerAnim.Run);
    }

    OnPlayAnimJump() {
        //this.playerAnimComp.play(playerAnim.Jump);
        this.partDino.getComponent(PartDinoController).OnPlayAnimJump();
    }

    OnPlayAnimJumpBack() {
        //this.playerAnimComp.play(playerAnim.JumpBack);
        this.partDino.getComponent(PartDinoController).OnPlayAnimJumpBack();
    }
    onAttackEndListener() {
        //this.statusLabel.string = "END : " + this._times++ + " times";
        this.OnPlayAnimIdle();
    };
    //#endregion



    //#region HEALTH, ATTACK, DIE
    Attack() {
        if (GameplayController.getInstance().stepData.attackType == 1) { // Shot
            this.Shot();
        }
        else if (GameplayController.getInstance().stepData.attackType == 2) { // Headbutt
            this.HeadButt();
        }
        else if (GameplayController.getInstance().stepData.attackType == 3) { // Finish
            this.SpecialAttack();
        }
    }

    Shot() {

        this.OnPlayAnimAttack();

        this.scheduleOnce(() => {
            let bullet = instantiate(this.attackBullet);
            bullet.parent = this.node.parent;
            bullet.setWorldPosition(this.attackPosNode.worldPosition);
            bullet.lookAt(this.BulletTargetPosition);
            tween(bullet)
                .to(0.3, { worldPosition: this.BulletTargetPosition }, {
                    easing: 'linear', onComplete: () => {
                        GameplayController.getInstance().AttackEnemy(3);

                        let eff = instantiate(this.attackEffect);
                        eff.parent = this.node.parent;
                        eff.setWorldPosition(bullet.worldPosition);

                        bullet.destroy();
                    }
                })
                .start();
        }, 0.2);

    }

    HeadButt() {
        this.OnPlayAnimJump();

        tween(this.node)
            .to(0.8, { worldPosition: this.AttackPosition }, {
                easing: 'linear', onComplete: () => {
                    this.OnPlayAnimHeadButt();
                    this.scheduleOnce(() => {
                        GameplayController.getInstance().AttackEnemy(3);
                    }, 0.5);
                }
            })
            .start();

        this.scheduleOnce(() => {
            this.OnPlayAnimJumpBack();

            tween(this.node)
                .to(0.8, { worldPosition: this.InitialPosition }, {
                    easing: 'linear', onComplete: () => {
                        this.OnPlayAnimIdle();
                    }
                })
                .start();

        }, 2);
    }

    SpecialAttack() {
        this.OnPlayAnimShot();

        let bullet = instantiate(this.attackBullet);
        this.scheduleOnce(() => {

            bullet.parent = this.node.parent;
            bullet.setWorldPosition(this.attackPosNode.worldPosition);
            bullet.lookAt(this.BulletTargetPosition);
            tween(bullet)
                .to(0.3, { worldPosition: this.BulletTargetPosition }, {
                    easing: 'linear', onComplete: () => {
                        GameplayController.getInstance().AttackEnemy(3);

                        let eff = instantiate(this.attackEffect);
                        eff.parent = this.node.parent;
                        eff.setWorldPosition(bullet.worldPosition);

                        bullet.destroy();
                    }
                })
                .start();
        }, 1.5);
    }

    // HitFirst(hitDamage: number) {
    //     // Set die trước khi dính damage để ko tấn công nữa
    //     this.damaged = (hitDamage - this.defense);
    //     if (this.damaged < 0)
    //         this.damaged = 1;
    //     if (this._currentHealth <= this.damaged)
    //         this.isDie = true;
    // }

    Hit(hitDamage: number) {
        this.damaged = hitDamage;
        if (this.damaged < 0)
            this.damaged = 1;
        this._currentHealth -= this.damaged;
        this.battleUI.updateHealthBar(this._currentHealth, this.health);
        if (this._currentHealth <= 0) {
            this.Die();
        }
        else {
            this.Fallback(GameplayController.getInstance().stepData.fallbackDistance);
        }
    }

    Fallback(fallbackDistance: number) {

        if (fallbackDistance == 1)
            this.OnPlayAnimHitBack();
        else if (fallbackDistance == 2)
            this.OnPlayAnimHitBackFar();
    }

    Die() {
        this.battleUI.updateHealthBar(0, this.health);
        this.OnPlayAnimDie();
        GameplayController.getInstance().GameFinish(false);
    }
    //#endregion
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
