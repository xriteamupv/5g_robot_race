using UnityEngine;
using UnityEngine.UI;

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////// Automotive Essential Instruments Script - Version 1.1.200115 - Unity 2018.3.4f1 - Maloke Games 2019
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


public class AutomotiveGUI : MonoBehaviour
{

    public static AutomotiveGUI current;


    //Config Variables
    [Header("Setup References")]
    public bool isActive = false;
    [Tooltip("Msg displayed on Console when GUI is activated")] public string activeMsg = "Engine On";

    [Space]
    [Tooltip("Link your Car Transform here for Automatic Calculations!")] public Transform car;

    [Space]
    [Tooltip("Use this in case your canvas is moving together with the car in 3D/WorldSpace mode. If True, the MiniMap Camera will be unparented from Canvas.")]
    public bool unParentMapCam = false;

    [Space]
    public RectTransform carPanel;
    public DisplayMsg consoleMsg;
    public GameObject mapCamObj, mapGUI, mirrorGUI, dataConsoleGUI, compassGUI;
    public GameObject lampLight, turnLeftLight, turnRightLight, temperatureLight, fuelLight;
    //


    [Space(5)]
    [Header("Heading")]
    public bool useHeading = true;
    public float headingAmplitude = 1, headingOffSet = 0;
    [Range(0, 1)] public float headingFilterFactor = 0.1f;
    public RectTransform carGPS, compassHSI;
    public Text headingTxt;
    public CompassBar compassBar;
    public RollDigitIndicator headingRollDigit;


    [Space(5)]
    [Header("Speed & Trip Meter")]
    public bool useSpeed = true;
    public float speedAmplitude = 10, speedOffSet = 0;
    [Range(0, 1)] public float speedFilterFactor = 0.25f;
    public RollDigitIndicator speedRollDigit;
    public PointerIndicator speedPointer;
    public Text speedTxt;

    [Space]
    public Text gearTxt;


    [Space]
    public bool useTrip = true;
    public float tripAmplitude = 1, tripOffSet = 0;
    public RollDigitIndicator tripRollDigit;
    public PointerIndicator tripPointer;
    public Text tripTxt;


    [Space(5)]
    [Header("G-Meter")]
    public bool useCarG = true;
    public float carGAmplitude = 1, maxHorizontalG = 1, maxForwardG = 1;
    [Range(0, 1)] public float carGFilterFactor = 0.125f;
    public CircleIndicator carGCircle;
    public Text carGTxt, horizontalCarGTxt, forwardCarGTxt;



    [Space(5)]
    [Header("Engine and Fuel")]
    public bool useEngine = true;
    public float maxRPM = 10000, engineAmplitude = 10000, accelFactor = 100;
    [Range(-1, 1)] public float engineOffSet = 0;
    [Range(0, 1)] public float engineFilterFactor = 0.025f; //engineFilterFactor = 0.0125f;
    public PointerIndicator enginePointer;
    public RollDigitIndicator engineRollDigit;
    public Text engineTxt;

    [Space]
    public bool useFuel = true;
    public float maxFuel = 100, fuelAmplitude = 100;//, fuelOffSet = 0;
    [Range(0, 1)] public float fuelFilterFactor = 0.0125f;
    public PointerIndicator fuelPointer;
    public RollDigitIndicator fuelRollDigit;
    public Text fuelTxt, fuelFlowTxt;



    [Space(5)]
    [Header("Temperature")]
    public bool useTemperature = true;    
    public float temperatureAmplitude = 100, temperatureOffSet = 0;
    [Range(0, 1)] public float temperatureFilterFactor = 0.25f;
    public RollDigitIndicator temperatureRollDigit;
    public PointerIndicator temperaturePointer;
    public Text temperatureTxt;



    [Space]
    [Header("--- Manual Controlers ---")]

    [Tooltip("If true, Heading value will be read from the Car reference.")] public bool autoHeading = true;
    public float headingTarget = 0;

    [Space]
    [Tooltip("If true, gears will be automatically shifted between R for negative speed, D for positive and N when stopped.")] public bool autoGear = true;
    public int gearIndex = 2;
    public string[] gears = new string[3] { "R", "N", "D"};

