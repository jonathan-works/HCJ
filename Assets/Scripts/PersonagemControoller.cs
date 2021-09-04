using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Player))]
[RequireComponent(typeof(BoxCollider2D))]
public class PersonagemControoller : MonoBehaviour
{
    public Player player;
    public Animator personagemAnimator;
    float input_x = 0;
    float input_y = 0;
    public float velocidade = 2.5f;
    bool esta_andando = false;
    public BoxCollider2D boxCollider2D;

    Rigidbody2D rb2D;
    Vector2 movimento = Vector2.zero;

    void Start()
    {
        esta_andando = false;
        rb2D = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if(boxCollider2D.isTrigger)
            boxCollider2D.isTrigger = false;

        input_x = Input.GetAxisRaw("Horizontal");
        input_y = Input.GetAxisRaw("Vertical");
        
        esta_andando = (input_x !=0 || input_y != 0);

        movimento = new Vector2(input_x, input_y);

        if (esta_andando)
        {
            personagemAnimator.SetFloat("input_x", input_x);
            personagemAnimator.SetFloat("input_y", input_y);
        }

        personagemAnimator.SetBool("esta_andando", esta_andando);
        
        if(player.entity.attackTimer <= 0){
            player.entity.attackTimer = 0;
        }else{
            player.entity.attackTimer -= Time.deltaTime;
        }

        if(player.entity.attackTimer == 0 && !esta_andando){
            if(Input.GetButtonDown("Fire1")){
                personagemAnimator.SetTrigger("ataque");
                player.entity.attackTimer = player.entity.cooldown;
                ataque();
            }
        }
    }

    private void FixedUpdate() 
    {
        rb2D.MovePosition(rb2D.position + movimento * player.entity.speed * Time.fixedDeltaTime);
    }

    private void OnTriggerStay2D(Collider2D collider) {
        if(collider.transform.tag == "Enemy"){
            player.entity.target = collider.transform.gameObject;
        }
    }
    
    private void OnTriggerExit2D(Collider2D collider) {
        if(collider.transform.tag == "Enemy"){
            player.entity.target = null;
        }
    }

    void ataque(){
        if(player.entity.target == null){
            Debug.Log("ESTÁ DANDO TARGET NULL");
            return;
        }

        Boss1 boss1 = player.entity.target.GetComponent<Boss1>();

        if(boss1.entity.dead){
            player.entity.target = null;
            return;
        }

        float distancia = Vector2.Distance(transform.position, player.entity.target.transform.position);

        if(distancia <= player.entity.attackDistance){
            int dmg = player.manager.CalculateDamage(player.entity, player.entity.damage);
            int monstroDefesa = player.manager.CalculateDefense(boss1.entity, boss1.entity.defense);
            int resultado = dmg - monstroDefesa;

            if(resultado < 0)
            {
                resultado = 0;
                Debug.Log("Inimigo defendeu tudo");
            }
            
            Debug.Log("Personagem atacando: "+ resultado.ToString() + "E vida do boss ficou: " + boss1.entity.currentHealth);
            boss1.entity.currentHealth -= resultado;
            boss1.entity.target = this.gameObject;
            boss1.animator.SetTrigger("apanhar");
        }
        else
        {
            Debug.Log("Inimigo longe");
            player.entity.target = boss1.gameObject;
        }
    }
}
