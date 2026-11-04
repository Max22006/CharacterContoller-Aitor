using UnityEngine;

public class MeleEnemy : Enemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();
        FindPlayer();
    }
    public override void Attack()// Sobreescribe lo que hace la funcion principal, El base.() Lo que hace es ejecutar la funcion principal sin modificar.
    {
        base.Attack();
    }
}
