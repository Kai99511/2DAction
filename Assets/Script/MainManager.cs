using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    [SerializeField, Header("ゲームオーバーUI")]
    private GameObject gameOverUI;
    [SerializeField,Header("ゲームクリアUI")]
    private GameObject gameClearUI;
    [SerializeField,Header("BGM")]
    private AudioSource bgm;
    [SerializeField, Header("決定音")]
    private GameObject submitSE;
    [SerializeField, Header("ゲームクリアSE")]
    private GameObject gameClearSE;
    [SerializeField, Header("ゲームオーバーSE")]
    private GameObject gameOverSE;
  
    private GameObject player;
    private bool bShowUI;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>().gameObject;
        bShowUI = false;
        FindObjectOfType<Fade>().FadeStart(MainStart);
        player.GetComponent<Player>().enabled = false;
        player.GetComponent<PlayerInput>().enabled = false;
        foreach (EnemySpawner enemySpawner in FindObjectsOfType<EnemySpawner>())
        {
            enemySpawner.enabled = false;
        }
    }

    private void MainStart()
    {
        player.GetComponent<Player>().enabled = true;
        player.GetComponent<PlayerInput>().enabled = true;
        foreach (EnemySpawner enemySpawner in FindObjectsOfType<EnemySpawner>())
        {
            enemySpawner.enabled = true;
        }
    }


    // Update is called once per frame
    void Update()
    {
        ShowGameOverUI();
    }

    private void ShowGameOverUI()
    {
        if ( player != null || gameOverUI.activeSelf) return;

        gameOverUI.SetActive(true);
        bShowUI = true;
        bgm.Stop();
        Instantiate(gameOverSE);
        
    }
    public void ShowGameClearUI()
    {
        if (gameClearUI.activeSelf) return;

        gameClearUI.SetActive(true);
        bShowUI = true;
        bgm.Stop();
        Instantiate(gameClearSE);
         
    }
    public void OnRestart(InputAction.CallbackContext context)
    {
        if (!bShowUI || !context.performed) return;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
        Instantiate(submitSE);
    }
   
    public void OnEscape(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        Application.Quit();
    }

}
