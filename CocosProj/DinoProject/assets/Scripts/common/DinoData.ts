
import { _decorator, Component, Node } from 'cc';
const { ccclass, property } = _decorator;
export class OldDinoData{
    public rarity:number;
    public dino_class:number;
}
export class DinoData{
    public body:number;
    public wing:number;
    public face:number;
    public teeth:number;
    public back:number;
    public horn:number;
}

export class DinoBodyDefine{
    public static NovisBody =[5,8,9];
    public static AquisBody =[0,2,6];
    public static TerrosBody =[1,3,7];
    public static DarkBody = [4,10];
    public static LightBody = [11];
    public static BodyArray = [DinoBodyDefine.NovisBody,DinoBodyDefine.AquisBody,DinoBodyDefine.TerrosBody,DinoBodyDefine.DarkBody,DinoBodyDefine.LightBody];
   
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
