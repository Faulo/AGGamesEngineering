using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class FocusTrigger : MonoBehaviour {
    [SerializeField]
    CinemachineTargetGroup targetGroup = default;
    [SerializeField]
    Transform newTarget = default;
    [SerializeField, Range(0f, 1f)]
    float newWeight = 1f;
    [SerializeField, Range(0f, 5f)]
    float newRadius = 1f;
    protected void OnTriggerEnter(Collider collision) {
        if (collision.CompareTag("Player")) {
            var temp = new List<Transform>();
            foreach (var member in targetGroup.m_Targets) {
                temp.Add(member.target);
                targetGroup.RemoveMember(member.target);
            }

            foreach (var target in temp) {
                targetGroup.AddMember(target, newWeight, newRadius);
            }
            targetGroup.AddMember(newTarget, newWeight, newRadius);
        }
    }
}
