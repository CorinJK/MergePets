using System;
using System.Collections;
using UnityEngine;

namespace TrackLogic
{
    public class SplineFollow : MonoBehaviour
    {
        public Transform[] Splines;

        private int pointToGo;
        private float tParam;

        private Vector2 catPosition;
        private float speed;
        private bool coroutineAllowed;

        public event Action<SplineFollow> OnFinished;
        private int distanceCounter = 1;
        
        private void Start()
        {
            pointToGo = 0;
            tParam = 0f;
            speed = 0.5f;
            coroutineAllowed = true;
        }

        private void OnEnable()
        {
            pointToGo = 0;
        }

        private void Update()
        {
            if (Splines == null)
            {
                return;
            }
            
            if (coroutineAllowed)
            {
                StartCoroutine(GoByTheSpline(pointToGo));
            }
        }

        private IEnumerator GoByTheSpline(int pointNumber)
        {
            coroutineAllowed = false;

            Vector2 p0 = Splines[pointNumber].GetChild(0).position;
            Vector2 p1 = Splines[pointNumber].GetChild(1).position;
            Vector2 p2 = Splines[pointNumber].GetChild(2).position;
            Vector2 p3 = Splines[pointNumber].GetChild(3).position;

            while (tParam < 1)
            {
                tParam += Time.deltaTime * speed;
                
                catPosition = Mathf.Pow(1 - tParam, 3) * p0 +
                              3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                              3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                              Mathf.Pow(tParam, 3) * p3;

                transform.position = catPosition;
                yield return new WaitForEndOfFrame();
            }

            tParam = 0f;
            pointToGo += 1;

            StartSplineAgain();
            PassFinishLine();

            coroutineAllowed = true;
        }

        private void StartSplineAgain()
        {
            if (pointToGo > Splines.Length - 1)
            {
                pointToGo = 0;
            }
        }

        private void PassFinishLine()
        {
            distanceCounter++;
            if (distanceCounter >= 2)
            {
                distanceCounter = 0;
                Debug.Log("Accrual of coins");
                OnFinished?.Invoke(this);
            }
        }
    }
}