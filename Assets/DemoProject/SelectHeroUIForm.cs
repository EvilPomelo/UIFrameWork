using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrameWork;

namespace Demo1
{
    public class SelectHeroUIForm : BaseUIForm
    {

        // Use this for initialization
        void Awake()
        {
            //窗体的性质
            CurrentUIType.UIForms_ShowMode = UIFormShowMode.HideOther;
            //注册进入主城的事件
            RigisterButtonObjectEvent("Btn_Enter", EnterMainCityUIForm);
            //注册放回方法
            RigisterButtonObjectEvent("Btn_Close", ReturnLoginUIForm);
        }

        private void Start()
        {
            //显示“UI管理器”内部的状态
            print("‘所有窗体集合’中的数量=" + UIManager.GetInstance().ShowALLUIFormCount());
            print("‘当前窗体集合’中的数量=" + UIManager.GetInstance().ShowCurrentUIFormsCount());
            print("‘栈窗体集合’中的数量=" + UIManager.GetInstance().ShowCurrentStackUIFormsCount());
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void EnterMainCityUIForm(GameObject go)
        {
            print("进入主城");
        }

        private void ReturnLoginUIForm(GameObject go)
        {
            print("关闭UI");
            CloseUIForm();
        }
    }
}