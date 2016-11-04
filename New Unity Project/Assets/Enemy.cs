using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D m_rigidbody;

    [SerializeField]
    private float m_enemyForce = 1.0f;

    private float m_health;

    // Use this for initialization
    void Start()
    {
        //pick the nearest player unit
        GameObject target = null;

        target = FindTarget();
        if(target == null)
        {
            return;
        }

        m_rigidbody.AddForce((target.transform.position - transform.position).normalized * m_enemyForce);


    }

    public void SetHealth(int h)
    {
        m_health = h;
    }

    public GameObject FindTarget()
    {
        List<Building> playerUnits = BuildingManager.ms_instance.GetBuildings();
        GameObject closestUnit = null;
        if (playerUnits.Count == 0 && GameObject.FindGameObjectsWithTag("Player").Length == 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        else if(playerUnits.Count == 0)
        {
            closestUnit = GameObject.FindGameObjectsWithTag("Player")[Random.Range(0, playerUnits.Count)].gameObject;
        }
        else
        {
            closestUnit = playerUnits[Random.Range(0, playerUnits.Count)].gameObject;
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

        m_health--;
        if (m_health <= 0)
        {
        }

    }
}
