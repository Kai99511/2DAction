using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField, Header("HPÉAÉCÉRÉì")]
    private GameObject playerIcon;

    private Player player;
    private int beforeHP;
    


    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        beforeHP = player.GetHP();
        CreateHPicon();
    }

    private void CreateHPicon()
    {
        for (int i = 0; i < player.GetHP(); i++)
        {
            GameObject playerHPobj = Instantiate(playerIcon);
            playerHPobj.transform.SetParent(transform, false);

        }
    }

    // Update is called once per frame
    void Update()
    {
        showHPicon();
    }

    private void showHPicon()
    {
        if (beforeHP == player.GetHP()) return;

        Image [] icons = transform.GetComponentsInChildren<Image>();
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].gameObject.SetActive(i< player.GetHP());
        }
        beforeHP = player.GetHP();
    }
}
