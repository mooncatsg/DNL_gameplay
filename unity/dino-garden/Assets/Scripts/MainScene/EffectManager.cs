using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    DROP,
    POWER_UP,
    HARVEST,
    UPGRADE,
}
public class EffectManager : FastSingleton<EffectManager>
{
    [SerializeField] GameObject dropCrop = null;
    [SerializeField] GameObject powerUp = null;
    [SerializeField] GameObject harvestEf = null;
    [SerializeField] GameObject upgradeEF = null;
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        DropCropPlay();
    //    }
    //    if (Input.GetKeyDown(KeyCode.Q))
    //    {
    //        PowerUpPlay();
    //    }
    //    if (Input.GetKeyDown(KeyCode.W))
    //    {
    //        PowerUpPlay();
    //    }
    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        HarvestEffectPlay();
    //    }if (Input.GetKeyDown(KeyCode.R))
    //    {
    //        UpgradeEFPlay();
    //    }
    //}
    public void PlayEffect(EffectType type, Vector3 position)
    {
        GameObject obj = null;
        switch(type)
        {
            case EffectType.DROP:
                obj = GameObject.Instantiate(dropCrop, this.transform);
                break;
            case EffectType.HARVEST:
                obj = GameObject.Instantiate(harvestEf, this.transform);
                break;
            case EffectType.POWER_UP:
                obj = GameObject.Instantiate(powerUp,  this.transform);
                break;
            case EffectType.UPGRADE:
                obj = GameObject.Instantiate(upgradeEF,  this.transform);
                break;
            default:
                break;
        }
        obj.transform.position = position;
    }
}

