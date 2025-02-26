using UnityEngine;

public class RobotEngineSound : MonoBehaviour
{
    public Rigidbody robotRigidbody;  // Referencia al Rigidbody del robot
    public AudioSource motorAudio;    // AudioSource con el sonido del motor
    public float minPitch = 0.5f;     // Tono mínimo del motor
    public float maxPitch = 4.0f;     // Tono máximo del motor
    public float maxSpeed = 2.0f;      // Velocidad máxima del robot

    void Start()
    {
        if (motorAudio == null)
        {
            Debug.LogError("No se asignó el AudioSource al motor");
            return;
        }

        // Asegurar que el sonido del motor esté en loop
        motorAudio.loop = true;

        // Iniciar el sonido del motor
        motorAudio.Play();
    }

    void Update()
    {
        if (robotRigidbody == null || motorAudio == null)
            return;

        // Obtener la velocidad del robot
        float velocidad = -Input.GetAxis("Pedal")+1.0f;
        Debug.Log(velocidad);

        // Normalizar entre 0 y 1
        float velocidadNormalizada = Mathf.Clamp01(velocidad / maxSpeed);

        // Ajustar el tono del motor según la velocidad
        motorAudio.pitch = Mathf.Lerp(minPitch, maxPitch, velocidadNormalizada);

        // Ajustar volumen según la velocidad
        motorAudio.volume = Mathf.Lerp(0.2f, 1.0f, velocidadNormalizada);
    }
}