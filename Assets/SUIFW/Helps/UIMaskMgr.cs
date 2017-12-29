using UnityEngine;
using System.Collections;

namespace UIFrameWork
{
    public class UIMaskMgr : MonoBehaviour
    {
        //本脚本私有单例
        private static UIMaskMgr _Instance = null;
        //UI根节点的对象
        private GameObject _GoCanvasRoot = null;
        //UI脚本节点对象
        private Transform _TraUIScriptsNode = null;
        //顶层面板
        private GameObject _GoTopPanel;
        //遮罩面板
        private GameObject _GoMaskPanel;
        //UI摄像机
        private Camera _UICamera;
        //UI摄像机原始的"景深"
        private float _OriginalUICameraDepth;

        // Use this for initialization
        public static UIMaskMgr GetInstance()
        {
            if (_Instance = null)
            {
                _Instance = new GameObject("_UIMaskMgr").AddComponent<UIMaskMgr>();
        }
        
            return _Instance;
        }

        // Update is called once per frame
        void Awake()
        {
            _GoCanvasRoot = GameObject.FindGameObjectWithTag(SysDefine.SYS_TAG_CANVAS);
            _TraUIScriptsNode = UnityHelper.FindTheChildNode(_GoCanvasRoot, SysDefine.SYS_Node_ScriptMgr);
            UnityHelper.AddChildNodeToParentNode(_TraUIScriptsNode, this.gameObject.transform);
        }
    }
}