using System;
using UnityEngine;

public class Position : MonoBehaviour
{
    //UI
    // public GameObject textGesture;
    //public GameObject textKPIs;
    public GameObject Obj_AR;
    // public GameObject heatmapObject;
    // public GameObject darkGreenObject, lightGreenObject, yellowObject, lightOrangeObject, darkOrangeObject, redObject;

    //Feedback
    private float posX, posY, posZ;
    // private float rsrp5G, sinr5G;
    public double latitude, longitude;
    private double latitude_offset, longitude_offset;
    private double latitude_scale, longitude_scale;

    //Digital Twin Logic
    //private float KPI; //KPI to draw signal
    // public float scaleValue = 0.1648799f;
    //private float oriX, oriY, oriZ = 0.0f;
    //private float oriW = 1.0f;
    // private float posX_ref, posY_ref, posZ_ref;
    public double latitude_ref = 39.4791670893878;
    public double longitude_ref = -0.335499320062985;
    public double anguloOffset = 21.1; //Offset entre el Norte y la linea divisoria del campo de futbol (Alineada con el eje x para medir distancias en Unity)
    public double coseno;
    public double seno;
    private Vector3 targetPosition;
    public float movementSpeed = 1.0f;
    //private Quaternion receivedOri, finalOri, targetOri = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);


    // Update is called once per frame
    void Start()
    {
        coseno = Math.Cos(anguloOffset * Math.PI / 180);
        seno = Math.Sin(anguloOffset * Math.PI / 180);
    }

    void Update()
    {
        /*if (Input.GetKeyDown("a"))
        {
            // posX_ref = posX;
            // posY_ref = posY;
            // posZ_ref = posZ;
            latitude_ref = latitude;
            longitude_ref = longitude;
        }*/
    }

    void FixedUpdate()
    {



        if (latitude != 0.0 || longitude != 0.0)
        {
            coseno = Math.Cos(anguloOffset * Math.PI / 180);
            seno = Math.Sin(anguloOffset * Math.PI / 180);

            latitude_offset = latitude - latitude_ref; //cambio del centro de coordenadas
            longitude_offset = longitude - longitude_ref; //cambio del centro de coordenadas
            latitude_scale = 110850 * latitude_offset; //cambio de escala (metros por grado de latitud en Valencia)
            longitude_scale = 85380 * longitude_offset; //cambio de escala (metros por grado de longitud en Valencia)
            posY = Convert.ToSingle(latitude_scale * seno + longitude_scale * coseno); //rotación del eje de coordenadas
            posX = Convert.ToSingle(latitude_scale * coseno - longitude_scale * seno); //rotación del eje de coordenadas
        }

        // finalOri = new Quaternion(oriX, -oriZ, oriY, oriW);
        // robotObject.transform.rotation = finalOri;
        // newPos = new Vector3(posX-posX_ref, posZ-posZ_ref, posY-posY_ref);
        targetPosition = new Vector3(-posX, 0.0f, posY);
        Obj_AR.transform.localPosition = Vector3.Lerp(Obj_AR.transform.localPosition, targetPosition, Time.deltaTime * movementSpeed);
        // scale = new Vector3(scaleValue, scaleValue, scaleValue);
        // robotObject.transform.localScale = scale;

        //textKPIs.GetComponent<TMP_Text>().text = "KPIs: latitude: " + latitude + ", longitude: " + longitude; //change
        // KPI = rsrp5G; //or sinr5G
    }

    void OnApplicationQuit()
    {

    }
}
