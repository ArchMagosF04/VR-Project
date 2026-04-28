using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image Red;
    public Image Green;
    public Image Blue;

    [SerializeField] float maxHealth;
    public float currentHealth;

    private void Awake()
    {
        maxHealth = 100;
        currentHealth = 50;
    }
    private void Start()
    {
        Red = GameObject.Find("Red").GetComponent<Image>();
        Green = GameObject.Find("Green").GetComponent<Image>();
    }

    void barBehaviour()
    {
        Red.fillAmount = currentHealth / maxHealth;
        Green.fillAmount = currentHealth / maxHealth;
    }

    void shieldOn()
    {
        if (currentHealth > maxHealth)
            {
            Blue.enabled = true;
        }
        else
        {
            Blue.enabled = false;
        }
    }

    private void Update()
    {
        barBehaviour();
        shieldOn();
    }
}
