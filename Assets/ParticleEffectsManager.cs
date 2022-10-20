using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectsManager : MonoBehaviour
{
    static ParticleEffectsManager Instance = null;

    public GameObject ExplosionPrefab;

    [Range(1,10)]
    public int ObjectPoolSize = 10;

    static List<GameObject> m_explosionObjects;
    static int m_nextExplosionObject = 0;

    private void OnEnable() {
        Instance = this;
    }

    void Start() {
        m_explosionObjects = new List<GameObject>();
        for (int i = 0; i < ObjectPoolSize; i++) {
            m_explosionObjects.Add(Instantiate(ExplosionPrefab, transform));
        }
    }

    public static void CreateExplosion(Vector3 position) {
        m_explosionObjects[m_nextExplosionObject].transform.position = position;
        m_explosionObjects[m_nextExplosionObject].GetComponent<ParticleSystem>().Play();
        m_nextExplosionObject = (m_nextExplosionObject + 1) % m_explosionObjects.Count;
    }
}
