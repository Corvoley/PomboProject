using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    
    // retorna funcao do tipo dentro do < >
    private Func<Vector3> GetCameraFollowPositionFunc;
    


    public void Setup(Func<Vector3> GetCameraFollowPositionFunc)
    {
        this.GetCameraFollowPositionFunc = GetCameraFollowPositionFunc;
    }

   
    void Update()
    {

        Vector3 cameraFollowPosition = GetCameraFollowPositionFunc();
        cameraFollowPosition.z = transform.position.z;
        transform.position = cameraFollowPosition;





         //suaviza a camera e evita bug em caso de fps baixo

        //Vector3 cameraFollowPosition = GetCameraFollowPositionFunc();
        //cameraFollowPosition.z = transform.position.z;

        //Vector3 cameraMoveDir = (cameraFollowPosition - transform.position).normalized;
        //float distance = Vector3.Distance(cameraFollowPosition, transform.position);
        //float cameraMoveSpeed = 5f;

        //if (distance > 0)
        //{
        //   Vector3 newCameraPosition =  transform.position = transform.position + cameraMoveDir * cameraMoveSpeed * Time.deltaTime;
        //    float distanceAfterMoving = Vector3.Distance(newCameraPosition, cameraFollowPosition);

        //    if (distanceAfterMoving > distance)
        //    {
        //        newCameraPosition = cameraFollowPosition;
        //    }
        //    transform.position = newCameraPosition;
        //}

    }
}
