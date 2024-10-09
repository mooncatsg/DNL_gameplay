import { _decorator, Component, Node, SkeletalAnimationComponent, log, Vec2, Vec3, CCFloat, systemEvent, SystemEvent, tween, AnimationComponent, Touch, ColliderComponent, ITriggerEvent, AudioClip, AudioSourceComponent, CCInteger, Material, DebugMode, SkinnedMeshRenderer, Label, Prefab, instantiate } from 'cc';
import { DEBUG } from 'cc/env';
import { GameManager } from './GameManager';
import { GameplayController, GameState } from './GameplayController';
import { BattleUI } from './UI/BattleUI';

const { ccclass, property } = _decorator;

const monsterAnim = {
    idle: 'Idle',
    attack: 'Attack',
    die: 'Die',
    happy: 'Happy',
    Headbutt: "Headbutt",
    FlyingAround: "FlyingAroundAtk",
    HitBack: 'HitBack',
    HitBackFar: 'HitBack_Ver2',
    Shot: 'Shot',
    Stun: 'Stun',
    Move: 'Move',
    MinibossHeadButt: "Heatbutt",
    MinibossFootAttack: "FootAttack",
}

export enum MonsterType {
    MONSTER = 0,
    MINI_BOSS = 1
    
}

@ccclass('MonsterController')
export class MonsterController extends Component {

    @property({ type: CCInteger })
    public health: number = 0;

    @property({ type: CCInteger })
    public attackDamage: number = 0;

    @property({ type: CCInteger })
    public defense: number = 0;

    @property({ type: CCFloat })
    public attackSpeed: number = 0;

    @property({ type: SkeletalAnimationComponent })
    public playerAnimComp: SkeletalAnimationComponent = null;

    @property({ type: BattleUI })
    public battleUI: BattleUI = null;

    @property({ type: Node })
    public attackPosNode: Node = null;

    @property({ type: Prefab })
    public attackBullet: Prefab = null;

    @property({ type: Prefab })
    public attackEffect: Prefab = null;

    @property({ type: Number })
    public monsterType: MonsterType = MonsterType.MONSTER;

    private _attackSpeed: number = 0;
    private _currentHealth: number = 0;
    private damaged: number = 1;
    private isDie: boolean = false;

    private InitialPosition: Vec3 = new Vec3(4.5, -0.4, 0);
    private AttackPosition: Vec3 = new Vec3(2.5, -0.4, 0);
    private MiniBossInitialPosition: Vec3 = new Vec3(4.5, 0, 0);
    private MiniBossAttackPosition: Vec3 = new Vec3(2.5, 0, 0);
    private ShotPosition: Vec3 = new Vec3(0.6, 0.6, 0);


    start() {

        this.health = 100;

        this._attackSpeed = this.attackSpeed / 2;
        this._currentHealth = this.health;
        this.battleUI.SetHealth(this.health);
        this.OnPlayAnimIdle();

    }

    update(deltaTime: number) {

        // if (!this.isDie && GameplayController.getInstance().gameState == GameState.PLAYING) {
        //     this._attackSpeed -= deltaTime;

        //     if (this._attackSpeed <= 0) {
        //         this._attackSpeed = this.attackSpeed;
        //         this.Attack();
        //     }
        // }
    }

    onDestroy() {
    }

    //#region  ANIMATION
    OnPlayAnimDie() {
        this.playerAnimComp.play(monsterAnim.die);
    }

    OnPlayAnimAttack() {
        this.playerAnimComp.play(monsterAnim.attack);
    }

    OnPlayAnimIdle() {
        this.playerAnimComp.play(monsterAnim.idle);
    }

    OnPlayAnimHappy() {
        this.playerAnimComp.play(monsterAnim.happy);
    }

    OnPlayAnimShot() {
        this.playerAnimComp.play(monsterAnim.Shot);
    }

    OnPlayAnimHitBack() {
        this.playerAnimComp.play(monsterAnim.HitBack);
    }

    OnPlayAnimHitBackFar() {
        this.playerAnimComp.play(monsterAnim.HitBackFar);
    }

    OnPlayAnimHeadButt() {
        this.playerAnimComp.play(monsterAnim.Headbutt);
    }

    OnPlayAnimFlyingAround() {
        this.playerAnimComp.play(monsterAnim.FlyingAround);
    }

    OnPlayAnimMinibossFootAttack() {
        this.playerAnimComp.play(monsterAnim.MinibossFootAttack);
    }

    OnPlayAnimMinibossHeadButt() {
        this.playerAnimComp.play(monsterAnim.MinibossHeadButt);
    }

    OnPlayAnimStun() {
        this.playerAnimComp.play(monsterAnim.Stun);
    }

    OnPlayAnimMove() {
        this.playerAnimComp.play(monsterAnim.Move);
    }



    onAttackEndListener() {
        this.OnPlayAnimIdle();
    };
    //#endregion


