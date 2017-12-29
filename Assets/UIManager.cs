using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIFrameWork
{

    public class UIManager : MonoBehaviour
    {
        #region 全局变量
        //字段
        private static UIManager _Instace = null;
        //UI窗体预设路径(参数1：窗体预设名称，2：窗体预设路径)
        private Dictionary<string, string> _DicFormPaths;
        //缓存所有UI窗体
        private Dictionary<string, BaseUIForm> _DicALLUIForms;
        //当前显示的UI窗体
        private Dictionary<string, BaseUIForm> _DicCurrentShowUIForms;
        //定义"栈"集合，存储显示当前所有[反向切换]的窗体类型
        private Stack<BaseUIForm> _StaCurrentUIForms;
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
        #endregion

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
            //初始化ResourcesMgr对象，加载Canvas对象。
            InitRootCanvasLoading();
            //字段初始化
            _DicFormPaths = new Dictionary<string, string>();
            _DicALLUIForms = new Dictionary<string, BaseUIForm>();
            _DicCurrentShowUIForms = new Dictionary<string, BaseUIForm>();
            _StaCurrentUIForms = new Stack<BaseUIForm>();
            _TraCanvasTransform = GameObject.FindGameObjectWithTag(SysDefine.SYS_TAG_CANVAS).transform;
            //得到UI根节点、全屏节点、固定节点、弹出节点
            _TraNormal = _TraCanvasTransform.Find(SysDefine.SYS_Node_Normal);
            _TraFixed = _TraCanvasTransform.Find(SysDefine.SYS_Node_Fixed);
            _TraPopUp = _TraCanvasTransform.Find(SysDefine.SYS_Node_PopUp);
            _TraUIScripts = _TraCanvasTransform.Find(SysDefine.SYS_Node_ScriptMgr);
            //将该对象加入__TraUIScripts节点下
            UnityHelper.AddChildNodeParentNode(_TraUIScripts, transform);
            DontDestroyOnLoad(_TraCanvasTransform);
            if (_DicFormPaths != null)
            {
                _DicFormPaths.Add("LoginUIForm", @"UIPrefabs\LoginUIForm");
                _DicFormPaths.Add("SelectHeroUIForm", @"UIPrefabs\SelectHeroUIForm");
            }
        }

        /// <summary>
        /// 初始化读取Canvas
        /// </summary>
        private void InitRootCanvasLoading()
        {
            ResourcesMgr.GetInstance().LoadAsset(SysDefine.SYS_PATH_CANVAS, false);
        }

        #region 打开与关闭窗体的管理类，上层所调用的方法
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

            if (baseUIForms == null) return;

            //是否清空"栈集合"中得数据
            if (baseUIForms.CurrentUIType.IsClearStack)
            {
                ClearStackArray();
            }

            //根据不同的UI窗体的显示模式，分别作不同的加载处理
            switch (baseUIForms.CurrentUIType.UIForms_ShowMode)
            {
                case UIFormShowMode.Normal:
                    EnterUIToCurrentCache(uiFormName);
                    break;
                case UIFormShowMode.ReverseChange:
                    PushUIFormToStack(uiFormName);
                    break;
                case UIFormShowMode.HideOther:
                    EnterUIFormsAndHideOther(uiFormName);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 关闭UI窗体的方法
        /// </summary>
        /// <param name="uiFormName"></param>
        public void CloseUIForms(string uiFormName)
        {
            BaseUIForm baseUiForm;
            //参数检查
            if (string.IsNullOrEmpty(uiFormName)) return;
            //"所有UI窗体"集合中，如果没有记录，则直接返回
            _DicALLUIForms.TryGetValue(uiFormName, out baseUiForm);
            if (baseUiForm == null) return;
            //根据窗体不同的显示类型，分别作不同的关闭处理
            switch (baseUiForm.CurrentUIType.UIForms_ShowMode)
            {
                case UIFormShowMode.Normal:
                    ExitUIForms(uiFormName);
                    break;
                case UIFormShowMode.ReverseChange:
                    PopUIFroms(uiFormName);
                    break;
                case UIFormShowMode.HideOther:
                    ExitUIFormsAndHidenOther(uiFormName);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region 获取窗体对象的方法
        /// <summary>
        /// 根据UI窗体的缓存集合判断是从缓存中取得还是从路径中读取uiFormName预制体
        /// </summary>
        /// <param name="uiFormName"></param>
        private BaseUIForm LoadFormsToAllUIFormsCatch(string uiFormName)
        {
            BaseUIForm baseuiForm = null;
            _DicALLUIForms.TryGetValue(uiFormName, out baseuiForm);
            if (baseuiForm == null)
            {
                baseuiForm = LoadUIForm(uiFormName);
                _DicALLUIForms.Add(uiFormName, baseuiForm);
            }
            return baseuiForm;
        }

        /// <summary>
        /// 从Resources中读取窗体预制体
        /// </summary>
        /// <param name="uiFormName"></param>
        private BaseUIForm LoadUIForm(string strUIName)
        {
            //定义三个局部变量，需加载的UI预制体的路径，预制体本身，预制体上挂载的baseUiForm
            string strUIFormPaths = null;
            GameObject goCloneUIPrefabs = null;
            BaseUIForm baseUiForm = null;

            //根据UI窗体名称，得到对应的加载路径
            _DicFormPaths.TryGetValue(strUIName, out strUIFormPaths);

            //如果未从缓存字典中得到集合
            if (!string.IsNullOrEmpty(strUIFormPaths))
            {
                //用自定义的资源读取脚本从路径中得到并生成预制体
                goCloneUIPrefabs = ResourcesMgr.GetInstance().LoadAsset(strUIFormPaths, false);
            }
            else
            {
                Debug.Log("未将该uiFormName的路径加入缓存中，参数uiFormName=" + strUIName);
            }

            //从该预制体上得到BaseUiForm(窗体控制脚本，实际得到的是BaseUiForm的子类)
            if (_TraCanvasTransform != null && goCloneUIPrefabs != null)
            {
                baseUiForm = goCloneUIPrefabs.GetComponent<BaseUIForm>();

                if (baseUiForm == null)
                {
                    Debug.Log("baseUiForm==null! ,请先确认窗体预设对象上是否加载了baseUIForm的子类脚本！ 参数 uiFormName=" + strUIName);
                    return null;
                }
                //根据BaseUiForm中持有的CurrentUIType类中的属性判断加载到哪个节点
                switch (baseUiForm.CurrentUIType.UIForms_Type)
                {
                    case UIFormType.Normal:
                        goCloneUIPrefabs.transform.SetParent(_TraNormal, false);
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
        #endregion

        #region 打开与关闭窗体
        /// <summary>
        /// 进入当前窗体并加载到当前集合中
        /// </summary>
        /// <param name="uiFormName"></param>
        void EnterUIToCurrentCache(string strUIName)
        {
            BaseUIForm baseUIForm = null;
            //如果"正在显示"的集合中，存在整个UI窗体，则直接返回
            _DicCurrentShowUIForms.TryGetValue(strUIName, out baseUIForm);
            if (baseUIForm != null) return;
            //把当前窗体，加载到"正在显示"集合中
            _DicALLUIForms.TryGetValue(strUIName, out baseUIForm);
            if (baseUIForm != null)
            {
                _DicCurrentShowUIForms.Add(strUIName, baseUIForm);
                baseUIForm.Display();
            }
        }

        /// <summary>
        /// 退出指定UI窗体
        /// </summary>
        /// <param name="strUIFormName"></param>
        void ExitUIForms(string strUIName)
        {
            BaseUIForm baseUIForm = null;
            _DicCurrentShowUIForms.TryGetValue(strUIName, out baseUIForm);
            if (baseUIForm == null) return;
            baseUIForm.Hiding();
            _DicCurrentShowUIForms.Remove(strUIName);
        }

        /// <summary>
        /// UI窗体入栈
        /// </summary>
        /// <param name="uiFormName"></param>
        void PushUIFormToStack(string strUIName)
        {
            BaseUIForm baseUIForm = null;
            if (_StaCurrentUIForms.Count > 0)
            {
                BaseUIForm TopUIForm = _StaCurrentUIForms.Peek();
                //冻结栈顶元素
                TopUIForm.Freeze();
            }
            //从缓存字典中获取该窗体
            _DicALLUIForms.TryGetValue(strUIName, out baseUIForm);
            if (baseUIForm == null)
            {
                Debug.Log("未能获取到该baseUIForm，参数strUIName=" + strUIName);
            }
            else
            {
                //显示该窗体
                baseUIForm.Display();
                //该窗体入栈操作
                _StaCurrentUIForms.Push(baseUIForm);
            }
        }

        /// <summary>
        /// (反向切换类型窗体)窗体出栈逻辑
        /// </summary>
        void PopUIFroms(string strUIName)
        {
            if (_StaCurrentUIForms.Count >= 2)
            {
                //出栈处理
                BaseUIForm topUIForm = _StaCurrentUIForms.Pop();
                //隐藏该出栈对象
                topUIForm.Hiding();
                //获取出栈后的栈顶对象
                BaseUIForm nextUIForm = _StaCurrentUIForms.Peek();
                //被覆盖的上一个窗体重新显示
                nextUIForm.Redisplay();
            }
            if (_StaCurrentUIForms.Count == 1)
            {
                //出栈处理
                BaseUIForm topUIForms = _StaCurrentUIForms.Pop();
                //做隐藏处理
                topUIForms.Hiding();
            }
        }

        /// <summary>
        /// 是否清空"栈集合"中得数据
        /// </summary>
        /// <returns></returns>
        private bool ClearStackArray()
        {
            if (_StaCurrentUIForms != null && _StaCurrentUIForms.Count >= 1)
            {
                _StaCurrentUIForms.Clear();
                return true;
            }
            return false;
        }

        /// <summary>
        /// ("隐藏其它"类型窗体)打开指定窗体，切隐藏其它窗体
        /// </summary>
        /// <param name="strUIName"></param>
        void EnterUIFormsAndHideOther(string strUIName)
        {
            BaseUIForm baseUIForm;
            BaseUIForm baseUIFormAll;

            if (string.IsNullOrEmpty(strUIName)) return;
            //当前窗体集合中获取该界面
            _DicCurrentShowUIForms.TryGetValue(strUIName, out baseUIForm);
            //若该界面为空
            if (baseUIForm != null) return;
            //把“正在显示集合”与“栈集合”中所有窗体隐藏
            foreach (BaseUIForm baseUI in _DicCurrentShowUIForms.Values)
            {
                baseUI.Hiding();
            }
            foreach (BaseUIForm baseUI in _StaCurrentUIForms)
            {
                baseUI.Hiding();
            }
            //把当前窗体加入到"正在显示的窗体"集合中，且做显示处理。
            _DicALLUIForms.TryGetValue(strUIName, out baseUIFormAll);
            if (baseUIFormAll != null)
            {
                _DicCurrentShowUIForms.Add(strUIName, baseUIFormAll);
                //窗体显示
                baseUIFormAll.Display();
            }
        }


        /// <summary>
        /// ("隐藏其它"类型窗体)关闭窗体，切显示其他窗体
        /// </summary>
        /// <param name="strUIName"></param>
        void ExitUIFormsAndHidenOther(string strUIName)
        {
            BaseUIForm baseUIForm;
            //参数检查
            if (string.IsNullOrEmpty(strUIName)) return;
            //当前缓存中获取该UIForm的值
            _DicCurrentShowUIForms.TryGetValue(strUIName, out baseUIForm);
            //如果当前缓存中没有该UIForm则返回
            if (baseUIForm == null) return;
            //隐藏该UIForm
            baseUIForm.Hiding();
            //从当前缓存中删除该strUIName
            _DicCurrentShowUIForms.Remove(strUIName);
            //遍历缓存栈和当前缓存字典，全部重新显示
            foreach (BaseUIForm baseUI in _DicCurrentShowUIForms.Values)
            {
                baseUI.Redisplay();
            }
            foreach (BaseUIForm baseUI in _StaCurrentUIForms)
            {
                baseUI.Redisplay();
            }
        }
        #endregion

        #region  显示“UI管理器”内部核心数据，测试使用

        /// <summary>
        /// 显示"所有UI窗体"集合的数量
        /// </summary>
        /// <returns></returns>
        public int ShowALLUIFormCount()
        {
            if (_DicALLUIForms != null)
            {
                return _DicALLUIForms.Count;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 显示"当前窗体"集合中数量
        /// </summary>
        /// <returns></returns>
        public int ShowCurrentUIFormsCount()
        {
            if (_DicCurrentShowUIForms != null)
            {
                return _DicCurrentShowUIForms.Count;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 显示“当前栈”集合中窗体数量
        /// </summary>
        /// <returns></returns>
        public int ShowCurrentStackUIFormsCount()
        {
            if (_StaCurrentUIForms != null)
            {
                return _StaCurrentUIForms.Count;
            }
            else
            {
                return 0;
            }
        }

        #endregion
    }
}