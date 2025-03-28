using UnityEngine;

public class SlashController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 0.15f);
    }
    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Block"))
        {
            Destroy(collision.gameObject);
        }else{
            Destroy(gameObject, 0.15f);
        }
    }
}
