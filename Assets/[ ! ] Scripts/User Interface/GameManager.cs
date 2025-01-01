using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private GameObject Player_GO;
    private GameObject Boss_GO;
    [SerializeField] private GameObject[] ShipList;
    [SerializeField] private TMP_Text Increased_Coin;
    [SerializeField] private GameObject VictoryScreen;
    [SerializeField] private GameObject DefeatedScreen;

    private void Start()
    {
        VictoryScreen.transform.localScale = Vector3.zero;
        DefeatedScreen.transform.localScale = Vector3.zero;

        Player_GO = GameObject.FindGameObjectWithTag("Player");
        if (Player_GO == null)
        {
            Instantiate(ShipList[GameDataManager.GetSelectedShip()], new Vector3(0, -3.5f, 0), ShipList[GameDataManager.GetSelectedShip()].transform.rotation);
        }
    }
    private void Update()
    {
        Player_GO = GameObject.FindGameObjectWithTag("Player");
        if (Player_GO != null && Player_GO.transform.localScale == Vector3.zero) 
        {
            GameDataManager.DeactivatePurchasedShip(GameDataManager.GetSelectedShip());
            DefeatedScreen.transform.localScale = new Vector3(2, 2, 2);
            StartCoroutine(WaitToLobby());
        }

        Boss_GO = GameObject.FindGameObjectWithTag("Enemy - Boss");
        if (Boss_GO != null) 
        { 
            BossHealth Boss = Boss_GO.GetComponent<BossHealth>();
            
            if (Boss != null) 
            {
                Increased_Coin.text = $"{Boss.IncreasedCoin}";
            }
        }
        if (Boss_GO != null && Boss_GO.transform.localScale == Vector3.zero)
        {
            VictoryScreen.transform.localScale = new Vector3(2, 2, 2);
            StartCoroutine(WaitToLobby());
        }

    }

    IEnumerator WaitToLobby() 
    {
        yield return new WaitForSeconds(3);
        VictoryScreen.transform.localScale = Vector3.zero;
        DefeatedScreen.transform.localScale = Vector3.zero;
        SceneManager.LoadScene("Main Lobby");
    }
}
