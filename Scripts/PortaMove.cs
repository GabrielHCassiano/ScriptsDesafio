using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortaMove : MonoBehaviour
{
    public float velocidade;

    public Transform porta;
    public Transform[] posAtual;
    public int idPos;

    private GameObject jogador;

    // Start is called before the first frame update
    void Start()
    {
        jogador = GameObject.FindWithTag("Player");
        porta.position = posAtual[0].position;
        idPos = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (jogador.GetComponent<Player>().ativa == true)
        {
            porta.position = Vector2.MoveTowards(porta.position, posAtual[idPos].position, velocidade * Time.deltaTime);

            if (porta.position == posAtual[idPos].position)
            {
                idPos += 1;
            }

            if (idPos == posAtual.Length)
            {
                idPos = 0;
            }
        }
    }
}
