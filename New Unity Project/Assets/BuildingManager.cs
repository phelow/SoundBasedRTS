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
    private GameObject[] m_buildingsToSpawn;
    
    [SerializeField]
    private float m_maxDensity = 3.0f;
    private float m_minerRange = 5.0f;

    [SerializeField]
    private Hv_TriTone_LibWrapper wrapper;

    [SerializeField]
    private AudioSource m_audioSource;

    private static bool ms_baseDropped = false;
    private static bool ms_isHectic = false;

    // Use this for initialization
    void Start()
    {
        ms_instance = this;
        foreach (Building building in m_buildings)
        {
            building.ResetYield();
        }

        StartCoroutine(TickProduction());
    }

    public IEnumerator TickProduction()
    {

        while (true)
        {
            yield return new WaitForSeconds(2.0f);
            List<Building> toRemove = new List<Building>();

            foreach (Building building in m_buildings)
            {
                if (building.PassiveProduce())
                {
                    toRemove.Add(building);
                }
            }

            foreach(Building building in toRemove)
            {
                RemoveBuilding(building);
            }
        }
    }

    public static void RemoveBuilding(Building building)
    {
        ms_instance.m_buildings.Remove(building);
        Destroy(building.gameObject);
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

        if(buildingsInRange.Count == 0)
        {
            return null;
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

    public static void DropBass()
    {
        ms_baseDropped = true;
        ms_isHectic = true;
    }

    public static void CalmDown()
    {
        ms_isHectic = false;
    }

    // Update is called once per frame
    void Update()
    {
        List<Building> closeBuildings = new List<Building>();

        foreach (Building building in m_buildings)
        {

            if (Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), building.transform.position) < 1.0f)
            {
                closeBuildings.Add(building);
            }
        }


        if (ms_baseDropped)
        {
            m_audioSource.volume = 1.0f;
            ms_baseDropped = false;
            //find the nearest building


            foreach (Building building in closeBuildings)
            {
                building.ActiveProduce();
            }

            foreach (Building building in m_buildings)
            {
                building.ResetYield();
            }

            if(m_buildings.Count == 0)
            {
                return;
            }

            Building randomBuilding = m_buildings[Random.Range(0, m_buildings.Count)];

            closeBuildings = new List<Building>();

            foreach (Building building in m_buildings)
            {

                if (Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), building.transform.position) < 1.0f)
                {
                    closeBuildings.Add(building);
                }
            }


            foreach (Building building in closeBuildings)
            {
                building.SetYield(Random.Range(3.0f, 6.0f));
            }
        }

        float totalYield = 0;

        foreach (Building building in closeBuildings)
        {
            totalYield += building.GetYield();
        }

        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //calculate the yield
        float totalMetro = Mathf.Lerp(2000, 200, Mathf.InverseLerp(.5f, 20.0f, totalYield));
        m_audioSource.volume = Mathf.Lerp(.1f, 1.0f, Mathf.InverseLerp(.5f, 20.0f, totalYield));

        if (ms_isHectic)
        {
            wrapper.metroVal = Random.Range(200,800);
            wrapper.metroVal2 = Random.Range(200, 800);
            wrapper.metroVal3 = Random.Range(200, 800);
        }
        else {
            wrapper.metroVal = totalMetro;
            wrapper.metroVal2 = totalMetro / 2;
            wrapper.metroVal3 = totalMetro / 4;
        }

    }
}
