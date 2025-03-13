using UnityEngine;
using TMPro;

public class FloatingDamageNumber : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float destroyTime = 1f;
    public TextMeshProUGUI damageText; // Public field to assign in the Inspector

    void Start()
    {
        // Debug log to check if damageText is assigned
        if (damageText == null)
        {
            Debug.LogError("TextMeshProUGUI component is not assigned.");
        }

        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
    }

    public void SetDamageText(int damageAmount)
    {
        if (damageText != null)
        {
            damageText.text = damageAmount.ToString();
        }
        else
        {
            Debug.LogError("damageText is not assigned.");
        }
    }
}
