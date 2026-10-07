using System.Collections.Generic;
using Autodesk.AutoCAD.DatabaseServices;

namespace CadBridgeDesigner.Tool.Tool.CadTool.EntityTool
{
    /// <summary>
    /// 三维实体工具类，提供创建和操作三维实体的方法
    /// </summary>
    public static class SolidTool
    {
        /// <summary>
        /// 将截面拉伸为三维实体
        /// </summary>
        /// <param name="section">要拉伸的面域截面对象</param>
        /// <param name="height">拉伸的总高度</param>
        /// <param name="twoSide">是否双向拉伸（true表示沿法线正反方向各拉伸一半，false表示仅沿法线方向拉伸）</param>
        /// <returns>拉伸后的三维实体对象，失败则返回null</returns>
        public static Solid3d ExtrudeSection(Region section, double height, bool twoSide)
        {
            Solid3d solid = new Solid3d();
            solid.CreateExtrudedSolid(section, section.Normal * height/2, new SweepOptions());
            if (twoSide)
            {
                Solid3d solid2 = new Solid3d();
                solid2.CreateExtrudedSolid(section, -section.Normal * height/2, new SweepOptions());
                solid.BooleanOperation(BooleanOperationType.BoolUnite, solid2);
            }
            return solid;
        }

        public static Solid3d LoftBySections(Polyline section1, Polyline section2, List<Line> guideLines)
        {
            Entity[] sectionsEntities = new Entity[2];
            sectionsEntities[0] = section1.TranslateToRegion();
            sectionsEntities[1] = section2.TranslateToRegion();
            Entity[] guideLinesEntities = new Entity[guideLines.Count];
            for (int i = 0; i < guideLines.Count; i++)
            {
                guideLinesEntities[i] = guideLines[i];
            }
            
            LoftOptionsBuilder loftOptionsBuilder = new LoftOptionsBuilder
            {
                AlignDirection = true,
                NoTwist = true,
                NormalOption = LoftOptionsNormalOption.AllNormal
            };
            LoftOptions loftOptions = loftOptionsBuilder.ToLoftOptions();
                    
            Solid3d solid = new Solid3d();
            solid.CreateLoftedSolid(sectionsEntities, guideLinesEntities, null, loftOptions);
            return solid;
        }
        
        public static Solid3d SubtractSolid(Solid3d solid1, Solid3d solid2)
        {
            solid1.BooleanOperation(BooleanOperationType.BoolSubtract, solid2.Clone() as Solid3d);
            return solid1;
        }
    }
}