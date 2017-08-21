using UnityEngine;

public class ExploreForBuild : MonoBehaviour
{
    void OnEnable()
    {
        Map.Curr.CalculateBounds();
        gameObject.GetComponent<Camera>().ViewWholeMap(
           Map.Curr.Info.MinBound.Get(),
           Map.Curr.Info.MaxBound.Get()
           , Map.Curr.Info.Center.Get());
    }
    void Update()
    {
        Camera.main.transform.RotateAround(Map.Curr.Info.Center.Get()
            , Vector3.up, 20 * Time.deltaTime);
    }


}