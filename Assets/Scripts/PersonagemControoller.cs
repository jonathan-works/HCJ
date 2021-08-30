using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PersonagemControoller : MonoBehaviour
{
    public Animator personagemAnimator;
    float input_x = 0;
    float input_y = 0;
    public float velocidade = 2.5f;
    bool esta_andando = false;

    Rigidbody2D rb2D;
    Vector2 movimento = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        esta_andando = false;
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
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

        if(Input.GetButtonDown("Fire1")){
            personagemAnimator.SetTrigger("ataque");
        }

    }

    private void FixedUpdate() 
    {
        rb2D.MovePosition(rb2D.position + movimento * velocidade * Time.fixedDeltaTime);
    }
}
