
import { _decorator, Component, Node,Vec3,tween,SkeletalAnimationComponent,Prefab,instantiate ,randomRangeInt,log, DebugMode} from 'cc';
import { HealthBar3D } from '../common/HealthBar3D';
import { HealthBar3DPVP } from '../common/HealthBar3DPVP';
const { ccclass, property } = _decorator;
import { PartDinoController } from '../PartDino/PartDinoController';
import { PVPManager,PVPGameState } from './PVPManager';

const heroAnim = {
    Idle: 'Idle',
    
    Attack: 'Attack',
    JumpShot: 'JumpShot',
    Bite: 'Bite',
    HeadButt: 'HeadButt',
    shot: 'shot',
    Bitting2Times:"Bitting2Times",

    Run: 'Run',
    Jump: 'Jump',
    JumpBack: 'JumpBack',
    HitBack: 'Hitback',

    Die: 'Die',
    Happy: 'Happy',
    Stun: 'Stun'
}

@ccclass('PVPHeroController')
export class PVPHeroController extends Component {

    @property({ type: SkeletalAnimationComponent })
    public animationController: SkeletalAnimationComponent = null;
    @property({ type: Prefab })
    public attackBullet: Prefab = null;
    @property({ type: Prefab })
    public attackEffect: Prefab = null;

    @property({ type: Node })
    public bulletNode: Node = null;
    @property({ type: Node })
    public attackedNode: Node = null;
    @property({ type: HealthBar3DPVP })
    public healthBar: HealthBar3DPVP = null;

    public moving:boolean = false;
    public targeting:boolean = false;
    public targets:PVPHeroController[] = [];
    attackTurnDataTargets:string = "";
    attackType:number = 0;
    
    private rootPos:Vec3;
    public nftId:number;
    public isBlockingNextTurn():boolean
    {
        return this.moving || this.targeting;
    }
    public initialize (pos:Node, expressTraits:any, rarity: number, _nftId: number) {
        this.node.parent.setWorldPosition(pos.getWorldPosition());
        this.rootPos = new Vec3(pos.worldPosition.x, pos.worldPosition.y, pos.worldPosition.z);
        this.nftId = _nftId;
        this.LoadData(expressTraits, rarity, _nftId);
        this.healthBar.node.active = true;
    }    
    
    public LoadData(expressTraits:any, rarity: number, nftId: number)
    {
        log("LoadData : "+ expressTraits + " - "+ rarity + " - " + nftId);
        this.animationController.getComponent(PartDinoController).loadDataByTrait(expressTraits, rarity, nftId);
    }
    //#region ATTACK
    public SkillActivate(type:number)
    {
        let skillType = Math.round((type-1)/5 + 1);
        let rarity = (type-1)%5;
        console.log("skillType : "+skillType);
        switch (skillType) {
            case 1:   
                this.AttackShot(this.targets);
                break;
            case 2:
                this.AttackShot(this.targets);
                break;
            case 3:
                if(this.targets != null && this.targets.length > 0)
                {
                    this.SkillAttackMeleeTwice(this.targets[0]);                    
                    this.targets[0].targeting = true;
                }                
                break;
            case 4:
                this.AttackShot(this.targets);
                break;
            case 5:            
                this.AttackShot(this.targets);    
                break;
            case 6:            
                this.AttackShot(this.targets);
                break;
            case 7:
                this.AttackShot(this.targets);
                break;
            case 8:
                this.AttackShot(this.targets);
                break;
            case 9:
                this.OnPlayAnimHappy();
                this.scheduleOnce(() => {
                    this.SkillHeal(this.targets);
                },1);
                
                break;
            case 0:
                this.OnPlayAnimHappy();
                this.scheduleOnce(() => {
                    this.SkillArmor(this.targets);
                },1);
                break;
        
            default:
                break;
        }
    }

