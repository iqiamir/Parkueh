using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public  int playerHP = 100;
    //Reference to healthbar
    public Healthbar healthbar;
    //Reference to the UI text
    public TextMeshProUGUI playerHPText;
    public GameObject bloodOverlay;

    public static bool isGameOver;

    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;
        playerHP = 100;
        healthbar.SetMaxHealth(playerHP);
    }

    // Update is called once per frame
    void Update()
    {
        //Updating the HP UI text 
        playerHPText.text = playerHP + "/100"; 
        if (isGameOver)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    public IEnumerator TakeDamage(int damageAmount)
    {
        bloodOverlay.SetActive(true);

        playerHP -= damageAmount;
        healthbar.SetHealth(playerHP);

        if(playerHP <= 0)
        {
            isGameOver = true;
        }

        yield return new WaitForSeconds(1.5f);
        bloodOverlay.SetActive(false);
    }
}
