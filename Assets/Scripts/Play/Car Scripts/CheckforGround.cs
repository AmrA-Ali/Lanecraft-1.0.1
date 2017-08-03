using UnityEngine;

public class CheckforGround : MonoBehaviour {

    public static int type = 0;
    public static Transform getMid;
    public static int neg = 1;
    private int conti = -1;
    void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Cube"))
        {
            getMid = other.gameObject.transform;
            type = 1;
            conti = 0;
        }
        else type = 0;

        if (getMid.eulerAngles.x > 280) neg = -1; else neg = 1;
        
       /* print(type);
        print(getMid.forward);
        print(getMid.eulerAngles.x);
        print("Rotation: " + getMid.rotation.x);*/
    }
    void Update()
    {
        if (conti < 10)
        {
            //Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, getMid.position,
            // 30 * Time.deltaTime);
            conti++;
        }
        else conti = -1;
    }
    void OnTriggerExit(Collider other)
    {
        //type = 0;
        //print(type);
    }
}
