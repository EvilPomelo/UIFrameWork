using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;
namespace Demo1
{
    public class StartProject : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            UIManager.GetInstance().ShowUIForms("LoginUIForm");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}