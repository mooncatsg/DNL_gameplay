import { _decorator, Component, Label, Sprite, Toggle } from 'cc';
const { ccclass, property } = _decorator;

import DropDown from "./DropDown";

@ccclass('DropDownItem')
export default class DropDownItem extends Component {
    @property(Label)
    public label: Label = undefined;
    @property(Sprite)
    public sprite: Sprite = undefined;
    @property(Toggle)
    public toggle: Toggle = undefined;
}


/**
 * Note: The original script has been commented out, due to the large number of changes in the script, there may be missing in the conversion, you need to convert it manually
 */
// import DropDown from "./DropDown";
// 
// const { ccclass, property } = cc._decorator;
// @ccclass()
// export default class DropDownItem extends cc.Component {
// 
//     @property(cc.Label)
//     public label: cc.Label = undefined;
//     @property(cc.Sprite)
//     public sprite: cc.Sprite = undefined;
//     @property(cc.Toggle)
//     public toggle: cc.Toggle = undefined;
// }
