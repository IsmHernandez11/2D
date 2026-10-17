using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDoor : MonoBehaviour
{
    public string nombreSiguienteEscena = "Nivel2";

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") || other.gameObject.name == "Player"){
            SceneManager.LoadScene(nombreSiguienteEscena);
        }
    }
}