    public SkillHeal(allies:PVPHeroController[])
    {
        this.OnPlayAnimIdle();
        for(let i=0;i<allies.length;i++)
        {
            let healObject = instantiate(PVPManager.getInstance().healPrefab);
            healObject.parent = allies[i].node.parent;
            healObject.setWorldPosition(allies[i].node.worldPosition); 
            this.scheduleOnce(() => {
                allies[i].Heal(80,40);
            },0.7);
        }
    }
    public SkillArmor(allies:PVPHeroController[])
    {
        this.OnPlayAnimIdle();
        for(let i=0;i<allies.length;i++)
        {
            let healObject = instantiate(PVPManager.getInstance().healPrefab);
            healObject.parent = allies[i].node.parent;
            healObject.setWorldPosition(allies[i].node.worldPosition); 
            this.scheduleOnce(() => {
                allies[i].Heal(80,40);
            },0.7);
        }
    }
    public SkillAttackMeleeTwice(enemy:PVPHeroController)
    {
        // Enemy look at
        this.lookAtTarget(enemy.node);
        enemy.lookAtTarget(this.node);

        // Move to enemy
        this.scheduleOnce(() => {
            this.moving = true;
            this.MoveToAttack(enemy);
            this.OnPlayAnimBitting2Times();
        },0.2);

        let attackTime = 2;
        // Attack
        this.scheduleOnce(() => {
            attackTime = 2;
            enemy.Damaged(this.attackTurnDataTargets[0]["hp"],this.attackTurnDataTargets[0]["remainHp"] + 50, this.attackTurnDataTargets[0]["mana"]);
            this.scheduleOnce(() => {
                enemy.Damaged(this.attackTurnDataTargets[0]["hp"],this.attackTurnDataTargets[0]["remainHp"], this.attackTurnDataTargets[0]["mana"]);
                },0.5);
        },1.4);

        // Move back
        this.scheduleOnce(() => {
            this.MoveToRootPos();
            enemy.targeting = false;
        }, attackTime);
    }
    //#endregion


    //#region ATTACK
    public Attack(type:number, attackTargets:PVPHeroController[],_turnDataTargets:string, mana:number)
    {
        this.attackType = type;
        this.targets = attackTargets;
        this.attackTurnDataTargets = _turnDataTargets;        
        this.healthBar.setManaValue(mana);
        if(this.attackType == 0)
        {
            this.AttackMelee(this.targets[0]);
            this.targets[0].targeting = true;
        }
        else
        {            
            this.SkillActivate(this.attackType);            
        }
    }


    public AttackShot(enemies:PVPHeroController[])
    {
        log("AttackShot :"+enemies.length);
        if(enemies != null && enemies.length > 0)
        {
            // Look at
            this.lookAtTarget(enemies[0].node);
            for(let i=0;i<enemies.length;i++)
            {
                enemies[i].lookAtTarget(this.node);
                enemies[i].targeting = true;
            }
            // Shot
            this.scheduleOnce(() => {
                this.OnPlayAnimShot();
            },0.4);            
        }
    }

    public AttackMelee(enemy:PVPHeroController)
    {
        // Enemy look at
        this.lookAtTarget(enemy.node);
        enemy.lookAtTarget(this.node);

        // Move to enemy
        this.scheduleOnce(() => {
            this.moving = true;
            this.OnPlayAnimJump();
            this.MoveToAttack(enemy);
        },0.2);

        let attackTime = 2;
        // Attack
        this.scheduleOnce(() => {
            let randomAtk = randomRangeInt(1,3);
            console.log("randomAtk :" + randomAtk);
            
            if(randomAtk == 1)
            {
                attackTime = 1.8;
                 this.OnPlayAnimHeadButt();
                 this.scheduleOnce(() => {
                    enemy.Damaged(this.attackTurnDataTargets[0]["hp"],this.attackTurnDataTargets[0]["remainHp"], this.attackTurnDataTargets[0]["mana"]);
                 },0.3);                 
            }                
            else
            {
                attackTime = 2;
                this.OnPlayAnimAttack();
                this.scheduleOnce(() => {
                    enemy.Damaged(this.attackTurnDataTargets[0]["hp"],this.attackTurnDataTargets[0]["remainHp"],this.attackTurnDataTargets[0]["mana"]);
                 },0.3);
            }
        },1);

        // Move back
        this.scheduleOnce(() => {
            this.OnPlayAnimJumpBack();
            this.MoveToRootPos();
            enemy.targeting = false;
        }, attackTime);
    }

    public ShootBullet()
    {
        let skillType = this.attackType%10;
        if(skillType == 1 || skillType == 6)
        {
            this.ShootBulletMulti();
        }
        else
        {
            if(this.targets != null && this.targets.length > 0)
            {
                this.ShootBulletSingle(this.targets[0],true);
            }            
        }
    }

