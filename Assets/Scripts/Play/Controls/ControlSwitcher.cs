using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class ControlSwitcher : MonoBehaviour {

    public GameObject m_car;


    public GameObject PlayModeButtons;
    private MastarCarUserControl m_cuc;
    public static bool AcceloMode = false;
    void Start()
    {
        m_cuc = m_car.GetComponent<MastarCarUserControl>();
    }
    void Awake()
    {
        if (AcceloMode)
        {
            PlayModeButtons.transform.GetChild(PlayModeButtons.transform.childCount - 1).gameObject.SetActive(false);
            PlayModeButtons.transform.GetChild(PlayModeButtons.transform.childCount - 2).gameObject.SetActive(false);
        }
        else
        {
            PlayModeButtons.transform.GetChild(PlayModeButtons.transform.childCount - 1).gameObject.SetActive(true);
            PlayModeButtons.transform.GetChild(PlayModeButtons.transform.childCount - 2).gameObject.SetActive(true);
        }
    }
    public void AcceloOn()
    {   AcceloMode = true;
        PlayModeButtons.transform.GetChild(PlayModeButtons.transform.childCount - 1).gameObject.SetActive(false);
        PlayModeButtons.transform.GetChild(PlayModeButtons.transform.childCount - 2).gameObject.SetActive(false);
    }

    public void ButtonsOn()
    {
        AcceloMode = false;
        PlayModeButtons.transform.GetChild(PlayModeButtons.transform.childCount - 1).gameObject.SetActive(true);
        PlayModeButtons.transform.GetChild(PlayModeButtons.transform.childCount - 2).gameObject.SetActive(true);
    }
}
