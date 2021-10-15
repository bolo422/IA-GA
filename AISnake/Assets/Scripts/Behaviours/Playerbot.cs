using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AIBehaviours/Playerbot")]
public class Playerbot : AIBehaviour
{
    public Vector3 waypoint = new Vector3(0, 0, 0);

    public float detectionRadius = 100f;
    public string botTag = "Bot";
    public string bodyTag = "Body";
    public string orbTag = "Orb";

   // bool encontrouInimigo = false;



    public override void Init(GameObject own, SnakeMovement ownMove)
    {
        base.Init(own, ownMove);
        ownerMovement.StartCoroutine(UpdateDirEveryXSeconds(0));
    }

    //seria interessante ter um controlador com o colisor que define o mundo pra poder gerar pontos dentro desse colisor

    public override void Execute()
    {
        MoveForward();
    }

    //ia basica, move, muda de direcao e move
    void MoveForward()
    {
        
        owner.transform.position = Vector2.MoveTowards(owner.transform.position, waypoint, ownerMovement.speed * Time.deltaTime);
        
    }


    IEnumerator UpdateDirEveryXSeconds(float x)
    {
        yield return new WaitForSeconds(x);
        ownerMovement.StopCoroutine(UpdateDirEveryXSeconds(1));
        //Collider[] hitColliders = Physics.OverlapSphere(owner.transform.position, detectionRadius);
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(owner.transform.position, detectionRadius);
        List<Vector2> orbs = new List<Vector2>();
        List<Vector2> bots = new List<Vector2>();
        List<Vector2> bodys = new List<Vector2>();


        // float menorDistanciaPos = -1;

      
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(orbTag))
            {
                orbs.Add(hitCollider.GetComponent<Transform>().position);
            }
            if (hitCollider.CompareTag(botTag))
            {
                bots.Add(hitCollider.GetComponent<Transform>().position);
            }
            if (hitCollider.CompareTag(bodyTag))
            {
                bodys.Add(hitCollider.GetComponent<Transform>().position);
            }
        }

        if (bots.Count < 1 && orbs.Count > 0)
        {
            Vector2[] orbsArray = orbs.ToArray();
            float menorDistancia = detectionRadius * 10;

            for (int i = 0; i < orbsArray.Length; i++)
            {
                if (Vector2.Distance(owner.transform.position, orbsArray[i]) < menorDistancia)
                {
                    menorDistancia = Vector2.Distance(owner.transform.position, orbsArray[i]);
                    waypoint = orbsArray[i];
                }
            }
        }
        //// a partir daqui está em desenvolvimento, se usar oq tem aicmas já funciona para seguir as comidas

        else if (orbs.Count > 0)
        {
            //Estado teste = Matar a cobrinha mais próxima, se ela estiver mais próxima q uma comida



            Vector2[] orbsArray = orbs.ToArray();
            float menorDistanciaOrbs = detectionRadius * 10;

            for (int i = 0; i < orbsArray.Length; i++)
            {
                if (Vector2.Distance(owner.transform.position, orbsArray[i]) < menorDistanciaOrbs)
                {
                    menorDistanciaOrbs = Vector2.Distance(owner.transform.position, orbsArray[i]);
                    waypoint = orbsArray[i];

                }
            }

            Vector2[] botsArray = bots.ToArray();
            float menorDistanciaBots = detectionRadius * 10;
            Vector2 botComMenorDistancia = new Vector2();

            for (int i = 0; i < botsArray.Length; i++)
            {
                if (Vector2.Distance(owner.transform.position, botsArray[i]) < menorDistanciaBots && Vector2.Distance(owner.transform.position, botsArray[i]) > 0)
                {
                    menorDistanciaBots = Vector2.Distance(owner.transform.position, botsArray[i]);
                    botComMenorDistancia = botsArray[i];
                }
            }
            Debug.Log("maq2");
            if (menorDistanciaOrbs >= menorDistanciaBots)
            {
                Debug.Log("maq3");
                Vector2[] bodysArray = bodys.ToArray();
                float menorDistanciaBodyToBot = detectionRadius * 10;
                Vector2 bodyComMenorDistancia = new Vector2(); ;

                for (int i = 0; i < bodysArray.Length; i++)
                {
                    float bodyToBot = Vector2.Distance(bodysArray[i], botComMenorDistancia);

                    if (bodyToBot < menorDistanciaBodyToBot)
                    {
                        menorDistanciaBodyToBot = bodyToBot;
                        bodyComMenorDistancia = bodysArray[i];
                    }
                }

                Vector2 pf = (botComMenorDistancia - bodyComMenorDistancia) + botComMenorDistancia;

                float mult = 1.5f;
                pf = new Vector2(pf.x + mult, pf.y + mult);
                waypoint = pf;
                Debug.Log("pf");

            }
            else
            {
                // vazio, pq o waypoint já está definido no orb mais próximo
            }





        }
        else if (bots.Count > 0)
        {
            Vector2[] botsArray = bots.ToArray();
            float menorDistanciaBots = detectionRadius * 10;
            Vector2 botComMenorDistancia = new Vector2();

            for (int i = 0; i < botsArray.Length; i++)
            {
                if (Vector2.Distance(owner.transform.position, botsArray[i]) < menorDistanciaBots && Vector2.Distance(owner.transform.position, botsArray[i]) > 0)
                {
                    menorDistanciaBots = Vector2.Distance(owner.transform.position, botsArray[i]);
                    botComMenorDistancia = botsArray[i];
                }
            }
            Debug.Log("maq2");

            Debug.Log("maq3");
            Vector2[] bodysArray = bodys.ToArray();
            float menorDistanciaBodyToBot = detectionRadius * 10;
            Vector2 bodyComMenorDistancia = new Vector2(); ;

            for (int i = 0; i < bodysArray.Length; i++)
            {
                float bodyToBot = Vector2.Distance(bodysArray[i], botComMenorDistancia);

                if (bodyToBot < menorDistanciaBodyToBot)
                {
                    menorDistanciaBodyToBot = bodyToBot;
                    bodyComMenorDistancia = bodysArray[i];
                }
            }

            Vector2 pf = (botComMenorDistancia - bodyComMenorDistancia) + botComMenorDistancia;

            float mult = 1.5f;
            pf = new Vector2(pf.x + mult, pf.y + mult);
            waypoint = pf;
            Debug.Log("pf");
        }
        else
        {
            waypoint = new Vector3(
        Random.Range(
            Random.Range(owner.transform.position.x - 10, owner.transform.position.x - 5),
            Random.Range(owner.transform.position.x + 5, owner.transform.position.x + 10)
            ),
        Random.Range(
            Random.Range(owner.transform.position.y - 10, owner.transform.position.y - 5),
            Random.Range(owner.transform.position.y + 5, owner.transform.position.y + 10)
            ),
            0
         );
            
        }


        direction = waypoint - owner.transform.position;
        direction.z = 0.0f;

        ownerMovement.StartCoroutine(UpdateDirEveryXSeconds(x));
    }






}
