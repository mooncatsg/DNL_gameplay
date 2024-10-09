import { _decorator, Component, Label } from 'cc';
const {ccclass, property} = _decorator;

@ccclass('Helloworld')
export default class Helloworld extends Component {
    @property(Label)
    label: Label | null = null;
    @property
    text: string = 'hello';
    start () {
        // init logic
        //this.label.string = this.text;
    }
}


/**
 * Note: The original script has been commented out, due to the large number of changes in the script, there may be missing in the conversion, you need to convert it manually
 */
// const {ccclass, property} = cc._decorator;
// 
// @ccclass
// export default class Helloworld extends cc.Component {
// 
//     @property(cc.Label)
//     label: cc.Label = null;
// 
//     @property
//     text: string = 'hello';
// 
//     start () {
//         // init logic
//         this.label.string = this.text;
//     }
// }
