/******************************************************************************
 *                                                                            *
 *                                                                            *
 *                          Fantasy Mirror                                    *
 *                                                                            *
 *                           Version: 1.1.0                                   *
 *                                                                            *
 *                                                                            *
 *                                                                            *
 *  Author: Yi Zhen                                                           *
 *  Date  : 2021-2                                                           *
 *                                                                            *
 ******************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Fantasy
{
    public class GameObjectDummy
    {
        public Vector3 v3Pos;
        public Vector3 v3Up;
        public Vector3 v3Right;
        public Vector3 v3Forward;

        public GameObjectDummy()
        {
            v3Pos = Vector3.zero;
            v3Up = Vector3.zero;
            v3Right = Vector3.zero;
            v3Forward = Vector3.zero;
        }

        public GameObjectDummy(Vector3 pos, Vector3 up, Vector3 forward, Vector3 right)
        {
            v3Pos = pos;
            v3Up = up;
            v3Forward = forward;
            v3Right = right;
        }

        public GameObjectDummy(GameObject GObj)
        {
            v3Pos = GObj.transform.position;
            v3Up = GObj.transform.up;
            v3Right = GObj.transform.right;
            v3Forward = GObj.transform.forward;
        }

        public void Set(GameObject rGob)
        {
            v3Pos = rGob.transform.position;
            v3Up = rGob.transform.up;
            v3Right = rGob.transform.right;
            v3Forward = rGob.transform.forward;
        }

        public void Set(Vector3 pos, Vector3 up, Vector3 forward, Vector3 right)
        {
            v3Pos = pos;
            v3Up = up;
            v3Forward = forward;
            v3Right = right;
        }
    }

    public class MainCameraAction : MonoBehaviour
    {
        Vector3 mv3PrevPos;
        float mMoveSpeed;

        GameObject mCurrentFocusMFM;    // the current look at mirror frame.  maybe is null

        public void OnResetCurrentFocusMirrorFrame(GameObject gob)
        {
            mCurrentFocusMFM = gob;
        }

        private void OnPreRender()
        {
            SetMirrorFrameViewMatrix();
        }


        GameObjectDummy mDummyGObj = new GameObjectDummy();
        void SetMirrorFrameViewMatrix()
        {
            if (mCurrentFocusMFM != null)
            {

                //RealMirrorMap mmap = mCurrentFocusMFM.GetComponent<RealMirrorMap>();
               
                mDummyGObj.Set(gameObject);
             //   mmap.OnSetRelativeCameraViewVectors(mDummyGObj, mDummyGObj, 1.0f, (float)Screen.width / (float)Screen.height);
            }
        }


        private void OnDrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Camera cam = GetComponent<Camera>();
            Gizmos.color = Color.red;
            Gizmos.DrawFrustum(Vector3.zero, cam.fieldOfView, cam.farClipPlane, cam.nearClipPlane, cam.aspect);
        }

        private void Awake()
        {
            mMoveSpeed = 0.2f;
            mCurrentFocusMFM = null;
        }

        // Use this for initialization
        void Start()
        {
            mv3PrevPos = Input.mousePosition;

        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetKey(KeyCode.W))
            {
                transform.position += transform.forward * mMoveSpeed;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.position += -transform.forward * mMoveSpeed;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                transform.position += -transform.right * mMoveSpeed;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.position += transform.right * mMoveSpeed;
            }



            Vector3 v3Dlt = (Input.mousePosition - mv3PrevPos) * Time.deltaTime;
            if (Input.GetMouseButton(1))
            {
                transform.RotateAround(transform.position, Vector3.up, v3Dlt.x);
                transform.RotateAround(transform.position, transform.right, -v3Dlt.y);
            }

            mv3PrevPos = Input.mousePosition;

        }
    }

}
