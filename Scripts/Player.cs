using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using Unity.VisualScripting.AssemblyQualifiedNameParser;

public class Player: MonoBehaviour
{
    private Rigidbody2D rb;

    public TextMeshProUGUI textoStars;
    public int quantidade;
    public Vector3 posisao;

    [Header("Animação Settings")]
    public Animator anim;

    [Header("velocidade Settings")]
    public float velocidade;
    public float velPulor;
    public float velDash;
    public float velQueda = -1;

    [Header("Input Settings")]
    public float inputHori;
    public float inputVert;

    [Header("Bool Settings")]
    public bool move = true;
    public bool podePular;
    public bool direcaoLado;
    public bool podeDash;
    public bool chao;
    public bool wall;
    public bool slide;

    [Header("Bau Settings")]
    public Animator bauAnim;
    private GameObject bau;
    public bool fechar;
        private GameObject item;
        public bool pegar;
        public bool segura;

    [Header("placa Settings")]
    private GameObject placa;
    public bool ativa;

    private GameObject star;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        bau = GameObject.FindWithTag("Bau");  
        placa = GameObject.FindWithTag("Placa");
        star = GameObject.FindWithTag("Star");
    }

    // Update is called once per frame
    void Update()
    {
        Flip();
        GetInput();
        Inputkey();
        animControler();
        Pulor();
        StartCoroutine((Dash()));
        PassaDeFase();

        if (wall == true)
        {
            StartCoroutine((WallQ()));
            if (slide == true)
            {
                slide = false;
                inputHori = 0;
                rb.velocity = new Vector2(inputHori, velQueda);
            }
        }
    }

    private void FixedUpdate()
    {
        MoveLogic();
    }   

    void GetInput()
    {
        inputHori = Input.GetAxis("Horizontal");
        inputVert = Input.GetAxis("Vertical");
    }

    void MoveLogic()
    {
        if(move == true)
        {
            rb.velocity = new Vector2(inputHori * velocidade,  rb.velocity.y);
        }
    }
    void Pulor()
    {
        if(Input.GetKeyDown(KeyCode.Space) && podePular == true)
        {
            podePular = false;
            rb.velocity = new Vector2(rb.velocity.x, velPulor);
            slide = false;
        }
    }

    private IEnumerator Dash()
    {
        if(Input.GetKeyDown(KeyCode.Z) || (Input.GetKeyDown(KeyCode.LeftShift) && podeDash == true))
        {   
            move = false;
            podeDash = false;
            rb.velocity = new Vector2(inputHori * velDash, inputVert * velDash);
            yield return new WaitForSeconds(0.7f);
            move = true;
        }
    }

    private IEnumerator WallQ()
    {
        yield return new WaitForSeconds(1f);
        slide = true;
    }

    void Flip()
    {
        if(direcaoLado && inputHori > 0)
        {
            FlipLogic();
        }

        if (!direcaoLado && inputHori < 0)
        {
            FlipLogic();
        }
    }

    void FlipLogic()
    {
        direcaoLado = !direcaoLado;
        transform.Rotate(0f, -180.0f, 0f);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("chao"))
        {
            chao = true;
            podePular = true;
            wall = false;
            slide = false;
            podeDash = true;
        }

        if (col.gameObject.layer == LayerMask.NameToLayer("wall"))
        {
            podePular = true;

            wall = true;
        }

        if (col.gameObject.layer == LayerMask.NameToLayer("Bau"))
        {   
            fechar = true;
        }

        if (col.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            pegar = true;
        }

        if (col.gameObject.layer == LayerMask.NameToLayer("Placa"))
        {
            ativa = true;
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("chao"))
        {
            chao = false;
        }

        if (col.gameObject.layer == LayerMask.NameToLayer("wall"))
        {
            wall = false;
            slide = false;
        }

        if (col.gameObject.layer == LayerMask.NameToLayer("Bau"))
        {   
            fechar = false;
        }

        if (col.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            pegar = false;
        }

        if (col.gameObject.layer == LayerMask.NameToLayer("Placa"))
        {
            ativa = false;
        }
    }

    void Inputkey(){
        if ((Input.GetKeyDown(KeyCode.E)) || (Input.GetKeyDown(KeyCode.C)))
        {
            if (fechar == true)
            {
                quantidade--;
                textoStars.text = quantidade.ToString();
                bauAnim.SetBool("Fechar", fechar);
                fechar = false;
                pegar = true;
                bau.gameObject.tag = "Item";
                bau.gameObject.layer = 9;
                item = GameObject.FindWithTag("Item");
            }

            if (pegar == true)
            {
                item.transform.parent = transform;
                item.transform.position = transform.position;
                segura = true;
            }
            if (pegar == false && segura == true)
            {
                item.transform.parent = null;
                posisao = new Vector3(2f * inputHori, 0f, 0f);
                item.transform.position = transform.position + posisao;
                segura = false;
            }
        }
    }

    void animControler()
    {
        anim.SetFloat("Horizontal", rb.velocity.x);
        anim.SetFloat("Vertical", rb.velocity.y);
        anim.SetBool("podePular", podePular);
    }

    void PassaDeFase()
    {
        if(quantidade <= 0)
        {
            CarregarNovaFase();
        }
    }

    private void CarregarNovaFase()
    {
        SceneManager.LoadScene("Fase2");
    }

}