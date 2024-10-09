
import { _decorator, Component, Node, SkeletalAnimationComponent, log, Vec2, Vec3, CCFloat, systemEvent, SystemEvent, tween, AnimationComponent, Touch, ColliderComponent, ITriggerEvent, AudioClip, AudioSourceComponent, CCInteger, Material, DebugMode, SkinnedMeshRenderer, Label, Sprite, ProgressBar, Scheduler, SpriteFrame, resources } from 'cc';
const { ccclass, property } = _decorator;

@ccclass('BattleUI')
export class BattleUI extends Component {

    @property({ type: Sprite })
    public icon: Sprite = null;

    @property({ type: Label })
    public nameLabel: Label = null;

    @property({ type: Node })
    public rarityNode: Node[] = [];

    @property({ type: Label })
    public healthBarLabel: Label = null;

    @property({ type: ProgressBar })
    public healthBar: ProgressBar = null;

    private currentHealthbarValue: number = 1;
    private targetHealthbarValue: number = 1;

    start() {
        this.currentHealthbarValue = 1;
        this.targetHealthbarValue = 1;
    }

    // public initUI(name: string, rarity: number, level: string) {
    //     this.nameLabel.string = name;
    //     this.levelLabel.string = level;
    //     for (let i = 0; i < this.rarityNode.length; i++) {
    //         this.rarityNode[i].active = (i < rarity);
    //     }
    // }

    public updateHealthBar(current: number, maxHealth: number) {
        this.targetHealthbarValue = current / maxHealth;
        this.healthBarLabel.string = current + " %";
    }

    update(deltaTime: number) {
        if (this.currentHealthbarValue > this.targetHealthbarValue) {
            this.currentHealthbarValue -= deltaTime * 0.5;
            this.healthBar.progress = this.currentHealthbarValue;
        }
    }

    public SetHealth(health: number) {
        this.healthBarLabel.string = health + " %";
    }

    public SetNameAndLevel(_name: string, _level: string) {
        if (_level)
            this.nameLabel.string = _name + " Lvl." + _level;
        else
            this.nameLabel.string = _name;
    }

    public SetRarity(_rarity: number) {
        for (let i = 0; i < this.rarityNode.length; i++) {
            this.rarityNode[i].active = (i < _rarity);
        }
    }

    public SetIconDino(_class: number, _rarity: number) {
        if (!_class)
            _class = 1;
        if (!_rarity)
            _rarity = 1;
        // let dino_id = ((_class - 1) * 5) + (_rarity - 1);
        // if (dino_id < this.iconDinoSpriteFrameList.length) {
        //     this.icon.spriteFrame = this.iconDinoSpriteFrameList[dino_id];
        // }

        resources.load("Image_Avatar/Dino/Dino" + _class + "_" + _rarity + "/spriteFrame", SpriteFrame, (err, spriteFrame) => {
            this.icon.spriteFrame = spriteFrame;
        });
    }
    public SetIconMonster(monster_type: number, monster_id: number) {
        if (!monster_id)
            monster_id = 1001;
        // monster_id = monster_id % 1000;
        // if (monster_id < this.iconMonsterSpriteFrameList.length) {
        //     this.icon.spriteFrame = this.iconMonsterSpriteFrameList[monster_id - 1];
        // }
        let monsterSpriteName = monster_id.toString();
        if (monster_type == 2)
            monsterSpriteName = "2001";
        resources.load("Image_Avatar/Enemy/" + monsterSpriteName + "/spriteFrame", SpriteFrame, (err, spriteFrame) => {
            this.icon.spriteFrame = spriteFrame;
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
