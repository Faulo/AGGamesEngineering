using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

public class FocusTriggerWeird : MonoBehaviour {
    [SerializeField]
    CinemachineTargetGroup targetGroup = default;

    [SerializeField, Range(0f, 5f)]
    float playerRadius = 1f;
    [SerializeField]
    AnimationCurve playerWeighting = AnimationCurve.Linear(0, 1, 0, 0);

    [SerializeField]
    Transform target = default;
    [SerializeField, Range(0f, 5f)]
    float targetRadius = 1f;
    [SerializeField]
    AnimationCurve targetWeighting = AnimationCurve.Linear(0, 0, 0, 1);

    [SerializeField, Range(0f, 5f)]
    float lerpDuration = 1;


    //protected void OnTriggerEnter(Collider collision) {
    //    if (collision.CompareTag("Player")) {
    //        StartCoroutine(LerpToWeigthAndRadius(collision.transform, playerRadius, playerWeighting));
    //        StartCoroutine(LerpToWeigthAndRadius(target, targetRadius, targetWeighting));
    //    }
    //}

    //IEnumerator LerpToWeigthAndRadius(Transform target, float radius, AnimationCurve weightCurve) {
    //    for (float time = 0; time < lerpDuration; time += Time.deltaTime) {
    //        float weight = weightCurve.Evaluate(time);
    //        targetGroup.RemoveMember(target);
    //        targetGroup.AddMember(target, weight, radius);
    //        yield return null;
    //    }
    //    targetGroup.RemoveMember(target);
    //    targetGroup.AddMember(target, weightCurve.lerpDuration.value, radius);
    //}
}
