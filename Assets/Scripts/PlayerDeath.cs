using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    PlayerStats stats;
    public static PlayerDeath instance;
    private bool died;


    public GameObject Player;
    public GameObject CamController;
    public GameObject DeathEffect;
    public GameObject PlayerUI;
    public GameObject Store1;
    public GameObject Store2;
    public GameObject PauseScreenUI;
    public GameObject RespawnUI;

    // Start is called before the first frame update
    void Start()
    {
        instance = this.GetComponent<PlayerDeath>();
        died = false;
    }

    // Update is called once per frame
    void Update()
    {

        stats = PlayerStats.instance;
        if (died && stats.health < 1)
        {
        
            Die();
        }

        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //   stats.health = 0;
        //}
    }




    public void Die()
    {
        if (!died)
        {

            Destroy(Instantiate(DeathEffect, Player.transform.position, Quaternion.identity, null), 1f);
            CamController.SetActive(false);
            Player.GetComponent<CharacterController>().enabled = false;
            Time.timeScale = 0.5f;
            StartCoroutine(Delay(0.5f));
            //died = true;
        }
        else
        {
            GameManager.instance.cursorLock = false;
            Time.timeScale = 0f;
            CamController.SetActive(false);
            PlayerUI.SetActive(false);
            Store1.SetActive(false);
            Store2.SetActive(false);
            PauseScreenUI.SetActive(false);
            RespawnUI.SetActive(true);

        }
    }



    private IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
        died = true;
    }


    //Respawn button commands

    public void Respawn()
    {

        //SceneManager.LoadScene("Respawn");
        Time.timeScale = 1;
        //SceneManager.UnloadSceneAsync("Scene");
        GameManager.instance.LoadData();
        
        GameManager.instance.cursorLock = true;

        CamController.SetActive(true);
        PlayerUI.SetActive(true);
        Store1.SetActive(false);
        Store2.SetActive(false);
        PauseScreenUI.SetActive(false);
        RespawnUI.SetActive(false);

        
    }

    public void Quit()
    {
        Time.timeScale = 1;
        //SceneManager.UnloadSceneAsync("Scene");
        GameManager.instance.LoadData();

        GameManager.instance.cursorLock = false;

        CamController.SetActive(true);
        PlayerUI.SetActive(true);
        Store1.SetActive(false);
        Store2.SetActive(false);
        PauseScreenUI.SetActive(false);
        RespawnUI.SetActive(false);

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("MainMenu");
    }
}
