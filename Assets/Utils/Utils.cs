using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UtilsClass
{
    public static class Utils
    {
        //Sortingorder
        private const int SORTINGORDER_DEFAULTVALUE = 5000;
        public static int GetSortingOrder(Vector3 position, int offset, int baseSortingOrder = SORTINGORDER_DEFAULTVALUE)
        {
            return (int)(baseSortingOrder - position.y) + offset;
        }

        //Mouse Position in 2D
        private static Camera mainCamera;
        public static Vector3 Get2DMouseWorldPosition()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;

            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0f;
            return mouseWorldPosition;
        }

        //Random Offset von einer Position. Nutzen so Vector3 pos + GetRandomOffset()
        public static Vector3 GetRandomOffset2D()
        {
            return new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
        }
        public static Vector3 GetRandomOffset3D()
        {
            return new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
        }

        //Angle von einem Vector3 der durch 2 Punkte normalized entstanden ist. 
        public static float GetAngleFromVector(Vector3 vector)
        {
            float angleRadian = Mathf.Atan2(vector.y, vector.x);
            float degrees = angleRadian * Mathf.Rad2Deg;
            return degrees;
        }

        //Ist die Mouse gerade über einem UI Element?
        public static bool IsPointerOverUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Screenshake durch FunktionTimer mit Update
        public static void ShakeCamera(float intensity, float timer)
        {
            Vector3 lastCameraMovement = Vector3.zero;
            Vector3 startPos = Camera.main.transform.position;

            FunctionTimer.Create(() => {
                timer -= Time.unscaledDeltaTime;
                Vector3 randomMovement = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized * intensity;
                Camera.main.transform.position = Camera.main.transform.position - lastCameraMovement + randomMovement;
                lastCameraMovement = randomMovement;

                if(timer <= 0f)
                {
                    Camera.main.transform.position = startPos;
                }

                return timer <= 0f;
            }, "CAMERA SHAKE", true);
        }

        //Gibt Ray zurück der von der Camera bis zu dem Point wo man klickt geht
        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
        //Gibt einen Ray zurück welcher von der Camera zur Mouse Position geht und gibt die Position zurück, wo es trifft
        public static Vector3 GetMouseRayHitPosition(LayerMask layer)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layer);
            return raycastHit.point;
        }
    }
}
