/******************************************************************************
 *                                                                            *
 *                                                                            *
 *                          Fantasy Mirror                                    *
 *                                                                            *
 *                           Version: 1.1.0                                     *
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

    public class BaseMirrorBlock : MonoBehaviour
    {

        protected GameObject mMirrorFrame;
        protected Renderer mRender;

        float mfScreenAspect;

        protected virtual AbstractMirrorMap GetMirrorMap()
        {
            return mMirrorFrame.GetComponent<BaseMirrorMap>();
        }

        protected virtual void Create()
        {
            mMirrorFrame = null;
            mRender = GetComponent<Renderer>();
            mfScreenAspect = (float)Screen.width / (float)Screen.height;

            // mRender.material = new Material(mRender.material);
        }

        protected virtual void Inital()
        {
            mMirrorFrame = transform.parent.gameObject;
            if (mMirrorFrame != null)
            {
              //   BaseMirrorMap mmp = mMirrorFrame.GetComponent<BaseMirrorMap>();
               //  AbstractMirrorMap mmp = GetMirrorMap();
               //  mRender.material.shader = mmp.currentShader;
            }
        }

        protected void Process()
        {
            if (mMirrorFrame != null)
            {
                mfScreenAspect = (float)Screen.width / (float)Screen.height;
                Shader.SetGlobalFloat("fAspect", mfScreenAspect);

                AbstractMirrorMap mmp = GetMirrorMap();
                mRender.material.SetTexture("_MirrorMap", mmp.mirrorImage);

            }
        }

        private void Awake()
        {
            Create();
        }

        // Use this for initialization
        private void Start()
        {
            Inital();
        }

        // Update is called once per frame
        private void Update()
        {
            Process();
        }
    }
}

