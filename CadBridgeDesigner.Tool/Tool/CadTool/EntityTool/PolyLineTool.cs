using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace CadBridgeDesigner.Tool.Tool.CadTool.EntityTool
{
    
    public static class PolyLineTool
    {
        /// <summary>
        /// 将闭合曲线转换为面域对象
        /// </summary>
        /// <param name="polyline">要转换的闭合曲线对象（如闭合多段线、圆、椭圆等）</param>
        /// <returns>转换后的面域对象，如果转换失败则返回null</returns>
        public static Region TranslateToRegion(this Polyline polyline)
        {
            DBObjectCollection curves = new DBObjectCollection() { polyline };
            DBObjectCollection regionCollection = Region.CreateFromCurves(curves);
            if (regionCollection.Count == 0)
            {
                return null;
            }
            Region region = regionCollection[0] as Region;
            return region;
        }
        /// <summary>
        /// 将闭合曲线转换为 hatch 对象
        /// </summary>
        /// <param name="polyline">要转换的闭合曲线对象（如闭合多段线、圆、椭圆等）</param>
        /// <returns>转换后的 hatch 对象</returns>
        public static Hatch TranslateToHatch(this Polyline polyline)
        {
            Hatch hatch = new Hatch();
            hatch.SetHatchPattern(HatchPatternType.PreDefined, "ANSI31");
                    
            Point2dCollection hatchPoints = new Point2dCollection();
            DoubleCollection bulges = new DoubleCollection();
                    
            for (int i = 0; i < polyline.NumberOfVertices; i++)
            {
                hatchPoints.Add(polyline.GetPoint2dAt(i));
                bulges.Add(0);
            }
                    
            hatchPoints.Add(polyline.GetPoint2dAt(0));
            bulges.Add(0); 
                    
            hatch.AppendLoop(HatchLoopTypes.Outermost, hatchPoints, bulges);
            hatch.EvaluateHatch(true);
            return hatch;
        }
    }
}