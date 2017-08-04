using UnityEngine;
using UnityEngine.UI;

public class FetchShapes : MonoBehaviour {

    [SerializeField]
    private Button _mapButton;
    private static GameObject[] _shapes;
    
    void Start()
    {
        _shapes = Resources.LoadAll<GameObject>("Prefabs/Shapes");   //Load shapes from prefabs file
        foreach (var t in _shapes)
        {
            var gb = Instantiate(_mapButton);
            var shapeName = t.name;
            gb.GetComponentInChildren<Text>().text = shapeName;
            gb.name = shapeName;
            gb.transform.SetParent(transform);
            gb.onClick.AddListener(delegate { AddBrickListener(shapeName); });
            gb.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private static void AddBrickListener(string name)
    {
		Map.Curr.AddBrick(name,true);
    }
}