using UnityEngine;
using UnityEngine.UI;
public class FetchShapes : MonoBehaviour {

    [SerializeField]
    private Button _mapButton;
    private static GameObject[] _shapes;
    
    void Start()
    {
        _shapes = Resources.LoadAll<GameObject>("Prefabs/Shapes");   //Load shapes from prefabs file
        Button gb;
        for (int i = 0; i < _shapes.Length; i++)
        {
            gb = Instantiate(_mapButton);
            string name = _shapes[i].name;
            gb.GetComponentInChildren<Text>().text = name;
            gb.name = name;
            gb.transform.SetParent(transform);
            gb.onClick.AddListener(delegate { AddBrickListener(name); });
            gb.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    void AddBrickListener(string name)
    {
		Map.Curr.AddBrick(name,true);
    }
}