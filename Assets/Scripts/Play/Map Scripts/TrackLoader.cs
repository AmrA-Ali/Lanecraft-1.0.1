using UnityEngine;
public class TrackLoader : MonoBehaviour
{
	void Start ()
	{
        Time.timeScale = 1;
        gameObject.map().Build();
    }

}
