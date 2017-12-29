using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIFrameWork
{
    public class BaseUIForm : MonoBehaviour
    {
        //字段
        private UIType _CurrentUIType = new UIType();

        //属性
        //当前UI窗体类型
        public UIType CurrentUIType
        {
            get { return _CurrentUIType; }
            set { _CurrentUIType = value; }
        }

        #region 显示状态
        /// <summary>
        /// 显示状态
        /// </summary>
        public virtual void Display()
        {
            this.gameObject.SetActive(true);
        }

        /// <summary>
        /// 隐藏状态
        /// </summary>
        public virtual void Hiding()
        {
            this.gameObject.SetActive(false);
        }

        /// <summary>
        /// 重新显示状态
        /// </summary>
        public virtual void Redisplay()
        {
            this.gameObject.SetActive(true);
        }

        /// <summary>
        /// 冻结状态
        /// </summary>
        public virtual void Freeze()
        {
            this.gameObject.SetActive(true);
        }
        #endregion

        #region 封装子类常用的方法

        /// <summary>
        /// 注册按钮事件方法
        /// </summary>
        /// <param name="goParent"></param>
        /// <param name="buttonName"></param>
        /// <param name="delHandle"></param>
        protected void RigisterButtonObjectEvent(string buttonName, EventTriggerListener.VoidDelegate delHandle)
        {
            Button btn = UnityHelper.GetTheChildNodeCompontScripts<Button>(this.gameObject, buttonName);
            if (btn != null)
            {
                EventTriggerListener.Get(btn.gameObject).onClick = delHandle;
            }
            else
            {
                print(this.gameObject.name);
                Debug.Log(this.gameObject + "路径下找不到" + buttonName + "按钮");
            }
        }

        protected void OpenUIForm(string strUIName)
        {
            UIManager.GetInstance().ShowUIForms(strUIName);
        }

        protected void CloseUIForm()
        {
            string strUIName = string.Empty;
            string uiName = GetType().ToString();
            strUIName = (GetType().ToString().Split('.'))[1];
            UIManager.GetInstance().CloseUIForms(strUIName);
        }


        #endregion
    }
}