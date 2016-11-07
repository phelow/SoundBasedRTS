using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager ms_instance;
    [SerializeField]
    private List<Building> m_buildings;
    [SerializeField]
    private Building m_activeBuilding;

    [SerializeField]
    private GameObject[] m_buildingsToSpawn;

    [SerializeField]
    private Text m_scoreText;
    
    [SerializeField]
    private float m_maxDensity = 3.0f;
    private float m_minerRange = 5.0f;
    private float m_closeProximity = .5f;

    

    private int m_yourScore = 0;


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
        float totalMetro = 200;
        wrapper.metroVal = totalMetro;
        wrapper.metroVal2 = totalMetro * 1.5f;
        wrapper.metroVal3 = totalMetro * 2;

        foreach (Building building in m_buildings)
        {
            building.ResetYield();
        }

        StartCoroutine(ProduceTownHall(wrapper.metroVal));
        StartCoroutine(ProduceMine(wrapper.metroVal2));
        StartCoroutine(ProduceArmory(wrapper.metroVal3));
    }


    public IEnumerator ProduceTownHall(float waitInMillseconds)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitInMillseconds/100.0f);
            List<Building> toRemove = new List<Building>();

            foreach (Building building in m_buildings)
            {
                if (building is TownCenter && building.PassiveProduce())
                {
                    toRemove.Add(building);
                }
            }

            foreach (Building building in toRemove)
            {
                RemoveBuilding(building);
            }
        }

    }


    public IEnumerator ProduceMine(float waitInMillseconds)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitInMillseconds / 100.0f);
            List<Building> toRemove = new List<Building>();

            foreach (Building building in m_buildings)
            {
                if (building is Mine && building.PassiveProduce())
                {
                    toRemove.Add(building);
                }
            }

            foreach (Building building in toRemove)
            {
                RemoveBuilding(building);
            }
        }

    }


    public IEnumerator ProduceArmory(float waitInMillseconds)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitInMillseconds / 100.0f);
            List<Building> toRemove = new List<Building>();

            foreach (Building building in m_buildings)
            {
                if (building is Armory && building.PassiveProduce())
                {
                    toRemove.Add(building);
                }
            }

            foreach (Building building in toRemove)
            {
                RemoveBuilding(building);
            }
        }

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

    public List<Building> GetBuildings()
    {
        return m_buildings;
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
        int numTownCenters = 0;
        int numArmories = 0;
        int numMines = 0;

        foreach (Building building in ms_instance.m_buildings)
        {
            if(building is Mine)
            {
                numMines++;
            }
            else if (building is Armory)
            {
                numArmories++;
            }else if(building is TownCenter)
            {
                numTownCenters++;
            }

            density += 1 / (1 + Mathf.Pow(Vector2.Distance(location.position, building.transform.position),2));
        }

        if (density > ms_instance.m_maxDensity)
        {
            return;
        }

        ms_instance.m_scoreText.text = "HighScore: " + PlayerPrefs.GetInt("HighScore", 0) + "\nYour Score: " + ms_instance.m_yourScore;
        ms_instance.m_yourScore++;

        if (ms_instance.m_yourScore > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", ms_instance.m_yourScore);
        }
        int choice = 0;

        int randomTC = Random.Range(0, numTownCenters);

        int randomM = Random.Range(0, numMines);
        int randomA = Random.Range(0, numArmories);

        if(randomTC <= randomM && randomTC <= randomA)
        {
            choice = 0;
        }
        else
        {
            choice = Random.Range(0, ms_instance.m_buildingsToSpawn.Length);
        }


        ms_instance.m_buildings.Add((GameObject.Instantiate(ms_instance.m_buildingsToSpawn[choice], location.position, location.rotation, null) as GameObject).GetComponent<Building>());
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

    public int GetNumBuildings()
    {
        return m_buildings.Count;
    }

    // Update is called once per frame
    void Update()
    {
        List<Building> closeBuildings = new List<Building>();

        wrapper.waveToggle = 0;
        wrapper.waveToggle2 = 0;
        wrapper.waveToggle3 = 0;

        foreach (Building building in m_buildings)
        {
            building.SetPassiveImage();
            if (Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), building.transform.position) < m_closeProximity)
            {
                if(building is TownCenter)
                {
                    wrapper.waveToggle = 1;
                } else if(building is Armory)
                {

                    wrapper.waveToggle2 = 1;
                }
                else if (building is Armory)
                {

                    wrapper.waveToggle3 = 1;
                }
                building.SetActiveImage();
                closeBuildings.Add(building);
            }
        }


        if (ms_baseDropped)
        {
            m_audioSource.volume = 1.0f;
            ms_baseDropped = false;
            //find the nearest building
            Debug.Log("Setting the yields");

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
            for (int i = 0; i < m_buildings.Count / 5 + 1; i++) {
                Building randomBuilding = m_buildings[Random.Range(0, m_buildings.Count)];

                closeBuildings = new List<Building>();

                foreach (Building building in m_buildings)
                {

                    if (Vector2.Distance(randomBuilding.transform.position, building.transform.position) < m_closeProximity)
                    {
                        closeBuildings.Add(building);
                    }
                }


                foreach (Building building in closeBuildings)
                {
                    building.SetYield(Random.Range(5.0f, 10.0f));
                }
            }
        }

        float totalYield = 0;

        foreach (Building building in closeBuildings)
        {
            totalYield += building.GetYield();
        }

        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //calculate the yield
        m_audioSource.volume = Mathf.Lerp(.1f, 1.0f, Mathf.InverseLerp(.5f, 15.0f, totalYield));
        
        //Debug.Log("totalYIeld:" + totalYield);
        wrapper.freq = Mathf.Lerp(200.0f, 600.0f, Mathf.InverseLerp(.5f, 100.0f, totalYield)); //TODO: set base pitch instead of octavelength

    }
}
