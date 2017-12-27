using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}