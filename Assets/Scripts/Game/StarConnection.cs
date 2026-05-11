using System;
using UnityEngine;

public class StarConnection
{
    public StarAnchor PointA;
    public StarAnchor PointB;
    public LineRenderer lineConnect;

    public StarObject StarA;
    public StarObject StarB;

    public StarConnection(StarAnchor pointA, StarAnchor pointB, LineRenderer lineConnect)
    {
        PointA = pointA;
        PointB = pointB;
        this.lineConnect = lineConnect;

        StarA = pointA.AnchoredStar;
        StarB = pointB.AnchoredStar;
    }
}

[Serializable]
public struct StarConnectionAnswer
{
    public StarAnchor PointA;
    public StarAnchor PointB;
    public bool Solved;
    public StarConnection correctConnection;
}
