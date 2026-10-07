using System;
using System.Collections.Generic;
using Autodesk.AutoCAD.Geometry;

namespace CadBridgeDesigner.Tool.Tool.CadTool.GeometryTool
{
    /// <summary>
    /// 曲线工具类，提供曲线相关的几何计算方法
    /// </summary>
    public static class CurveTool
    {
        /// <summary>
        /// 获取曲线上指定比例位置的切向量
        /// </summary>
        /// <param name="curve">三维曲线对象（直线或圆弧）</param>
        /// <param name="value">曲线参数值，范围0~1（0=起点，1=终点）</param>
        /// <returns>该位置的单位切向量，不支持的类型返回NaN向量</returns>
        private static Vector3d GetTangentVector(Curve3d curve, double value)
        {
            switch (curve)
            {
                case LineSegment3d line:
                {
                    Vector3d tangentVector = line.EndPoint - line.StartPoint;
                    return tangentVector.GetNormal();
                }
                case CircularArc3d arc:
                {
                    double startAngle = arc.StartAngle;
                    double endAngle = arc.EndAngle;
                    double sweep = endAngle - startAngle;
                    if (sweep < 0)
                    {
                        sweep += Math.PI * 2;
                    }
                    double relativeAngle = sweep * value;
                    Vector3d directionAtStart = (arc.StartPoint - arc.Center).GetNormal();
                    Vector3d perpendicular = arc.Normal.CrossProduct(directionAtStart);
                    Vector3d tangentVector = directionAtStart * (-Math.Sin(relativeAngle)) + perpendicular * Math.Cos(relativeAngle);
                    return tangentVector.GetNormal();
                }
                default:
                {
                    return new Vector3d(double.NaN, double.NaN, double.NaN);
                }
            }
        }

        /// <summary>
        /// 获取曲线上的等分点集合
        /// </summary>
        /// <param name="curve">三维曲线对象（直线或圆弧）</param>
        /// <param name="numberOfDividePoints">等分点的数量（包含端点时为总点数）</param>
        /// <param name="withStartPointAndEndPoint">是否包含起点和终点，默认为true</param>
        /// <returns>等分点列表</returns>
        private static List<Point3d> GetDividePoints(Curve3d curve, int numberOfDividePoints, bool withStartPointAndEndPoint = true)
        {
            if (withStartPointAndEndPoint)
            {
                numberOfDividePoints -= 2;
                if (numberOfDividePoints < 0)
                {
                    numberOfDividePoints = 0;
                }
            }
            List<Point3d> dividePoints = new List<Point3d>();
            switch (curve)
            {
                case LineSegment3d line:
                {
                    Point3d startPoint3D = line.StartPoint;
                    Point3d endPoint3D = line.EndPoint;
                    double distanceInX = endPoint3D.X - startPoint3D.X;
                    double distanceInY = endPoint3D.Y - startPoint3D.Y;
                    double distanceInZ = endPoint3D.Z - startPoint3D.Z;
                    for (int i = 1; i < numberOfDividePoints + 1; i++)
                    {
                        double x = startPoint3D.X + distanceInX * i / (numberOfDividePoints + 1);
                        double y = startPoint3D.Y + distanceInY * i / (numberOfDividePoints + 1);
                        double z = startPoint3D.Z + distanceInZ * i / (numberOfDividePoints + 1);
                        dividePoints.Add(new Point3d(x, y, z));
                    }
                    if (withStartPointAndEndPoint)
                    {
                        dividePoints.Insert(0, startPoint3D);
                        dividePoints.Add(endPoint3D);
                    }
                    return dividePoints;
                }

                case CircularArc3d arc:
                {
                    double startAngle = arc.StartAngle;
                    double endAngle = arc.EndAngle;
                    double sweep = endAngle - startAngle;
                    if (sweep < 0)
                    {
                        sweep += Math.PI * 2;
                    }
                    Vector3d directionAtStart = (arc.StartPoint - arc.Center).GetNormal();
                    Vector3d perpendicular = arc.Normal.CrossProduct(directionAtStart);
                    for (int i = 1; i < numberOfDividePoints + 1; i++)
                    {
                        double relativeAngle = sweep * i / (numberOfDividePoints + 1);
                        Point3d point = arc.Center + arc.Radius * (directionAtStart * Math.Cos(relativeAngle) + perpendicular * Math.Sin(relativeAngle));
                        dividePoints.Add(point);
                    }
                    if (withStartPointAndEndPoint)
                    {
                        dividePoints.Insert(0, arc.StartPoint);
                        dividePoints.Add(arc.EndPoint);
                    }
                    return dividePoints;
                }
                default:
                {
                    return dividePoints;
                }
                    
            }
        }

