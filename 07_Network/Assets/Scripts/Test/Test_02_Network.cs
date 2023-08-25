using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_02_Network : TestBase
{
    public Vector3 newPos = new Vector3();
    public TextMeshProUGUI test;

    protected override void Test1(InputAction.CallbackContext context)
    {
        NetPlayer netPlayer = FindObjectOfType<NetPlayer>();
        netPlayer.position.Value = newPos;
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        StringBuilder st = new StringBuilder();
        st.AppendLine("<#ff0000>1111</color>");
        st.AppendLine("asda");
        st.AppendLine("<#00ff00>33333</color>");


        Debug.Log(st.ToString());
        test.text = st.ToString();
    }

    protected override void Test3(InputAction.CallbackContext context)
    {
        string str = "(가가가)나나나나(다다다)라라라라";
        string[] result = str.Split('(', ')');
        foreach (string s in result)
        {
            Debug.Log(s);
        }
    }
}
