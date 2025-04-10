using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class MoneyText : MonoBehaviour
{
    private TextMeshProUGUI moneyText;
    public GameManager gameManager;
    public PlaceBuilding placeBuilding;

    private int currentMoney;
    private int maxMoney;
    private float displayMoney;
    private float displayMaxMoney;
    public float lerpSpeed = 10f;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        moneyText = GetComponent<TextMeshProUGUI>(); 
        placeBuilding = FindObjectOfType<PlaceBuilding>();
        displayMoney = gameManager.money;
        displayMaxMoney = (placeBuilding.bankCount * 1000) + 10000;
    }
    void Update()
    {
        currentMoney = gameManager.money;
        maxMoney = (placeBuilding.bankCount * 1000) + 10000;
        if (currentMoney > maxMoney)
        {
            gameManager.money = maxMoney;
        }

        displayMoney = Mathf.Lerp(displayMoney, currentMoney, Time.deltaTime * lerpSpeed);
        displayMaxMoney = Mathf.Lerp(displayMaxMoney, maxMoney, Time.deltaTime * lerpSpeed);

        moneyText.text = "Money: $" + Mathf.RoundToInt(displayMoney) + "/" + Mathf.RoundToInt(displayMaxMoney) + "K";
    }
}
