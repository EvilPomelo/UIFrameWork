using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrameWork;
using System;
namespace Demo1
{
    public class LoginUIForm : BaseUIForm
    {

        public void Awake()
        {
            RigisterButtonObjectEvent("Btn_Log", p => OpenUIForm("SelectHeroUIForm"));
        }


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// 登陆方法
        /// </summary>
        /// <param name="go"></param>
        public void LoginSys(GameObject go)
        {
            //校验账户
            OpenUIForm("SelectHeroUIForm");
        }
    }
}