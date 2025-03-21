using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ParticleType
{
    DamageExplosion,
    WeaponFire,
    WeaponSmoke,
}

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance { get; private set; }

    private Dictionary<ParticleType, GameObject> particleSystemDic = new Dictionary<ParticleType, GameObject>();
    private Dictionary<ParticleType, Queue<GameObject>> particlePools = new Dictionary<ParticleType, Queue<GameObject>>();

    public GameObject weaponExplosionParticle;
    public GameObject weaponFireParticle;
    public GameObject weaponSmokeParticle;

    public int poolSize = 30;

    //private void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }

    //    ParticleAdd();

    //}

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //void ParticleAdd()
    //{
    //    particleSystemDic.Add(ParticleType.DamageExplosion, weaponExplosionParticle);
    //    particleSystemDic.Add(ParticleType.WeaponFire, weaponFireParticle);
    //    particleSystemDic.Add(ParticleType.WeaponSmoke, weaponSmokeParticle);

    //    foreach (var type in particleSystemDic.Keys)
    //    {
    //        Queue<GameObject> pool = new Queue<GameObject>();
    //        for (int i = 0; i < poolSize; i++)
    //        {
    //            GameObject p_obj = Instantiate(particleSystemDic[type]);
    //            p_obj.gameObject.SetActive(false);
    //            pool.Enqueue(p_obj);
    //        }
    //        particlePools.Add(type, pool);
    //    }
    //}

    //public void ParticlePlay(ParticleType type, Vector3 position, Vector3 scale)
    //{
    //    if (particlePools.ContainsKey(type))
    //    {
    //        GameObject particleObj = particlePools[type].Dequeue();

    //        if (particleObj != null)
    //        {
    //            particleObj.transform.position = position;
    //            ParticleSystem particleSystem = particleObj.GetComponentInChildren<ParticleSystem>(); //�ϳ��� ������Ʈ�� �������� ��ƼŬ�� ������ �ִٸ� �ٸ� ��������Ʈ

    //            if (particleSystem.isPlaying)
    //            {
    //                particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    //            }
    //            particleSystem.transform.localScale = scale;
    //            particleObj.SetActive(true);
    //            particleSystem.Play();
    //            StartCoroutine(particleEnd(type, particleObj, particleSystem));
    //        }
    //        //if (particleSystemDic.ContainsKey(type))
    //        //{ 
    //        //    //�����ڵ�
    //        //    ParticleSystem particle = Instantiate(particleSystemDic[type], position, Quaternion.identity);
    //        //    particle.gameObject.transform.localScale = scale; //ũ�⿡ ���Ѱ� 
    //        //    particle.Play();
    //        //    Destroy(particle.gameObject, particle.main.duration);
    //        //    //��ƼŬ�� ����� �Ŀ� ���ŵ�
    //        //}
    //    }
    //}
    //IEnumerator particleEnd(ParticleType type, GameObject particleObj, ParticleSystem particleSystem)
    //{
    //    while (particleSystem.isPlaying)
    //    {
    //        yield return null;
    //    }
    //    particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    //    particleObj.SetActive(false);
    //    particlePools[type].Enqueue(particleObj);
    //}

}
