using System;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace CadBridgeDesigner.Tool.Tool.CadTool.EntityTool
{
    public static class DimensionTool
    {
        public static DBObjectCollection GetDimensionGroup(Point3dCollection point3Ds, double rotationAngle, double distance)
        {
            DBObjectCollection dimensions = new DBObjectCollection();
            rotationAngle = Math.PI / 2 - rotationAngle;
            double dimLinePointX = point3Ds[0].X + Math.Sin(rotationAngle) * distance;
            double dimLinePointY = point3Ds[0].Y + Math.Cos(rotationAngle) * distance;
            Point3d dimLinePoint = new Point3d(dimLinePointX, dimLinePointY, 0);
            
            for(int i = 0; i < point3Ds.Count - 1; i++)
            {
                RotatedDimension rotatedDimension = new RotatedDimension();
                rotatedDimension.Rotation = rotationAngle;
                rotatedDimension.XLine1Point = point3Ds[i];
                rotatedDimension.XLine2Point = point3Ds[i + 1];
                rotatedDimension.DimLinePoint = dimLinePoint;
                dimensions.Add(rotatedDimension);
            }
            return dimensions;
        }
    }
}


