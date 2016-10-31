using UnityEngine;
using System.Collections;

public class Miner : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D m_rigidbody;


    [SerializeField]
    private float m_minerForce = 5.0f;

    [SerializeField]
    private Building m_targetBuilding;

    // Use this for initialization
    void Start()
    {
        m_targetBuilding = BuildingManager.GetBuildingForMiner(transform);
    }

    // Update is called once per frame
    void Update()
    {
        if(m_targetBuilding == null)
        {
            Destroy(this.gameObject);
        }

        m_rigidbody.AddForce((m_targetBuilding.transform.position - transform.position).normalized * m_minerForce * Time.deltaTime);

    }


    void OnCollisionEnter2D(Collision2D coll)
    {
        Building collidedBuilding = coll.gameObject.GetComponent<Building>();

        if(collidedBuilding == null || collidedBuilding is Mine)
        {
            return;
        }

        if(collidedBuilding != m_targetBuilding)
        {
            return;
        }

        collidedBuilding.ActiveProduce();
        Destroy(this.gameObject);

    }
}
