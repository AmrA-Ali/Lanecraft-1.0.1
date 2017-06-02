using UnityEngine;
public class ExploreBeforePlay : MonoBehaviour
{
    void OnEnable()
    {
        gameObject.map().CalculateBounds();
        gameObject.GetComponent<Camera>().ViewWholeMap(
           gameObject.map().info.minBound.get(),
           gameObject.map().info.maxBound.get()
           , gameObject.map().info.center.get());
    }
    void Update()
    {
        Camera.main.transform.RotateAround(gameObject.map().info.center.get()
            , Vector3.up, 20 * Time.deltaTime);
    }
}