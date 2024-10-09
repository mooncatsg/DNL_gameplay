
import { _decorator, Component,Prefab, Node, SkeletalAnimationComponent,log, director, randomRangeInt, randomRange, __private, tween, Vec3, CCFloat, CCInteger, Quat, instantiate } from 'cc';
import { HealthBar3D } from '../common/HealthBar3D';
import { PartDinoController } from '../PartDino/PartDinoController';
import { WBBossController } from './WB_BossController';
const { ccclass, property, requireComponent } = _decorator;
import { WBManager,WBGameState } from './WB_Manager';


const heroAnim = {
    idle: 'Idle',
    attack1: 'Attack',
    attack2: 'JumpShot',
    attack3: 'Shot2Times',
    attack4: 'Bitting2Times',
    die: 'Die',
}

export enum HeroState {
    Idle = 0,
    Targeting,
    MovingToTarget,
    Attacking,
    Waitting,
}

@ccclass('WBHeroController')
@requireComponent(SkeletalAnimationComponent)
export class WBHeroController extends Component {
    @property({ type: CCFloat})
    public maxhealth:number = 100;
    @property({ type: CCFloat})
    public damage:number = 10;
    @property({ type: CCFloat})
    public waitingTime:number = 1;    
    @property({ type: SkeletalAnimationComponent })
    public animationController: SkeletalAnimationComponent = null;
    @property({ type: Prefab })
    public attackBullet: Prefab = null;
    @property({ type: Prefab })
    public attackEffect: Prefab = null;
    @property({ type: Node })
    public attackPosNode: Node = null;
    @property({ type: Node })
    public attackPosNodeHigh: Node = null;
    @property({ type: HealthBar3D })
    public healthBar: HealthBar3D = null;

    public currentState: HeroState;
    public currentHealth:number;
    public get isDead():boolean {
        return this.currentHealth <= 0;
    }
    private currentWaitingTime:number = 0;
    private rootPos:Vec3;

    start () {
        this.currentWaitingTime = randomRange(this.waitingTime-1,this.waitingTime+1);
        this.currentHealth = this.maxhealth;
        this.OnPlayAnimIdle();
        this.rootPos = new Vec3(this.node.worldPosition.x,this.node.worldPosition.y,this.node.worldPosition.z);       
        this.lookAtTarget();
        this.healthBar.node.active = true;
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

    public LoadData(expressTraits:string, rarity: number, nftId: number)
    {
        log("LoadData : "+ expressTraits + " - "+ rarity + " - " + nftId);
        this.animationController.getComponent(PartDinoController).loadDataByTrait(expressTraits,rarity,nftId);

    }

    /**
     * Attack
     */
     randomAtk:number = 0;
     public Attack() {
         if(WBManager.getInstance().boss != null && !WBManager.getInstance().boss.getComponent(WBBossController).isDead)
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
                     this.OnPlayAnimAttack4();
                     break;
             }
         }
     }

    public lookAtTarget() {
        if(WBManager.getInstance().boss != null)
        {            
            const temp:Vec3 = this.node.parent.forward;
            const that= this;
            tween(temp)
            .by(0.5, WBManager.getInstance().boss.getWorldPosition(), {
                'onStart': () => {
                    
                },
                'onUpdate': () => {
                    that.node.parent.lookAt(temp, Vec3.UP);
                },
                'onComplete': () => {
                    that.node.parent.lookAt(WBManager.getInstance().boss.getWorldPosition(), Vec3.UP);
                    
                }
            }).start();
        }
    }

    //#region ACTION
    /**
     *  Damaged
     * 
     */
    public Damaged(dmg:number) {
        if(this.currentHealth <= 0)
            return;
        this.currentHealth -= dmg;
        if(this.healthBar != null)
            this.healthBar.setHealthValue(this.currentHealth/this.maxhealth);
        if(this.isDead)
        {
            WBManager.getInstance().heroAmountLeft --;
            this.OnPlayAnimDie();
            this.scheduleOnce(()=>{
                this.node.active = false;
            },1);
            WBManager.getInstance().CheckFinishGame();

        }
    }
    //#endregion

    //#region FUNCTION IN ANIMATION
    OnPlayAnimIdle() {
        this.animationController.play(heroAnim.idle);
    }
    OnPlayAnimDie() {
        this.animationController.play(heroAnim.die);
    }
    OnPlayAnimAttack1() {
        this.bulletPos = this.attackPosNode.worldPosition;
        this.animationController.play(heroAnim.attack1);
    }
    OnPlayAnimAttack2() {
        //Nhảy lên bắn 1 lần
        this.bulletPos = this.attackPosNodeHigh.worldPosition;
        this.animationController.play(heroAnim.attack2);
    }
    OnPlayAnimAttack3() {
        //Nhảy lên bắn 2 lần
        this.bulletPos = this.attackPosNode.worldPosition;
        this.animationController.play(heroAnim.attack3);
    }
    OnPlayAnimAttack4() {
        //Nhảy lên bắn 3 lần
        this.bulletPos = this.attackPosNode.worldPosition;
        this.animationController.play(heroAnim.attack4);
    }

    /**
     * ShootBullet
     */
    bulletPos:Vec3;
    public ShootBullet() {

        let bullet = instantiate(this.attackBullet);
            bullet.parent = this.node.parent;            
            bullet.setWorldPosition(this.bulletPos);
            bullet.lookAt(WBManager.getInstance().boss.getComponent(WBBossController).beShootPosNode.worldPosition);
            tween(bullet)
                .to(0.2, { worldPosition: WBManager.getInstance().boss.getComponent(WBBossController).beShootPosNode.worldPosition}, {
                    easing: 'linear', onComplete: () => {
                        
                        let eff = instantiate(this.attackEffect);
                        eff.parent = this.node.parent;
                        eff.setWorldPosition(bullet.worldPosition);
                        
                        // Boss damaged
                        this.onMakeDamageInAttack();

                        bullet.destroy();
                    }
                })
                .start();
    }

    public MoveToAttack() {
        tween(this.node).to(0.6, { worldPosition: WBManager.getInstance().boss.getComponent(WBBossController).beAttackedPosNode.worldPosition}, {easing: 'linear'}).start();
    }
    public MoveToRootPos() {
        tween(this.node).to(0.6, { worldPosition: this.rootPos}, {easing: 'linear'}).start();
    }

    public onMakeDamageInAttack()
    {     
        if(WBManager.getInstance().boss.getComponent(WBBossController).isDead == false)
        {
            WBManager.getInstance().boss.getComponent(WBBossController).Damaged(this.damage);
        }
    }

    onAttackEndListener() {
        this.OnPlayAnimIdle();
    };

    OnGoToIdleState() {
        this.OnPlayAnimIdle();
    };
    //#endregion
}