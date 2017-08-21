using UnityEngine;
using UnityEngine.UI;

public class Count321 : MonoBehaviour
{
    Text b;
    [SerializeField]
    float scale = 1.7f;
    float acum;
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
            Camera.main.transform.position = new Vector3(-0.21f, 3.35f, -10f);
            Camera.main.transform.eulerAngles = new Vector3(9.405001f, -0.328f, 0.808f);
        }
        else if(acum>1 && acum <= 2)
        {
            Camera.main.transform.position = new Vector3(22.49f, 8.07f, -3.94f);
            Camera.main.transform.eulerAngles = new Vector3(15.797f, -96.45701f, 0);
        }
        else if (acum > 2 && acum <= 3)
        {
            Camera.main.transform.position = new Vector3(0.15f, 2.81f, -2.63f);
            Camera.main.transform.eulerAngles = new Vector3(11.7f, -177f, 0.75f);
        }
        b.text = (3 - (int)(acum*scale)).ToString();
        if (acum >= 3/scale)
        {
            gameObject.SwitchPage("page");
        }
    }
}
