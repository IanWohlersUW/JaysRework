﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraShake : MonoBehaviour
{
    private static Vector3 originalPos;
    public static CameraShake instance;

    void Awake()
    {
        instance = this;
        originalPos = transform.localPosition;
    }

    public static void Shake(float duration, float amount)
    {
        instance.StopAllCoroutines();
        instance.transform.localPosition = originalPos;
        instance.StartCoroutine(instance.cShake(duration, amount));
    }

    private IEnumerator cShake(float duration, float amount)
    {
        float endTime = Time.time + duration;

        while (Time.time < endTime)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * amount;

            duration -= Time.deltaTime;

            yield return null;
        }
        transform.localPosition = originalPos;
    }
}
