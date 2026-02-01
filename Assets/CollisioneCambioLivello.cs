using UnityEngine;
using UnityEngine.SceneManagement;
public class CollisioneCambioLivello : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        Debug.Log("Acceso");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   

    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("Funziona???");
       SceneManager.LoadScene(2);
        



    }
}
