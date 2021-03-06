﻿using UnityEngine;
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

        m_rigidbody.AddForce((m_targetBuilding.transform.position - transform.position).normalized * m_minerForce * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnCollisionEnter2D(Collision2D coll)
    {
        Building collidedBuilding = coll.gameObject.GetComponentInChildren<Building>();

        if(collidedBuilding == null || collidedBuilding is Mine)
        {
            return;
        }

        collidedBuilding.QuadrupleProduce();
        Destroy(this.gameObject);

    }
}
