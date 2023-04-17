using System.Collections.Generic;
using UnityEngine;

//ChatGPT generated
public class PointsConnectorLong
{
    public static List<Vector3> ConnectPoints(List<Vector3> points)
    {
        List<Vector3> path = new List<Vector3>();

        if (points.Count <= 1)
        {
            return path;
        }

        // Start with the first point
        Vector3 currentPoint = points[0];
        path.Add(currentPoint);
        points.RemoveAt(0);

        while (points.Count > 0)
        {
            // Find the farthest point from the current point
            float maxDistance = float.MinValue;
            Vector3 farthestPoint = Vector3.zero;

            foreach (Vector3 point in points)
            {
                float distance = Vector3.Distance(currentPoint, point);

                if (distance > maxDistance && !IntersectsExistingConnections(path, currentPoint, point))
                {
                    maxDistance = distance;
                    farthestPoint = point;
                }
            }

            // If we found a valid point, add it to the path
            if (farthestPoint != Vector3.zero)
            {
                path.Add(farthestPoint);
                points.Remove(farthestPoint);
                currentPoint = farthestPoint;
            }
            else
            {
                // If we couldn't find a valid point, break out of the loop
                break;
            }
        }

        return path;
    }

    private static bool IntersectsExistingConnections(List<Vector3> path, Vector3 point1, Vector3 point2)
    {
        // Check if the line between point1 and point2 intersects with any existing connections
        for (int i = 0; i < path.Count - 1; i++)
        {
            Vector3 line1Start = path[i];
            Vector3 line1End = path[i + 1];

            if (Vector3.Distance(point1, line1Start) < 0.01f || Vector3.Distance(point1, line1End) < 0.01f
                || Vector3.Distance(point2, line1Start) < 0.01f || Vector3.Distance(point2, line1End) < 0.01f)
            {
                // Skip if any of the points are the same (prevents division by zero errors)
                continue;
            }

            Vector3 dir1 = (line1End - line1Start).normalized;
            Vector3 dir2 = (point2 - point1).normalized;

            if (Mathf.Abs(Vector3.Dot(dir1, dir2)) > 0.999f)
            {
                // Skip if the lines are parallel (prevents division by zero errors)
                continue;
            }

            Vector3 cross = Vector3.Cross(dir1, dir2);
            float t1 = Vector3.Dot((point1 - line1Start), cross) / Vector3.Dot(dir1, cross);
            float t2 = Vector3.Dot((point1 - point2), cross) / Vector3.Dot(dir2, cross);

            if (t1 >= 0f && t1 <= 1f && t2 >= 0f && t2 <= 1f)
            {
                return true;
            }
        }

        return false;
    }
}
