using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        slider.maxValue = player.GetHealth();
    }

    void Update()
    {
        slider.value = player.GetHealth();
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
