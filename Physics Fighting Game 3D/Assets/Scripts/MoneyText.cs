using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class MoneyText : MonoBehaviour
{
    private TextMeshProUGUI moneyText;
    public GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        moneyText = GetComponent<TextMeshProUGUI>(); 
    }
    void Update()
    {
        moneyText.text = "Money: $" + gameManager.money;
    }
}
