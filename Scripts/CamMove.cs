using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{

    private GameObject jogador;
    public Vector3 posisao;

    // Start is called before the first frame update
    void Start()
    {
        jogador = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = jogador.transform.position + posisao;
    }
}
