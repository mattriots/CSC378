using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShurikenCount : MonoBehaviour
{

    [SerializeField] public PlayerCombat player;
    [SerializeField] public Text shurikenText;


    // Start is called before the first frame update
    void Start()
    {
        shurikenText.text = "Shuriken: 0";
    }

    // Update is called once per frame
    void Update()
    {
        shurikenText.text = "Shuriken: " + player.shurikenCount;
    }
}
