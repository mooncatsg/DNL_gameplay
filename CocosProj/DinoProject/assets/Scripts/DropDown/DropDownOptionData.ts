import { _decorator, SpriteFrame } from 'cc';
const {ccclass, property} = _decorator;

@ccclass('DropDownOptionData')
export default class DropDownOptionData{
    @property(String)
    public optionString: string = "";
    @property(SpriteFrame)
    public optionSf: SpriteFrame = undefined;

    constructor(s: string) {this.optionString = s;}
}


/**
 * Note: The original script has been commented out, due to the large number of changes in the script, there may be missing in the conversion, you need to convert it manually
 */
// const {ccclass, property} = cc._decorator;
// 
// @ccclass("DropDownOptionData")
// export default class DropDownOptionData{
//     @property(String)
//     public optionString: string = "";
//     @property(cc.SpriteFrame)
//     public optionSf: cc.SpriteFrame = undefined;
// }
