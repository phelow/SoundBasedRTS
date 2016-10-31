using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingManager : MonoBehaviour
{
    private static BuildingManager ms_instance;
    [SerializeField]
    private List<Building> m_buildings;
    [SerializeField]
    private Building m_activeBuilding;
    [SerializeField]
    private Rigidbody2D m_cameraRigidbody;

    [SerializeField]
    private GameObject[] m_buildingsToSpawn;

    [SerializeField]
    private float m_maxDensity = 3.0f;
    private float m_minerRange = 5.0f;

    // Use this for initialization
    void Start()
    {
        ms_instance = this;

    }

    public static void RemoveBuilding(Building building)
    {
        ms_instance.m_buildings.Remove(building);
        Destroy(building.gameObject);

        //TODO: highscore stuff
    }

    public static Building GetBuildingForMiner(Transform minerTransform)
    {
        List<Building> buildingsInRange = new List<Building>();

        foreach (Building building in ms_instance.m_buildings)
        {
            if(building is Mine)
            {
                continue;
            }

            if (Vector2.Distance(minerTransform.transform.position, building.transform.position) < ms_instance.m_minerRange)
            {
                buildingsInRange.Add(building);
            }
        }

        return buildingsInRange[Random.Range(0, buildingsInRange.Count)];
    }

    public static void SpawnBuildingAt(Transform location)
    {
        float density = 0.0f;
        foreach (Building building in ms_instance.m_buildings)
        {
            density += 1 / (1 + Vector2.Distance(location.position, building.transform.position));
        }

        if (density > ms_instance.m_maxDensity)
        {
            return;
        }

        ms_instance.m_buildings.Add((GameObject.Instantiate(ms_instance.m_buildingsToSpawn[Random.Range(0, ms_instance.m_buildingsToSpawn.Length)], location.position, location.rotation, null) as GameObject).GetComponent<Building>());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //find the nearest building
            foreach (Building building in m_buildings)
            {
                if (m_activeBuilding == null)
                {
                    m_activeBuilding = building;
                    continue;
                }

                if (Vector3.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), building.transform.position) < Vector3.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), m_activeBuilding.transform.position))
                {
                    m_activeBuilding = building;
                }
            }
        }

        foreach (Building building in m_buildings)
        {
            building.PassiveProduce();
        }

        if (m_activeBuilding == null)
        {
            return;
        }
        m_activeBuilding.ActiveProduce();
        m_cameraRigidbody.AddForce((m_activeBuilding.transform.position - m_cameraRigidbody.transform.position).normalized * Vector2.Distance(m_activeBuilding.transform.position, m_cameraRigidbody.transform.position));


    }
}