    //#region HEALTH, ATTACK, DIE
    Attack() {
        if (GameplayController.getInstance().stepData.attackType == 1) { // Shot
            if (this.monsterType == MonsterType.MONSTER)
                this.Shot();
            else this.MiniBossShot();
        }
        else if (GameplayController.getInstance().stepData.attackType == 2) { // Headbutt
            if (this.monsterType == MonsterType.MONSTER)
                this.HeadButt();
            else this.MiniBossHeadButt();
        }
        else if (GameplayController.getInstance().stepData.attackType == 3) { // Finish
            if (this.monsterType == MonsterType.MONSTER)
                this.MonsterFlyingAround();
            else this.MiniBossFootAttack();
        }
    }

    Shot() {
        this.OnPlayAnimShot();

        this.scheduleOnce(() => {
            let bullet = instantiate(this.attackBullet);
            bullet.parent = this.node.parent;
            bullet.setWorldPosition(this.attackPosNode.worldPosition);
            bullet.lookAt(this.ShotPosition);
            tween(bullet)
                .to(0.3, { worldPosition: this.ShotPosition }, {
                    easing: 'linear', onComplete: () => {
                        GameplayController.getInstance().AttackPlayer(3);

                        let eff = instantiate(this.attackEffect);
                        eff.parent = this.node.parent;
                        eff.setWorldPosition(bullet.worldPosition);

                        bullet.destroy();
                    }
                })
                .start();
        }, 1.2);
    }

    HeadButt() {
        this.OnPlayAnimMove();

        tween(this.node)
            .to(0.5, { worldPosition: this.AttackPosition }, {
                easing: 'linear', onComplete: () => {
                    this.OnPlayAnimHeadButt();
                    this.scheduleOnce(() => {
                        GameplayController.getInstance().AttackPlayer(3);
                    }, 0.15);


                    // Move to root pos
                    this.scheduleOnce(() => {
                        this.OnPlayAnimMove();

                        tween(this.node)
                            .to(0.5, { worldPosition: this.InitialPosition }, {
                                easing: 'linear', onComplete: () => {
                                    this.OnPlayAnimIdle();
                                }
                            })
                            .start();
                    }, 0.5);
                }
            })
            .start();

    }

    MonsterFlyingAround() {
        this.OnPlayAnimFlyingAround();

        this.scheduleOnce(() => {
            GameplayController.getInstance().AttackPlayer(3);
        }, 1);
    }

    MiniBossShot() {
        this.OnPlayAnimShot();

        this.scheduleOnce(() => {
            let bullet = instantiate(this.attackBullet);
            bullet.parent = this.node.parent;
            bullet.setWorldPosition(this.attackPosNode.worldPosition);
            bullet.lookAt(this.ShotPosition);
            tween(bullet)
                .to(0.2, { worldPosition: this.ShotPosition }, {
                    easing: 'linear', onComplete: () => {
                        GameplayController.getInstance().AttackPlayer(3);

                        let eff = instantiate(this.attackEffect);
                        eff.parent = this.node.parent;
                        eff.setWorldPosition(bullet.worldPosition);

                        bullet.destroy();
                    }
                })
                .start();
        }, 0.8);
    }

    MiniBossHeadButt() {
        this.OnPlayAnimMove();

        tween(this.node)
            .to(0.5, { worldPosition: new Vec3(this.MiniBossAttackPosition.x - 0.5, this.MiniBossAttackPosition.y, this.MiniBossAttackPosition.z) }, {
                easing: 'linear', onComplete: () => {
                    this.OnPlayAnimMinibossHeadButt();
                    this.scheduleOnce(() => {
                        GameplayController.getInstance().AttackPlayer(3);
                    }, 0.15);


                    // Move to root pos
                    this.scheduleOnce(() => {
                        this.OnPlayAnimMove();

                        tween(this.node)
                            .to(0.5, { worldPosition: this.MiniBossInitialPosition }, {
                                easing: 'linear', onComplete: () => {
                                    this.OnPlayAnimIdle();
                                }
                            })
                            .start();
                    }, 0.5);
                }
            })
            .start();
    }

    MiniBossFootAttack() {
        this.OnPlayAnimMove();

        tween(this.node)
            .to(0.5, { worldPosition: this.MiniBossAttackPosition }, {
                easing: 'linear', onComplete: () => {
                    this.OnPlayAnimMinibossFootAttack();
                    this.scheduleOnce(() => {
                        GameplayController.getInstance().AttackPlayer(3);
                    }, 0.8);


                    // Move to root pos
                    this.scheduleOnce(() => {
                        this.OnPlayAnimMove();

                        tween(this.node)
                            .to(0.5, { worldPosition: this.MiniBossInitialPosition }, {
                                easing: 'linear', onComplete: () => {
                                    this.OnPlayAnimIdle();
                                }
                            })
                            .start();
                    }, 1.2);
                }
            })
            .start();
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
        GameplayController.getInstance().GameFinish(true);
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
