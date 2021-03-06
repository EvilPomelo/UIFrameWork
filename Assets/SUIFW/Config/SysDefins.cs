﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIFrameWork
{
    #region 系统枚举类型
    /// <summary>
    /// UI窗体(位置)类型
    /// </summary>
    public enum UIFormType
    {
        //普通窗体，
        Normal,
        //固定窗体，
        Fixed,
        //弹出窗体
        PopUp
    }
    /// <summary>
    /// UI窗体显示类型
    /// </summary>
    public enum UIFormShowMode
    {
        //普通窗体
        Normal,
        //固定窗体
        ReverseChange,
        //弹出窗体
        HideOther
    }

    /// <summary>
    /// UI窗体透明度类型
    /// </summary>
    public enum UIFormLucenyType
    {
        //完全透明，不能穿透
        Lucency,
        //半透明，不能穿透
        Translucence,
        //低透明度，不能穿透
        ImPenetrable,
        //可以穿透
        Pentrate
    }
    #endregion

    public class SysDefine : MonoBehaviour
    {
        /* 路径常量 */
        public const string SYS_PATH_CANVAS = @"UIPrefabs\Canvas";
        /* 标签常量 */
        public const string SYS_TAG_CANVAS = "_TagCanvas";
        /*节点常量*/
        public const string SYS_Node_Normal = "_Normal";
        public const string SYS_Node_Fixed = "_Fixed";
        public const string SYS_Node_PopUp= "_PopUp";
        public const string SYS_Node_ScriptMgr = "_ScriptMgr";

        /* 全局性的方法 */
        //Todo...

        /* 委托的定义 */
        //Todo....

    }
}