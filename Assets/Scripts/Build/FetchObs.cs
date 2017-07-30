using UnityEngine;
using UnityEngine.UI;
public class FetchObs : MonoBehaviour
{

    [SerializeField]
    private Button MapButton;
    private static GameObject[] Shapes;

    void Start()
    {
        Shapes = Resources.LoadAll<GameObject>("Prefabs/Obstacles");   //Load shapes from prefabs file
        Button gb;
        for (int i = 0; i < Shapes.Length; i++)
        {
            gb = Instantiate(MapButton);
            string name = Shapes[i].name;
            gb.GetComponentInChildren<Text>().text = name;
            gb.name = name;
            gb.transform.SetParent(transform);
            gb.onClick.AddListener(delegate { AddObsListener(name); });
            gb.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    void AddObsListener(string name)
    {
        Map.curr.AddObstacle(name, true);
    }
}