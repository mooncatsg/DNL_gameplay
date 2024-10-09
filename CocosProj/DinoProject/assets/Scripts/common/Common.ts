import { resources, SpriteFrame } from "cc";
import { DinoBodyDefine, DinoData } from "./DinoData";

export  class Common {
    public static isShowEvolve:boolean;
    public static walletAddress:string;
    public static currentLoadScene:string;
    public static currentData:any;
    public static currentHatchingData:any;
    public static currentDinoData:any;    
    public static currentWBDinoData:any;   
    public static currentWBDinoPosition:number[];
    public static currentWBDinoResult:any;
    public static isApprovedEvolve:boolean;
    public static evolveDNLFee:string;
    public static totalEvolveBook:number;
    public static evolveDNLFlag:boolean;
    public static isTransferDino:boolean;

    /**
     * static ToStringWithMaxLength
     */
    public static ToStringWithMaxLength(val:number, maxLength:number):string {
        if(val.toString().length > maxLength){
            return val.toString().substring(0, maxLength-1) + "..";
        }else
            return val.toString();
    }

    /**
    * GetPartRarityString
    */
   public static GetPartRarityString(rarityNum:number) {
        let rarityString:string;
        switch (rarityNum) {
            case 1:
            case 2:
            case 3:
                rarityString = "Normal";
                break;
            case 4:
            case 5:
            case 6:
                rarityString = "Rare";
                break;
            case 7:
            case 8:
                rarityString = "Epic";
                break;
            case 9:
                rarityString = "Legendary";
                break;
            case 10:
                rarityString = "Super";
                break;
        }
        return rarityString;
    }
   /**
    * GetRarityString
    */
   public static GetRarityString(rarityNum:number) {
        let rarityString:string;
        switch (rarityNum) {
            case 1:
                rarityString = "Normal";
                break;
            case 2:
                rarityString = "Rare";
                break;
            case 3:
                rarityString = "SuperRare";
                break;
            case 4:
                rarityString = "Legendary";
                break;
            case 5:
                rarityString = "Mystic";
                break;
        }
        return rarityString;
   }

   public static GetRarityBossNumber(rarityStr:string) {
    let rarityNum:number;
    switch (rarityStr) {
        case "boss_normal":
            rarityNum = 1;
            break;
        case "boss_rare":
            rarityNum = 2;
            break;
        case "boss_super_rare":
            rarityNum = 3;
            break;
        case "boss_legendary":
            rarityNum = 4;
            break;
        case "boss_mystic":
            rarityNum = 5;
            break;
        default:
            rarityNum = 0;
            break;
    }
    return rarityNum;
}
   /**
    * getGenderString
    */
   public static getGenderString(genderNum:number) {
       return genderNum == 1 ? "Male" : "Female";
   }

   /**
    * GetClassString
    */
   public static GetClassString(classNum:number) {
       
        let classString:string;
        switch (classNum) {
            case 1:
                classString = "Novis";
                break;
            case 2:
                classString = "Aquis";
                break;
            case 3:
                classString = "Terros";
                break;
            case 4:
                classString = "Dark";
                break;
            case 5:
                classString = "Light";
                break;
        }
        return classString;
    }

   /**
    * GetClassIcon
    */
    public static GetStoneIcon(StoneType:number, callback:any) {
        resources.load("Stones/" + StoneType + "/spriteFrame", SpriteFrame, (err, spriteFrame) => {
            callback(err, spriteFrame);
        });
   }

   /**
    * GetClassIcon
    */
   public static GetClassIcon(classNum:number, callback:any) {
        let classString:string = Common.GetClassString(classNum);
        resources.load("DinoClass/Class_" + classString + "/spriteFrame", SpriteFrame, (err, spriteFrame) => {
            callback(err, spriteFrame);
        });
   }
   
   /**
    * getGenderIcon
    */
   public static getGenderIcon(genderNum:number, callback:any) {
        let genderString:string = Common.getGenderString(genderNum);
        resources.load("DinoGender/icon_Gender_" + genderString + "/spriteFrame", SpriteFrame, (err, spriteFrame) => {
            callback(err, spriteFrame);
        });
   }


   /**
    * getCoinName
    */
    public static getCoinName(cashType:number) {
       
        let coinName:string;
        switch (cashType) {
            case 0:
                coinName = "DNL";
                break;
            case 1:
                coinName = "BUSD";
                break;
            case 2:
                coinName = "BNB";
                break;
        }
        return coinName;
    }
   /**
    * getCoinIcon
    */
    public static getCoinIcon(cashType:number, callback:any) {
        let coinName:string = Common.getCoinName(cashType);
        resources.load("CoinIcon/" + coinName + "/spriteFrame", SpriteFrame, (err, spriteFrame) => {
            callback(err, spriteFrame);
        });
   }

   /**
    * getRarityCard
    */
   public static getRarityCard(rarityNum:number, callback:any) {
        let rarityString:string = Common.GetRarityString(rarityNum);
        resources.load("DinoCard/" + rarityString + "/spriteFrame", SpriteFrame, (err, spriteFrame) => {
            callback(err, spriteFrame);
        });
   }

   /**
    * getRarityIcon
    */
   public static getRarityIcon(rarityNum:number, callback:any) {
        let rarityString:string = Common.GetRarityString(rarityNum);
        resources.load("Rarity/" + rarityString + "/spriteFrame", SpriteFrame, (err, spriteFrame) => {
            callback(err, spriteFrame);
        });
   }

