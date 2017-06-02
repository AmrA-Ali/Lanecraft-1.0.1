using UnityEngine;
public class ExploreForBuild : MonoBehaviour
{
    void OnEnable()
    {
        BuildSession.map.CalculateBounds();
        gameObject.GetComponent<Camera>().ViewWholeMap(
           BuildSession.map.info.minBound.get(),
           BuildSession.map.info.maxBound.get()
           , BuildSession.map.info.center.get());
    }
    void Update()
    {
        Camera.main.transform.RotateAround(BuildSession.map.info.center.get()
            , Vector3.up, 20 * Time.deltaTime);
    }
}