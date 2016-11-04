using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(DelayedDestroy());
	}
	
    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(10.0f);
        Destroy(this.gameObject);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
