using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AIBehaviours/Playerbot")]
public class Playerbot : AIBehaviour
{
    public Vector3 waypoint = new Vector3(0, 0, 0);

    public float detectionRadius = 1000;
    public string tagEnemyBoy = "bot";
    public string tagComidas = "Orb";



    public override void Init(GameObject own, SnakeMovement ownMove)
    {
        base.Init(own, ownMove);
        ownerMovement.StartCoroutine(UpdateDirEveryXSeconds(timeChangeDir));
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

        Collider[] hitColliders = Physics.OverlapSphere(owner.transform.position, detectionRadius);
        List<Vector2> comidas = new List<Vector2>();
        List<Vector2> bots = new List<Vector2>();


        float menorDistancia = detectionRadius * 10;
       // float menorDistanciaPos = -1;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(tagComidas))
            {
                comidas.Add(hitCollider.GetComponent<Transform>().position);
                Debug.Log("colisão");

            }
        }
        
        if (bots.Count < 1)
        {
            Vector2[] comidasArray = comidas.ToArray();



            for (int i = 0; i < comidasArray.Length; i++)
            {
                if(Vector2.Distance(owner.transform.position, comidasArray[i]) < menorDistancia)
                {
                    menorDistancia = Vector2.Distance(owner.transform.position, comidasArray[i]);

                    waypoint = comidasArray[i];
                    //enorDistanciaPos = -1;
                }
            }
        }


        direction = waypoint - owner.transform.position;
        direction.z = 0.0f;

        ownerMovement.StartCoroutine(UpdateDirEveryXSeconds(x));
    }



}
