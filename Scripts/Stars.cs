using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Stars : MonoBehaviour
{
    public TextMeshProUGUI textoStars;
    public int coleta;
    public bool pegarStar;

    private GameObject jogador;

    // Start is called before the first frame update
    void Start()
    {
        jogador = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(jogador.GetComponent<Player>().chao == true && pegarStar == true)
        {
            jogador.GetComponent<Player>().chao = false;
            pegarStar = false;
            coleta = jogador.GetComponent<Player>().quantidade--;
            textoStars.text = coleta.ToString();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        pegarStar = true;
        transform.parent = jogador.transform;
    }

    private void OnTriggerExit2D(Collider2D col)
    {

    }
}
