using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum SoundType
{
    BUTCLICK,
    CHICKEN,
    COW,
    SHEEP,
    PIG,
    BUILD,
    UPGRADE,
}

public class SoundManager : FastSingleton<SoundManager>
{
    [SerializeField] AudioSource effectSoundSource;
    [SerializeField] GameObject backgroundSource;
    [SerializeField] bool isPlayEffectSound = true;
    [SerializeField] bool isPlayBgSound = true;

    [SerializeField] AudioClip buttonClickSound;
    [SerializeField] AudioClip chickenSound;
    [SerializeField] AudioClip cowSound;
    [SerializeField] AudioClip sheepSound;
    [SerializeField] AudioClip pigSound;
    [SerializeField] AudioClip buildingInteractSound;
    [SerializeField] AudioClip BuildingUpgradeRepairSound;
    
    private void Update()
    {
        if (isPlayBgSound) backgroundSource.SetActive(true);
        else backgroundSource.SetActive(false);
    }
    public void setEffectSound(bool _isPlay)
    {
        isPlayEffectSound = _isPlay;
    }
    public void setBgSound(bool _isPlay)
    {
        isPlayBgSound = _isPlay;
    }
    public void PlaySound(SoundType type)
    {
        if (isPlayEffectSound)
        {
            switch (type)
            {
                case SoundType.BUTCLICK:
                    effectSoundSource.clip = buttonClickSound;
                    break;
                case SoundType.CHICKEN:
                    effectSoundSource.clip = chickenSound;
                    break;
                case SoundType.COW:
                    effectSoundSource.clip = cowSound;
                    break;
                case SoundType.PIG:
                    effectSoundSource.clip = pigSound;

                    break;
                case SoundType.SHEEP:
                    effectSoundSource.clip = sheepSound;

                    break;
                case SoundType.BUILD:
                    effectSoundSource.clip = buildingInteractSound;

                    break;
                case SoundType.UPGRADE:
                    effectSoundSource.clip = BuildingUpgradeRepairSound;

                    break;

            }
            effectSoundSource.Play();
        }
    }
}
