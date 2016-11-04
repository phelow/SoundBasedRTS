using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D m_rigidbody;

    [SerializeField]
    private float m_enemyForce = 1.0f;

    // Use this for initialization
    void Start()
    {
        //pick the nearest player unit
        StartCoroutine(Seek());

    }

    private IEnumerator Seek()
    {
        GameObject target = null;
        while (true)
        {
            if (target == null)
            {
                target = FindTarget();
            }

            m_rigidbody.AddForce((target.transform.position - transform.position).normalized * Time.deltaTime * m_enemyForce);

            yield return new WaitForEndOfFrame();
        }
    }

    public GameObject FindTarget()
    {
        GameObject[] playerUnits = GameObject.FindGameObjectsWithTag("Player");
        GameObject closestUnit = null;

        foreach (GameObject go in playerUnits)
        {
            if (closestUnit == null)
            {
                closestUnit = go;
                continue;
            }

            if (Vector3.Distance(go.transform.position, transform.position) < Vector3.Distance(closestUnit.transform.position, transform.position))
            {
                closestUnit = go;
            }
        }

        if(playerUnits.Length == 0)
        {
            BuildingManager.CheckForEndGame();
        }

        return closestUnit;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Enemy enemy = coll.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            return;
        }

        Building building = coll.gameObject.GetComponent<Building>();

        if (building != null)
        {
            BuildingManager.RemoveBuilding(building);
            Destroy(this.gameObject);
            return;
        }

        Destroy(coll.gameObject);
        Destroy(this.gameObject);


    }
}
