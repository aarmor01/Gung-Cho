﻿﻿using UnityEngine;

//Script que cambia el color del jugador cuando recibe daño e instancia unas partículas cuando se muere el jugador
public class EfectoJugadorDanyado : MonoBehaviour
{
    [SerializeField] Color ColorA = Color.red;
    [SerializeField] Color ColorB = Color.green;
    [SerializeField] float speed = 1.0f;
    [SerializeField] GameObject PrefabParticulasMuerteJugador = null;

    Color ColorIni;
    float tiempoInvulnerable = 3f;
    GameObject particulas;
    SpriteRenderer spriteRenderer;


    //comienza desactivado
    private void Awake()
    {
        enabled = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        ColorIni = spriteRenderer.color;  //guardamos el color inicial
    }

    private void Start()
    {
        VidaJugador vidaJugador = GetComponent<VidaJugador>();
        tiempoInvulnerable = vidaJugador.GetTiempoInvulnerable();  //establecemos el mismo tiempo de invulnerabilidad que actua en VidaJugador
    }

    //al activarse, si las vidas del jugador son 0, se instancian unas particulas que indican que se ha muerto
    private void OnEnable()
    {
        if (GameManager.instance.getVidas() <= 0)
        {
            particulas = Instantiate(PrefabParticulasMuerteJugador, transform.position, Quaternion.Euler(0f, 0f, 0f));
            ParticleSystem.MainModule main = particulas.GetComponent<ParticleSystem>().main;
            ParticleSystem particleSystem = particulas.GetComponent<ParticleSystem>();
            particleSystem.Play();
            //destruimos su GO con respecto al "lifetime" establecido en las particulas
            Destroy(particulas, main.startLifetime.constant);
            enabled = false; //y se desactiva el script
        }
    }

    //mientras esté activo, se cambia el color del jugador para indicar que ha recibido daño y que es invulnerable
    void Update()
    {
        spriteRenderer.color = Color.Lerp(ColorA, ColorB, Mathf.PingPong(Time.time * speed, 1.0f));
    }

    //al desactivarse, el jugador recupera su color original
    private void OnDisable()
    {
        if (spriteRenderer != null) spriteRenderer.color = ColorIni;
    }
}