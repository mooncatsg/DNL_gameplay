
import { _decorator, Component, Node, SkeletalAnimationComponent, director, randomRangeInt, randomRange, __private, tween, Vec3, CCFloat, CCInteger, Quat, random } from 'cc';
import { WBDataManager } from './WB_DataManager';
const { ccclass, property, requireComponent } = _decorator;
import { WBHeroController } from './WB_HeroController';
import { WBGameState, WBManager } from './WB_Manager';


const bossAnim = {
    idle: 'Idle',
    attack1: 'Attack_1',
    attack2: 'Attack_2',
    attack3: 'Attack_1',    
    attack4: 'Attack_1',
    die: 'Die',
    move: 'Move',
    happy: 'Dino_Happy',
}

export enum BossState {
    Idle = 0,
    Targeting,
    MovingToTarget,
    Attacking,
    Waitting,
}

@ccclass('WBBossController')
@requireComponent(SkeletalAnimationComponent)
export class WBBossController extends Component {
    @property({ type: CCFloat})
    public maxhealth:number = 100;
    @property({ type: CCFloat})
    public minDamage:number = 10;
    @property({ type: CCFloat})
    public maxDamage:number = 10;
    @property({ type: CCFloat})
    public waitingTime:number = 1;    
    @property({ type: SkeletalAnimationComponent })
    public animationController: SkeletalAnimationComponent = null;
    @property({ type: Node })
    public target: Node = null;
    @property({ type: Node })
    public parent: Node = null;
    @property({ type: Node })
    public beShootPosNode: Node = null;
    @property({ type: Node })
    public beAttackedPosNode: Node = null;
    

    public currentState: BossState;
    public currentHealth:number;
    public get isDead():boolean {
        return this.currentHealth <= 0;
    }
    private currentWaitingTime:number = 0;


    start () {
        this.currentWaitingTime = randomRange(2,3);        
        this.currentHealth = this.maxhealth;
        this.currentState = BossState.Idle;

        // Rotate to target
        this.lookAtTarget();
    }

    update(deltaTime: number) {
        if (WBManager.getInstance().gameState == WBGameState.PLAYING && this.isDead == false) {
            this.currentWaitingTime -= deltaTime;
            if(this.currentWaitingTime <= 0)
            {
                this.currentWaitingTime = randomRange(this.waitingTime-1,this.waitingTime+1);
                this.Attack();
            }
        }
    }

    //#region ACTION
    /**
     * lookAtTarget
     */
    public lookAtTarget() {
        if(this.target != null)
        {
            const temp:Vec3 = this.node.parent.forward;
            const that= this;
            tween(temp)
            .by(0.5, this.target.getWorldPosition(), {
                'onStart': () => {
                    
                },
                'onUpdate': () => {
                    that.node.parent.lookAt(temp, Vec3.UP);
                },
                'onComplete': () => {
                    that.node.parent.lookAt(this.target.getWorldPosition(), Vec3.UP);                    
                }
            }).start();
        }
    }

    /**
     * Attack
     */
    randomAtk:number = 0;
    public Attack() {        
        if(this.target == null || this.target.getComponent(WBHeroController).isDead)
        {
            this.target = WBManager.getInstance().GetTargetForBoss();
        }
        if(this.target != null && !this.target.getComponent(WBHeroController).isDead)
        {
            this.lookAtTarget();
            this.randomAtk = randomRangeInt(1,5);            
            switch(this.randomAtk)
            {
                case 1:
                    this.OnPlayAnimAttack1();
                    break;
                case 2:
                    this.OnPlayAnimAttack2();
                    break;
                case 3:
                    this.OnPlayAnimAttack3();
                    break;
                case 4:
                    //this.currentWaitingTime += 4; // Add time because this anim 4 seconds
                    this.OnPlayAnimAttack4();
                    break;
            }
        }
    }

    /**
     *  RandomDamage
     */
    public RandomDamage():number {
        if(WBManager.getInstance().isBossDie == true && WBManager.getInstance().heroAmountLeft <= 1 ) // BOSS DIE HERE
            return 0;
        return randomRangeInt(this.minDamage,this.maxDamage);
    }

    /**
     *  Damaged
     * 
     */
    public Damaged(dmg:number) {
        if(this.isDead)
            return;

        if(WBManager.getInstance().isBossDie == true && WBManager.getInstance().heroAmountLeft <= 1) // BOSS DIE HERE
        {
            console.log("BOSS DIE : "+WBManager.getInstance().isBossDie  +" : "+ WBManager.getInstance().heroAmountLeft);
            this.BossDie();
        }
    }

    /**
     * BossDie
     */
    public BossDie() {
        this.currentHealth = 0;
        this.OnPlayAnimDie();
        WBManager.getInstance().CheckFinishGame();
    }
    //#endregion

    //#region FUNCTION IN ANIMATION
    OnPlayAnimIdle() {
        this.animationController.play(bossAnim.idle);
    }
    OnPlayAnimDie() {
        this.animationController.play(bossAnim.die);
    }
    OnPlayAnimAttack1() {
        // Đánh nhẹ, target all range
        this.animationController.play(bossAnim.attack1);
    }
    OnPlayAnimAttack2() {
        // Đánh chí mạng, target 1
        this.animationController.play(bossAnim.attack2);
    }
    OnPlayAnimAttack3() {
        // Đánh nhẹ, target 1
        this.animationController.play(bossAnim.attack3);
    }
    OnPlayAnimAttack4() {
        // Đánh chí mạng, target all
        this.animationController.play(bossAnim.attack4);
    }

    public onMakeDamageInAttack1()
    {
        for(let i=0;i<WBManager.getInstance().heroes.length;i++)
        {
            if(WBManager.getInstance().heroes[i].getComponent(WBHeroController).isDead == false)
            {
                WBManager.getInstance().heroes[i].getComponent(WBHeroController).Damaged(this.RandomDamage() * WBDataManager.getInstance().damageRate);
            }            
        }   
    }
    public onMakeDamageInAttack2()
    {
        if(this.target != null && this.target.active == true)
            this.target.getComponent(WBHeroController).Damaged(this.RandomDamage() * 2 * WBDataManager.getInstance().damageRate);
    }
    public onMakeDamageInAttack3()
    {
        if(this.target != null && this.target.active == true)
            this.target.getComponent(WBHeroController).Damaged(this.RandomDamage() * WBDataManager.getInstance().damageRate);
    }

    public onAttackEndListener() {
        this.OnPlayAnimIdle();
    };
    //#endregion
}
