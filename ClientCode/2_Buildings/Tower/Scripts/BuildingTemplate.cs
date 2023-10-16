using UnityEngine;

[CreateAssetMenu]
public class BuildingTemplate : ScriptableObject
{
    [SerializeField] public AudioClip doneBuildSound;
    [SerializeField] public AudioClip doneUpgradeSound;
    [SerializeField] public ParticleSystem doneBuild;
    [SerializeField] public ParticleSystem doneUpgrade;
}
