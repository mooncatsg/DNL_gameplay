using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CloudController : MonoBehaviour
{
    public void Start()
    {
        float rd = UnityEngine.Random.Range(0f, 5f);
        this.ActionWaitForSeconds(rd, () => {
            playCloudEf();
        });
    }
    public void playCloudEf()
    {
        float rdSpeed = UnityEngine.Random.Range(60f, 100f);
        float rdXStart = UnityEngine.Random.Range(-2f, -1f);
        float rdZStart = UnityEngine.Random.Range(-3.5f, -4.5f);
        float rdXEnd = UnityEngine.Random.Range(3f, 3.5f);
        float rdZEnd = UnityEngine.Random.Range(0, 1.5f); 
        float rd = UnityEngine.Random.Range(0f, 5f);
        List<Vector3> position = new List<Vector3>();
        position.Add(new Vector3(rdXStart, 2f, rdZStart));
        position.Add(new Vector3(rdXEnd, 2f, rdZEnd));
        position.Add(new Vector3(rdXStart, 2f, rdZStart));
        this.ActionWaitForSeconds(rd, () => {
            this.transform.DOLocalPath(position.ToArray(), rdSpeed, PathType.Linear).SetLoops(-1);
        });
    }
}
