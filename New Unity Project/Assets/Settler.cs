using UnityEngine;
using System.Collections;

public class Settler : MonoBehaviour {
    [SerializeField]
    private Rigidbody2D m_rigidbody;
    [SerializeField]
    private float m_forceMultiplier = 10.0f;
    [SerializeField]
    private float m_minSpawnTime = 5.0f;
    [SerializeField]
    private float m_maxSpawnTime = 10.0f;
	// Use this for initialization
	void Start () {
        m_rigidbody.AddForce(new Vector2(Random.Range(-2, 2), Random.Range(-2, 2)).normalized * m_forceMultiplier);
        StartCoroutine(DropABuilding());
	}

    private IEnumerator DropABuilding()
    {
        yield return new WaitForSeconds(Random.Range(m_minSpawnTime, m_maxSpawnTime));

        BuildingManager.SpawnBuildingAt(this.transform);
        Destroy(this.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
