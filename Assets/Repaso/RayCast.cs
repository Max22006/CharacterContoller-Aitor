using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class RayCast : MonoBehaviour
{
    /*Crear Input de posicion del raton(Action type [Value], Control type [Vector 2], Binding [Position Mouse]). 
    Acordarse de los using. Crear los 2 inputs y el Awake y el Vector2.
    Añadir el using de añadir escenas*/

    private InputAction _clickAction;
    private InputAction _lookAction;

    private Vector2 _mousePosition;

    
    void Awake()
    {
        _clickAction = InputSystem.actions["Attack"];
        _lookAction = InputSystem.actions["MousePosition"];
    }
    
    void Update()
    {
        if(_clickAction.WasPressedThisFrame())
        {
            ShotRaycast();
        }
    }
    /*Crear el movimiento del personaje en esta funcion.*/
    void ShotRaycast()
    { 
        _mousePosition = _lookAction.ReadValue<Vector2>();

        Ray ray = Camera.main.ScreenPointToRay(_mousePosition);
        RaycastHit hit;
        
        if(Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            //Se puede usar cualquiera de las 3 dependiendo de lo que necesite.
            if(hit.collider.name == "objeto1")
            {
                SceneManager.LoadScene("Escena 1");  //Nombre de la escena a cargar, Añadir las escenas en el Build Profiles y arrastrar las escenas creadas.
            }
            if(hit.collider.CompareTag("Tag1"))
            {

            }
            if(hit.collider.gameObject.layer == 3)
            {

            }
        }

    }
}
