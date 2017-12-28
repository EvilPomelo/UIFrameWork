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

        /// <summary>
        /// 初始化数据
        /// </summary>
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
            transform.SetParent(_TraUIScripts);
            DontDestroyOnLoad(_TraCanvasTransform);
            if (_DicFormPaths !=null)
            {
                _DicFormPaths.Add("LoginUIForm", @"UIPrefabs\LoginUIForm");
            }
        }

        /// <summary>
        /// 初始化读取Canvas
        /// </summary>
        private void InitRootCanvasLoading()
        {
            ResourcesMgr.GetInstance().LoadAsset(SysDefine.SYS_PATH_CANVAS,false);
        }

        /// <summary>
        /// 通过uiFormName读取UIForms，并确定显示模式
        /// </summary>
        /// <param name="uiFormName"></param>
        public void ShowUIForms(string uiFormName)
        {
            BaseUIForm baseUIForms = null;
            if (string.IsNullOrEmpty(uiFormName))
            {
                Debug.Log("uiFormName 不能为0");
                return;
            }
            baseUIForms = LoadFormsToAllUIFormsCatch(uiFormName);
            switch (baseUIForms.CurrentUIType.UIForms_ShowMode)
            {
                case UIFormShowMode.Normal:
                    break;
                case UIFormShowMode.ReverseChange:
                    break;
                case UIFormShowMode.HideOther:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 根据UI窗体的缓存集合判断是从缓存中加载，还是从路径中重新读取
        /// </summary>
        /// <param name="uiFormName"></param>
        private BaseUIForm LoadFormsToAllUIFormsCatch(string uiFormName)
        {
            BaseUIForm baseuiForm = null;
            _DicALLUIForms.TryGetValue(uiFormName,out baseuiForm);
            if (baseuiForm == null)
            {
                baseuiForm = LoadUIForm(uiFormName);
                _DicALLUIForms.Add(uiFormName, baseuiForm);
            }
            return baseuiForm;
        }

        /// <summary>
        /// UIForm
        /// </summary>
        /// <param name="uiFormName"></param>
        private BaseUIForm LoadUIForm(string uiFormName)
        {
            //定义三个局部变量，需加载的UI预制体的路径，预制体本身，预制体上挂载的baseUiForm
            string strUIFormPaths = null;
            GameObject goCloneUIPrefabs = null;
            BaseUIForm baseUiForm = null;

            //根据UI窗体名称，得到对应的加载路径
            _DicFormPaths.TryGetValue(uiFormName,out strUIFormPaths);
            
            //如果未从缓存字典中得到集合
            if (!string.IsNullOrEmpty(strUIFormPaths))
            {
                //用自定义的资源读取脚本从路径中得到并生成预制体
                goCloneUIPrefabs = ResourcesMgr.GetInstance().LoadAsset(strUIFormPaths, false);
            }
            else
            {
                Debug.Log("未将该uiFormName的路径加入缓存中，参数uiFormName="+uiFormName);
            }

            //从该预制体上得到BaseUiForm(窗体控制脚本，实际得到的是BaseUiForm的子类)
            if (_TraCanvasTransform != null && goCloneUIPrefabs != null)
            {
                baseUiForm = goCloneUIPrefabs.GetComponent<BaseUIForm>();

                if (baseUiForm == null)
                {
                    Debug.Log("baseUiForm==null! ,请先确认窗体预设对象上是否加载了baseUIForm的子类脚本！ 参数 uiFormName=" + uiFormName); 
                    return null;
                }
                //根据BaseUiForm中持有的CurrentUIType类中的属性判断加载到哪个节点
                switch (baseUiForm.CurrentUIType.UIForms_Type)
                {
                    case UIFormType.Normal:
                        goCloneUIPrefabs.transform.SetParent(_TraNormal,false);
                        break;
                    case UIFormType.PopUp:
                        goCloneUIPrefabs.transform.SetParent(_TraPopUp, false);
                        break;
                    case UIFormType.Fixed:
                        goCloneUIPrefabs.transform.SetParent(_TraFixed, false);
                        break;
                    default:
                        break;
                }
                //设置隐藏
                goCloneUIPrefabs.SetActive(false);
            }
            return baseUiForm;
        }
    }
}