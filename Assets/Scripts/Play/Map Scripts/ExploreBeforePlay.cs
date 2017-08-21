using UnityEngine;

public class ExploreBeforePlay : MonoBehaviour
{
    void OnEnable()
    {
        gameObject.Map().CalculateBounds();
        gameObject.GetComponent<Camera>().ViewWholeMap(
           gameObject.Map().Info.MinBound.Get(),
           gameObject.Map().Info.MaxBound.Get()
           , gameObject.Map().Info.Center.Get());
    }
    void Update()
    {
        Camera.main.transform.RotateAround(gameObject.Map().Info.Center.Get()
            , Vector3.up, 20 * Time.deltaTime);
    }
}