using UnityEngine;
using UnityEngine.UI;
public class FetchShapes : MonoBehaviour {

    [SerializeField]
    private Button MapButton;
    private static GameObject[] Shapes;
    
    void Start()
    {
        Shapes = Resources.LoadAll<GameObject>("Prefabs/Shapes");   //Load shapes from prefabs file
        Button gb;
        for (int i = 0; i < Shapes.Length; i++)
        {
            gb = Instantiate(MapButton);
            string name = Shapes[i].name;
            gb.GetComponentInChildren<Text>().text = name;
            gb.name = name;
            gb.transform.SetParent(transform);
            gb.onClick.AddListener(delegate { AddBrickListener(name); });
            gb.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    void AddBrickListener(string name)
    {
		Map.curr.AddBrick(name,true);
    }
}