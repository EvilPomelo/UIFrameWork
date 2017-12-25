using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUIFW
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
        Fixed,
        //弹出窗体
        PopUp,
    }
    #endregion
}