using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class SpawnEnemies : MonoBehaviour {
    [SerializeField]
    private GameObject m_enemy;

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
        while (true)
        {
            m_image.color = Color.Lerp(Color.grey, Color.clear, .5f);
            yield return new WaitForSeconds(nightCycle);
            m_image.color = Color.Lerp(Color.red, Color.clear, .5f);
            spawnRate *= .99f;
            nightCycle += 5.0f;


            float spawnTime = nightCycle / 2;

            while(spawnTime > 0)
            {
                spawnTime -= spawnRate;

                GameObject.Instantiate(m_enemy, transform.position, transform.rotation, null);

                yield return new WaitForSeconds(spawnRate);
            }

        }
    }
}
