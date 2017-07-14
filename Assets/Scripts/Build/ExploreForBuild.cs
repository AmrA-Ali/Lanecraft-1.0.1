using UnityEngine;

public class ExploreForBuild : MonoBehaviour
{
    void OnEnable()
    {
        Map.curr.CalculateBounds();
        gameObject.GetComponent<Camera>().ViewWholeMap(
           Map.curr.info.minBound.get(),
           Map.curr.info.maxBound.get()
           , Map.curr.info.center.get());
    }
    void Update()
    {
        Camera.main.transform.RotateAround(Map.curr.info.center.get()
            , Vector3.up, 20 * Time.deltaTime);
    }


}