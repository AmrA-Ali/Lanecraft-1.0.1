using UnityEngine;
using UnityEngine.UI;
public class Count321 : MonoBehaviour
{
    Text b;
    [SerializeField]
    float scale = 1.7f;
    float acum = 0;
    void Start()
    {
        b = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        acum += Time.deltaTime;
        if(acum <= 1)
        {
            Camera.main.transform.position = new Vector3(-7.13f, 3.43f, -8.19f);
            Camera.main.transform.eulerAngles = new Vector3(19.8f, 61.6f, 0);
        }
        else if(acum>1 && acum <= 2)
        {
            Camera.main.transform.position = new Vector3(6.83f, 5.21f, -0.64f);
            Camera.main.transform.eulerAngles = new Vector3(31f, -120f, 0);
        }
        else if (acum > 2 && acum <= 3)
        {
            Camera.main.transform.position = new Vector3(0.03f, 0.92f, -4.92f);
            Camera.main.transform.eulerAngles = new Vector3(11.7f, -177f, 0.75f);
        }
        b.text = (3 - (int)(acum*scale)).ToString();
        if (acum >= 3/scale)
        {
            gameObject.SwitchPage("page");
        }
    }
}
