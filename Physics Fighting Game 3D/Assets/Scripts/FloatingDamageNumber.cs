using UnityEngine;
using TMPro;

public class FloatingDamageNumber : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float destroyTime = 1f;
    private TextMeshProUGUI damageText;

    void Start()
    {
        damageText = GetComponent<TextMeshProUGUI>();
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
    }

    public void SetDamageText(int damageAmount)
    {
        damageText.text = damageAmount.ToString();
    }
}
