
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    Transform LookingTarget = null;

    public void SetUp(Transform attackTarget)
    {
        this.LookingTarget = attackTarget;
    }

    private void Update()
    {
        if (LookingTarget != null)
        {
            transform.LookAt(LookingTarget);
        }
    }
    
    

}
