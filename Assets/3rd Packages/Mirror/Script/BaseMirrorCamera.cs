
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
 *  Date  : 2021-2                                                            *
 *                                                                            *
 ******************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Fantasy
{

    [ExecuteInEditMode]
    public class BaseMirrorCamera : MonoBehaviour
    {

        protected GameObject mMirrorFrame;

        //reserve
        protected GameObject mSceneCamerGObj;

        protected virtual AbstractMirrorMap GetMirrorMap()
        {
            return mMirrorFrame.GetComponent<BaseMirrorMap>();
        }


        void OnDrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Camera cam = GetComponent<Camera>();
            Gizmos.color = Color.green;
            Gizmos.DrawFrustum(Vector3.zero, cam.fieldOfView, cam.farClipPlane, cam.nearClipPlane, cam.aspect);
            //Debug.Log("base mirror camera onDrawGizmos.");
        }


        protected void Inital()
        {
            //Set scene camera.  
            mSceneCamerGObj = Camera.main.gameObject;

            //Get mirror frame in this mirror effect object.
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                GameObject gob = transform.parent.GetChild(i).gameObject;
                if (gob != gameObject)
                {
                    mMirrorFrame = gob;
                    break;
                }
            }

            //Set mirror camera parameters.
            Camera cam = GetComponent<Camera>();

            // BaseMirrorMap mmp = mMirrorFrame.GetComponent<BaseMirrorMap>();
            AbstractMirrorMap mmp = GetMirrorMap();
            cam.targetTexture = mmp.renderTarget;
        }

        protected void Process()
        {
            //lock rolling
            LockRolling();


            if (mMirrorFrame != null && mSceneCamerGObj != null)
            {
                //Adjust position
                Vector3 v3OffLocal = mSceneCamerGObj.transform.position - mMirrorFrame.transform.position;
                transform.position = mSceneCamerGObj.transform.position
                                     + 2.0f * (Vector3.Dot(v3OffLocal, -mMirrorFrame.transform.forward)
                                               * mMirrorFrame.transform.forward);

                //Adjust pose
                Vector3 v3MrUp = CaculateMirrorVector3(mSceneCamerGObj.transform.up, mMirrorFrame.transform.right,
                                                       mMirrorFrame.transform.up, mMirrorFrame.transform.forward);
                Vector3 v3MrForward = CaculateMirrorVector3(mSceneCamerGObj.transform.forward, mMirrorFrame.transform.right,
                                                            mMirrorFrame.transform.up, mMirrorFrame.transform.forward);


                transform.LookAt(transform.position + v3MrForward, v3MrUp);

            }
        }

        private void Awake()
        {
            mMirrorFrame = null;
        }


        protected void LockRolling()
        {
            Vector3 v3Euler = transform.eulerAngles;
            v3Euler.z = 0.0f;
            transform.eulerAngles = v3Euler;
        }


        // Use this for initialization
        // The MirrorEffect Node just have to append two child: MirrorFrame and MirrorCamera.
        void Start()
        {
            Inital();
        }

        // Update is called once per frame
        void Update()
        {

            Process();

        }


        protected Vector3 CaculateMirrorVector3(Vector3 v3Input, Vector3 v3MPlaneX, Vector3 v3MPlaneY, Vector3 v3MPlaneZ)
        {

            float x = Vector3.Dot(v3Input, v3MPlaneX);
            float y = Vector3.Dot(v3Input, v3MPlaneY);
            float z = Vector3.Dot(v3Input, v3MPlaneZ);

            return (v3MPlaneX * x + v3MPlaneY * y - v3MPlaneZ * z);
        }

    }
}

