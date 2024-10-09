
import { _decorator, Component, Node, sys } from 'cc';
import { Common } from '../common/Common';
import { DinoCardController } from './DinoCardController';
const { ccclass, property } = _decorator;

@ccclass('MarketController')
export class MarketController extends Component {
    private static singleton: MarketController;
    public static getInstance(): MarketController {
        return MarketController.singleton;
    }
    private _xhrXHR: XMLHttpRequest | null = null;

    @property({ type: DinoCardController })
    public cardDino: DinoCardController[] = [];

    @property({ type: Node })
    public nodata: Node;

    public dinoDatas:any;

    start () {
        MarketController.singleton = this;
        this.dinoDatas = Common.currentData;
        // [3]
        if(this.dinoDatas && this.dinoDatas.length > 0 ){
            this.nodata.active = false;
            for (let index = 0; index < this.cardDino.length; index++) {
                const element = this.cardDino[index];
                if(index >= this.dinoDatas.length){
                    element.node.active = false;
                } else{
                    element.node.active = true;
                    element.initialize(this.dinoDatas[index]);
                }
            }
        }else{
            this.nodata.active = true;
        }
        this.sendXHR();
    }

    onDestroy() {
        this.rmXhrEventListener(this._xhrXHR);
    }

    sendXHR() {
        let xhr = new XMLHttpRequest();
        let envi:string = localStorage.getItem('environment');
        xhr.open('GET', "https://api.coingecko.com/api/v3/simple/price?ids=binancecoin&vs_currencies=usd", true);
        if (sys.isNative) {
            xhr.setRequestHeader('Accept-Encoding', 'gzip,deflate');
        }
        xhr.timeout = 10000;// 10 seconds for timeout

        this.XhrEventListener(xhr);
        xhr.send();
        this._xhrXHR = xhr;
    }

    rmXhrEventListener(xhr: any) {
        if (!xhr) {
            return;
        }

        ['loadstart', 'abort', 'error', 'load', 'loadend', 'timeout'].forEach((eventName) => {
            xhr['on' + eventName] = null;
        });
        xhr.onreadystatechange = null;
    }

    XhrEventListener(xhr: any) {
        if (!xhr) {
            return;
        }

        ['loadstart', 'abort', 'error', 'load', 'loadend', 'timeout'].forEach((eventName) => {
            xhr[('on' + eventName) as 'onloadstart' | 'onabort' | 'onerror' | 'onload' | 'onloadend' | 'ontimeout'] = function () {
                let logString = 'Event : ' + eventName;
                if (eventName === 'timeout') {
                    logString += '(timeout)';
                }
                else if (eventName === 'loadend') {
                    logString += '...loadend!';
                    logString += '\n' + (xhr as XMLHttpRequest).responseText;
                    let data = JSON.parse((xhr as XMLHttpRequest).responseText);
                    for (let index = 0; index < MarketController.getInstance().cardDino.length; index++) {
                        const element = MarketController.getInstance().cardDino[index];
                        if(index < MarketController.getInstance().dinoDatas.length){
                            element.UpdateUSDPrice(data["binancecoin"]["usd"]);
                        }
                    }
                }
                console.log(logString);
            };
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
