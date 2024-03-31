using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGuidanceSystem : MonoBehaviour
{

    [SerializeField] private Projectile _projectile;
    [SerializeField] private float minBallisticAngle = 50f;
    [SerializeField] private float maxBallisticAngle = 75f;

    private float ballisticAngle;
    private Vector3 targetDirection;

    //归巢携程，指代导弹的自动导航
    public IEnumerator HomingCoroutine(GameObject target)
    {
        ballisticAngle = Random.Range(minBallisticAngle,maxBallisticAngle);
        while (gameObject.activeSelf)
        {
            if (target.activeSelf)
            {
                //move to target//往目标移动
                targetDirection = target.transform.position - transform.position;
                
                //rotate to target
                
                //var angle = Mathf.Atan2(targetDirection.y,targetDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(targetDirection.y,targetDirection.x) * Mathf.Rad2Deg,Vector3.forward);
                transform.rotation *= Quaternion.Euler(0f, 0f, ballisticAngle);
                
                _projectile.Move();
            }
            else
            {
                //move projectile in move direction沿着原来的路径移动
                _projectile.Move();
            }

            yield return null;
        }
        
    }
}
