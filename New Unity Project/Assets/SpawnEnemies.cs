using UnityEngine;
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
        StartCoroutine(DistortCamera());
	}

    [SerializeField]
    private AudioSource m_distortionSource;
    [SerializeField]
    private AudioClip m_distortionClip;
    [SerializeField]
    private CameraGlitch m_cameraGlitch;

    private IEnumerator DistortCamera()
    {
        float timeRun = 0.0f;
        float undistortedInterval = 30.0f;
        float spikeTime = 1.0f;

        while (true)
        { 
            yield return new WaitForSeconds(Random.Range(undistortedInterval/2,undistortedInterval * 2));
            
            //spike the distortion
            float spikeLength = Random.Range(spikeTime / 2, spikeTime * 2);

            while(spikeLength > 0)
            {
                m_distortionSource.pitch = Random.Range(0.1f, 1.0f);
                m_distortionSource.PlayOneShot(m_distortionClip);

                m_cameraGlitch.Intensity = Mathf.Lerp(5.0f, 0.0f, spikeLength);
                spikeLength--;
                m_cameraGlitch.ShiftAmount += Time.deltaTime;
                    yield return new WaitForEndOfFrame();
            }


            yield return new WaitForSeconds(Random.Range(undistortedInterval / 16, undistortedInterval / 8));

            spikeLength = Random.Range(spikeTime / 2, spikeTime * 2);

            while (spikeLength > 0)
            {
                m_distortionSource.pitch = Random.Range(0.1f, 1.0f);
                m_distortionSource.PlayOneShot(m_distortionClip);

                m_cameraGlitch.Intensity = Mathf.Lerp(0.0f, 5.0f, spikeLength);
                spikeLength--;
                m_cameraGlitch.ShiftAmount -= Time.deltaTime * 10f;
                yield return new WaitForEndOfFrame();
            }
            m_cameraGlitch.ShiftAmount = 0.0f;
            undistortedInterval += 10.0f;

        }
    }

    private IEnumerator SpawnEnemiesCoroutine()
    {
        float nightCycle = 10.0f;
        float spawnRate = 1.0f;
        float spawnMultiplier = .8f;
        

        int numToSpawn = 0;
        while (true)
        {
            m_image.color = Color.Lerp(Color.grey, Color.clear, .5f);
            BuildingManager.CalmDown();

            float t = nightCycle;
            while(t > 0.0f)
            {
                t -= Time.deltaTime;
                //wrapper.octaveLength = Mathf.Lerp(20, 5, t);
                yield return new WaitForEndOfFrame();
            }


            BuildingManager.DropBass();
            m_image.color = Color.Lerp(Color.red, Color.clear, .5f);
            spawnRate *= .99f;

            spawnMultiplier *= 1.1f;
            numToSpawn++;

            float spawnTime = nightCycle * spawnMultiplier / 2;

            while(spawnTime > 0)
            {
                spawnTime -= spawnRate;
                m_cameraGlitch.ShiftAmount += spawnRate * .01f;

                for (int i = 0; i < numToSpawn; i++) {
                    (GameObject.Instantiate(m_enemy, transform.position, transform.rotation, null) as GameObject).GetComponent<Enemy>().SetHealth(numToSpawn + BuildingManager.ms_instance.GetNumBuildings()/10);
                }

                yield return new WaitForSeconds(spawnRate);
            }

        }
    }
}
