using System.Collections;
using UnityEngine;

namespace behavior
{
    public class TouchDraw : MonoBehaviour
    {
        private Coroutine _drawing;
        public GameObject linePrefab;


        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartLine();
            }

            if (Input.GetMouseButtonUp(0))
            {
                FinishLine();
            }
        }

        private void StartLine()
        {
            if (_drawing != null) StopCoroutine(_drawing);
            _drawing = StartCoroutine(DrawLine());
        }

        private void FinishLine()
        {
            StopCoroutine(_drawing);
        }

        private IEnumerator DrawLine()
        {
            GameObject line = Instantiate(linePrefab, transform);
            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
            lineRenderer.positionCount = 1;

            while (true)
            {
                Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                position.z = 0;
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, position);
                yield return null;
            }
        }
    }
}