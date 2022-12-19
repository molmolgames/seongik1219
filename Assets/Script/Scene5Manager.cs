using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class Scene5Manager : MonoBehaviour
{
    GameObject[] enemy;
    public Slider bossHpSlider;
    public Text clearText;
    public GameObject backGroundMusic;
    public GameObject BossMusic;
    public GameObject clearMusic;
    private int CNT = 0;

    private void Update()
    {
        enemy = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemy.Length >= 1)
        {
            CNT = 1;
            backGroundMusic.gameObject.SetActive(false);
            bossHpSlider.gameObject.SetActive(true);
            BossMusic.gameObject.SetActive(true);
        }
        if(enemy.Length <= 0 && CNT == 1)
        {
            BossMusic.gameObject.SetActive(false);
            bossHpSlider.gameObject.SetActive(false);
            clearText.gameObject.SetActive(true);
            clearMusic.gameObject.SetActive(true);
        }
    }
}
