using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonFunctions : MonoBehaviour
{
    [Header("Main Lobby")]
    [SerializeField] private Button Play_Button;
    [SerializeField] private Button Settings_Button;
    [SerializeField] private Button Shop_Button; // this one is also added to the "Ship Selector"
    [SerializeField] private GameObject[] ShipList;
    [SerializeField] private GameObject Notification;


    [Header("Level Select")]
    [SerializeField] private Button HighLevel_Button;
    [SerializeField] private Button MidLevel_Button;
    [SerializeField] private Button LowLevel_Button;

    [Header("General Buttons")]
    [SerializeField] private Button Return_Button;

    private int selectedShip;

    private void Start()
    {

        if (Play_Button != null)
        {
            Play_Button.onClick.AddListener(AbleToPlay);
        }

        if (Settings_Button != null)
        {
            Settings_Button.onClick.AddListener(() => LoadScene("Settings"));
        }

        if (Shop_Button != null)
        {
            Shop_Button.onClick.AddListener(() => LoadScene("Shop"));
        }

        if (Return_Button != null)
        {
            Return_Button.onClick.AddListener(() => LoadScene("Main Lobby"));
        }

        if (HighLevel_Button != null)
        {
            HighLevel_Button.onClick.AddListener(() => LoadScene("Hard Challenge"));
        }

        if (MidLevel_Button != null)
        {
            MidLevel_Button.onClick.AddListener(() => LoadScene("Medium Challenge"));
        }

        if (LowLevel_Button != null)
        {
            LowLevel_Button.onClick.AddListener(() => LoadScene("Easy Challenge"));
        }
    }

    private void Update()
    {
        if (GameDataManager.GetTotalPurchasedShips() == 0) 
        {
            selectedShip = -1;
        }

        if (GameDataManager.GetTotalPurchasedShips() > 0)
        {
            selectedShip = GameDataManager.GetSelectedShip();
        }

        if (ShipList != null && ShipList.Length > 0)
        {
            bool validSelection = selectedShip >= 0 && selectedShip < ShipList.Length;

            for (int counter = 0; counter < ShipList.Length; counter++)
            {
                ShipList[counter]?.SetActive(validSelection && counter == selectedShip);
            }

            if (Notification != null)
            {
                Notification.SetActive(!validSelection);
            }
        }
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void AbleToPlay() 
    {
        if (GameDataManager.GetTotalPurchasedShips() != 0 && selectedShip >= 0) 
        {
            SceneManager.LoadScene("Level Select");
        }
    }
}
