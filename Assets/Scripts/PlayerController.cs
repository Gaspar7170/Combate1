using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public int vida = 20;
    public float velocidad = 5f;
    public float salto = 5f;
    private bool puedeSaltar = true;
    private bool puedeatacar = true;
    private float cooldownataque = 1f;
    private float direccion = 1f;
    private bool bloqueando = false;
    private EnemyController enemy;
    private Vector3 posicionenemigo;
    private bool puedeMoverse = true;

    public GameObject slash;
    public GameObject thrust;
    public GameObject block;
    public GameObject corazonesRotos;
    public Transform personaje;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //Obtenemos el componente del enemigo para saber de donde nos ataca.
        GameObject enemyObject = GameObject.FindGameObjectWithTag("Enemigo");
        if (enemyObject != null)
        {
            enemy = enemyObject.GetComponent<EnemyController>();
        }
    }


    // Update is called once per frame
    void Update()
    {
        //Movimiento Lateral
        if (!puedeMoverse) return;
        if (Input.GetKey(KeyCode.D))
        {
            if(direccion == -1f){Flip();}
            transform.Translate(Vector2.right * velocidad * Time.deltaTime);
            direccion = 1f;
            
        }
        if (Input.GetKey(KeyCode.A))
        {
            if(direccion == 1f){Flip();}
            transform.Translate(Vector2.left * velocidad * Time.deltaTime);
            direccion = -1f;
        }
        //Salto
        if (Input.GetKeyDown(KeyCode.W) && puedeSaltar)
        {
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(Vector2.up * salto, ForceMode2D.Impulse);
            puedeSaltar = false;
        }

        

        //Ataque slash
        if(Input.GetMouseButtonDown(0) && puedeatacar){

            Vector3 posicionAtaque= personaje.position + new Vector3(3f*direccion,0f,0f);
            StartCoroutine(Freeze(0.3f));
            if(direccion == 1f){
                Instantiate(slash, posicionAtaque, Quaternion.Euler(0f,0f,0f));
            }
            else{
                Instantiate(slash, posicionAtaque, Quaternion.Euler(0f,0f,180f));
            }
            
            
            puedeatacar = false;
            StartCoroutine(Cooldown());

        }
        //Ataque thrust
        if(Input.GetMouseButtonDown(1) && puedeatacar){

            Vector3 posicionAtaque= personaje.position + new Vector3(3f*direccion,0f,0f);
            StartCoroutine(Freeze(0.3f));
            if(direccion == 1f){
                Instantiate(thrust, posicionAtaque, Quaternion.Euler(0f,0f,0f));
            }
            else{
                Instantiate(thrust, posicionAtaque, Quaternion.Euler(0f,0f,180f));
            }
            
            
            puedeatacar = false;
            StartCoroutine(Cooldown());
        }
        //Bloqueo
        if(Input.GetKeyDown(KeyCode.Q)){
            Vector3 posicionAtaque= personaje.position + new Vector3(3f*direccion,1f,0f);

            if(direccion == 1f){
                Instantiate(block, posicionAtaque, Quaternion.Euler(0f,0f,0f));
            }
            else{
                Instantiate(block, posicionAtaque, Quaternion.Euler(0f,180f,0f));
            }

            bloqueando = true;
            puedeMoverse = false;
            puedeatacar = false;
            Debug.Log("¡El jugador está bloqueando!");
            StartCoroutine(CooldownBlock());
        }
        
        

    }
    private IEnumerator CooldownBlock()
    {
        yield return new WaitForSeconds(3f);
        bloqueando = false;
        puedeatacar = true;
        puedeMoverse = true;
        Debug.Log("¡El jugador dejo de bloqueando!");
    }
    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldownataque);
        puedeatacar = true;
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            puedeSaltar = true;
        }

        

        if (collision.gameObject.CompareTag("Enemigo"))
        {
            vida -= 5;
            if (vida <= 0)
            {
                Destroy(gameObject);
            }
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Slash") && !bloqueando)
        {
            Debug.Log("¡El Slash ha golpeado al jugador!");
            MostrarCorazon();
            vida -= 5;
            Knockback();
            if (vida <= 0){
                Destroy(gameObject);
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
    void Knockback(){
        
        posicionenemigo = enemy.transform.position;
        if (transform.position.x < posicionenemigo.x)
        {
            rb.AddForce(Vector2.left * 3f, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(Vector2.right * 3f, ForceMode2D.Impulse);
        }
        rb.AddForce(Vector2.up * 2f, ForceMode2D.Impulse);
    }
    
    private IEnumerator Freeze(float segundos)
    {
        puedeMoverse = false;
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(segundos);
        puedeMoverse = true;
    }

    void Flip(){
    // Cambia la escala en el eje X, lo que invierte el sprite visualmente
    Vector3 localScale = transform.localScale;
    localScale.x = -localScale.x; // Cambia la dirección (flipa en Y)
    transform.localScale = localScale;
    }
}
