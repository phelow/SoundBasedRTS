using UnityEngine;
using System.Collections;

public class Defender : MonoBehaviour {

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
        float t = 5.0f;
        while (t > 0.0f)
        {
            t -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
            if (target == null)
            {
                target = FindTarget();
                continue;
            }

            m_rigidbody.AddForce((target.transform.position - transform.position).normalized * Time.deltaTime * m_enemyForce);
        }
        Destroy(this.gameObject);
    }

    public GameObject FindTarget()
    {
        GameObject[] playerUnits = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closestUnit = null;

        foreach (GameObject go in playerUnits)
        {
            if (closestUnit == null)
            {
                closestUnit = go;
                continue;
            }

            if (Vector3.Distance(go.transform.position, transform.position) < Vector3.Distance(go.transform.position, transform.position))
            {
                closestUnit = go;
            }
        }

        return closestUnit;
    }
}
