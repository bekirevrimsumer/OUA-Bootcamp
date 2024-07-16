
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
    public class AbstractMirrorMap : MonoBehaviour
    {
        public virtual Texture2D mirrorImage
        {
            get
            {
                return null;
            }
        }

        public virtual RenderTexture renderTarget
        {
            get
            {
                return null;
            }
        }

        public virtual Shader currentShader
        {
            get
            {
                return null;
            }
        }


        public virtual GameObject mirrorCamereObj
        {
            get
            {
                return null;
            }
        }
    }

    [ExecuteInEditMode]
    public class BaseMirrorMap : AbstractMirrorMap
    {
        public enum EVertex { LT=0, RT, LB, RB, COUNT}

        public Light MainLight;

        [Range(0.0f, 256.0f)]
        public float Width = 10.0f;

        [Range(0.0f, 256.0f)]
        public float Height = 10.0f;

        [Range(512, 1024)]
        public int Size = 512;

        ArrayList malMirrorBlocks = new ArrayList();


        RenderTexture mRt;
        Texture2D mTmpPic;
        Rect mRect;

        protected Material mMatMirror;

        //reserve
        protected GameObject mSceneCameraGObj;
        protected GameObject mMirrorCameraGObj;

        protected float mfWidth;
        protected float mfHeight;
        protected int mImageSize;
        protected Light mMainLight;

        private Vector3[] maryVertex; // four vertex: left-top, right-top, left-bottom, right-bottom.

        public float width
        {
            get
            {
                return mfWidth;
            }
        }

        public float height
        {
            get
            {
                return mfHeight;
            }
        }

        public Vector3[] boundsVertices
        {
            get
            {
                return maryVertex;
            }
        }


        public override Texture2D mirrorImage
        {
            get
            {
                return mTmpPic;
            }
        }

        public override RenderTexture renderTarget
        {
            get
            {
                return mRt;
            }
        }

        public override Shader currentShader
        {
            get
            {
                return mMatMirror == null ? null : mMatMirror.shader;

            }
        }


        public override GameObject mirrorCamereObj
        {
            get
            {
                return mMirrorCameraGObj;
            }
        }

        public bool isInitialized()
        {
            return (mSceneCameraGObj != null) && (mMirrorCameraGObj != null);
        }


        private void OnDrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(mfWidth, mfHeight, 0.0f));

        }


        protected virtual void Create()
        {
            SetupParamters();

            mMatMirror = Resources.Load<Material>("matBaseMirror");
          

            mRt = new RenderTexture(mImageSize, mImageSize, 0, RenderTextureFormat.ARGB32);
            mRt.antiAliasing = 8;
            mRt.autoGenerateMips = false;
            mRt.filterMode = FilterMode.Trilinear;
            mRt.wrapMode = TextureWrapMode.Clamp;
            mRt.antiAliasing = 8;
            mRt.useMipMap = false;
            mRt.name = name + "'s RenderTexture.";

            Vector3 v3OffUp = mfHeight * transform.up;
            Vector3 v3OffRight = mfWidth * transform.right;

            maryVertex = new Vector3[(int)EVertex.COUNT];
            maryVertex[(int)EVertex.LT] = transform.position + v3OffUp - v3OffRight;
            maryVertex[(int)EVertex.RT] = transform.position + v3OffUp + v3OffRight;
            maryVertex[(int)EVertex.LB] = transform.position - v3OffUp - v3OffRight;
            maryVertex[(int)EVertex.RB] = transform.position - v3OffUp + v3OffRight;
        }

        protected virtual void Inital()
        {
            SetupParamters();

            mSceneCameraGObj = Camera.main.gameObject;

            //Get mirror camera GameObject in this mirror effect object.
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                GameObject gob = transform.parent.GetChild(i).gameObject;
                if (gob != gameObject)
                {
                    mMirrorCameraGObj = gob;
                    break;
                }
            }

            if (mRt != null)
            {
                mTmpPic = new Texture2D(mRt.width, mRt.height);
                mRect = new Rect(0, 0, mRt.width, mRt.height);
            }
        }

        protected virtual void Process()
        {
            SetupParamters();

            //Lock Rolling
            Vector3 v3El = transform.eulerAngles;
            v3El.z = 0.0f;
            transform.eulerAngles = v3El;


            //Get mirror image.
            if (mRt != null && mTmpPic != null)
            {

                RenderTexture currentActiveRt = RenderTexture.active;

                RenderTexture.active = mRt;
                mTmpPic.ReadPixels(mRect, 0, 0);

                mTmpPic.Apply();

                RenderTexture.active = currentActiveRt;
            }
        }

        private void SetupParamters()
        {
            mfWidth = Width;
            mfHeight = Height;
            mImageSize = Size;
            mMainLight = MainLight;
        }


        private void Awake()
        {
            Create();
        }

        // Use this for initialization
        void Start()
        {
            Inital();
        }

        // Update is called once per frame
        void Update()
        {
            Process();
        }

    }

}
