
import { _decorator, Component, Node, Quat } from 'cc';
import { PartDinoController } from './PartDinoController';
const { ccclass, property } = _decorator;

@ccclass('OneDinoManager')
export class OneDinoManager extends Component {
    // [1]
    // dummy = '';
    @property({ type: PartDinoController })
    public partDino: PartDinoController;
    // [2]
    // @property
    // serializableDummy = 0;

    start () {
        // [3]
        let paramGet = this.parseURLParams(window.location.toString());
        let dinoData = JSON.parse(atob(""+paramGet["oneDinoData"][0]));
        if(dinoData["isEvolved"]){
            if(!this.partDino)
                this.partDino = this.getComponentInChildren<PartDinoController>(PartDinoController);
            this.partDino.loadDataByTrait(dinoData["expressTraits"],dinoData["geneRarity"],dinoData["nftId"]);
        }else{
            this.partDino.loadDataVer1(dinoData["class"], dinoData["rarity"]);
        }
    }

    parseURLParams(url: any) {
        var queryStart = url.indexOf("?") + 1,
            queryEnd = url.indexOf("#") + 1 || url.length + 1,
            query = url.slice(queryStart, queryEnd - 1),
            pairs = query.replace(/\+/g, " ").split("&"),
            parms = {}, i, n, v, nv;

        if (query === url || query === "") return;

        for (i = 0; i < pairs.length; i++) {
            nv = pairs[i].split("=", 2);
            n = decodeURIComponent(nv[0]);
            v = decodeURIComponent(nv[1]);

            if (!parms.hasOwnProperty(n)) parms[n] = [];
            parms[n].push(nv.length === 2 ? v : null);
        }
        return parms;
    }

    private _temp_quat: Quat = new Quat();
    update (deltaTime: number) {
        //if(this.autoRotate){
            Quat.fromEuler(this._temp_quat,0, -50 * deltaTime, 0);
            this.partDino.node.rotate(this._temp_quat);
        //}
        // [4]
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
