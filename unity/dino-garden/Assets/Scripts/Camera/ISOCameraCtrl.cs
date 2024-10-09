using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraRange
{

    public float min;
    public float max;
    public float Clamp(float input)
    {
        float rt = input;
        if (rt > max)
        {
            rt = max;
        }else if (rt < min)
        {
            rt = min;
        }
        return rt;
    }
}

[System.Serializable]
public struct SPEEDZOOM
{
    public float mouse;
    public float touch;
}

public struct VecTouch3D
{
    public float x;
    public float z;
    public bool hit;
}
public class ISOCameraCtrl : MonoBehaviour
{
    float DELTA_TOUCH = 15.0f;
    int layerMask = -100;
    public CameraRange cam_range;
    public float mapSize = 5;

    private bool wasZoomingLastFrame; // Touch mode only
    private Vector2[] lastZoomPositions; // Touch mode only


    private Vector2 last_Pan;
    private Camera main_cam;
    private int panFingerId; // Touch mode only
    INPUTSTATE inputState = INPUTSTATE.NONE;
    float holdDuration = 0.3f;
    Vector2 firtPos;
    CameraData cameraData;
    public SPEEDZOOM zoomSpeed;

    Ray cameraRay;                // The ray that is cast from the camera to the mouse position
    //RaycastHit cameraRayHit;

    public GameObject pointObj;
    public bool testOnly = false;
    void Start()
    {
        main_cam = Camera.main;
        layerMask = LayerMask.GetMask("LANDMESH");
        if (layerMask < 0)
        {
            Debug.LogError("----LANDMESH=" + layerMask);
        }
        this.UpdatePosCenter();
    }

    
    void UpdatePosCenter()
    {
        Vector3 targetPosition = pointObj.transform.position;
        transform.LookAt(targetPosition);
    }

    
    void Update()
    {
        if (MainMenuUI.Instance.IsUIActive())
            return;
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

                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    this.TouchBegin(touch.position);
                    last_Pan = touch.position;
                    //lastRay = this.getPos3DTouch(last_Pan);
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

                    ZoomCamera(offset, this.zoomSpeed.touch);

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
        
        if (Input.GetMouseButtonDown(0))
        {
            last_Pan = Input.mousePosition;
            //lastRay = this.getPos3DTouch(last_Pan);
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
        /*
        if (inputState != INPUTSTATE.MOVE)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            ZoomCamera(scroll, this.zoomSpeed.mouse);
        }
        */
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scroll, this.zoomSpeed.mouse);
    }



    void ZoomCamera(float offset, float speed)
    {
        if (Mathf.Abs(offset) > 0)
        {
            float dtzoom = speed * offset;
            float  ftmp= this.main_cam.fieldOfView-dtzoom;
            this.main_cam.fieldOfView= this.cam_range.Clamp(ftmp);
            
        }

        
    }

    void PanCamera(Vector2 newPanPosition)
    {
        Vector2 dtv = last_Pan - newPanPosition;
        
        if (dtv.magnitude <1)
        {
            return;
        }
        VecTouch3D vt3d = this.getPos3DTouch(newPanPosition);
        VecTouch3D lastRay = this.getPos3DTouch(last_Pan);
        if (!vt3d.hit)
        {
            Debug.Log("---AAAAAA----");
            return;
        }
        if (lastRay.hit)
        {
            float minPos = -this.mapSize;
            float dtx = vt3d.x - lastRay.x;
            float dtz = vt3d.z - lastRay.z;
            Vector3 player_pos = this.pointObj.transform.position;
            player_pos.x -= dtx;
            player_pos.z -= dtz;
            this.pointObj.transform.position = player_pos;
            Vector3 localPos = this.pointObj.transform.localPosition;
            if (localPos.x > this.mapSize)
            {
                localPos.x = mapSize;
            }else if (localPos.x < minPos)
            {
                localPos.x = minPos;
            }
            if (localPos.z > this.mapSize)
            {
                localPos.z = mapSize;
            }
            else if (localPos.z < minPos)
            {
                localPos.z = minPos;
            }
            this.pointObj.transform.localPosition = localPos;

            UpdatePosCenter();
            
            
        }

        last_Pan = newPanPosition;
    }

    private VecTouch3D getPos3DTouch(Vector2 posT)
    {
        Camera atCam = MapController.instance.getActiveCamera();
        cameraRay = atCam.ScreenPointToRay(posT);
        RaycastHit hit;
        VecTouch3D vtrt =new VecTouch3D();
        vtrt.hit = false;
        if (Physics.Raycast(cameraRay, out hit, 1000, layerMask))
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.name == "LAND")
                {
                    //tgMove.UpdatePosTG(hit.point);
                    Vector3 vtpos = hit.point;
                    vtrt.hit = true;
                    vtrt.x = vtpos.x;
                    vtrt.z = vtpos.z;
                }

            }

        }

        return vtrt;
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
        
        if (testOnly)
        {
            inputState = INPUTSTATE.MOVE;
            return;
        }
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
       

    }
    void TouchEnded()
    {

        if (testOnly)
        {
            return;
        }
        
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
              //  Debug.Log("---save camera state");
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
