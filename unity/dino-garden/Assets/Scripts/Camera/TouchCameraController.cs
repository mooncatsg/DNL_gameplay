using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public class TouchCameraController : MonoBehaviour
{
    float DELTA_TOUCH = 15.0f;
    public enum CAMERA_DIRECTION
    {
        LEFT,
        MIDDLE,
        RIGHT,
    }

    public static TouchCameraController Instance;

    [SerializeField] float PanSpeed = 50f;
    [SerializeField] float ZoomSpeedTouch = 0.1f;
    //[SerializeField] float 
    float ZoomSpeedMouse = 50f;

    [SerializeField] float[] BoundsX = new float[] { -20, 50f };
    [SerializeField] float[] BoundsZ = new float[] { -30f, 20f };
    [SerializeField] float[] ZoomBounds = new float[] { 10f, 85f };
    public CAMERA_DIRECTION camDirection = CAMERA_DIRECTION.MIDDLE;

    private Camera cam;

    private Vector3 lastPanPosition = new Vector3(-1000, -1000, -1000);
    private int panFingerId; // Touch mode only

    private bool wasZoomingLastFrame; // Touch mode only
    private Vector2[] lastZoomPositions; // Touch mode only

    private Vector3 originPosition;
    public Vector3 OriginPosition
    {
        get { return originPosition; }
        set { originPosition = value; }
    }

    INPUTSTATE inputState = INPUTSTATE.NONE;
    float holdDuration = 0.3f;
    Vector2 firtPos;


    CameraData cameraData;
    void Awake()
    {
        Instance = this;
        cam = GetComponent<Camera>();
        originPosition = this.transform.position;

    }
    void Start()
    {
        cameraData = new CameraData();
        Vector3[] vts = cameraData.LoadFromSystem();
        if (vts != null)
        {
            cam.gameObject.transform.position = vts[0];
            // cam.gameObject.transform.eulerAngles = vts[1];
        }
    }
    void Update()
    {
        if (MainMenuUI.Instance.IsUIActive())
        {
            Debug.LogWarning("==> Click: IsUIActive");
            inputState = INPUTSTATE.NONE;
            return;
        }

        if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer)
        {
            HandleTouch();
        }
        else
        {
            HandleMouse();
        }
    }

    void HandleTouch()
    {
        switch (Input.touchCount)
        {

            case 1: // Panning
                wasZoomingLastFrame = false;

                // If the touch began, capture its position and its finger ID.
                // Otherwise, if the finger ID of the touch doesn't match, skip it.
                Touch touch = Input.GetTouch(0);






                if (touch.phase == TouchPhase.Began)
                {
                    this.TouchBegin(touch.position);
                    lastPanPosition = touch.position;
                    panFingerId = touch.fingerId;
                }
                else if (touch.phase == TouchPhase.Stationary)
                {

                    this.checkTouchForMobile(touch.position);
                    return;
                }
                else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Moved)
                {
                    this.TouchMove(touch.position);

                    if (inputState != INPUTSTATE.HOLD_AND_MOVE)
                    {
                        PanCamera(touch.position);
                    }


                }
                if (touch.phase == TouchPhase.Ended)
                {
                    this.TouchEnded();
                }
                break;

            case 2: // Zooming
                inputState = INPUTSTATE.ENDED;
                //this.TouchEnded();
                Vector2[] newPositions = new Vector2[] { Input.GetTouch(0).position, Input.GetTouch(1).position };
                if (!wasZoomingLastFrame)
                {
                    lastZoomPositions = newPositions;
                    wasZoomingLastFrame = true;
                }
                else
                {
                    // Zoom based on the distance between the new positions compared to the 
                    // distance between the previous positions.
                    float newDistance = Vector2.Distance(newPositions[0], newPositions[1]);
                    float oldDistance = Vector2.Distance(lastZoomPositions[0], lastZoomPositions[1]);
                    float offset = newDistance - oldDistance;

                    ZoomCamera(offset, ZoomSpeedTouch);

                    lastZoomPositions = newPositions;
                }
                break;

            default:
                wasZoomingLastFrame = false;
                break;
        }
    }

    void HandleMouse()
    {
        // On mouse down, capture it's position.
        // Otherwise, if the mouse is still down, pan the camera.


        if (Input.GetMouseButtonDown(0))
        {
            lastPanPosition = Input.mousePosition;
            /// Debug.Log("----BEGIN-----");
            this.TouchBegin(Input.mousePosition);



        }
        else if (Input.GetMouseButton(0))
        {


            this.TouchMove(Input.mousePosition);
            if (inputState == INPUTSTATE.MOVE)
            {
                PanCamera(Input.mousePosition);
            }


        }
        else
        {
            this.TouchEnded();

        }

        if (inputState != INPUTSTATE.MOVE)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            ZoomCamera(scroll, ZoomSpeedMouse);
        }
    }

    void PanCamera(Vector3 newPanPosition)
    {
        // Determine how much to move the camera
        Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);

        Vector3 move = new Vector3(offset.x * PanSpeed, 0, offset.y * PanSpeed); //MIDDLE
        if (camDirection == CAMERA_DIRECTION.LEFT)
            move = new Vector3(offset.x * PanSpeed + offset.y * PanSpeed, 0, offset.y * PanSpeed - offset.x / 2 * PanSpeed); //LEFT
        else if (camDirection == CAMERA_DIRECTION.RIGHT)
            move = new Vector3(offset.x * PanSpeed - offset.y * PanSpeed, 0, offset.y * PanSpeed + offset.x / 2 * PanSpeed); //RIGHT

        // Perform the movement
        transform.Translate(move, Space.World);

        // Ensure the camera remains within bounds.
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, BoundsX[0], BoundsX[1]);
        pos.z = Mathf.Clamp(transform.position.z, BoundsZ[0], BoundsZ[1]);
        transform.position = pos;

        // Cache the position
        lastPanPosition = newPanPosition;
    }

    void ZoomCamera(float offset, float speed)
    {

        if (Mathf.Abs(offset) > 0.0000001f)
        {
            //cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - (offset * speed), ZoomBounds[0], ZoomBounds[1]);
            Vector3 posDestination = cam.transform.position + cam.transform.forward * (offset * speed);
            if (posDestination.y > ZoomBounds[0] && posDestination.y < ZoomBounds[1])
            {
                if (posDestination.z < BoundsZ[0])
                {
                    posDestination.z = BoundsZ[0];
                }
                else if (posDestination.z > BoundsZ[1])
                {
                    posDestination.z = BoundsZ[1];
                }
                cam.transform.position = posDestination;
            }

        }
    }

    public void SetZoomCamera(float zoomValue)
    {
        if (zoomValue > ZoomBounds[0] && zoomValue < ZoomBounds[1])
        {
            cam.transform.position = new Vector3(cam.transform.position.x, zoomValue, cam.transform.position.z);
        }
    }

    public float ZoomInY()
    {
        return ZoomBounds[0] + 0.5f;
    }

    public void ResetToOriginPosition()
    {
        this.transform.position = OriginPosition;
    }




    #region ---check touch and move item in map

    void TouchBegin(Vector2 pos)
    {

        if (inputState != INPUTSTATE.BEGIN)
        {
            inputState = INPUTSTATE.BEGIN;
            holdDuration = 0.3f;
            firtPos.x = pos.x;
            firtPos.y = pos.y;
            //Debug.Log("----BEGIN-----");
        }
        else
        {
            //Debug.Log("----WTF-----");
        }
    }
    void TouchMove(Vector2 pos)
    {
        if (inputState == INPUTSTATE.ENDED)
        {
            return;
        }

        if (inputState == INPUTSTATE.MOVE)
        {
            return;
        }
        if (inputState == INPUTSTATE.HOLD_AND_MOVE)
        {
            MapController.instance.MoveItemSelected(pos);

            return;
        }

        holdDuration -= Time.deltaTime;
        Vector2 crpos2 = new Vector2();
        crpos2.x = pos.x;
        crpos2.y = pos.y;
        Vector2 vtdt = firtPos - crpos2;


        if (holdDuration < 0)
        {
            if (vtdt.magnitude < DELTA_TOUCH)
            {
                if (inputState != INPUTSTATE.HOLD_AND_MOVE)
                {
                    PLACE_TYPE pType = MapController.instance.CheckSelectItem(pos, true);
                    if (pType == PLACE_TYPE.MOVE)
                    {
                        inputState = INPUTSTATE.HOLD_AND_MOVE;
                    }
                    else
                    {
                        inputState = INPUTSTATE.ENDED;
                        this.TouchObject3D(MapController.instance.itemSelected);
                    }

                }

            }
        }
        else if (vtdt.magnitude > DELTA_TOUCH)
        {
            //Debug.Log("----------vtdt.magnitude=" + vtdt.magnitude);
            inputState = INPUTSTATE.MOVE;
        }


    }
    void checkTouchForMobile(Vector2 pos)
    {
        if (inputState == INPUTSTATE.ENDED)
        {
            return;
        }
        if (inputState == INPUTSTATE.HOLD_AND_MOVE)
        {
            return;
        }
        if (inputState == INPUTSTATE.MOVE)
        {
            return;
        }
        if (inputState == INPUTSTATE.BEGIN)
        {
            holdDuration -= Time.deltaTime;
            if (holdDuration < 0)
            {
                PLACE_TYPE pType = MapController.instance.CheckSelectItem(pos, true);
                if (pType == PLACE_TYPE.MOVE)
                {

                    inputState = INPUTSTATE.HOLD_AND_MOVE;
                }
                else
                {
                    inputState = INPUTSTATE.ENDED;
                    this.TouchObject3D(MapController.instance.itemSelected);

                }
            }
        }
        /*
        if (inputState != INPUTSTATE.HOLD_AND_MOVE)
        {


            PLACE_TYPE pType = MapController.instance.CheckSelectItem(pos,true);
            if (pType == PLACE_TYPE.MOVE)
            {
                Debug.Log("---vao day ---HOLD_AND_MOVE");
                inputState = INPUTSTATE.HOLD_AND_MOVE;
            }
            else
            {
                inputState = INPUTSTATE.ENDED;
                this.TouchObject3D(MapController.instance.itemSelected);
                Debug.Log("---vao day ---ENDED");
            }

        }
        */

    }
    void TouchEnded()
    {
        if (inputState != INPUTSTATE.ENDED)
        {

            if (inputState == INPUTSTATE.BEGIN)
            {
                PLACE_TYPE pType = MapController.instance.CheckSelectItem(firtPos, false);
                if (MapController.instance.itemSelected != null)
                {
                    //Debug.Log("----Click object---");
                    this.TouchObject3D(MapController.instance.itemSelected);

                }

            }

            if (inputState == INPUTSTATE.MOVE)
            {
                Debug.Log("---save camera state");
                cameraData.SaveData(cam.gameObject.transform);
            }

            inputState = INPUTSTATE.ENDED;
            MapController.instance.DeSelectAllItem();
        }
    }

    void TouchObject3D(ItemPlaceController obj)
    {
        if (GameplayManager.instance != null)
        {
            GameplayManager.instance.TouchObject3D(obj);
        }
    }

    #endregion
}