    public ShootBulletSingle(enemy:PVPHeroController,skillActivate:boolean = false,bulletStartNode:Node = null)
    {        
        log("ShootBulletSingle : ");
        let bullet = instantiate(this.attackBullet);
        bullet.parent = this.node.parent;
        if(bulletStartNode == null)
            bullet.setWorldPosition(this.bulletNode.worldPosition);
        else
            bullet.setWorldPosition(bulletStartNode.worldPosition);
        bullet.lookAt(enemy.GetAttackedPosition());
        tween(bullet)
            .to(0.5, { worldPosition: enemy.GetAttackedPosition()}, {
                easing: 'linear', onComplete: () => {
                    
                    let eff = instantiate(this.attackEffect);
                    eff.parent = this.node.parent;
                    eff.setWorldPosition(bullet.worldPosition);
                    
                    enemy.OnPlayAnimHitBack();

                    // Damaged
                    let hp = 0; let remainHp=0;let mana =0;
                    for (let index = 0; index < this.attackTurnDataTargets.length; index++) 
                    {
                        if(enemy.nftId == this.attackTurnDataTargets[index]["nftId"])
                        {
                            hp = this.attackTurnDataTargets[index]["hp"];
                            remainHp = this.attackTurnDataTargets[index]["remainHp"];
                            mana = this.attackTurnDataTargets[index]["mana"];
                            break;
                        }
                    }
                    enemy.Damaged(hp,remainHp,mana);
                    
                    // SKILLS
                    if(skillActivate)
                    {
                        let skillType = this.attackType%10;
                        if(skillType == 2)
                        {
                            // Explosion
                            let explosion = instantiate(PVPManager.getInstance().explosionPrefab2);
                            explosion.parent = enemy.bulletNode.parent;
                            //explosion.node.worldPosition = enemy.bulletNode.worldPosition;
                            // Damaged
                            this.scheduleOnce(() => {
                                if(this.targets.length > 1)
                                {
                                    for(let i=1;i<this.targets.length;i++)
                                    {
                                        let hp = 0; let remainHp=0;
                                        for (let index = 0; index < this.attackTurnDataTargets.length; index++) 
                                        {
                                            if(this.targets[i].nftId == this.attackTurnDataTargets[index]["nftId"])
                                            {
                                                hp = this.attackTurnDataTargets[index]["hp"];
                                                remainHp = this.attackTurnDataTargets[index]["remainHp"];
                                                mana = this.attackTurnDataTargets[index]["mana"];
                                                break;
                                            }
                                        }
                                        this.targets[i].Damaged(hp,remainHp,mana);
                                    }
                                }
                            }, 2);
                        }
                        else if(skillType == 7 || skillType == 5)
                        {
                            // Explosion
                            let explosion = instantiate(PVPManager.getInstance().explosionPrefab3);
                            explosion.parent = enemy.bulletNode.parent;
    
                            // Damaged
                            this.scheduleOnce(() => {
                                if(this.targets.length > 1)
                                {
                                    for(let i=1;i<this.targets.length;i++)
                                    {
                                        let hp = 0; let remainHp=0;let mana =0;
                                        for (let index = 0; index < this.attackTurnDataTargets.length; index++) 
                                        {
                                            if(this.targets[i].nftId == this.attackTurnDataTargets[index]["nftId"])
                                            {
                                                hp = this.attackTurnDataTargets[index]["hp"];
                                                remainHp = this.attackTurnDataTargets[index]["remainHp"];
                                                mana = this.attackTurnDataTargets[index]["mana"];
                                                break;
                                            }
                                        }
                                        this.targets[i].Damaged(hp,remainHp,mana);
                                    }
                                }
                            }, 1.5);
                        }
                        else if(skillType == 8)
                        {
                            // Explosion
                            let explosion = instantiate(PVPManager.getInstance().explosionPrefab1);
                            explosion.parent = enemy.bulletNode.parent;
    
                            // Damaged
                            this.scheduleOnce(() => {
                                if(this.targets.length > 1)
                                {
                                    for(let i=1;i<this.targets.length;i++)
                                    {
                                        let hp = 0; let remainHp=0; let mana = 0;
                                        for (let index = 0; index < this.attackTurnDataTargets.length; index++) 
                                        {
                                            if(this.targets[i].nftId == this.attackTurnDataTargets[index]["nftId"])
                                            {
                                                hp = this.attackTurnDataTargets[index]["hp"];
                                                remainHp = this.attackTurnDataTargets[index]["remainHp"];
                                                mana = this.attackTurnDataTargets[index]["mana"];
                                                break;
                                            }
                                        }
                                        this.targets[i].Damaged(hp,remainHp,mana);
                                    }
                                }
                            }, 0.5);
                        }
                        else if(skillType == 4)
                        {
                            if(this.targets.length > 1)
                            {
                                for(let i=1;i<this.targets.length;i++)
                                {
                                    this.ShootBulletSingle(this.targets[i],false,this.targets[0].node); 
                                }
                            }
                        }      
                    }                                  
                    
                    // Destroy bullet
                    bullet.destroy();
                }
            }).start();        
    }

