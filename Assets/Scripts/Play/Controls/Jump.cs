using UnityEngine;
using UnityEngine.UI;

public class Jump : MonoBehaviour {

    private GameObject car;
    [SerializeField]
    private float jumpLegoDistance = 500000;
    void Start()
    {
        car = GameObject.FindGameObjectWithTag("Player");
        GetComponent<Button>().onClick.AddListener(delegate { Jumper(); });
    }
    public void Jumper()
    {
        if (checkground.isGroundforJump)
            car.GetComponent<Rigidbody>().AddForce((car.transform.position - car.transform.GetChild(
                car.transform.childCount-1).position).normalized * jumpLegoDistance);
    }
}
