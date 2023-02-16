using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{

    public GameObject healthUIPrefab;
    public Transform barPoint;
    public bool alwaysVisiable;
    private float timeLeft;
    public float visiableTime;

    Image healthSlider;
    Transform UIbar;
    Transform cam;

    CharacterStats currentStats;

    private void Awake()
    {
        currentStats= GetComponent<CharacterStats>();

        currentStats.UpdateHealthBarOnAttack += UpdateHealthBar;
    }

    private void OnEnable()
    {
        cam = Camera.main.transform;

        foreach (Canvas canvas in FindObjectsOfType<Canvas>()) 
        {
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                UIbar = Instantiate(healthUIPrefab, canvas.transform).transform;             
                healthSlider = UIbar.GetChild(0).GetComponent<Image>();
                UIbar.gameObject.SetActive(alwaysVisiable);
            }
        }
    }
    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if(currentHealth <= 0)
            Destroy(UIbar.gameObject);
        UIbar.gameObject.SetActive(true);
        timeLeft = visiableTime;
        float sliderPercent = (float)currentHealth / maxHealth;    
        healthSlider.fillAmount = sliderPercent;
    }

    private void LateUpdate()
    {
        if (UIbar != null)
        { 
            UIbar.position = barPoint.position;
            UIbar.forward = -cam.forward;
            if (timeLeft <= 0 && !alwaysVisiable)
                UIbar.gameObject.SetActive(false);
            else
                timeLeft -= Time.deltaTime;
            
            
        }
    }


}
