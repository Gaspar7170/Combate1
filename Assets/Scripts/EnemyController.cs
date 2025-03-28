using UnityEngine;
using System.Collections;
public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;

    public int vida = 20;

    public float velocidad = 3f;

    public float espera = 2f;

    private bool puedeRecibirGolpe = true;
    private PlayerController player;
    private Vector3 posicionplayer;

    public GameObject slash;
    public GameObject thrust;
    public GameObject corazonesRotos;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.GetComponent<PlayerController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(vida <= 0){
            Destroy(gameObject);
        }
        
            
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!puedeRecibirGolpe) return;
        if (collision.CompareTag("Slash"))
        {
            
            MostrarCorazon();
            vida -= 5;
            if (vida <= 0){
                Destroy(gameObject);
            }
            
            Knockback();
            
            StartCoroutine(Cooldown());
            


        }
    }

    void Atacar()
    {
        
        int numeroAleatorio = Random.Range(0, 2);
        posicionplayer = player.transform.position;

        

        if (transform.position.x < posicionplayer.x)
        {
            // Ataque desde la derecha
            Vector3 posicionAtaque = transform.position + new Vector3(3.7f, 0f, 0f);
            if(numeroAleatorio == 0){
                Instantiate(slash, posicionAtaque, Quaternion.Euler(0f,0f,180f));
            }
            else{
                Instantiate(thrust, posicionAtaque, Quaternion.Euler(0f,0f,0f));
            }
        }
        else
        {
            // Ataque desde la izquierda
            Vector3 posicionAtaque = transform.position + new Vector3(-3.7f, 0f, 0f);
            if(numeroAleatorio == 0){
                Instantiate(slash, posicionAtaque, Quaternion.Euler(0f,0f,0f));
            }
            else{
                Instantiate(thrust, posicionAtaque, Quaternion.Euler(0f,0f,180f));
            }
        }
    }

    void MostrarCorazon()
    {
        Vector3 posicionCorazon = transform.position + new Vector3(0, 3.5f, 0);
        GameObject corazon = Instantiate(corazonesRotos, posicionCorazon, Quaternion.identity);
        Destroy(corazon, 0.3f);  // Destruye el corazón después de 0.3 segundo
        return;
    }
    private IEnumerator Cooldown()
    {
        puedeRecibirGolpe = false;
        yield return new WaitForSeconds(espera);
        puedeRecibirGolpe = true;
        Atacar();
    }
    void Knockback(){

        posicionplayer = player.transform.position;
        if (transform.position.x < posicionplayer.x)
        {
            rb.AddForce(Vector2.left * 3f, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(Vector2.right * 3f, ForceMode2D.Impulse);
        }
        rb.AddForce(Vector2.up * 2f, ForceMode2D.Impulse);
    }
}
