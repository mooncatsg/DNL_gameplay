using System;
using System.Collections;
using UnityEngine;

public static class MonobehaviourExtension
{
    public static void ActionWaitForEndOfFrame(this MonoBehaviour input, Action action)
    {
        input.StartCoroutine(IEWaitTime(Time.deltaTime, action));
    }
    public static void ActionWaitForSeconds(this MonoBehaviour input, float time, Action action)
    {
        input.StartCoroutine(IEWaitTime(time, action));
    }
    static IEnumerator IEWaitTime(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }
}
