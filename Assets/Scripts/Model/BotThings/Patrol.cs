using System.Linq;
using UnityEngine;
using UnityEngine.AI;


namespace Old_Code
{
    public static class Patrol
    {
        //private static readonly Vector3[] _listPoint;
        private static int _indexCurPoint;

        private static int _minDistance = 50;
        private static int _maxDistance = 200;
        private static int _minCloseDis = 1;
        private static int _maxCloseDis = 5;        

        //private static NavMeshHit hit;

        static Patrol()
        {
            //var tempPoints = Object.FindObjectsOfType<DestroyPoint>();
            //_listPoint = tempPoints.Select(o => o.transform.position).ToArray();
        }

        public static Vector3 GenericPoint(Transform agent, bool closeDistance = false, bool isRandom = true)
        {
            Vector3 result;

            if (isRandom)
            {
                int dis = closeDistance ? CheckDistance(_minCloseDis, _maxCloseDis) : CheckDistance(_minDistance, _maxDistance);
                var randomPoint = Random.insideUnitSphere * dis;

                if (NavMesh.SamplePosition(agent.position + randomPoint, out var hit, dis, NavMesh.AllAreas)) result = hit.position;
                else result = Vector3.zero;
            }
            else result = Vector3.zero;

            //else // Свой алгоритм
            //{
            //	if (_indexCurPoint < _listPoint.Length - 1)
            //	{
            //		_indexCurPoint++;
            //	}
            //	else
            //	{
            //		_indexCurPoint = 0;
            //	}
            //	result = _listPoint[_indexCurPoint];
            //}

            return result;
        }

        private static int CheckDistance(int min, int max)
        {
            int distance = Random.Range(min, max);
            return distance;
        }
    }
}