    [Space]
    [Tooltip("If true, Speed will be automatically calculated from the Car reference.")] public bool autoSpeed = true;
    public float speedTarget = 0;
    public float maxSpeed = 100;

    [Space]
    [Tooltip("If true, Trip value will be automatically calculated from the Car reference.")] public bool autoTrip = true;
    public bool allowReverse = false;
    public float tripTarget = 0;


    [Space]
    [Tooltip("If True, Engine RPM will automaticaly follow current Speed and will be 100% at MaxSpeed value.")] public bool autoRPM = true;
    [Range(0, 1)] public float engineTarget = 0.75f;
    public float idleEngine = 800f, criticalEngine = 8000f;
    public AudioSource EngineAS;
    public float minPitch = 0.25f, maxPitch = 2.0f;

    [Space]
    [Tooltip("Set it to False if you wish to manually control Temperature value")] public bool autoTemperature = true;
    [Tooltip("If True, the Temperature Light will be automatically On/Off according to CriticalTemp value, otherwise, you can manually control it.")]
    public bool autoTempLight = true;
    [Range(0, 1)] public float temperatureTarget = 0.5f;
    public float maxTemperature = 100, idleTemperature = 35f, criticalTemp = 75f;
    [Tooltip("Multiplier to how fast the Temperature increases and decreases (Default 1)")] public float tempFlow = 1f;

    [Space]
    [Space]
    [Tooltip("If True, the Fuel Light will be automatically On/Off according to criticalFuel value, otherwise, you can manually control it.")]
    public bool autoFuelLight = true;
    [Range(0, 3)] public float fuelTarget = 0.8f;
    public float criticalFuel = 12.5f;
    [Tooltip("Percentage of Consumed Fuel per Minute at Máx Engine RPM")]
    public float maxfuelFlow = 1.00f;
    [Tooltip("Min Percentage of consumed Fuel per minute at any Engine RPM")]
    public float idlefuelFlow = 0.25f;



    [Space]
    [Space]
    public bool lampIsOn;
    public bool turnLeftIsOn, turnRightIsOn, temperatureIsOn, fuelIsOn;

    [Space]
    public bool useKeys = true;
    public KeyCode lampKey = KeyCode.F, leftSignalKey = KeyCode.Q, rightSignalKey = KeyCode.E, resetTripKey = KeyCode.R, 
        toogleMapKey= KeyCode.M,  mapZoomInKey= KeyCode.Equals,  mapZoomOutKey= KeyCode.Minus, mapZoomResetKey = KeyCode.Backspace
        , toogleMirrorKey = KeyCode.N, toogleDataConsoleKey = KeyCode.V, toogleCompassKey = KeyCode.C, gearUpKey = KeyCode.PageUp, gearDownKey = KeyCode.PageDown;


    //All Current Variables
    [Space(10)]
    [Header("--- Current Variables - ReadOnly! ---")]
    public int gear;
    public float speed;
    public float trip, heading, engine, fuel, fuelFlow, temperature;
    public Vector3 carGForce;
    //


    //Internal Calculation Variables
    Vector3 currentPosition, lastPosition, relativeSpeed, absoluteSpeed, lastSpeed, relativeAccel;

    Vector3 angularSpeed;
    Quaternion currentRotation, lastRotation, deltaTemp;
    float angleTemp = 0.0f;
    Vector3 axisTemp = Vector3.zero;

    float engineReNormalized, fuelReNormalized;

    MoveObject mapCamScript;
    Camera mapCam;

