using UnityEngine;
using TMPro;
public class GameShareUI : MonoBehaviour
{
    public static GameShareUI Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    [SerializeField] TMP_Text[] coinsUIText;

    void Start()
    {
        UpdateCoinsUIText();
    }

    private void Update()
    {
        UpdateCoinsUIText();
    }

    public void UpdateCoinsUIText()
    {
        for (int i = 0; i < coinsUIText.Length; i++)
        {
            SetCoinsText(coinsUIText[i], GameDataManager.GetCoins());
        }
    }

    void SetCoinsText(TMP_Text textMesh, int value)
    {
        textMesh.text = value.ToString();
    }
}