        /// <summary>
        /// 获取曲线上等分点处的切向量集合
        /// </summary>
        /// <param name="curve">三维曲线对象（直线或圆弧）</param>
        /// <param name="numberOfDividePoints">等分点的数量（包含端点时为总点数）</param>
        /// <param name="withStartPointAndEndPoint">是否包含起点和终点的切向量，默认为true</param>
        /// <returns>等分点处的切向量列表</returns>
        public static List<Vector3d> GetDivideTangentVectors(Curve3d curve, int numberOfDividePoints, bool withStartPointAndEndPoint = true)
        {
            if (withStartPointAndEndPoint)
            {
                numberOfDividePoints -= 2;
                if (numberOfDividePoints < 0)
                {
                    numberOfDividePoints = 0;
                }
            }
            List<Vector3d> divideTangentVector = new List<Vector3d>();
            switch (curve)
            {
                case LineSegment3d line:
                {
                    for (int i = 0; i < numberOfDividePoints; i++)
                    {
                        double value =  (double)(i + 1) / (numberOfDividePoints + 1);
                        Vector3d tangentVector = GetTangentVector(line, value);
                        if (tangentVector == new Vector3d(double.NaN, double.NaN, double.NaN))
                        {
                            return divideTangentVector;
                        }
                        divideTangentVector.Add(tangentVector);
                    }
                    if (withStartPointAndEndPoint)
                    {
                        divideTangentVector.Insert(0, GetTangentVector(line, 0.0));
                        divideTangentVector.Add(GetTangentVector(line, 1.0));
                    }
                    return divideTangentVector;
                }
                case CircularArc3d arc:
                {
                    for (int i = 0; i < numberOfDividePoints; i++)
                    {
                        double value =  (double)(i + 1) / (numberOfDividePoints + 1);
                        Vector3d tangentVector = GetTangentVector(arc, value);
                        if (tangentVector == new Vector3d(double.NaN, double.NaN, double.NaN))
                        {
                            return divideTangentVector;
                        }
                        divideTangentVector.Add(tangentVector);
                    }
                    if (withStartPointAndEndPoint)
                    {
                        divideTangentVector.Insert(0, GetTangentVector(arc, 0.0));
                        divideTangentVector.Add(GetTangentVector(arc, 1.0));
                    }
                    return divideTangentVector;
                }
                default:
                {
                    return divideTangentVector;
                }
            }
        }

        /// <summary>
        /// 将曲线等分为多条子曲线
        /// </summary>
        /// <param name="curve">三维曲线对象（直线或圆弧）</param>
        /// <param name="numberOfDivideCurves">等分的子曲线数量</param>
        /// <returns>等分后的子曲线列表</returns>
        public static List<Curve3d> GetDivideCurves(Curve3d curve, int numberOfDivideCurves)
        {
            int numberOfDividePoints = numberOfDivideCurves+1;
            List<Curve3d> divideCurves = new List<Curve3d>();
            switch (curve)
            {
                case LineSegment3d line:
                {
                    List<Point3d> dividePoints = GetDividePoints(line, numberOfDividePoints);
                    for (int i = 0; i < dividePoints.Count - 1; i++)
                    {
                        Point3d startPoint = dividePoints[i];
                        Point3d endPoint = dividePoints[i + 1];
                        LineSegment3d lineItem = new LineSegment3d(startPoint, endPoint);
                        divideCurves.Add(lineItem);
                    }
                        
                    return divideCurves;
                }
                case CircularArc3d arc:
                {
                    if (numberOfDivideCurves < 1)
                    {
                        divideCurves.Add(arc);
                        return divideCurves;
                    }
                    double startParam = arc.StartAngle;
                    double endParam = arc.EndAngle;
                    double sweep = endParam - startParam;
                    if (sweep < 0)
                    {
                        sweep += Math.PI * 2;
                    }
                    for (int i = 0; i < numberOfDivideCurves; i++)
                    {
                        double paramStart = startParam + sweep * i / numberOfDivideCurves;
                        double paramEnd = startParam + sweep * (i + 1) / numberOfDivideCurves;
                        CircularArc3d arcItem = new CircularArc3d(arc.Center, arc.Normal, arc.ReferenceVector, arc.Radius, paramStart, paramEnd);
                        divideCurves.Add(arcItem);
                    }
                    return divideCurves;
                }
                default:
                {
                    return divideCurves;
                }
            }
        }
    }
}