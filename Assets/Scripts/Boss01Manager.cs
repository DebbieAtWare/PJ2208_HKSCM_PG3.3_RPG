using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss01Manager : MonoBehaviour
{
    [Header("Boss")]
    public BossObject bossObj;

    CommonUtils commonUtils;

    void Start()
    {
        commonUtils = CommonUtils.instance;
        bossObj.Setup(commonUtils.dialogBox_BossAlert, commonUtils.Boss01);
    }

    void Update()
    {
        bossObj.UpdateRun();
    }

}
