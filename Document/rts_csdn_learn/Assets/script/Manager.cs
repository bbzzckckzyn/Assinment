using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Manager : MonoBehaviour
{
    public static Manager ins;
    private void Awake()
    {
        ins = this;
    }

    public Life _enemycastle,castle;

    public Text enemycastle, myCastle;
    private void Update()
    {
        enemycastle.text = "enemy castle: "+_enemycastle.hp.ToString();
        myCastle.text = "my castle: "+castle.hp.ToString();
        moneyText.text = "money: " + ((int)money).ToString();
        money += Time.deltaTime;
        if(_enemycastle.hp<=0)
        {
            Vec();

        }
        if(castle.hp<=0)
        {
            FailGame();
        }
    }
    public Text moneyText;
    public float money = 100;
    public Transform pos;
    public GameObject cross;
    public void getCross()
    {
        if(money>=100)
        {
            money -= 100;
            Instantiate(cross, pos.position, Quaternion.identity);
        }
    }
    public GameObject fail;
    public GameObject vec;
    void FailGame()
    {
        fail.SetActive(true);
        Time.timeScale = 0;
    }
    void Vec() 
    {
        vec.SetActive(true);
        Time.timeScale = 0;
    }
    public void NewGame()
    {
        SceneManager.LoadScene("DemoScene_02", LoadSceneMode.Single);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