    int waitInit = 6;
    //


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////// Inicialization
    void Awake()
    {
        if (car == null) car = Camera.main.transform;   //If there is no reference set, then it gets the MainCamera

        if(mapCamObj != null)
        {
            if (mapCamScript == null) mapCamScript = mapCamObj.GetComponent<MoveObject>();
            if (mapCam == null) mapCam = mapCamObj.GetComponent<Camera>();
        }
    }
    //
    void OnEnable()
    {
        if (car == null) car = Camera.main.transform;
        if (mapCamObj != null)
        {
            if (unParentMapCam)
            {
                mapCamObj.transform.parent = null;
                mapCamObj.transform.rotation = Quaternion.Euler(90, 0, 0);
            }

            if (mapCamScript == null) mapCamScript = mapCamObj.GetComponent<MoveObject>();
            if (mapCam == null) mapCam = mapCamObj.GetComponent<Camera>();
        }

        if (current != null && current != this) current.gameObject.SetActive(false); //Disables Previous GUI
        ResetHud();
    }
    //
    public void ResetHud()
    {
        current = this;
        if (mapCamObj != null && mapCamScript != null) mapCamScript.toObj = car.gameObject;
        if (mapCamObj != null && mapCam != null) mapCam.orthographicSize = 30;


        waitInit = 6;

        trip = tripOffSet; //Reset Trip Meter value

        isActive = true;
        if (consoleMsg != null) DisplayMsg.current = consoleMsg;
        if (activeMsg != "") DisplayMsg.show(activeMsg, 5);

        //Ignition Sound
        SndPlayer.play(3);
    }
    public void toogleHud()
    {
        SndPlayer.playClick();
        carPanel.gameObject.SetActive(!carPanel.gameObject.activeSelf);


        if (!carPanel.gameObject.activeSelf)
        {
            isActive = false; current = null;
            DisplayMsg.show("Hud Disabled", 5);
        }
        else { if (!isActive) ResetHud(); }
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////// Inicialization



    /////////////////////////////////////////////////////// Updates and Calculations
    void FixedUpdate() //Update()
    {
        // Return if not active
        if (!isActive || carPanel == null || !carPanel.gameObject.activeSelf) return;
        //

        //////////////////////////////////////////// Frame Calculations
        lastPosition = currentPosition;
        lastSpeed = relativeSpeed;
        lastRotation = currentRotation;

        if (car != null) //Have a Car Reference for Speed and Heading values to be calculated from
        {
            currentPosition = car.transform.position;
            absoluteSpeed = (currentPosition - lastPosition) / Time.fixedDeltaTime;
            relativeSpeed = car.transform.InverseTransformDirection((currentPosition - lastPosition) / Time.fixedDeltaTime);
            relativeAccel = (relativeSpeed - lastSpeed) / Time.fixedDeltaTime;
            currentRotation = car.transform.rotation;

            //angular speed
            deltaTemp = currentRotation * Quaternion.Inverse(lastRotation);
            angleTemp = 0.0f;
            axisTemp = Vector3.zero;
            deltaTemp.ToAngleAxis(out angleTemp, out axisTemp);
            //
            angularSpeed = car.InverseTransformDirection(angleTemp * axisTemp) * Mathf.Deg2Rad / Time.fixedDeltaTime;
            //
        }
        //
        if (waitInit > 0) { waitInit--; return; } //Wait some frames for stablization before starting calculating
        //
        //////////////////////////////////////////// Frame Calculations


        //////////////////////////////////////////// Compass Heading
        if (useHeading)
        {
            if(autoHeading) heading = Mathf.LerpAngle(heading, headingAmplitude * currentRotation.eulerAngles.y + headingOffSet, headingFilterFactor) % 360f;
            else heading = Mathf.LerpAngle(heading, /*headingAmplitude **/ headingTarget + headingOffSet, headingFilterFactor) % 360f;


            //Send values to Gui and Instruments
            if (carGPS != null) carGPS.localRotation = Quaternion.Euler(0, 0, -heading);
            if (compassHSI != null) compassHSI.localRotation = Quaternion.Euler(0, 0, heading);
            if (compassBar != null) compassBar.setValue(heading);
            if (headingRollDigit != null) headingRollDigit.setValue((heading < 0) ? (heading + 360f) : heading);
            if (headingTxt != null) { if (heading < 0) headingTxt.text = (heading + 360f).ToString("000"); else headingTxt.text = heading.ToString("000"); }

        }
        //////////////////////////////////////////// Compass Heading


        //////////////////////////////////////////// Speed & Trip Meter
        if (useSpeed)
        {
            if(autoSpeed) speed = Mathf.Lerp(speed, speedOffSet + speedAmplitude * relativeSpeed.z, speedFilterFactor);
            else speed = Mathf.Lerp(speed, speedOffSet + /*speedAmplitude **/ speedTarget, speedFilterFactor);

            //Send values to Gui and Instruments
            if (speedRollDigit != null) speedRollDigit.setValue(speed);
            if (speedPointer != null) speedPointer.setValue(speed/maxSpeed);
            if (speedTxt != null) speedTxt.text = speed.ToString("0").PadLeft(5);//.ToString("##0");
        }
        //
        if (useTrip)
        {
            if (autoTrip)
            {
                if (allowReverse) trip += tripAmplitude * relativeSpeed.z * Time.fixedDeltaTime;
                else trip += Mathf.Abs(tripAmplitude * relativeSpeed.z * Time.fixedDeltaTime);
            }
            else trip = tripTarget;

            //Send values to Gui and Instruments
            if (tripRollDigit != null) tripRollDigit.setValue(trip);
            if (tripPointer != null) tripPointer.setValue(trip);
            if (tripTxt != null) tripTxt.text = trip.ToString("0").PadLeft(5);//.ToString("##0");
        }
        //////////////////////////////////////////// Speed & Trip Meter



        //////////////////////////////////////////// Engine & Fuel
        if (useEngine)
        {
            //Auto RPM control and Fuel Condition
            //if (autoRPM) engineTarget = Mathf.Abs(speed / maxSpeed + idleEngine / engineAmplitude);
            if (autoRPM) engineTarget = Mathf.Abs((speed + accelFactor * relativeAccel.z) / maxSpeed + idleEngine/engineAmplitude);
            if (useFuel && fuelReNormalized < 0.01f) engineTarget = 0;
            //

            //Updates current Engine RPM
            engineTarget = Mathf.Clamp01(Mathf.Abs(engineTarget));
            engine = Mathf.Lerp(engine, engineAmplitude * Mathf.Clamp01(engineTarget + engineOffSet), engineFilterFactor);

            if (engineTarget == 0 && engine < 0.01f) engine = 0;
            engineReNormalized = Mathf.Clamp01((engine - engineOffSet) / engineAmplitude);

            if (useFuel && fuel == 0) { engineTarget = 0; engine = 0; /*engineReNormalized = 0;*/ }
            //

            //Engine Sound and Pitch
            if (EngineAS != null && EngineAS.isActiveAndEnabled)
            {
                if (!EngineAS.isPlaying && engineTarget > 0) EngineAS.Play();

                if (engineReNormalized > 0.01f) EngineAS.pitch = Mathf.Lerp(minPitch, maxPitch, engineReNormalized);
                else { EngineAS.Stop(); EngineAS.pitch = 1; }
            }
            //

            //Send values to Gui and Instruments
            if (engineRollDigit != null) engineRollDigit.setValue(engine);
            if (enginePointer != null) enginePointer.setValue(engine / maxRPM);
            if (engineTxt != null) engineTxt.text = engine.ToString("##0");
        }
        //
        if (useFuel)
        {
            //Calculates Fuel Consumption
            if (maxfuelFlow != 0 || idlefuelFlow != 0)
            {
                if (engine != 0)
                {
                    fuelFlow = Mathf.Clamp(engineReNormalized * maxfuelFlow, idlefuelFlow, maxfuelFlow) * Time.fixedDeltaTime / 0.60f;
                    fuelTarget -= fuelFlow / 100f;
                }
                else fuelFlow = 0;
            }
            else fuelFlow = 0;
            //

            //Updates current Fuel value
            if (fuelTarget < 0) fuelTarget = 0;
            fuel = Mathf.Lerp(fuel, fuelAmplitude * fuelTarget, fuelFilterFactor);

            if (fuel < 0) fuel = 0;
            if (fuelTarget == 0 && fuel < 0.01f) fuel = 0;
            fuelReNormalized = fuel / fuelAmplitude;
            //

            //Send values to Gui and Instruments
            if (autoFuelLight)
            {
                if (fuel <= criticalFuel && fuelTarget <= criticalFuel / fuelAmplitude && !fuelIsOn) fuelIsOn = true; else if (fuel > criticalFuel && fuelIsOn) fuelIsOn = false;
            }
            if (fuelRollDigit != null) fuelRollDigit.setValue(fuel);
            if (fuelPointer != null) fuelPointer.setValue(fuel/maxFuel);
            if (fuelTxt != null) fuelTxt.text = fuel.ToString("##0");
            if (fuelFlowTxt != null) fuelFlowTxt.text = (fuelAmplitude * fuelFlow).ToString("##0.0");//.ToString("0.0").PadLeft(4);  //.ToString("##0");     
        }
        //////////////////////////////////////////// Engine & Fuel


        //////////////////////////////////////////// Temperature
        if (useTemperature)
        {
            //Automatic Temperature control
            if (autoTemperature)
            {
                if (useFuel && fuel == 0) temperatureTarget += 3 * (0 - temperatureTarget) * tempFlow * Time.fixedDeltaTime / 60f; //temperatureTarget = 0;
                else
                {
                    //Backup of simpler versions
                    //////temperatureTarget += factor * (engineTarget - temperatureTarget) * tempFlow * Time.fixedDeltaTime / 60f;
                    //int factor = (engineTarget < temperatureTarget) ? 2 : 1; //Cools 2 times faster than it heats
                    //temperatureTarget += factor * (engineTarget - temperatureTarget + idleTemperature / temperatureAmplitude) * tempFlow * Time.fixedDeltaTime / 60f;

                    if (engineReNormalized >= criticalEngine / engineAmplitude)
                        temperatureTarget += 1.5f * engineReNormalized * tempFlow * Time.fixedDeltaTime / 60f;
                        //temperatureTarget += 4 * (engineReNormalized - temperatureTarget) * tempFlow * Time.fixedDeltaTime / 60f;
                    else if (engineTarget == 0)
                        temperatureTarget += 3 * (0 - temperatureTarget) * tempFlow * Time.fixedDeltaTime / 60f;
                    else if(engineReNormalized < idleTemperature / temperatureAmplitude || engineReNormalized < temperatureTarget)
                        temperatureTarget += 3 * (idleTemperature / temperatureAmplitude - temperatureTarget) * tempFlow * Time.fixedDeltaTime / 60f;
                    else
                        temperatureTarget += 1 * (engineReNormalized - temperatureTarget) * tempFlow * Time.fixedDeltaTime / 60f;
                }
            }
            //


            //Updates current Engine Temperature
            temperatureTarget = Mathf.Clamp01(temperatureTarget);
            temperature = Mathf.Lerp(temperature, temperatureAmplitude * temperatureTarget + temperatureOffSet, temperatureFilterFactor);


            //Send values to Gui and Instruments
            if (autoTempLight)
            {
                if (temperature >= criticalTemp && !temperatureIsOn) temperatureIsOn = true; else if (temperature < criticalTemp && temperatureIsOn) temperatureIsOn = false;
            }
            if (temperatureRollDigit != null) temperatureRollDigit.setValue(temperature);
            if (temperaturePointer != null) temperaturePointer.setValue(temperature / maxTemperature);
            if (temperatureTxt != null) temperatureTxt.text = temperature.ToString("##0");
        }
        //////////////////////////////////////////// Temperature


        ////////////////////////////////////////////  Car Mode G-Force 
        if (useCarG)
        {
            //G-FORCE -> Horizontal Acceleration + Centripetal Acceleration (v * w) radians
            Vector3 carG = carGAmplitude * (-relativeAccel - Vector3.Cross(angularSpeed, relativeSpeed.magnitude * relativeSpeed.normalized)) / Physics.gravity.magnitude;
            if (float.IsNaN(carG.magnitude)) carG = Vector3.zero;

            carGForce = Vector3.Lerp(carGForce, carG, carGFilterFactor);
            carGForce = new Vector3(carGForce.x, 0, carGForce.z);
            //


            //Send values to Gui and Instruments 
            if (carGCircle != null) carGCircle.setValue(new Vector2(carGForce.x / maxHorizontalG, carGForce.z / maxForwardG));

            if (carGTxt != null) carGTxt.text = carGForce.magnitude.ToString("0.0").PadLeft(3);
            if (horizontalCarGTxt != null) horizontalCarGTxt.text = carGForce.x.ToString("0.0").PadLeft(3);
            if (forwardCarGTxt != null) forwardCarGTxt.text = carGForce.z.ToString("0.0").PadLeft(3);
            //
        }
        ////////////////////////////////////////////  Car Mode G-Force 

    }
    //
    void Update()
    {
        if (waitInit > 0) return; //Wait some frames for initialization before starting


        ////////////////// Lights Signal

        //Keys for LampLight and TurnSignals
        if (useKeys)
        {
            if (Input.GetKeyDown(lampKey)) toogleLamp();
            if (Input.GetKeyDown(leftSignalKey)) toogleLeftSignal();
            if (Input.GetKeyDown(rightSignalKey)) toogleRightSignal();
            if (Input.GetKeyDown(resetTripKey)) resetTrip();
            if (Input.GetKeyDown(toogleMapKey)) toogleMap();
            if (Input.GetKeyDown(toogleMirrorKey)) toogleMirror();            
            if (Input.GetKeyDown(toogleDataConsoleKey)) toogleDataConsole();
            if (Input.GetKeyDown(toogleCompassKey)) toogleCompass();            
            if (Input.GetKeyDown(mapZoomOutKey)) mapZoomOut();
            if (Input.GetKeyDown(mapZoomInKey)) mapZoomIn();
            if (Input.GetKeyDown(mapZoomResetKey)) mapZoomReset();
            if (Input.GetKeyDown(gearUpKey)) gearUp();
            if (Input.GetKeyDown(gearDownKey)) gearDown();
        }
        //

        //Gear Shift - AutoGear + Update GUI
        if (autoGear)
        {
            if (speed < -1) gearIndex = 0;
            else if (speed < 1 && speed > -1) gearIndex = 1;
            else gearIndex = 2;
        }
        //
        if (gear != gearIndex)
        {
            gear = gearIndex;
            if (gearTxt != null) gearTxt.text = gears[gear];
            SndPlayer.play(7);
        }
        //

        //Light GUI On/Off
        if (fuelLight != null && fuelIsOn != fuelLight.activeInHierarchy)
        {
            fuelLight.SetActive(fuelIsOn);
            if (fuelIsOn)
            {
                if (consoleMsg != null) consoleMsg.displayQuickMsg("Bingo Fuel");
                SndPlayer.play(4);
            }
        }
        if (lampLight != null && lampIsOn != lampLight.activeInHierarchy)
        {
            lampLight.SetActive(lampIsOn);
            foreach (ColorImg colorImg in FindObjectsOfType<ColorImg>()) colorImg.setColor((lampIsOn) ? 1 : 0);
            SndPlayer.play(4);
        }
        //
        if (temperatureLight != null && temperatureIsOn != temperatureLight.activeInHierarchy)
        {
            temperatureLight.SetActive(temperatureIsOn);
            if (temperatureIsOn)
            {
                if (consoleMsg != null) consoleMsg.displayQuickMsg("Engine Overheating");
                SndPlayer.play(4);
            }
        }
        //
        if (turnLeftLight != null && turnLeftIsOn != turnLeftLight.activeInHierarchy)
        {
            if (turnRightIsOn) { turnRightIsOn = false; turnRightLight.SetActive(false); }

            turnLeftLight.SetActive(turnLeftIsOn);
            SndPlayer.play(5);
        }
        if (turnRightLight != null && turnRightIsOn != turnRightLight.activeInHierarchy)
        {
            if (turnLeftIsOn) { turnLeftIsOn = false; turnLeftLight.SetActive(false); }

            turnRightLight.SetActive(turnRightIsOn);
            SndPlayer.play(5);
        }
        //
        //TurnSignal Sound
        if ((turnLeftIsOn || turnRightIsOn) && SndPlayer.current.som.clip == null)
        {
            SndPlayer.current.som.clip = SndPlayer.current.sounds[6];
            SndPlayer.current.som.Play();
            //SndPlayer.play(5);
        }
        else if (!turnLeftIsOn && !turnRightIsOn && SndPlayer.current.som.clip != null)
        {
            SndPlayer.current.som.clip = null;
            //SndPlayer.play(5);
        }
        //
        ////////////////// Lights Signal

    }
    //
    /////////////////////////////////////////////////////// Updates and Calculations


    




    ////////////////////////////////////////////////////////////////////////   External Calls
    public void setGear(int index = 0)
    {
        if (index <= 0) gearIndex = 0;
        else if (index > gears.Length - 1) gearIndex = gears.Length - 1;
        else gearIndex = index;
    }
    //
    public void toogleLamp()
    {
        lampIsOn = !lampIsOn;
        if (consoleMsg != null)
        {
            if (lampIsOn) consoleMsg.displayQuickMsg("Lights ON"); else consoleMsg.displayQuickMsg("Lights OFF");
        }
    }
    //
    public void resetTrip()
    {
        trip = 0;
        if (consoleMsg != null) consoleMsg.displayQuickMsg("Trip Reset");
        SndPlayer.play(7);
    }
    //
    public void toogleMap()
    {
        if (mapGUI != null) mapGUI.SetActive(!mapGUI.activeSelf);
        if (consoleMsg != null)
        {
            if (mapGUI.activeSelf) consoleMsg.displayQuickMsg("GPS Map ON"); else consoleMsg.displayQuickMsg("GPS Map OFF");
        }
        SndPlayer.play(7);
    }
    //
    public void toogleMirror()
    {
        if (mirrorGUI != null) mirrorGUI.SetActive(!mirrorGUI.activeSelf);
        if (consoleMsg != null)
        {
            if (mirrorGUI.activeSelf) consoleMsg.displayQuickMsg("Rear Mirror ON"); else consoleMsg.displayQuickMsg("Rear Mirror OFF");
        }
        SndPlayer.play(7);
    }
    //
    public void toogleDataConsole()
    {
        if (dataConsoleGUI != null)
        {
            dataConsoleGUI.SetActive(!dataConsoleGUI.activeSelf);
            if (consoleMsg != null)
            {
                if (dataConsoleGUI.activeSelf) consoleMsg.displayQuickMsg("DataConsole ON"); else consoleMsg.displayQuickMsg("DataConsole OFF");
            }
            SndPlayer.play(7);
        }
    }
    //
    public void toogleCompass()
    {
        if (compassGUI != null)
        {
            compassGUI.SetActive(!compassGUI.activeSelf);
            if (consoleMsg != null)
            {
                if (compassGUI.activeSelf) consoleMsg.displayQuickMsg("Compass ON"); else consoleMsg.displayQuickMsg("Compass OFF");
            }
            SndPlayer.play(7);
        }
    }    
    //
    public void mapZoomOut() { if (mapCam != null) mapCam.orthographicSize = Mathf.Clamp(mapCam.orthographicSize + 5, 1, 150); }
    public void mapZoomIn() { if (mapCam != null) mapCam.orthographicSize = Mathf.Clamp(mapCam.orthographicSize - 5, 1, 150); }
    public void mapZoomReset() { if (mapCam != null) mapCam.orthographicSize = 30; }
    public void gearUp() {  if (gearIndex + 1 > gears.Length - 1) gearIndex = gears.Length - 1; else gearIndex += 1; }
    public void gearDown() { if (gearIndex - 1 < 0) gearIndex = 0; else gearIndex -= 1; }
    public void toogleLeftSignal() { turnLeftIsOn = !turnLeftIsOn; }
    public void toogleRightSignal() { turnRightIsOn = !turnRightIsOn; }
    ////////////////////////////////////////////////////////////////////////   External Calls

}
