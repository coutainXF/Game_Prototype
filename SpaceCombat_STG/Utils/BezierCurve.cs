using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve
{
    //返回二次贝塞尔曲线上的值
    public static Vector3 QuadraticPoint(Vector3 startPoint,Vector3 endPoint,Vector3 controlPoint,float by)
    {
        return Vector3.Lerp(Vector3.Lerp(startPoint,controlPoint,by),
            Vector3.Lerp(controlPoint,endPoint,by),
            by);
    }
}
