using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    void Awake()
    {
        Instance = this;
    }

    public void Sacudir(float duracion, float fuerza)
    {
        StartCoroutine(RutinaSacudida(duracion, fuerza));
    }

    public IEnumerator RutinaSacudida(float duracion, float fuerza)
    {
        Vector3 posicionOriginal = transform.localPosition;

        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracion)
        {
            float offsetX = Random.Range(-1f, 1f) * fuerza;
            float offsetY = Random.Range(-1f, 1f) * fuerza;

            transform.localPosition = new Vector3(
                posicionOriginal.x + offsetX,
                posicionOriginal.y + offsetY,
                posicionOriginal.z
            );

            tiempoTranscurrido += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = posicionOriginal;
    }
}