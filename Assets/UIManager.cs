using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIFrameWork
{

    public class UIManager : MonoBehaviour {
        //字段
        private static UIManager _Instace = null;
        //UI窗体预设路径(参数1：窗体预设名称，2：窗体预设路径)
        private Dictionary<string, string> _DicFormPaths;
        //缓存所有UI窗体
        private Dictionary<string, BaseUIForm> _DicALLUIForms;
        //当前显示的UI窗体
        private Dictionary<string, BaseUIForm> _DicCurrentShowUIForms;
        //UI节点
        private Transform _TraCanvasTransform = null;
        //全屏幕显示节点
        private Transform _TraNormal = null;
        //固定显示节点
        private Transform _TraFixed = null;
        //弹出节点
        private Transform _TraPopUp = null;
        //UI管理脚本节点
        private Transform _TraUIScripts = null;

        /// <summary>
        /// 得到实例
        /// </summary>
        /// <returns></returns>
        public static UIManager GetInstance()
        {
            if (_Instace == null)
            {
                _Instace = new GameObject("_UIManager").AddComponent<UIManager>();
            }
            return _Instace;
        }

        private void Awake()
        {
            InitRootCanvasLoading();
            _DicFormPaths = new Dictionary<string, string>();
            _DicALLUIForms = new Dictionary<string, BaseUIForm>();
            _DicCurrentShowUIForms = new Dictionary<string, BaseUIForm>();
            _TraCanvasTransform = GameObject.FindGameObjectWithTag(SysDefine.SYS_TAG_CANVAS).transform;
            _TraNormal= _TraCanvasTransform.Find("Normal");
            _TraFixed = _TraCanvasTransform.Find("Fixed");
            _TraPopUp = _TraCanvasTransform.Find("_TraPopUp");
            _TraUIScripts = _TraCanvasTransform.Find("_ScriptMgr");

        }

        private void InitRootCanvasLoading()
        {
            ResourcesMgr.GetInstance().LoadAsset(SysDefine.SYS_PATH_CANVAS,false);
        }
    }
}