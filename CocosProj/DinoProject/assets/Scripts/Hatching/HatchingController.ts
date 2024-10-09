
import { _decorator,Tween,Vec3,Quat, Component, Node, SkeletalAnimationComponent, log, randomRangeInt, ParticleSystem, ParticleSystemComponent, SkinnedMeshRenderer, tween, color, Color, Material, director, renderer, AudioSource, AudioClip  } from 'cc';
const { ccclass, property } = _decorator;

const cocosHatchingAnim = {
    Dino_Spawn_Main_Idle: 'IdleMain',
    Dino_Spawn_Idle: 'Dino_Spawn_Idle',
    Dino_Spawn_Idle2: 'Dino_Spawn_Idle2',
    Dino_Spawn_Idle3: 'Dino_Spawn_Idle3',
    Dino_Spawn_Idle4: 'Dino_Spawn_Idle4',
}

@ccclass('HatchingController')
export class HatchingController extends Component {
    // [1]
    // dummy = '';
    @property({ type: SkeletalAnimationComponent })
    public hatchingAnimComp: SkeletalAnimationComponent = null;

    @property({ type: Node })
    public burnEffect: Node = null;

    @property({ type: Node })
    public boomEffect: Node = null;

    @property({ type: Node })
    public starnname: Node = null;

    @property({ type: AudioSource })
    public audioSource: AudioSource;

    @property({type: AudioClip})
    public clip: AudioClip;

    // @property({ type: Material })
    // public mat2: Material;

    private idleTimes = 2;
    private pass: renderer.Pass;

    start () {
        this.burnEffect.active = false;
    }

    OnMoveEnd()
    {
        this.burnEffect.active = true;
    }

    OnSpawn()
    {
        this.boomEffect.active = true;
        this.audioSource.playOneShot(this.clip);
    }

    OnSpawnEnd(){
        this.idleTimes = randomRangeInt(5,10);
        this.PlayAnim(cocosHatchingAnim.Dino_Spawn_Main_Idle);
        this.burnEffect.getComponentsInChildren(ParticleSystemComponent).forEach(element => {
            element.rateOverTime.constant = 0;
        });
        this.starnname.active = true;
        // this.hideEgg = true;
    }

    OnDinoIdleMainPingpong(){
        this.idleTimes--;
        if(this.idleTimes <= 0)
        {
           
            let idle = randomRangeInt(0,4);
            let idleAnim = cocosHatchingAnim.Dino_Spawn_Idle;
            if(idle == 1)
                idleAnim = cocosHatchingAnim.Dino_Spawn_Idle2;
            else if(idle == 2)
                idleAnim = cocosHatchingAnim.Dino_Spawn_Idle3;
            else if(idle == 3)
                idleAnim = cocosHatchingAnim.Dino_Spawn_Idle4;
            this.PlayAnim(idleAnim);
        }
    }

    OnDinoIdleEnd(){
        this.idleTimes = randomRangeInt(10,30);
        this.PlayAnim(cocosHatchingAnim.Dino_Spawn_Main_Idle);
    }

    OnDinoPingpong(){
    }

    PlayAnim(anim) {
        this.hatchingAnimComp.play(anim);
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
