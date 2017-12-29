using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIFrameWork
{
    public class UnityHelper : MonoBehaviour
    {
        /// <summary>
        /// 查找子节点对象
        /// 内部使用"递归算法"
        /// </summary>
        /// <param name="goParent">父对象</param>
        /// <param name="childName">需查找的子对象</param>
        /// <returns></returns>
        public static Transform FindTheChildNode(GameObject goParent, string childName)
        {
            Transform searchTrans = null;//查找结果
            searchTrans = goParent.transform.Find(childName);
            if (searchTrans == null)
            {
                foreach (Transform trans in goParent.transform)
                {
                    searchTrans = FindTheChildNode(trans.gameObject, childName);
                    if (searchTrans != null)
                    {
                        return searchTrans;
                    }
                }
            }
            return searchTrans;
        }

        /// <summary>
        /// 获取子节点(对象)脚本
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="goParent">父对象</param>
        /// <param name="childName">子对象名称</param>
        /// <returns></returns>
        public static T GetTheChildNodeCompontScripts<T>(GameObject goParent, string childName) where T : Component
        {
            Transform searchTranformNode = null;//查找特定子节点
            searchTranformNode = FindTheChildNode(goParent, childName);
            if (searchTranformNode != null)
            {
                return searchTranformNode.gameObject.GetComponent<T>();
            }
            else
            {
                return null;
            }
        }

        public static T AddChildNodeComponent<T>(GameObject goparent, string childName) where T : Component
        {
            Transform searchTransform = null;
            //查找特定子节点
            searchTransform = FindTheChildNode(goparent, childName);
            //如果查找成功，则考虑如果有相同脚本则先删除
            if (searchTransform != null)
            {
                //如果有相同脚本
                T[] componentScriptsArray = searchTransform.GetComponents<T>();
                foreach (T t in componentScriptsArray)
                {
                    if (t != null)
                    {
                        Destroy(t);
                    }
                }
                return searchTransform.gameObject.AddComponent<T>();
            }
            else
            {
                return null;
            }
            //如果查找不成功，返回null
        }

        /// <summary>
        /// 给子节点添加父对象
        /// </summary>
        /// <param name="parents">父对象的方法</param>
        /// <param name="child">子对象的方法</param>
        public static void AddChildNodeParentNode(Transform parents, Transform child)
        {
            child.SetParent(parents, false);
            child.localPosition = Vector3.zero;
            child.localScale = Vector3.one;
            child.localEulerAngles = Vector3.zero;
        }
    }
}