   /**
    * getRarityDinoBG
    */
   public static getRarityDinoBG(rarityNum:number, callback:any) {
        let rarityString:string = Common.GetRarityString(rarityNum);
        resources.load("DinoCardBG/BG_Card_" + rarityString + "_Dino/spriteFrame", SpriteFrame, (err, spriteFrame) => {
            callback(err, spriteFrame);
        });
   }

   /**
    * getRarityDinoDetail
    */
   public static getRarityDinoDetail(rarityNum:number, callback:any) {
        let rarityString:string = Common.GetRarityString(rarityNum);
        resources.load("DinoDetailBG/BG_" + rarityString + "/spriteFrame", SpriteFrame, (err, spriteFrame) => {
            callback(err, spriteFrame);
        });
   }

      /**
    * getWorldBossDinoBGByRarity
    */
       public static getDinoBGByRarity(rarityNum:number, callback:any) {
        let rarityString:string = Common.GetRarityString(rarityNum);
        resources.load("WBCardBG/" + rarityString + "/spriteFrame", SpriteFrame, (err, spriteFrame) => {
            callback(err, spriteFrame);
        });
   }

    /**
    * getMidasBox
    */
       public static getMidasBox(rarityNum:number, callback:any) {
        resources.load("Midas/Chest" + rarityNum + "/spriteFrame", SpriteFrame, (err, spriteFrame) => {
            callback(err, spriteFrame);
        });
   }
    /**
    * getMidasBG
    */
     public static getMidasBG(rarityNum:number, callback:any) {
        resources.load("MidasBG/" + rarityNum + "/spriteFrame", SpriteFrame, (err, spriteFrame) => {
            callback(err, spriteFrame);
        });
   }


   /**
    * romanize
    */
   public static romanize(num:number) {
    var roman = {
        M: 1000,
        CM: 900,
        D: 500,
        CD: 400,
        C: 100,
        XC: 90,
        L: 50,
        XL: 40,
        X: 10,
        IX: 9,
        V: 5,
        IV: 4,
        I: 1
      };
      var str = '';
    
      for (var i of Object.keys(roman)) {
        var q = Math.floor(num / roman[i]);
        num -= q * roman[i];
        str += i.repeat(q);
      }
    
      return str;
   }
   public static formatPrice(number){
        const transfernumber = parseFloat(number).toFixed(2).replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,');
        return transfernumber.replace(/\.00/g, '');
    }
   public static  numberWithCommas(x:number) {
        var parts = x.toString().split('.');
        parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        return parts.join('.');
    }
    private static check(a) {
        return(/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino|android|ipad|playbook|silk/i.test(a)||/1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0,4))) 
    }
    private static checkMobile() {
        return this.check(navigator.userAgent)||this.check(navigator.vendor);
    }
    public static mobileAndTabletCheck () {
        let val = Common.checkMobile();
        console.log ("mobileAndTabletCheck " + val);
        return val;
    };

    public static ConvertGetGenerationString (value: number) {
        if(value === 1) return "Alpha";
        else if(value === 2) return "Beta";
        else if(value === 3) return "Gamma";
        else if(value === 4) return "Delta";
        else if(value === 5) return "Epsilon";
        else if(value === 7) return "Zeta";
        else if(value === 8) return "Eta";
        else if(value === 9) return "Theta";
        else if(value === 10) return "Iota";
        else if(value <= 20) return "Kappa";
        else if(value <= 30) return "Lambda";
        else if(value <= 40) return "Mu";
        else if(value <= 50) return "Nu";
        else if(value <= 60) return "Xi";
        else if(value <= 70) return "Omicron";
        else if(value <= 80) return "Pi";
        else if(value <= 100) return "Rho";
        else if(value <= 200) return "Sigma";
        else if(value <= 300) return "Tau";
        else if(value <= 400) return "Upsilon";
        else if(value <= 500) return "Phi";
        else if(value <= 600) return "Chi";
        else if(value <= 700) return "Psi";
        else return "Omega"

    };

    public static getBreedingFee (breedingCount:number) {        
        switch(breedingCount)
        {
            case 1 :
                return 800;
                break;
            case 2 :
                return 2000;
                break;
            case 3 :
                return 4000;
                break;
            case 4 :
                return 7200;
                break;
            case 5 :
                return 12000;
                break;
        }
        return 800;
    };

    public static getSkillRarityString(skillType:number){
        let skillRarity : number = skillType % 5;
        switch(skillRarity)
        {
            case 1 :
                return "Normal";
                break;
            case 2 :
                return "Rare";
                break;
            case 3 :
                return "Super Rare";
                break;
            case 4 :
                return "Legendary";
                break;
            case 0 :
                return "Mystic";
                break;
            default:
                return "Normal";
                break;
        }
    }

    /**
    * getSkillRarityIcon
    */
   public static getSkillRarityIcon(skillType:number, callback:any) {
        let genderString:string = Common.getSkillRarityString(skillType);
        resources.load("Skill/Frame Rarity Skill/" + genderString + "/spriteFrame", SpriteFrame, (err, spriteFrame) => {
            callback(err, spriteFrame);
        });
    }

   /**
    * getSkillIcon
    */
    public static getSkillIcon(skillType:number, callback:any) {
        let skill : number = Math.floor((skillType - 1) / 5 ) + 1;
        resources.load("Skill/Skill Dino/skill_" + skill + "/spriteFrame", SpriteFrame, (err, spriteFrame) => {
            callback(err, spriteFrame);
        });
   }
}