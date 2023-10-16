using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveAxisState
{
    X,
    Y,
    Z
}
public class SimpleRotate : MonoBehaviour
{
    public float rotateSpeed = 0f;
    public MoveAxisState Axis;
    void update()
    {
        if(Axis == MoveAxisState.X)
        {
            transform.Rotate(Vector3.right * Time.deltaTime * rotateSpeed);
        } 
        else if(Axis == MoveAxisState.Y)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
        }
        else
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed);
        }
    }

}
