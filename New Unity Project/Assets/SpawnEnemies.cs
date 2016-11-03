﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class SpawnEnemies : MonoBehaviour {
    [SerializeField]
    private GameObject m_enemy;

    [SerializeField]
    private Hv_TriTone_LibWrapper wrapper;

    [SerializeField]
    private Image m_image;

	// Use this for initialization
	void Start () {
        StartCoroutine(SpawnEnemiesCoroutine());
	}

    private IEnumerator SpawnEnemiesCoroutine()
    {
        float nightCycle = 10.0f;
        float spawnRate = 1.0f;
        float spawnMultiplier = 1.0f;
        while (true)
        {
            m_image.color = Color.Lerp(Color.grey, Color.clear, .5f);
            BuildingManager.CalmDown();

            float t = nightCycle;
            while(t > 0.0f)
            {
                t -= Time.deltaTime;
                wrapper.octaveLength = Mathf.Lerp(20, 5, t);
                yield return new WaitForEndOfFrame();
            }


            BuildingManager.DropBass();
            m_image.color = Color.Lerp(Color.red, Color.clear, .5f);
            spawnRate *= .99f;

            spawnMultiplier *= 1.1f;

            float spawnTime = nightCycle * spawnMultiplier / 2;

            while(spawnTime > 0)
            {
                spawnTime -= spawnRate;

                GameObject.Instantiate(m_enemy, transform.position, transform.rotation, null);

                yield return new WaitForSeconds(spawnRate);
            }

        }
    }
}
