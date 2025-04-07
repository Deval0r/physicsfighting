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

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        moneyText = GetComponent<TextMeshProUGUI>(); 
        placeBuilding = FindObjectOfType<PlaceBuilding>();
    }
    void Update()
    {
        currentMoney = gameManager.money;
        maxMoney = (placeBuilding.bankCount * 1000) + 10000;
        if (currentMoney > maxMoney)
        {
            gameManager.money = maxMoney;
        }

        moneyText.text = "Money: $" + gameManager.money + "/" + maxMoney + "K";
    }
}
