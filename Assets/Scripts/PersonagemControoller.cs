using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonagemControoller : MonoBehaviour
{
    public Animator personagemAnimator;
    float input_x = 0;
    float input_y = 0;
    public float velocidade = 2.5f;
    bool esta_andando = false;
    // Start is called before the first frame update
    void Start()
    {
        esta_andando = false;
    }

    // Update is called once per frame
    void Update()
    {
        input_x = Input.GetAxisRaw("Horizontal");
        input_y = Input.GetAxisRaw("Vertical");
        
        esta_andando = (input_x !=0 || input_y != 0);

        if (esta_andando)
        {
            var movimento = new Vector3(input_x, input_y, 0).normalized;
            transform.position += movimento * velocidade * Time.deltaTime;
            personagemAnimator.SetFloat("input_x", input_x);
            personagemAnimator.SetFloat("input_y", input_y);
        }
        personagemAnimator.SetBool("esta_andando", esta_andando);

    }
}
