using UnityEngine;
using TMPro;

public class PowerScript : MonoBehaviour
{
    private TextMeshProUGUI powerText;
    public GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        powerText = GetComponent<TextMeshProUGUI>(); 
    }
    void Update()
    {
        powerText.text = "Power: " + gameManager.power + "W";
    }
}
