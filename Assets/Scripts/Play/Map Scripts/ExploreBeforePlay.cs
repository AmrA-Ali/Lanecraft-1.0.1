using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ExploreBeforePlay : MonoBehaviour {

	// Use this for initialization
	void Start () {
       gameObject.GetComponent<Camera>().ViewWholeMap(
           gameObject.map().info.minBound.get(),
           gameObject.map().info.maxBound.get()
           ,gameObject.map().info.center.get());
	}
	
	// Update is called once per frame
	void Update () {
        Camera.main.transform.RotateAround(gameObject.map().info.center.get()
            , Vector3.up, 20 * Time.deltaTime);
	}
}
