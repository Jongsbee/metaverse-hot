using UnityEngine;

[CreateAssetMenu]
public class TowerTemplate : ScriptableObject
{
    public GameObject towerPrefab;
    public Weapon[] weapon;

    [System.Serializable]
    public struct Weapon
    {
        public GameObject Prefab;
        public float Damage;
        public float rate;
        public float range;
        public int cost;
        public bool isParticle;
        public int isDeBuff;
        public bool isZangPan;
        public bool isLaser;
        public float LaserRate;
        public GameObject missile;
        public float missileUp;
        public float missileWaitSecond;
        public float missileSpeed;
        public ParticleSystem hitEffect;
        public AudioClip[] FireSound;
        public AudioClip[] HitSound;
    }
}