    public ShootBulletMulti() {
        log("ShootBulletMulti");
        if(this.targets != null && this.targets.length > 0)
        {
            for(let i=0;i<this.targets.length;i++)
            {            
                let bullet = instantiate(this.attackBullet);
                bullet.parent = this.node.parent;
                bullet.setWorldPosition(this.bulletNode.worldPosition);
                bullet.lookAt(this.targets[i].GetAttackedPosition());
                tween(bullet)
                    .to(0.5, { worldPosition: this.targets[i].GetAttackedPosition()}, {
                        easing: 'linear', onComplete: () => {
                            
                            let eff = instantiate(this.attackEffect);
                            eff.parent = this.node.parent;
                            eff.setWorldPosition(bullet.worldPosition);
                            
                            this.targets[i].OnPlayAnimHitBack();
    
                            // Damaged
                            let hp = 0; let remainHp=0; let mana=0;
                            for (let index = 0; index < this.attackTurnDataTargets.length; index++) 
                            {
                                if(this.targets[i].nftId == this.attackTurnDataTargets[index]["nftId"])
                                {
                                    hp = this.attackTurnDataTargets[index]["hp"];
                                    remainHp = this.attackTurnDataTargets[index]["remainHp"];
                                    mana = this.attackTurnDataTargets[index]["mana"];
                                    break;
                                }
                            }
                            this.targets[i].Damaged(hp,remainHp,mana);
    
                            // Destroy
                            bullet.destroy();
                        }
                    })
                    .start();
            }
        }
    }

    public MoveToAttack(enemy:PVPHeroController) {
        tween(this.node.parent).to(0.8, { worldPosition: enemy.attackedNode.worldPosition}, {easing: 'linear'}).start();
    }
    public MoveToRootPos() {
        tween(this.node.parent).to(0.8, { worldPosition: this.rootPos}, {easing: 'linear' ,onComplete: () => {
            this.OnPlayAnimIdle();
            this.moving = false;
        }},).start();
    }
    public lookAtTarget(enemyNode:Node) {
        if(enemyNode != null)
        {
            //this.node.parent.lookAt(enemyNode.worldPosition, Vec3.UP);
            const temp:Vec3 = this.node.parent.forward;
            const that= this;
            tween(temp)
            .by(0.2, enemyNode.worldPosition, {
                'onStart': () => {
                    
                },
                'onUpdate': () => {
                    that.node.parent.lookAt(temp, Vec3.UP);
                },
                'onComplete': () => {
                    that.node.parent.lookAt(enemyNode.worldPosition, Vec3.UP);                    
                }
            }).start();
        }
    }

    public GetAttackedPosition():Vec3
    {
        return new Vec3(this.node.worldPosition.x,this.node.worldPosition.y + 0.5,this.node.worldPosition.z);
    }

    public Heal(health:number,remainHp:number)
    {
        if(this.healthBar != null)
        this.healthBar.setHealthValue(remainHp/health);
    }

    public Damaged(health:number,remainHp:number, mana:number)
    {
        this.targeting = false;
        if(this.healthBar != null)
            this.healthBar.setHealthValue(remainHp/health);
            this.healthBar.setManaValue(mana/100);
        if(remainHp <= 0)
            this.Die();
    }

    public IconSkill(skillId:number){
        this.healthBar.setIconSkill(skillId);
    }

    public Die()
    {
        this.OnPlayAnimDie();
        this.scheduleOnce(() => {
            this.healthBar.node.active = false;
            this.node.active = false;
        },3);
    }
    //#endregion

    //#region ANIMATION
    OnGoToIdleState()
    {
        log("OnGoToIdleState");

        this.OnPlayAnimIdle();
    }

    OnPlayAnimIdle() {
        this.animationController.play(heroAnim.Idle);
    }
    OnPlayAnimAttack() {
        this.animationController.play(heroAnim.Attack);
    }
    OnPlayAnimJumpShot() {
        this.animationController.play(heroAnim.JumpShot);
    }
    OnPlayAnimBite() {
        this.animationController.play(heroAnim.Bite);
    }
    OnPlayAnimBitting2Times() {
        this.animationController.play(heroAnim.Bitting2Times);
    }
    OnPlayAnimHeadButt() {
        this.animationController.play(heroAnim.HeadButt);    
    }
    OnPlayAnimShot() {
        this.animationController.play(heroAnim.shot);
    }

    OnPlayAnimRun() {
        this.animationController.play(heroAnim.Run);
    }
    OnPlayAnimJump() {
        this.animationController.play(heroAnim.Jump);
    }
    OnPlayAnimJumpBack() {
        this.animationController.play(heroAnim.JumpBack);
    }
    OnPlayAnimHitBack() {
        this.animationController.play(heroAnim.HitBack);
    }

    OnPlayAnimDie() {
        this.animationController.play(heroAnim.Die);
    }
    OnPlayAnimHappy() {
        this.animationController.play(heroAnim.Happy);
    }
    OnPlayAnimStun() {
        this.animationController.play(heroAnim.Stun);
    }

    //#endregion

    //#region EFFECT
    //#endregion
}