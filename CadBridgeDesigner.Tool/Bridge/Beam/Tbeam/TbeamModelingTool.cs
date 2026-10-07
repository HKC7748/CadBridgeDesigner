using System;
using System.Collections.Generic;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using CadBridgeDesigner.Tool.Bridge.Base;
using CadBridgeDesigner.Tool.Tool.CadTool.EntityTool;

namespace CadBridgeDesigner.Tool.Bridge.Beam.Tbeam
{
    public static class TbeamModelingTool
    {
        public static DBObjectCollection GetTbeamSolids(this TbeamModel tbeamModel)
        {
            DBObjectCollection tbeamEntities = new DBObjectCollection();
            double beamEndGap = tbeamModel.BeamEndGap;
            double spanLength = tbeamModel.SpanLength;
            double height = tbeamModel.Height;
            int numberOfTbeam = tbeamModel.NumberOfTbeam;
            
            double widthOfFlange = tbeamModel.WidthOfFlange;
            double heightOfFlange = tbeamModel.HeightOfFlange;
            double widthOfFlangeHaunch = tbeamModel.WidthOfFlangeHaunch;
            double heightOfFlangeHaunch = tbeamModel.HeightOfFlangeHaunch;
            
            double widthOfWeb = tbeamModel.WidthOfWeb;
            
            double widthOfButtonBulb = tbeamModel.WidthOfButtonBulb;
            double heightOfButtonBulb = tbeamModel.HeightOfButtonBulb;
            double heightOfButtonBulbSlope = tbeamModel.HeightOfButtonBulbSlope;
            double distanceOfStartOfCrossSection = tbeamModel.DistanceOfStartOfCrossSection;
            double distanceOfEndOfCrossSection = tbeamModel.DistanceOfEndOfCrossSection;
    
            double spaceOfDiaphragm = tbeamModel.SpaceOfDiaphragm;
            double thicknessOfDiaphragm = tbeamModel.ThicknessOfDiaphragm;
            int numberOfMiddleDiaphragm = tbeamModel.NumberOfMiddleDiaphragm;
            double distanceOfEndDiaphragm = tbeamModel.DistanceOfEndDiaphragm;
            
            double widthOfWetJoint = tbeamModel.WidthOfWetJoint;
            
            //顶部湿接缝
            for (int i = 0; i < numberOfTbeam - 1; i++)
            {
                for (int j = 0; j < numberOfMiddleDiaphragm; j++)
                {
                    Solid3d solid = new Solid3d();
                    solid.CreateBox(widthOfWetJoint, heightOfFlange, spanLength - beamEndGap * 2);
                    double x = widthOfFlange * (numberOfTbeam - 2) / 2 - widthOfFlange * i;
                    double y = height - heightOfFlange / 2;
                    ModifyTool.MoveByVector(solid, new Vector3d(x, y, 0));
                    tbeamEntities.Add(solid);
                }
            }
            
            // 梁体
            // 支点段
            for (int i = 0; i < numberOfTbeam; i++)
            {
                Polyline section = new Polyline();
                section.AddVertexAt(section.NumberOfVertices, new Point2d(widthOfFlange / 2, height), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(widthOfFlange / 2, height - heightOfFlange), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(widthOfWeb / 2 + widthOfFlangeHaunch, height - heightOfFlange), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, height - heightOfFlange - (heightOfFlangeHaunch - (widthOfButtonBulb - widthOfWeb) / 2 * heightOfFlangeHaunch / widthOfFlangeHaunch)), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, heightOfButtonBulb + 0.0000001), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, heightOfButtonBulb), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, 0), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(-widthOfButtonBulb / 2, 0), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(-widthOfButtonBulb / 2, heightOfButtonBulb), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(-widthOfButtonBulb / 2, heightOfButtonBulb + 0.0000001), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(-widthOfButtonBulb / 2, height - heightOfFlange - (heightOfFlangeHaunch - (widthOfButtonBulb - widthOfWeb) / 2 * heightOfFlangeHaunch / widthOfFlangeHaunch)), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(-widthOfWeb / 2 - widthOfFlangeHaunch, height - heightOfFlange), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(-widthOfFlange / 2, height - heightOfFlange), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(-widthOfFlange / 2, height), 0, 0, 0);

                section.Closed = true;
                Solid3d solid = SolidTool.ExtrudeSection(section.TranslateToRegion(), distanceOfStartOfCrossSection, true);
                double x = widthOfFlange * (numberOfTbeam - 1) / 2 - widthOfFlange * i;
                double z = spanLength / 2 - beamEndGap - distanceOfStartOfCrossSection / 2;
                ModifyTool.MoveByVector(solid, new Vector3d(x, 0, z));
                foreach (Entity entity in tbeamEntities)
                {
                    solid = SolidTool.SubtractSolid(solid, entity as Solid3d);
                }
                Solid3d solidClone = solid.Clone() as Solid3d;
                ModifyTool.MirrorEntityByPlane(solidClone, new Plane(new Point3d(0, 0, 0), Vector3d.ZAxis));
                tbeamEntities.Add(solid);
                tbeamEntities.Add(solidClone);
            }
            // 支点-跨中过渡段
            for (int i = 0; i < numberOfTbeam; i++)
            {
                Polyline startSection = new Polyline();
                startSection.AddVertexAt(startSection.NumberOfVertices, new Point2d(widthOfFlange / 2, height), 0, 0, 0);
                startSection.AddVertexAt(startSection.NumberOfVertices, new Point2d(widthOfFlange / 2, height - heightOfFlange), 0, 0, 0);
                startSection.AddVertexAt(startSection.NumberOfVertices, new Point2d(widthOfWeb / 2 + widthOfFlangeHaunch, height - heightOfFlange), 0, 0, 0);
                startSection.AddVertexAt(startSection.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, height - heightOfFlange - (heightOfFlangeHaunch - (widthOfButtonBulb - widthOfWeb) / 2 * heightOfFlangeHaunch / widthOfFlangeHaunch)), 0, 0, 0);
                startSection.AddVertexAt(startSection.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, heightOfButtonBulb + 0.0000001), 0, 0, 0);
                startSection.AddVertexAt(startSection.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, heightOfButtonBulb), 0, 0, 0);
                startSection.AddVertexAt(startSection.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, 0), 0, 0, 0);
                startSection.AddVertexAt(startSection.NumberOfVertices, new Point2d(-widthOfButtonBulb / 2, 0), 0, 0, 0);
                startSection.AddVertexAt(startSection.NumberOfVertices, new Point2d(-widthOfButtonBulb / 2, heightOfButtonBulb), 0, 0, 0);
                startSection.AddVertexAt(startSection.NumberOfVertices, new Point2d(-widthOfButtonBulb / 2, heightOfButtonBulb + 0.0000001), 0, 0, 0);
                startSection.AddVertexAt(startSection.NumberOfVertices, new Point2d(-widthOfButtonBulb / 2, height - heightOfFlange - (heightOfFlangeHaunch - (widthOfButtonBulb - widthOfWeb) / 2 * heightOfFlangeHaunch / widthOfFlangeHaunch)), 0, 0, 0);
                startSection.AddVertexAt(startSection.NumberOfVertices, new Point2d(-widthOfWeb / 2 - widthOfFlangeHaunch, height - heightOfFlange), 0, 0, 0);
                startSection.AddVertexAt(startSection.NumberOfVertices, new Point2d(-widthOfFlange / 2, height - heightOfFlange), 0, 0, 0);
                startSection.AddVertexAt(startSection.NumberOfVertices, new Point2d(-widthOfFlange / 2, height), 0, 0, 0);
                startSection.Closed = true;
                
                Polyline endSection = new Polyline();
                endSection.AddVertexAt(endSection.NumberOfVertices, new Point2d(widthOfFlange / 2, height), 0, 0, 0);
                endSection.AddVertexAt(endSection.NumberOfVertices, new Point2d(widthOfFlange / 2, height - heightOfFlange), 0, 0, 0);
                endSection.AddVertexAt(endSection.NumberOfVertices, new Point2d(widthOfWeb / 2 + widthOfFlangeHaunch, height - heightOfFlange), 0, 0, 0);
                endSection.AddVertexAt(endSection.NumberOfVertices, new Point2d(widthOfWeb / 2, height - heightOfFlange - heightOfFlangeHaunch), 0, 0, 0);
                endSection.AddVertexAt(endSection.NumberOfVertices, new Point2d(widthOfWeb / 2, heightOfButtonBulb + heightOfButtonBulbSlope), 0, 0, 0);
                endSection.AddVertexAt(endSection.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, heightOfButtonBulb), 0, 0, 0);
                endSection.AddVertexAt(endSection.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, 0), 0, 0, 0);
                endSection.AddVertexAt(endSection.NumberOfVertices, new Point2d(-widthOfButtonBulb / 2, 0), 0, 0, 0);
                endSection.AddVertexAt(endSection.NumberOfVertices, new Point2d(-widthOfButtonBulb / 2, heightOfButtonBulb), 0, 0, 0);
                endSection.AddVertexAt(endSection.NumberOfVertices, new Point2d(-widthOfWeb / 2, heightOfButtonBulb + heightOfButtonBulbSlope), 0, 0, 0);
                endSection.AddVertexAt(endSection.NumberOfVertices, new Point2d(-widthOfWeb / 2, height - heightOfFlange - heightOfFlangeHaunch), 0, 0, 0);
                endSection.AddVertexAt(endSection.NumberOfVertices, new Point2d(-widthOfWeb / 2 - widthOfFlangeHaunch, height - heightOfFlange), 0, 0, 0);
                endSection.AddVertexAt(endSection.NumberOfVertices, new Point2d(-widthOfFlange / 2, height - heightOfFlange), 0, 0, 0);
                endSection.AddVertexAt(endSection.NumberOfVertices, new Point2d(-widthOfFlange / 2, height), 0, 0, 0);
                endSection.Closed = true;
                double x = widthOfFlange * (numberOfTbeam - 1) / 2 - widthOfFlange * i;
                double startZ = spanLength / 2 - beamEndGap - distanceOfStartOfCrossSection;
                double endZ = spanLength / 2 - beamEndGap - distanceOfEndOfCrossSection;
                ModifyTool.MoveByVector(startSection, new Vector3d(x, 0, startZ));
                ModifyTool.MoveByVector(endSection, new Vector3d(x, 0, endZ));
                
                List<Line> guideLines = new List<Line>();
                for (int k = 0; k < startSection.NumberOfVertices; k++)
                {
                    Line line = new Line(startSection.GetPoint3dAt(k), endSection.GetPoint3dAt(k));
                    guideLines.Add(line);
                }
                
                Solid3d solid = SolidTool.LoftBySections(startSection, endSection, guideLines);
                foreach (Entity entity in tbeamEntities)
                {
                    solid = SolidTool.SubtractSolid(solid, entity as Solid3d);
                }
                Solid3d solidClone = solid.Clone() as Solid3d;
                ModifyTool.MirrorEntityByPlane(solidClone, new Plane(new Point3d(0, 0, 0), Vector3d.ZAxis));
                tbeamEntities.Add(solid);
                tbeamEntities.Add(solidClone);
            }
            // 跨中段
            for (int i = 0; i < numberOfTbeam; i++)
            { 
                Polyline section = new Polyline();
                section.AddVertexAt(section.NumberOfVertices, new Point2d(widthOfFlange / 2, height), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(widthOfFlange / 2, height - heightOfFlange), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(widthOfWeb / 2 + widthOfFlangeHaunch, height - heightOfFlange), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(widthOfWeb / 2, height - heightOfFlange - heightOfFlangeHaunch), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(widthOfWeb / 2, heightOfButtonBulb + heightOfButtonBulbSlope), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, heightOfButtonBulb), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, 0), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(-widthOfButtonBulb / 2, 0), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(-widthOfButtonBulb / 2, heightOfButtonBulb), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(-widthOfWeb / 2, heightOfButtonBulb + heightOfButtonBulbSlope), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(-widthOfWeb / 2, height - heightOfFlange - heightOfFlangeHaunch), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(-widthOfWeb / 2 - widthOfFlangeHaunch, height - heightOfFlange), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(-widthOfFlange / 2, height - heightOfFlange), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(-widthOfFlange / 2, height), 0, 0, 0);
                section.Closed = true;
                double x = widthOfFlange * (numberOfTbeam - 1) / 2 - widthOfFlange * i;
                Solid3d solid = SolidTool.ExtrudeSection(section.TranslateToRegion(), spanLength - beamEndGap - distanceOfEndOfCrossSection * 2, true);
                ModifyTool.MoveByVector(solid, new Vector3d(x, 0, 0));
                foreach (Entity entity in tbeamEntities)
                {
                    solid = SolidTool.SubtractSolid(solid, entity as Solid3d);
                }
                tbeamEntities.Add(solid);
            }
            
            //横隔板
            for (int i = 0; i < numberOfTbeam - 1; i++)
            {
                //中横隔板
                for (int j = 0; j < numberOfMiddleDiaphragm; j++)
                {
                    Polyline section = new Polyline();
                    section.AddVertexAt(section.NumberOfVertices, new Point2d(0, heightOfButtonBulb), 0, 0, 0);
                    section.AddVertexAt(section.NumberOfVertices, new Point2d(0, height - heightOfFlange), 0, 0, 0);
                    section.AddVertexAt(section.NumberOfVertices, new Point2d((widthOfFlange - widthOfWetJoint) / 2, height - heightOfFlange), 0, 0, 0);
                    section.AddVertexAt(section.NumberOfVertices, new Point2d((widthOfFlange - widthOfWetJoint) / 2, heightOfButtonBulb), 0, 0, 0);
                    section.Closed = true;
                    Solid3d solid = SolidTool.ExtrudeSection(section.TranslateToRegion(), thicknessOfDiaphragm, true);
                    double x = widthOfFlange * (numberOfTbeam - 3) / 2 - widthOfFlange * i;
                    double z = spaceOfDiaphragm * (numberOfMiddleDiaphragm - 1) / 2 - spaceOfDiaphragm * j;
                    ModifyTool.MoveByVector(solid, new Vector3d(x, 0, z));
                    foreach (Entity entity in tbeamEntities)
                    {
                        solid = SolidTool.SubtractSolid(solid, entity as Solid3d);
                    }
                    Solid3d solidClone = solid.Clone() as Solid3d;
                    ModifyTool.MirrorEntityByPlane(solidClone, new Plane(new Point3d(0, 0, 0), Vector3d.XAxis));
                    tbeamEntities.Add(solid);
                    tbeamEntities.Add(solidClone);
                }
                
                //端横隔板
                for (int j = 0; j < 1; j++)
                {
                    Polyline section = new Polyline();
                    section.AddVertexAt(section.NumberOfVertices, new Point2d(0, heightOfButtonBulb), 0, 0, 0);
                    section.AddVertexAt(section.NumberOfVertices, new Point2d(0, height - heightOfFlange), 0, 0, 0);
                    section.AddVertexAt(section.NumberOfVertices, new Point2d((widthOfFlange - widthOfWetJoint) / 2, height - heightOfFlange), 0, 0, 0);
                    section.AddVertexAt(section.NumberOfVertices, new Point2d((widthOfFlange - widthOfWetJoint) / 2, heightOfButtonBulb), 0, 0, 0);
                    section.Closed = true;
                    Solid3d solid = SolidTool.ExtrudeSection(section.TranslateToRegion(), thicknessOfDiaphragm, true);
                    double x = widthOfFlange * (numberOfTbeam - 3) / 2 - widthOfFlange * i;
                    double z = spanLength / 2 - distanceOfEndDiaphragm;
                    ModifyTool.MoveByVector(solid, new Vector3d(x, 0, z));
                    foreach (Entity entity in tbeamEntities)
                    {
                        solid = SolidTool.SubtractSolid(solid, entity as Solid3d);
                    }
                    Solid3d solidClone = solid.Clone() as Solid3d;
                    ModifyTool.MirrorEntityByPlane(solidClone, new Plane(new Point3d(0, 0, 0), Vector3d.XAxis));
                    
                    Solid3d solid2 = solid.Clone() as Solid3d;
                    Solid3d solidClone2 = solidClone?.Clone() as Solid3d; 
                    ModifyTool.MirrorEntityByPlane(solid2, new Plane(new Point3d(0, 0, 0), Vector3d.ZAxis));
                    ModifyTool.MirrorEntityByPlane(solidClone2, new Plane(new Point3d(0, 0, 0), Vector3d.ZAxis));
                    
                    tbeamEntities.Add(solid);
                    tbeamEntities.Add(solidClone);
                    tbeamEntities.Add(solid2);
                    tbeamEntities.Add(solidClone2);
                }
            }
            
            //横隔板湿接缝
            for (int i = 0; i < numberOfTbeam - 1; i++)
            {
                //中横隔板湿接缝
                for (int j = 0; j < numberOfMiddleDiaphragm; j++)
                {
                    Solid3d solid = new Solid3d();
                    solid.CreateBox(widthOfWetJoint, height - heightOfFlange - heightOfButtonBulb, thicknessOfDiaphragm);
                    double x = widthOfFlange * (numberOfTbeam - 2) / 2 - widthOfFlange * i;
                    double y = (height - heightOfFlange - heightOfButtonBulb) / 2 + heightOfButtonBulb;
                    double z = spaceOfDiaphragm * (numberOfMiddleDiaphragm - 1) / 2 - spaceOfDiaphragm * j;
                    ModifyTool.MoveByVector(solid, new Vector3d(x, y, z));
                    tbeamEntities.Add(solid);
                }
                
                //端横隔板湿接缝
                for (int j = 0; j < 1; j++)
                {
                    Solid3d solid = new Solid3d();
                    solid.CreateBox(widthOfWetJoint, height - heightOfFlange - heightOfButtonBulb, thicknessOfDiaphragm);
                    double x = widthOfFlange * (numberOfTbeam - 2) / 2 - widthOfFlange * i;
                    double y = (height - heightOfFlange - heightOfButtonBulb) / 2 + heightOfButtonBulb;
                    double z = spanLength / 2 - distanceOfEndDiaphragm;
                    ModifyTool.MoveByVector(solid, new Vector3d(x, y, z));
                    Solid3d solidClone = solid.Clone() as Solid3d;
                    ModifyTool.MirrorEntityByPlane(solidClone, new Plane(new Point3d(0, 0, 0), Vector3d.ZAxis));
                    tbeamEntities.Add(solidClone);
                    tbeamEntities.Add(solid);
                }
            }
            
            

            foreach (Entity entity in tbeamEntities)
            {
                ModifyTool.RotateEntityByAngle(entity, Point3d.Origin, Vector3d.XAxis, Math.PI / 2);
            }
            
            return tbeamEntities;
        }

        public static BridgeComponentDiagram GetTbeamPointEdgeSectionDiagrams(this TbeamModel tbeamModel)
        {
            BridgeComponentDiagram bridgeComponentDiagram = new BridgeComponentDiagram();
            
            double height = tbeamModel.Height;
            
            double widthOfFlange = tbeamModel.WidthOfFlange;
            double heightOfFlange = tbeamModel.HeightOfFlange;
            double widthOfFlangeHaunch = tbeamModel.WidthOfFlangeHaunch;
            double heightOfFlangeHaunch = tbeamModel.HeightOfFlangeHaunch;
            
            double widthOfWeb = tbeamModel.WidthOfWeb;
            
            double widthOfButtonBulb = tbeamModel.WidthOfButtonBulb;
            double heightOfButtonBulb = tbeamModel.HeightOfButtonBulb;

            double widthOfWetJoint = tbeamModel.WidthOfWetJoint;
            
            //中心线
            Polyline centerLine = new Polyline();
            centerLine.AddVertexAt(centerLine.NumberOfVertices, new Point2d(0, -25), 0, 0, 0);
            centerLine.AddVertexAt(centerLine.NumberOfVertices, new Point2d(0, height + 25), 0, 0, 0);
            bridgeComponentDiagram.CenterCurves.Add(centerLine);
            
            //梁体多段线
            Polyline pointEdgeSection = new Polyline();
            pointEdgeSection.AddVertexAt(pointEdgeSection.NumberOfVertices, new Point2d(widthOfFlange / 2, height), 0, 0, 0);
            pointEdgeSection.AddVertexAt(pointEdgeSection.NumberOfVertices, new Point2d(widthOfFlange / 2, height - heightOfFlange), 0, 0, 0);
            pointEdgeSection.AddVertexAt(pointEdgeSection.NumberOfVertices, new Point2d(widthOfWeb / 2 + widthOfFlangeHaunch, height - heightOfFlange), 0, 0, 0);
            pointEdgeSection.AddVertexAt(pointEdgeSection.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, height - heightOfFlange - (heightOfFlangeHaunch - (widthOfButtonBulb - widthOfWeb) / 2 * heightOfFlangeHaunch / widthOfFlangeHaunch)), 0, 0, 0);
            pointEdgeSection.AddVertexAt(pointEdgeSection.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, heightOfButtonBulb), 0, 0, 0);
            pointEdgeSection.AddVertexAt(pointEdgeSection.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, 0), 0, 0, 0);
            pointEdgeSection.AddVertexAt(pointEdgeSection.NumberOfVertices, new Point2d(-widthOfButtonBulb / 2, 0), 0, 0, 0);
            pointEdgeSection.AddVertexAt(pointEdgeSection.NumberOfVertices, new Point2d(-widthOfButtonBulb / 2, heightOfButtonBulb), 0, 0, 0);
            pointEdgeSection.AddVertexAt(pointEdgeSection.NumberOfVertices, new Point2d(-widthOfButtonBulb / 2, height - heightOfFlange - (heightOfFlangeHaunch - (widthOfButtonBulb - widthOfWeb) / 2 * heightOfFlangeHaunch / widthOfFlangeHaunch)), 0, 0, 0);
            pointEdgeSection.AddVertexAt(pointEdgeSection.NumberOfVertices, new Point2d(-widthOfWeb / 2 - widthOfFlangeHaunch, height - heightOfFlange), 0, 0, 0);
            pointEdgeSection.AddVertexAt(pointEdgeSection.NumberOfVertices, new Point2d(-widthOfFlange / 2, height - heightOfFlange), 0, 0, 0);
            pointEdgeSection.AddVertexAt(pointEdgeSection.NumberOfVertices, new Point2d(-widthOfFlange / 2, height), 0, 0, 0);
            pointEdgeSection.Closed = true;
            bridgeComponentDiagram.StructureCurves.Add(pointEdgeSection);
            
            //右侧横隔板多段线
            Polyline rightDiaphragm = new Polyline();
            rightDiaphragm.AddVertexAt(rightDiaphragm.NumberOfVertices, new Point2d(widthOfFlange / 2 - widthOfWetJoint / 2, height - heightOfFlange), 0, 0, 0);
            rightDiaphragm.AddVertexAt(rightDiaphragm.NumberOfVertices, new Point2d(widthOfWeb / 2 + widthOfFlangeHaunch, height - heightOfFlange), 0, 0, 0);
            rightDiaphragm.AddVertexAt(rightDiaphragm.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, height - heightOfFlange - (heightOfFlangeHaunch - (widthOfButtonBulb - widthOfWeb) / 2 * heightOfFlangeHaunch / widthOfFlangeHaunch)), 0, 0, 0);
            rightDiaphragm.AddVertexAt(rightDiaphragm.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, heightOfButtonBulb), 0, 0, 0);
            rightDiaphragm.AddVertexAt(rightDiaphragm.NumberOfVertices, new Point2d(widthOfFlange / 2 - widthOfWetJoint / 2, heightOfButtonBulb), 0, 0, 0);
            rightDiaphragm.Closed = true;
            bridgeComponentDiagram.StructureCurves.Add(rightDiaphragm);
            
            //右侧湿接缝多段线
            Polyline rightWetJoint = new Polyline();
            rightWetJoint.AddVertexAt(rightWetJoint.NumberOfVertices, new Point2d(widthOfFlange / 2, height), 0, 0, 0);
            rightWetJoint.AddVertexAt(rightWetJoint.NumberOfVertices, new Point2d(widthOfFlange / 2, heightOfButtonBulb), 0, 0, 0);
            rightWetJoint.AddVertexAt(rightWetJoint.NumberOfVertices, new Point2d(widthOfFlange / 2 - widthOfWetJoint / 2, heightOfButtonBulb), 0, 0, 0);
            rightWetJoint.AddVertexAt(rightWetJoint.NumberOfVertices, new Point2d(widthOfFlange / 2 - widthOfWetJoint / 2, height), 0, 0, 0);
            rightWetJoint.Closed = true;
            bridgeComponentDiagram.HatchCurves.Add(rightWetJoint);
            
            //上方一级标注
            Point3dCollection topFirstPoints = new Point3dCollection()
            {
                pointEdgeSection.GetPoint3dAt(11),
                new Point3d(0, height, 0),
                new Point3d(pointEdgeSection.GetPoint3dAt(0).X - widthOfWetJoint / 2, height, 0),
                pointEdgeSection.GetPoint3dAt(0)
            };
            DBObjectCollection topFirstDimension = DimensionTool.GetDimensionGroup(topFirstPoints, Math.PI / 2, 25);
            foreach (RotatedDimension dimension in topFirstDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            //上方二级标注
            Point3dCollection topSecondPoints = new Point3dCollection()
            {
                pointEdgeSection.GetPoint3dAt(11),
                pointEdgeSection.GetPoint3dAt(0)
            };
            DBObjectCollection topSecondDimension = DimensionTool.GetDimensionGroup(topSecondPoints, Math.PI / 2, 50);
            foreach (RotatedDimension dimension in topSecondDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            //左侧一级标注
            Point3dCollection leftFirstPoints = new Point3dCollection()
            {
                pointEdgeSection.GetPoint3dAt(11),
                pointEdgeSection.GetPoint3dAt(10),
                pointEdgeSection.GetPoint3dAt(6)
            };
            DBObjectCollection leftFirstDimension = DimensionTool.GetDimensionGroup(leftFirstPoints, Math.PI, 25);
            foreach (RotatedDimension dimension in leftFirstDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            //左侧二级标注
            Point3dCollection leftSecondPoints = new Point3dCollection()
            {
                pointEdgeSection.GetPoint3dAt(11),
                pointEdgeSection.GetPoint3dAt(6)
            };
            DBObjectCollection leftSecondDimension = DimensionTool.GetDimensionGroup(leftSecondPoints, Math.PI, 50);
            foreach (RotatedDimension dimension in leftSecondDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            //下方一级标注
            Point3dCollection bottomFirstPoints = new Point3dCollection()
            {
                new Point3d(pointEdgeSection.GetPoint3dAt(11).X, 0, 0),
                pointEdgeSection.GetPoint3dAt(6),
                pointEdgeSection.GetPoint3dAt(5),
                new Point3d(pointEdgeSection.GetPoint3dAt(0).X - widthOfWetJoint / 2, 0, 0),
                pointEdgeSection.GetPoint3dAt(0)
            };
            DBObjectCollection bottomFirstDimension = DimensionTool.GetDimensionGroup(bottomFirstPoints, Math.PI * 3 / 2 , 25);
            foreach (RotatedDimension dimension in bottomFirstDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            //右方一级标注
            Point3dCollection rightFirstPoints = new Point3dCollection()
            {
                pointEdgeSection.GetPoint3dAt(0),
                pointEdgeSection.GetPoint3dAt(1),
                pointEdgeSection.GetPoint3dAt(4),
                pointEdgeSection.GetPoint3dAt(5)
            };
            DBObjectCollection rightFirstDimension = DimensionTool.GetDimensionGroup(rightFirstPoints, 0, 25);
            foreach (RotatedDimension dimension in rightFirstDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            //右方二级标注
            Point3dCollection rightSecondPoints = new Point3dCollection()
            {
                pointEdgeSection.GetPoint3dAt(0),
                pointEdgeSection.GetPoint3dAt(5)
            };
            DBObjectCollection rightSecondDimension = DimensionTool.GetDimensionGroup(rightSecondPoints, 0, 50);
            foreach (RotatedDimension dimension in rightSecondDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            //内部标注
            Point3dCollection internalPoints = new Point3dCollection()
            {
                pointEdgeSection.GetPoint3dAt(10),
                pointEdgeSection.GetPoint3dAt(9),
                pointEdgeSection.GetPoint3dAt(8),
                pointEdgeSection.GetPoint3dAt(3),
                pointEdgeSection.GetPoint3dAt(2),
                new Point3d(pointEdgeSection.GetPoint3dAt(1).X - widthOfWetJoint / 2, pointEdgeSection.GetPoint3dAt(1).Y, 0)
            };
            DBObjectCollection internalDimension = DimensionTool.GetDimensionGroup(internalPoints, Math.PI * 3 / 2, 50);
            foreach (RotatedDimension dimension in internalDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            //左侧承托标注
            Point3dCollection leftHaunchPoints = new Point3dCollection()
            {
                pointEdgeSection.GetPoint3dAt(9),
                pointEdgeSection.GetPoint3dAt(8)
            };
            DBObjectCollection leftHaunchDimension = DimensionTool.GetDimensionGroup(leftHaunchPoints, Math.PI, 0);
            bridgeComponentDiagram.Dimensions.Add(leftHaunchDimension[0]);
            
            //右侧承托标注
            Point3dCollection rightHaunchPoints = new Point3dCollection()
            {
                pointEdgeSection.GetPoint3dAt(2),
                pointEdgeSection.GetPoint3dAt(3)
            };
            DBObjectCollection rightHaunchDimension = DimensionTool.GetDimensionGroup(rightHaunchPoints, 0, 0);
            bridgeComponentDiagram.Dimensions.Add(rightHaunchDimension[0]);
            
            return bridgeComponentDiagram;
        }
        
        public static BridgeComponentDiagram GetTbeamPointMiddleSectionDiagrams(this TbeamModel tbeamModel)
        {
            BridgeComponentDiagram bridgeComponentDiagram = new BridgeComponentDiagram();
            
            double height = tbeamModel.Height;
            
            double widthOfFlange = tbeamModel.WidthOfFlange;
            double heightOfFlange = tbeamModel.HeightOfFlange;
            double widthOfFlangeHaunch = tbeamModel.WidthOfFlangeHaunch;
            double heightOfFlangeHaunch = tbeamModel.HeightOfFlangeHaunch;
            
            double widthOfWeb = tbeamModel.WidthOfWeb;
            
            double widthOfButtonBulb = tbeamModel.WidthOfButtonBulb;
            double heightOfButtonBulb = tbeamModel.HeightOfButtonBulb;

            double widthOfWetJoint = tbeamModel.WidthOfWetJoint;
            
            //中心线
            Polyline centerLine = new Polyline();
            centerLine.AddVertexAt(centerLine.NumberOfVertices, new Point2d(0, -25), 0, 0, 0);
            centerLine.AddVertexAt(centerLine.NumberOfVertices, new Point2d(0, height + 25), 0, 0, 0);
            bridgeComponentDiagram.CenterCurves.Add(centerLine);
            
            //梁体多段线
            Polyline pointEdgeSection = new Polyline();
            pointEdgeSection.AddVertexAt(pointEdgeSection.NumberOfVertices, new Point2d(widthOfFlange / 2, height), 0, 0, 0);
            pointEdgeSection.AddVertexAt(pointEdgeSection.NumberOfVertices, new Point2d(widthOfFlange / 2, height - heightOfFlange), 0, 0, 0);
            pointEdgeSection.AddVertexAt(pointEdgeSection.NumberOfVertices, new Point2d(widthOfWeb / 2 + widthOfFlangeHaunch, height - heightOfFlange), 0, 0, 0);
            pointEdgeSection.AddVertexAt(pointEdgeSection.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, height - heightOfFlange - (heightOfFlangeHaunch - (widthOfButtonBulb - widthOfWeb) / 2 * heightOfFlangeHaunch / widthOfFlangeHaunch)), 0, 0, 0);
            pointEdgeSection.AddVertexAt(pointEdgeSection.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, heightOfButtonBulb), 0, 0, 0);
            pointEdgeSection.AddVertexAt(pointEdgeSection.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, 0), 0, 0, 0);
            pointEdgeSection.AddVertexAt(pointEdgeSection.NumberOfVertices, new Point2d(-widthOfButtonBulb / 2, 0), 0, 0, 0);
            pointEdgeSection.AddVertexAt(pointEdgeSection.NumberOfVertices, new Point2d(-widthOfButtonBulb / 2, heightOfButtonBulb), 0, 0, 0);
            pointEdgeSection.AddVertexAt(pointEdgeSection.NumberOfVertices, new Point2d(-widthOfButtonBulb / 2, height - heightOfFlange - (heightOfFlangeHaunch - (widthOfButtonBulb - widthOfWeb) / 2 * heightOfFlangeHaunch / widthOfFlangeHaunch)), 0, 0, 0);
            pointEdgeSection.AddVertexAt(pointEdgeSection.NumberOfVertices, new Point2d(-widthOfWeb / 2 - widthOfFlangeHaunch, height - heightOfFlange), 0, 0, 0);
            pointEdgeSection.AddVertexAt(pointEdgeSection.NumberOfVertices, new Point2d(-widthOfFlange / 2, height - heightOfFlange), 0, 0, 0);
            pointEdgeSection.AddVertexAt(pointEdgeSection.NumberOfVertices, new Point2d(-widthOfFlange / 2, height), 0, 0, 0);
            pointEdgeSection.Closed = true;
            bridgeComponentDiagram.StructureCurves.Add(pointEdgeSection);
            
            //右侧横隔板多段线
            Polyline rightDiaphragm = new Polyline();
            rightDiaphragm.AddVertexAt(rightDiaphragm.NumberOfVertices, new Point2d(widthOfFlange / 2 - widthOfWetJoint / 2, height - heightOfFlange), 0, 0, 0);
            rightDiaphragm.AddVertexAt(rightDiaphragm.NumberOfVertices, new Point2d(widthOfWeb / 2 + widthOfFlangeHaunch, height - heightOfFlange), 0, 0, 0);
            rightDiaphragm.AddVertexAt(rightDiaphragm.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, height - heightOfFlange - (heightOfFlangeHaunch - (widthOfButtonBulb - widthOfWeb) / 2 * heightOfFlangeHaunch / widthOfFlangeHaunch)), 0, 0, 0);
            rightDiaphragm.AddVertexAt(rightDiaphragm.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, heightOfButtonBulb), 0, 0, 0);
            rightDiaphragm.AddVertexAt(rightDiaphragm.NumberOfVertices, new Point2d(widthOfFlange / 2 - widthOfWetJoint / 2, heightOfButtonBulb), 0, 0, 0);
            rightDiaphragm.Closed = true;
            bridgeComponentDiagram.StructureCurves.Add(rightDiaphragm);
            
            //左侧横隔板多段线
            Polyline leftDiaphragm = rightDiaphragm.Clone() as Polyline;
            ModifyTool.MirrorEntityByPlane(leftDiaphragm, new Plane(Point3d.Origin, Vector3d.XAxis));
            bridgeComponentDiagram.StructureCurves.Add(leftDiaphragm);
            
            //右侧湿接缝多段线
            Polyline rightWetJoint = new Polyline();
            rightWetJoint.AddVertexAt(rightWetJoint.NumberOfVertices, new Point2d(widthOfFlange / 2, height), 0, 0, 0);
            rightWetJoint.AddVertexAt(rightWetJoint.NumberOfVertices, new Point2d(widthOfFlange / 2, heightOfButtonBulb), 0, 0, 0);
            rightWetJoint.AddVertexAt(rightWetJoint.NumberOfVertices, new Point2d(widthOfFlange / 2 - widthOfWetJoint / 2, heightOfButtonBulb), 0, 0, 0);
            rightWetJoint.AddVertexAt(rightWetJoint.NumberOfVertices, new Point2d(widthOfFlange / 2 - widthOfWetJoint / 2, height), 0, 0, 0);
            rightWetJoint.Closed = true;
            bridgeComponentDiagram.HatchCurves.Add(rightWetJoint);
            
            //左侧湿接缝多段线
            Polyline leftWetJoint = rightWetJoint.Clone() as Polyline;
            ModifyTool.MirrorEntityByPlane(leftWetJoint, new Plane(Point3d.Origin, Vector3d.XAxis));
            bridgeComponentDiagram.HatchCurves.Add(leftWetJoint);
            
            //上方一级标注
            Point3dCollection topFirstPoints = new Point3dCollection()
            {
                pointEdgeSection.GetPoint3dAt(11),
                new Point3d(pointEdgeSection.GetPoint3dAt(11).X + widthOfWetJoint / 2, height, 0),
                new Point3d(0, height, 0),
                new Point3d(pointEdgeSection.GetPoint3dAt(0).X - widthOfWetJoint / 2, height, 0),
                pointEdgeSection.GetPoint3dAt(0)
            };
            DBObjectCollection topFirstDimension = DimensionTool.GetDimensionGroup(topFirstPoints, Math.PI / 2, 25);
            foreach (RotatedDimension dimension in topFirstDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            //上方二级标注
            Point3dCollection topSecondPoints = new Point3dCollection()
            {
                pointEdgeSection.GetPoint3dAt(11),
                pointEdgeSection.GetPoint3dAt(0)
            };
            DBObjectCollection topSecondDimension = DimensionTool.GetDimensionGroup(topSecondPoints, Math.PI / 2, 50);
            foreach (RotatedDimension dimension in topSecondDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            //左侧一级标注
            Point3dCollection leftFirstPoints = new Point3dCollection()
            {
                pointEdgeSection.GetPoint3dAt(11),
                pointEdgeSection.GetPoint3dAt(10),
                pointEdgeSection.GetPoint3dAt(7),
                pointEdgeSection.GetPoint3dAt(6)
            };
            DBObjectCollection leftFirstDimension = DimensionTool.GetDimensionGroup(leftFirstPoints, Math.PI, 25);
            foreach (RotatedDimension dimension in leftFirstDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            //左侧二级标注
            Point3dCollection leftSecondPoints = new Point3dCollection()
            {
                pointEdgeSection.GetPoint3dAt(11),
                pointEdgeSection.GetPoint3dAt(6)
            };
            DBObjectCollection leftSecondDimension = DimensionTool.GetDimensionGroup(leftSecondPoints, Math.PI, 50);
            foreach (RotatedDimension dimension in leftSecondDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            //下方一级标注
            Point3dCollection bottomFirstPoints = new Point3dCollection()
            {
                new Point3d(pointEdgeSection.GetPoint3dAt(11).X, 0, 0),
                new Point3d(pointEdgeSection.GetPoint3dAt(11).X + widthOfWetJoint / 2, 0, 0),
                pointEdgeSection.GetPoint3dAt(6),
                pointEdgeSection.GetPoint3dAt(5),
                new Point3d(pointEdgeSection.GetPoint3dAt(0).X - widthOfWetJoint / 2, 0, 0),
                pointEdgeSection.GetPoint3dAt(0)
            };
            DBObjectCollection bottomFirstDimension = DimensionTool.GetDimensionGroup(bottomFirstPoints, Math.PI * 3 / 2 , 25);
            foreach (RotatedDimension dimension in bottomFirstDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            //右方一级标注
            Point3dCollection rightFirstPoints = new Point3dCollection()
            {
                pointEdgeSection.GetPoint3dAt(0),
                pointEdgeSection.GetPoint3dAt(1),
                pointEdgeSection.GetPoint3dAt(4),
                pointEdgeSection.GetPoint3dAt(5)
            };
            DBObjectCollection rightFirstDimension = DimensionTool.GetDimensionGroup(rightFirstPoints, 0, 25);
            foreach (RotatedDimension dimension in rightFirstDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            //右方二级标注
            Point3dCollection rightSecondPoints = new Point3dCollection()
            {
                pointEdgeSection.GetPoint3dAt(0),
                pointEdgeSection.GetPoint3dAt(5)
            };
            DBObjectCollection rightSecondDimension = DimensionTool.GetDimensionGroup(rightSecondPoints, 0, 50);
            foreach (RotatedDimension dimension in rightSecondDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            //内部标注
            Point3dCollection internalPoints = new Point3dCollection()
            {
                new Point3d(pointEdgeSection.GetPoint3dAt(10).X + widthOfWetJoint / 2, pointEdgeSection.GetPoint3dAt(10).Y, 0),
                pointEdgeSection.GetPoint3dAt(9),
                pointEdgeSection.GetPoint3dAt(8),
                pointEdgeSection.GetPoint3dAt(3),
                pointEdgeSection.GetPoint3dAt(2),
                new Point3d(pointEdgeSection.GetPoint3dAt(1).X - widthOfWetJoint / 2, pointEdgeSection.GetPoint3dAt(1).Y, 0)
            };
            DBObjectCollection internalDimension = DimensionTool.GetDimensionGroup(internalPoints, Math.PI * 3 / 2, 50);
            foreach (RotatedDimension dimension in internalDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            //左侧承托标注
            Point3dCollection leftHaunchPoints = new Point3dCollection()
            {
                pointEdgeSection.GetPoint3dAt(9),
                pointEdgeSection.GetPoint3dAt(8)
            };
            DBObjectCollection leftHaunchDimension = DimensionTool.GetDimensionGroup(leftHaunchPoints, Math.PI, 0);
            bridgeComponentDiagram.Dimensions.Add(leftHaunchDimension[0]);
            
            //右侧承托标注
            Point3dCollection rightHaunchPoints = new Point3dCollection()
            {
                pointEdgeSection.GetPoint3dAt(2),
                pointEdgeSection.GetPoint3dAt(3)
            };
            DBObjectCollection rightHaunchDimension = DimensionTool.GetDimensionGroup(rightHaunchPoints, 0, 0);
            bridgeComponentDiagram.Dimensions.Add(rightHaunchDimension[0]);
            
            return bridgeComponentDiagram;
        }
        
        public static BridgeComponentDiagram GetTbeamMidspanEdgeSectionDiagrams(this TbeamModel tbeamModel)
        {
            BridgeComponentDiagram bridgeComponentDiagram = new BridgeComponentDiagram();
            
            double height = tbeamModel.Height;
            
            double widthOfFlange = tbeamModel.WidthOfFlange;
            double heightOfFlange = tbeamModel.HeightOfFlange;
            double widthOfFlangeHaunch = tbeamModel.WidthOfFlangeHaunch;
            double heightOfFlangeHaunch = tbeamModel.HeightOfFlangeHaunch;
            
            double widthOfWeb = tbeamModel.WidthOfWeb;
            
            double widthOfButtonBulb = tbeamModel.WidthOfButtonBulb;
            double heightOfButtonBulb = tbeamModel.HeightOfButtonBulb;
            double heightOfButtonBulbSlope = tbeamModel.HeightOfButtonBulbSlope;

            double widthOfWetJoint = tbeamModel.WidthOfWetJoint;
            
            //中心线
            Polyline centerLine = new Polyline();
            centerLine.AddVertexAt(centerLine.NumberOfVertices, new Point2d(0, -25), 0, 0, 0);
            centerLine.AddVertexAt(centerLine.NumberOfVertices, new Point2d(0, height + 25), 0, 0, 0);
            bridgeComponentDiagram.CenterCurves.Add(centerLine);
            
            //梁体多段线
            Polyline midSpanEdgeSection = new Polyline();
            midSpanEdgeSection.AddVertexAt(midSpanEdgeSection.NumberOfVertices, new Point2d(widthOfFlange / 2, height), 0, 0, 0);
            midSpanEdgeSection.AddVertexAt(midSpanEdgeSection.NumberOfVertices, new Point2d(widthOfFlange / 2, height - heightOfFlange), 0, 0, 0);
            midSpanEdgeSection.AddVertexAt(midSpanEdgeSection.NumberOfVertices, new Point2d(widthOfWeb / 2 + widthOfFlangeHaunch, height - heightOfFlange), 0, 0, 0);
            midSpanEdgeSection.AddVertexAt(midSpanEdgeSection.NumberOfVertices, new Point2d(widthOfWeb / 2, height - heightOfFlange - heightOfFlangeHaunch), 0, 0, 0);
            midSpanEdgeSection.AddVertexAt(midSpanEdgeSection.NumberOfVertices, new Point2d(widthOfWeb / 2, heightOfButtonBulb + heightOfButtonBulbSlope), 0, 0, 0);
            midSpanEdgeSection.AddVertexAt(midSpanEdgeSection.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, heightOfButtonBulb), 0, 0, 0);
            midSpanEdgeSection.AddVertexAt(midSpanEdgeSection.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, 0), 0, 0, 0);
            midSpanEdgeSection.AddVertexAt(midSpanEdgeSection.NumberOfVertices, new Point2d(-widthOfButtonBulb / 2, 0), 0, 0, 0);
            midSpanEdgeSection.AddVertexAt(midSpanEdgeSection.NumberOfVertices, new Point2d(-widthOfButtonBulb / 2, heightOfButtonBulb), 0, 0, 0);
            midSpanEdgeSection.AddVertexAt(midSpanEdgeSection.NumberOfVertices, new Point2d(-widthOfWeb / 2, heightOfButtonBulb + heightOfButtonBulbSlope), 0, 0, 0);
            midSpanEdgeSection.AddVertexAt(midSpanEdgeSection.NumberOfVertices, new Point2d(-widthOfWeb / 2, height - heightOfFlange - heightOfFlangeHaunch), 0, 0, 0);
            midSpanEdgeSection.AddVertexAt(midSpanEdgeSection.NumberOfVertices, new Point2d(-widthOfWeb / 2 - widthOfFlangeHaunch, height - heightOfFlange), 0, 0, 0);
            midSpanEdgeSection.AddVertexAt(midSpanEdgeSection.NumberOfVertices, new Point2d(-widthOfFlange / 2, height - heightOfFlange), 0, 0, 0);
            midSpanEdgeSection.AddVertexAt(midSpanEdgeSection.NumberOfVertices, new Point2d(-widthOfFlange / 2, height), 0, 0, 0);
            midSpanEdgeSection.Closed = true;
            bridgeComponentDiagram.StructureCurves.Add(midSpanEdgeSection);
            
            //右侧横隔板多段线
            Polyline rightDiaphragm = new Polyline();
            rightDiaphragm.AddVertexAt(rightDiaphragm.NumberOfVertices, new Point2d(widthOfFlange / 2 - widthOfWetJoint / 2, height - heightOfFlange), 0, 0, 0);
            rightDiaphragm.AddVertexAt(rightDiaphragm.NumberOfVertices, new Point2d(widthOfWeb / 2 + widthOfFlangeHaunch, height - heightOfFlange), 0, 0, 0);
            rightDiaphragm.AddVertexAt(rightDiaphragm.NumberOfVertices, new Point2d(widthOfWeb / 2, height - heightOfFlange - heightOfFlangeHaunch), 0, 0, 0);
            rightDiaphragm.AddVertexAt(rightDiaphragm.NumberOfVertices, new Point2d(widthOfWeb / 2, heightOfButtonBulb + heightOfButtonBulbSlope), 0, 0, 0);
            rightDiaphragm.AddVertexAt(rightDiaphragm.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, heightOfButtonBulb), 0, 0, 0);
            rightDiaphragm.AddVertexAt(rightDiaphragm.NumberOfVertices, new Point2d(widthOfFlange / 2 - widthOfWetJoint / 2, heightOfButtonBulb), 0, 0, 0);
            rightDiaphragm.Closed = true;
            bridgeComponentDiagram.StructureCurves.Add(rightDiaphragm);
            
            //右侧湿接缝多段线
            Polyline rightWetJoint = new Polyline();
            rightWetJoint.AddVertexAt(rightWetJoint.NumberOfVertices, new Point2d(widthOfFlange / 2, height), 0, 0, 0);
            rightWetJoint.AddVertexAt(rightWetJoint.NumberOfVertices, new Point2d(widthOfFlange / 2, heightOfButtonBulb), 0, 0, 0);
            rightWetJoint.AddVertexAt(rightWetJoint.NumberOfVertices, new Point2d(widthOfFlange / 2 - widthOfWetJoint / 2, heightOfButtonBulb), 0, 0, 0);
            rightWetJoint.AddVertexAt(rightWetJoint.NumberOfVertices, new Point2d(widthOfFlange / 2 - widthOfWetJoint / 2, height), 0, 0, 0);
            rightWetJoint.Closed = true;
            bridgeComponentDiagram.HatchCurves.Add(rightWetJoint);
            
            //上方一级标注
            Point3dCollection topFirstPoints = new Point3dCollection()
            {
                midSpanEdgeSection.GetPoint3dAt(13),
                new Point3d(0, height, 0),
                new Point3d(midSpanEdgeSection.GetPoint3dAt(0).X - widthOfWetJoint / 2, height, 0),
                midSpanEdgeSection.GetPoint3dAt(0)
            };
            DBObjectCollection topFirstDimension = DimensionTool.GetDimensionGroup(topFirstPoints, Math.PI / 2, 25);
            foreach (RotatedDimension dimension in topFirstDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            //上方二级标注
            Point3dCollection topSecondPoints = new Point3dCollection()
            {
                midSpanEdgeSection.GetPoint3dAt(13),
                midSpanEdgeSection.GetPoint3dAt(0)
            };
            DBObjectCollection topSecondDimension = DimensionTool.GetDimensionGroup(topSecondPoints, Math.PI / 2, 50);
            foreach (RotatedDimension dimension in topSecondDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            //左侧一级标注
            Point3dCollection leftFirstPoints = new Point3dCollection()
            {
                midSpanEdgeSection.GetPoint3dAt(13),
                midSpanEdgeSection.GetPoint3dAt(12),
                midSpanEdgeSection.GetPoint3dAt(7)
            };
            DBObjectCollection leftFirstDimension = DimensionTool.GetDimensionGroup(leftFirstPoints, Math.PI, 25);
            foreach (RotatedDimension dimension in leftFirstDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            //左侧二级标注
            Point3dCollection leftSecondPoints = new Point3dCollection()
            {
                midSpanEdgeSection.GetPoint3dAt(13),
                midSpanEdgeSection.GetPoint3dAt(7)
            };
            DBObjectCollection leftSecondDimension = DimensionTool.GetDimensionGroup(leftSecondPoints, Math.PI, 50);
            foreach (RotatedDimension dimension in leftSecondDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            //下方一级标注
            Point3dCollection bottomFirstPoints = new Point3dCollection()
            {
                new Point3d(midSpanEdgeSection.GetPoint3dAt(13).X, 0, 0),
                midSpanEdgeSection.GetPoint3dAt(7),
                midSpanEdgeSection.GetPoint3dAt(6),
                new Point3d(midSpanEdgeSection.GetPoint3dAt(0).X - widthOfWetJoint / 2, 0, 0),
                midSpanEdgeSection.GetPoint3dAt(0)
            };
            DBObjectCollection bottomFirstDimension = DimensionTool.GetDimensionGroup(bottomFirstPoints, Math.PI * 3 / 2 , 25);
            foreach (RotatedDimension dimension in bottomFirstDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            //右方一级标注
            Point3dCollection rightFirstPoints = new Point3dCollection()
            {
                midSpanEdgeSection.GetPoint3dAt(0),
                midSpanEdgeSection.GetPoint3dAt(1),
                midSpanEdgeSection.GetPoint3dAt(5),
                midSpanEdgeSection.GetPoint3dAt(6)
            };
            DBObjectCollection rightFirstDimension = DimensionTool.GetDimensionGroup(rightFirstPoints, 0, 25);
            foreach (RotatedDimension dimension in rightFirstDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            //右方二级标注
            Point3dCollection rightSecondPoints = new Point3dCollection()
            {
                midSpanEdgeSection.GetPoint3dAt(0),
                midSpanEdgeSection.GetPoint3dAt(6)
            };
            DBObjectCollection rightSecondDimension = DimensionTool.GetDimensionGroup(rightSecondPoints, 0, 50);
            foreach (RotatedDimension dimension in rightSecondDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            //内部标注
            Point3dCollection internalPoints = new Point3dCollection()
            {
                midSpanEdgeSection.GetPoint3dAt(12),
                midSpanEdgeSection.GetPoint3dAt(11),
                midSpanEdgeSection.GetPoint3dAt(10),
                midSpanEdgeSection.GetPoint3dAt(3),
                midSpanEdgeSection.GetPoint3dAt(2),
                new Point3d(midSpanEdgeSection.GetPoint3dAt(1).X - widthOfWetJoint / 2, midSpanEdgeSection.GetPoint3dAt(1).Y, 0)
            };
            DBObjectCollection internalDimension = DimensionTool.GetDimensionGroup(internalPoints, Math.PI * 3 / 2, 50);
            foreach (RotatedDimension dimension in internalDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            //左侧承托标注
            Point3dCollection leftHaunchPoints = new Point3dCollection()
            {
                midSpanEdgeSection.GetPoint3dAt(11),
                midSpanEdgeSection.GetPoint3dAt(10)
            };
            DBObjectCollection leftHaunchDimension = DimensionTool.GetDimensionGroup(leftHaunchPoints, Math.PI, 0);
            bridgeComponentDiagram.Dimensions.Add(leftHaunchDimension[0]);
            
            //右侧承托标注
            Point3dCollection rightHaunchPoints = new Point3dCollection()
            {
                midSpanEdgeSection.GetPoint3dAt(2),
                midSpanEdgeSection.GetPoint3dAt(3)
            };
            DBObjectCollection rightHaunchDimension = DimensionTool.GetDimensionGroup(rightHaunchPoints, 0, 0);
            bridgeComponentDiagram.Dimensions.Add(rightHaunchDimension[0]);
            
            
            //左侧马蹄斜坡标注
            Point3dCollection leftHaunchSlopePoints = new Point3dCollection()
            {
                midSpanEdgeSection.GetPoint3dAt(8),
                midSpanEdgeSection.GetPoint3dAt(9),
                new Point3d(midSpanEdgeSection.GetPoint3dAt(8).X, midSpanEdgeSection.GetPoint3dAt(8).Y - heightOfButtonBulbSlope, 0)
            };
            DBObjectCollection leftHaunchSlopeDimension = DimensionTool.GetDimensionGroup(leftHaunchSlopePoints, Math.PI, 0);
            bridgeComponentDiagram.Dimensions.Add(leftHaunchSlopeDimension[0]);
            
            //右侧马蹄斜坡标注
            Point3dCollection rightHaunchSlopePoints = new Point3dCollection()
            {
                midSpanEdgeSection.GetPoint3dAt(5),
                midSpanEdgeSection.GetPoint3dAt(4),
                new Point3d(midSpanEdgeSection.GetPoint3dAt(2).X, midSpanEdgeSection.GetPoint3dAt(2).Y - heightOfButtonBulbSlope, 0)
            };
            DBObjectCollection rightHaunchSlopeDimension = DimensionTool.GetDimensionGroup(rightHaunchSlopePoints, 0, 0);
            bridgeComponentDiagram.Dimensions.Add(rightHaunchSlopeDimension[0]);
            
            return bridgeComponentDiagram;
        }
        
        public static BridgeComponentDiagram GetTbeamMidspanMiddleSectionDiagrams(this TbeamModel tbeamModel)
        {
            BridgeComponentDiagram bridgeComponentDiagram = new BridgeComponentDiagram();
            
            double height = tbeamModel.Height;
            
            double widthOfFlange = tbeamModel.WidthOfFlange;
            double heightOfFlange = tbeamModel.HeightOfFlange;
            double widthOfFlangeHaunch = tbeamModel.WidthOfFlangeHaunch;
            double heightOfFlangeHaunch = tbeamModel.HeightOfFlangeHaunch;
            
            double widthOfWeb = tbeamModel.WidthOfWeb;
            
            double widthOfButtonBulb = tbeamModel.WidthOfButtonBulb;
            double heightOfButtonBulb = tbeamModel.HeightOfButtonBulb;
            double heightOfButtonBulbSlope = tbeamModel.HeightOfButtonBulbSlope;

            double widthOfWetJoint = tbeamModel.WidthOfWetJoint;
            
            //中心线
            Polyline centerLine = new Polyline();
            centerLine.AddVertexAt(centerLine.NumberOfVertices, new Point2d(0, -25), 0, 0, 0);
            centerLine.AddVertexAt(centerLine.NumberOfVertices, new Point2d(0, height + 25), 0, 0, 0);
            bridgeComponentDiagram.CenterCurves.Add(centerLine);
            
            //梁体多段线
            Polyline midSpanMiddleSection = new Polyline();
            midSpanMiddleSection.AddVertexAt(midSpanMiddleSection.NumberOfVertices, new Point2d(widthOfFlange / 2, height), 0, 0, 0);
            midSpanMiddleSection.AddVertexAt(midSpanMiddleSection.NumberOfVertices, new Point2d(widthOfFlange / 2, height - heightOfFlange), 0, 0, 0);
            midSpanMiddleSection.AddVertexAt(midSpanMiddleSection.NumberOfVertices, new Point2d(widthOfWeb / 2 + widthOfFlangeHaunch, height - heightOfFlange), 0, 0, 0);
            midSpanMiddleSection.AddVertexAt(midSpanMiddleSection.NumberOfVertices, new Point2d(widthOfWeb / 2, height - heightOfFlange - heightOfFlangeHaunch), 0, 0, 0);
            midSpanMiddleSection.AddVertexAt(midSpanMiddleSection.NumberOfVertices, new Point2d(widthOfWeb / 2, heightOfButtonBulb + heightOfButtonBulbSlope), 0, 0, 0);
            midSpanMiddleSection.AddVertexAt(midSpanMiddleSection.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, heightOfButtonBulb), 0, 0, 0);
            midSpanMiddleSection.AddVertexAt(midSpanMiddleSection.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, 0), 0, 0, 0);
            midSpanMiddleSection.AddVertexAt(midSpanMiddleSection.NumberOfVertices, new Point2d(-widthOfButtonBulb / 2, 0), 0, 0, 0);
            midSpanMiddleSection.AddVertexAt(midSpanMiddleSection.NumberOfVertices, new Point2d(-widthOfButtonBulb / 2, heightOfButtonBulb), 0, 0, 0);
            midSpanMiddleSection.AddVertexAt(midSpanMiddleSection.NumberOfVertices, new Point2d(-widthOfWeb / 2, heightOfButtonBulb + heightOfButtonBulbSlope), 0, 0, 0);
            midSpanMiddleSection.AddVertexAt(midSpanMiddleSection.NumberOfVertices, new Point2d(-widthOfWeb / 2, height - heightOfFlange - heightOfFlangeHaunch), 0, 0, 0);
            midSpanMiddleSection.AddVertexAt(midSpanMiddleSection.NumberOfVertices, new Point2d(-widthOfWeb / 2 - widthOfFlangeHaunch, height - heightOfFlange), 0, 0, 0);
            midSpanMiddleSection.AddVertexAt(midSpanMiddleSection.NumberOfVertices, new Point2d(-widthOfFlange / 2, height - heightOfFlange), 0, 0, 0);
            midSpanMiddleSection.AddVertexAt(midSpanMiddleSection.NumberOfVertices, new Point2d(-widthOfFlange / 2, height), 0, 0, 0);
            midSpanMiddleSection.Closed = true;
            bridgeComponentDiagram.StructureCurves.Add(midSpanMiddleSection);
            
            //右侧横隔板多段线
            Polyline rightDiaphragm = new Polyline();
            rightDiaphragm.AddVertexAt(rightDiaphragm.NumberOfVertices, new Point2d(widthOfFlange / 2 - widthOfWetJoint / 2, height - heightOfFlange), 0, 0, 0);
            rightDiaphragm.AddVertexAt(rightDiaphragm.NumberOfVertices, new Point2d(widthOfWeb / 2 + widthOfFlangeHaunch, height - heightOfFlange), 0, 0, 0);
            rightDiaphragm.AddVertexAt(rightDiaphragm.NumberOfVertices, new Point2d(widthOfWeb / 2, height - heightOfFlange - heightOfFlangeHaunch), 0, 0, 0);
            rightDiaphragm.AddVertexAt(rightDiaphragm.NumberOfVertices, new Point2d(widthOfWeb / 2, heightOfButtonBulb + heightOfButtonBulbSlope), 0, 0, 0);
            rightDiaphragm.AddVertexAt(rightDiaphragm.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, heightOfButtonBulb), 0, 0, 0);
            rightDiaphragm.AddVertexAt(rightDiaphragm.NumberOfVertices, new Point2d(widthOfFlange / 2 - widthOfWetJoint / 2, heightOfButtonBulb), 0, 0, 0);
            rightDiaphragm.Closed = true;
            bridgeComponentDiagram.StructureCurves.Add(rightDiaphragm);
            
            //左侧横隔板多段线
            Polyline leftDiaphragm = rightDiaphragm.Clone() as Polyline;
            ModifyTool.MirrorEntityByPlane(leftDiaphragm, new Plane(Point3d.Origin, Vector3d.XAxis));
            bridgeComponentDiagram.StructureCurves.Add(leftDiaphragm);
            
            //右侧湿接缝多段线
            Polyline rightWetJoint = new Polyline();
            rightWetJoint.AddVertexAt(rightWetJoint.NumberOfVertices, new Point2d(widthOfFlange / 2, height), 0, 0, 0);
            rightWetJoint.AddVertexAt(rightWetJoint.NumberOfVertices, new Point2d(widthOfFlange / 2, heightOfButtonBulb), 0, 0, 0);
            rightWetJoint.AddVertexAt(rightWetJoint.NumberOfVertices, new Point2d(widthOfFlange / 2 - widthOfWetJoint / 2, heightOfButtonBulb), 0, 0, 0);
            rightWetJoint.AddVertexAt(rightWetJoint.NumberOfVertices, new Point2d(widthOfFlange / 2 - widthOfWetJoint / 2, height), 0, 0, 0);
            rightWetJoint.Closed = true;
            bridgeComponentDiagram.HatchCurves.Add(rightWetJoint);
            
            //左侧湿接缝多段线
            Polyline leftWetJoint = rightWetJoint.Clone() as Polyline;
            ModifyTool.MirrorEntityByPlane(leftWetJoint, new Plane(Point3d.Origin, Vector3d.XAxis));
            bridgeComponentDiagram.HatchCurves.Add(leftWetJoint);
            
            //上方一级标注
            Point3dCollection topFirstPoints = new Point3dCollection()
            {
                midSpanMiddleSection.GetPoint3dAt(13),
                new Point3d(midSpanMiddleSection.GetPoint3dAt(13).X + widthOfWetJoint / 2, height, 0),
                new Point3d(0, height, 0),
                new Point3d(midSpanMiddleSection.GetPoint3dAt(0).X - widthOfWetJoint / 2, height, 0),
                midSpanMiddleSection.GetPoint3dAt(0)
            };
            DBObjectCollection topFirstDimension = DimensionTool.GetDimensionGroup(topFirstPoints, Math.PI / 2, 25);
            foreach (RotatedDimension dimension in topFirstDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            //上方二级标注
            Point3dCollection topSecondPoints = new Point3dCollection()
            {
                midSpanMiddleSection.GetPoint3dAt(13),
                midSpanMiddleSection.GetPoint3dAt(0)
            };
            DBObjectCollection topSecondDimension = DimensionTool.GetDimensionGroup(topSecondPoints, Math.PI / 2, 50);
            foreach (RotatedDimension dimension in topSecondDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            //左侧一级标注
            Point3dCollection leftFirstPoints = new Point3dCollection()
            {
                midSpanMiddleSection.GetPoint3dAt(13),
                midSpanMiddleSection.GetPoint3dAt(12),
                midSpanMiddleSection.GetPoint3dAt(8),
                midSpanMiddleSection.GetPoint3dAt(7)
            };
            DBObjectCollection leftFirstDimension = DimensionTool.GetDimensionGroup(leftFirstPoints, Math.PI, 25);
            foreach (RotatedDimension dimension in leftFirstDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            //左侧二级标注
            Point3dCollection leftSecondPoints = new Point3dCollection()
            {
                midSpanMiddleSection.GetPoint3dAt(13),
                midSpanMiddleSection.GetPoint3dAt(7)
            };
            DBObjectCollection leftSecondDimension = DimensionTool.GetDimensionGroup(leftSecondPoints, Math.PI, 50);
            foreach (RotatedDimension dimension in leftSecondDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            //下方一级标注
            Point3dCollection bottomFirstPoints = new Point3dCollection()
            {
                new Point3d(midSpanMiddleSection.GetPoint3dAt(13).X, 0, 0),
                new Point3d(midSpanMiddleSection.GetPoint3dAt(13).X + widthOfWetJoint / 2, 0, 0),
                midSpanMiddleSection.GetPoint3dAt(7),
                midSpanMiddleSection.GetPoint3dAt(6),
                new Point3d(midSpanMiddleSection.GetPoint3dAt(0).X - widthOfWetJoint / 2, 0, 0),
                midSpanMiddleSection.GetPoint3dAt(0)
            };
            DBObjectCollection bottomFirstDimension = DimensionTool.GetDimensionGroup(bottomFirstPoints, Math.PI * 3 / 2 , 25);
            foreach (RotatedDimension dimension in bottomFirstDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            //右方一级标注
            Point3dCollection rightFirstPoints = new Point3dCollection()
            {
                midSpanMiddleSection.GetPoint3dAt(0),
                midSpanMiddleSection.GetPoint3dAt(1),
                midSpanMiddleSection.GetPoint3dAt(4),
                midSpanMiddleSection.GetPoint3dAt(5)
            };
            DBObjectCollection rightFirstDimension = DimensionTool.GetDimensionGroup(rightFirstPoints, 0, 25);
            foreach (RotatedDimension dimension in rightFirstDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            //右方二级标注
            Point3dCollection rightSecondPoints = new Point3dCollection()
            {
                midSpanMiddleSection.GetPoint3dAt(0),
                midSpanMiddleSection.GetPoint3dAt(5)
            };
            DBObjectCollection rightSecondDimension = DimensionTool.GetDimensionGroup(rightSecondPoints, 0, 50);
            foreach (RotatedDimension dimension in rightSecondDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            //内部标注
            Point3dCollection internalPoints = new Point3dCollection()
            {
                new Point3d(midSpanMiddleSection.GetPoint3dAt(12).X + widthOfWetJoint / 2, midSpanMiddleSection.GetPoint3dAt(10).Y, 0),
                midSpanMiddleSection.GetPoint3dAt(11),
                midSpanMiddleSection.GetPoint3dAt(10),
                midSpanMiddleSection.GetPoint3dAt(3),
                midSpanMiddleSection.GetPoint3dAt(2),
                new Point3d(midSpanMiddleSection.GetPoint3dAt(1).X - widthOfWetJoint / 2, midSpanMiddleSection.GetPoint3dAt(1).Y, 0)
            };
            DBObjectCollection internalDimension = DimensionTool.GetDimensionGroup(internalPoints, Math.PI * 3 / 2, 50);
            foreach (RotatedDimension dimension in internalDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            //左侧承托标注
            Point3dCollection leftHaunchPoints = new Point3dCollection()
            {
                midSpanMiddleSection.GetPoint3dAt(11),
                midSpanMiddleSection.GetPoint3dAt(10)
            };
            DBObjectCollection leftHaunchDimension = DimensionTool.GetDimensionGroup(leftHaunchPoints, Math.PI, 0);
            bridgeComponentDiagram.Dimensions.Add(leftHaunchDimension[0]);
            
            //右侧承托标注
            Point3dCollection rightHaunchPoints = new Point3dCollection()
            {
                midSpanMiddleSection.GetPoint3dAt(2),
                midSpanMiddleSection.GetPoint3dAt(3)
            };
            DBObjectCollection rightHaunchDimension = DimensionTool.GetDimensionGroup(rightHaunchPoints, 0, 0);
            bridgeComponentDiagram.Dimensions.Add(rightHaunchDimension[0]);
            
            //左侧马蹄斜坡标注
            Point3dCollection leftHaunchSlopePoints = new Point3dCollection()
            {
                midSpanMiddleSection.GetPoint3dAt(8),
                midSpanMiddleSection.GetPoint3dAt(9),
                new Point3d(midSpanMiddleSection.GetPoint3dAt(8).X, midSpanMiddleSection.GetPoint3dAt(8).Y - heightOfButtonBulbSlope, 0)
            };
            DBObjectCollection leftHaunchSlopeDimension = DimensionTool.GetDimensionGroup(leftHaunchSlopePoints, Math.PI, 0);
            bridgeComponentDiagram.Dimensions.Add(leftHaunchSlopeDimension[0]);
            
            //右侧马蹄斜坡标注
            Point3dCollection rightHaunchSlopePoints = new Point3dCollection()
            {
                midSpanMiddleSection.GetPoint3dAt(5),
                midSpanMiddleSection.GetPoint3dAt(4),
                new Point3d(midSpanMiddleSection.GetPoint3dAt(2).X, midSpanMiddleSection.GetPoint3dAt(2).Y - heightOfButtonBulbSlope, 0)
            };
            DBObjectCollection rightHaunchSlopeDimension = DimensionTool.GetDimensionGroup(rightHaunchSlopePoints, 0, 0);
            bridgeComponentDiagram.Dimensions.Add(rightHaunchSlopeDimension[0]);

            
            return bridgeComponentDiagram;
        }
        
        public static BridgeComponentDiagram GetTbeamPointCrossSectionDiagrams(this TbeamModel tbeamModel)
        {
            BridgeComponentDiagram bridgeComponentDiagram = new BridgeComponentDiagram();
            double height = tbeamModel.Height;
            int numberOfTbeam = tbeamModel.NumberOfTbeam;
            
            double widthOfFlange = tbeamModel.WidthOfFlange;
            double heightOfFlange = tbeamModel.HeightOfFlange;
            double widthOfFlangeHaunch = tbeamModel.WidthOfFlangeHaunch;
            double heightOfFlangeHaunch = tbeamModel.HeightOfFlangeHaunch;
            
            double widthOfWeb = tbeamModel.WidthOfWeb;
            
            double widthOfButtonBulb = tbeamModel.WidthOfButtonBulb;
            double heightOfButtonBulb = tbeamModel.HeightOfButtonBulb;
            
            double widthOfWetJoint = tbeamModel.WidthOfWetJoint;
            
            
            //梁体多段线
            DBObjectCollection beamSections = new DBObjectCollection();
            for (int i = 0; i < numberOfTbeam; i++)
            {
                Polyline section = new Polyline();
                section.AddVertexAt(section.NumberOfVertices, new Point2d(widthOfFlange / 2, height), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(widthOfFlange / 2, height - heightOfFlange), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(widthOfWeb / 2 + widthOfFlangeHaunch, height - heightOfFlange), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, height - heightOfFlange - (heightOfFlangeHaunch - (widthOfButtonBulb - widthOfWeb) / 2 * heightOfFlangeHaunch / widthOfFlangeHaunch)), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, heightOfButtonBulb), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, 0), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(-widthOfButtonBulb / 2, 0), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(-widthOfButtonBulb / 2, heightOfButtonBulb), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(-widthOfButtonBulb / 2, height - heightOfFlange - (heightOfFlangeHaunch - (widthOfButtonBulb - widthOfWeb) / 2 * heightOfFlangeHaunch / widthOfFlangeHaunch)), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(-widthOfWeb / 2 - widthOfFlangeHaunch, height - heightOfFlange), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(-widthOfFlange / 2, height - heightOfFlange), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(-widthOfFlange / 2, height), 0, 0, 0);

                section.Closed = true;
                double x = widthOfFlange * i - widthOfFlange * (numberOfTbeam - 1) / 2;
                ModifyTool.MoveByVector(section, new Vector3d(x, 0, 0));
                beamSections.Add(section);
                bridgeComponentDiagram.StructureCurves.Add(section);
            }
            
            //横隔板多段线
            for (int i = 0; i < numberOfTbeam - 1; i++)
            {
                Polyline rightDiaphragm = new Polyline();
                rightDiaphragm.AddVertexAt(rightDiaphragm.NumberOfVertices, new Point2d(widthOfFlange / 2 - widthOfWetJoint / 2, height - heightOfFlange), 0, 0, 0);
                rightDiaphragm.AddVertexAt(rightDiaphragm.NumberOfVertices, new Point2d(widthOfWeb / 2 + widthOfFlangeHaunch, height - heightOfFlange), 0, 0, 0);
                rightDiaphragm.AddVertexAt(rightDiaphragm.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, height - heightOfFlange - (heightOfFlangeHaunch - (widthOfButtonBulb - widthOfWeb) / 2 * heightOfFlangeHaunch / widthOfFlangeHaunch)), 0, 0, 0);
                rightDiaphragm.AddVertexAt(rightDiaphragm.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, heightOfButtonBulb), 0, 0, 0);
                rightDiaphragm.AddVertexAt(rightDiaphragm.NumberOfVertices, new Point2d(widthOfFlange / 2 - widthOfWetJoint / 2, heightOfButtonBulb), 0, 0, 0);
                rightDiaphragm.Closed = true;
                
                double x = widthOfFlange * (numberOfTbeam - 3) / 2 - widthOfFlange * i;
                ModifyTool.MoveByVector(rightDiaphragm, new Vector3d(x, 0, 0));
                Polyline leftDiaphragm = rightDiaphragm.Clone() as Polyline;
                ModifyTool.MirrorEntityByPlane(leftDiaphragm, new Plane(Point3d.Origin, Vector3d.XAxis));
                

                
                bridgeComponentDiagram.StructureCurves.Add(rightDiaphragm);
                bridgeComponentDiagram.StructureCurves.Add(leftDiaphragm);
            }
            
            //湿接缝多段线
            DBObjectCollection wetJointSections = new DBObjectCollection();
            for (int i = 0; i < numberOfTbeam - 1; i++)
            {
                Polyline wetJoint = new Polyline();
                wetJoint.AddVertexAt(wetJoint.NumberOfVertices, new Point2d(widthOfFlange / 2 + widthOfWetJoint / 2, height), 0, 0, 0);
                wetJoint.AddVertexAt(wetJoint.NumberOfVertices, new Point2d(widthOfFlange / 2 + widthOfWetJoint / 2, heightOfButtonBulb), 0, 0, 0);
                wetJoint.AddVertexAt(wetJoint.NumberOfVertices, new Point2d(widthOfFlange / 2 - widthOfWetJoint / 2, heightOfButtonBulb), 0, 0, 0);
                wetJoint.AddVertexAt(wetJoint.NumberOfVertices, new Point2d(widthOfFlange / 2 - widthOfWetJoint / 2, height), 0, 0, 0);
                wetJoint.Closed = true;
                
                double x = widthOfFlange * (numberOfTbeam - 3) / 2 - widthOfFlange * i;
                ModifyTool.MoveByVector(wetJoint, new Vector3d(x, 0, 0));
                wetJointSections.Add(wetJoint);
                bridgeComponentDiagram.HatchCurves.Add(wetJoint);
            }
            
            //上方一级标注
            Point3dCollection topFirstPoints = new Point3dCollection()
            {
                ((Polyline)beamSections[0]).GetPoint3dAt(11),
                ((Polyline)beamSections[beamSections.Count - 1]).GetPoint3dAt(0)
            };
            DBObjectCollection topFirstDimension = DimensionTool.GetDimensionGroup(topFirstPoints, Math.PI / 2, 25);
            foreach (RotatedDimension dimension in topFirstDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            //左边一级标注
            Point3dCollection leftFirstPoints = new Point3dCollection()
            {
                ((Polyline)beamSections[0]).GetPoint3dAt(11),
                ((Polyline)beamSections[0]).GetPoint3dAt(6)
            };
            DBObjectCollection leftFirstDimension = DimensionTool.GetDimensionGroup(leftFirstPoints, Math.PI, 25);
            bridgeComponentDiagram.Dimensions.Add(leftFirstDimension[0]);
            
            //右边一级标注
            Point3dCollection rightFirstPoints = new Point3dCollection()
            {
                ((Polyline)beamSections[beamSections.Count - 1]).GetPoint3dAt(0),
                ((Polyline)beamSections[beamSections.Count - 1]).GetPoint3dAt(5)
            };
            DBObjectCollection rightFirstDimension = DimensionTool.GetDimensionGroup(rightFirstPoints, 0, 25);
            bridgeComponentDiagram.Dimensions.Add(rightFirstDimension[0]);
            
            
            //下方一级标注
            for (int i = 0; i < beamSections.Count; i++)
            {
                Point3dCollection downFirstPoints;
                if (i == 0)
                {
                    downFirstPoints = new Point3dCollection()
                    {
                        new Point3d(((Polyline)beamSections[0]).GetPoint3dAt(0).X - widthOfWetJoint / 2, 0, 0),
                        ((Polyline)beamSections[0]).GetPoint3dAt(11)
                    };
                    DBObjectCollection downFirstDimensionForFirst = DimensionTool.GetDimensionGroup(downFirstPoints, Math.PI * 3 / 2, 25);
                    bridgeComponentDiagram.Dimensions.Add(downFirstDimensionForFirst[0]);
                }
                
                else if (i == beamSections.Count - 1)
                {
                    downFirstPoints = new Point3dCollection()
                    {
                        new Point3d(((Polyline)beamSections[beamSections.Count - 1]).GetPoint3dAt(11).X + widthOfWetJoint / 2, 0, 0),
                        ((Polyline)beamSections[beamSections.Count - 1]).GetPoint3dAt(0)
                    };
                    DBObjectCollection downFirstDimensionForLast = DimensionTool.GetDimensionGroup(downFirstPoints, Math.PI * 3 / 2, 25);
                    bridgeComponentDiagram.Dimensions.Add(downFirstDimensionForLast[0]);
                }
                else
                {
                    downFirstPoints = new Point3dCollection()
                    {
                        new Point3d(((Polyline)beamSections[i]).GetPoint3dAt(11).X + widthOfWetJoint / 2, 0, 0),
                        new Point3d(((Polyline)beamSections[i]).GetPoint3dAt(0).X - widthOfWetJoint / 2, 0, 0)
                    };
                    DBObjectCollection downFirstDimensionForMiddle = DimensionTool.GetDimensionGroup(downFirstPoints, Math.PI * 3 / 2, 25);
                    bridgeComponentDiagram.Dimensions.Add(downFirstDimensionForMiddle[0]);
                }
            }

            for (int i = 0; i < wetJointSections.Count; i++)
            {
                Point3dCollection downFirstPoints = new Point3dCollection()
                {
                    ((Polyline)wetJointSections[i]).GetPoint3dAt(1),
                    ((Polyline)wetJointSections[i]).GetPoint3dAt(2),
                };
                DBObjectCollection downFirstDimensionForWetJoint = DimensionTool.GetDimensionGroup(downFirstPoints, Math.PI * 3 / 2, 25 + heightOfButtonBulb);
                bridgeComponentDiagram.Dimensions.Add(downFirstDimensionForWetJoint[0]);
            }
            
            
            //下方二级标注
            Point3dCollection downSecondPoints = new Point3dCollection()
            {
                new Point3d(((Polyline)beamSections[0]).GetPoint3dAt(11).X, 0, 0),
            };
            for (int i = 0; i < numberOfTbeam; i++)
            {
                double x = widthOfFlange * i - widthOfFlange * (numberOfTbeam - 1) / 2;
                downSecondPoints.Add(new Point3d(x, 0, 0));
            }
            downSecondPoints.Add(new Point3d(((Polyline)beamSections[beamSections.Count - 1]).GetPoint3dAt(0).X, 0, 0));
            
            
            DBObjectCollection downSecondDimension = DimensionTool.GetDimensionGroup(downSecondPoints, Math.PI * 3 / 2, 50);
            foreach (RotatedDimension dimension in downSecondDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            
            return bridgeComponentDiagram;
        }
        
        public static BridgeComponentDiagram GetTbeamMidspanCrossSectionDiagrams(this TbeamModel tbeamModel)
        {
            BridgeComponentDiagram bridgeComponentDiagram = new BridgeComponentDiagram();
            double height = tbeamModel.Height;
            int numberOfTbeam = tbeamModel.NumberOfTbeam;
            
            double widthOfFlange = tbeamModel.WidthOfFlange;
            double heightOfFlange = tbeamModel.HeightOfFlange;
            double widthOfFlangeHaunch = tbeamModel.WidthOfFlangeHaunch;
            double heightOfFlangeHaunch = tbeamModel.HeightOfFlangeHaunch;
            
            double widthOfWeb = tbeamModel.WidthOfWeb;
            
            double widthOfButtonBulb = tbeamModel.WidthOfButtonBulb;
            double heightOfButtonBulb = tbeamModel.HeightOfButtonBulb;
            double heightOfButtonBulbSlope = tbeamModel.HeightOfButtonBulbSlope;
            
            double widthOfWetJoint = tbeamModel.WidthOfWetJoint;
            
            
            //梁体多段线
            DBObjectCollection beamSections = new DBObjectCollection();
            for (int i = 0; i < numberOfTbeam; i++)
            {
                Polyline section = new Polyline();
                section.AddVertexAt(section.NumberOfVertices, new Point2d(widthOfFlange / 2, height), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(widthOfFlange / 2, height - heightOfFlange), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(widthOfWeb / 2 + widthOfFlangeHaunch, height - heightOfFlange), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(widthOfWeb / 2, height - heightOfFlange - heightOfFlangeHaunch), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(widthOfWeb / 2, heightOfButtonBulb + heightOfButtonBulbSlope), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, heightOfButtonBulb), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, 0), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(-widthOfButtonBulb / 2, 0), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(-widthOfButtonBulb / 2, heightOfButtonBulb), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(-widthOfWeb / 2, heightOfButtonBulb + heightOfButtonBulbSlope), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(-widthOfWeb / 2, height - heightOfFlange - heightOfFlangeHaunch), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(-widthOfWeb / 2 - widthOfFlangeHaunch, height - heightOfFlange), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(-widthOfFlange / 2, height - heightOfFlange), 0, 0, 0);
                section.AddVertexAt(section.NumberOfVertices, new Point2d(-widthOfFlange / 2, height), 0, 0, 0);
                section.Closed = true;
                double x = widthOfFlange * i - widthOfFlange * (numberOfTbeam - 1) / 2;
                ModifyTool.MoveByVector(section, new Vector3d(x, 0, 0));
                beamSections.Add(section);
                bridgeComponentDiagram.StructureCurves.Add(section);
            }
            
            //横隔板多段线
            for (int i = 0; i < numberOfTbeam - 1; i++)
            {
                Polyline rightDiaphragm = new Polyline();
                rightDiaphragm.AddVertexAt(rightDiaphragm.NumberOfVertices, new Point2d(widthOfFlange / 2 - widthOfWetJoint / 2, height - heightOfFlange), 0, 0, 0);
                rightDiaphragm.AddVertexAt(rightDiaphragm.NumberOfVertices, new Point2d(widthOfWeb / 2 + widthOfFlangeHaunch, height - heightOfFlange), 0, 0, 0);
                rightDiaphragm.AddVertexAt(rightDiaphragm.NumberOfVertices, new Point2d(widthOfWeb / 2, height - heightOfFlange - heightOfFlangeHaunch), 0, 0, 0);
                rightDiaphragm.AddVertexAt(rightDiaphragm.NumberOfVertices, new Point2d(widthOfWeb / 2, heightOfButtonBulb + heightOfButtonBulbSlope), 0, 0, 0);
                rightDiaphragm.AddVertexAt(rightDiaphragm.NumberOfVertices, new Point2d(widthOfButtonBulb / 2, heightOfButtonBulb), 0, 0, 0);
                rightDiaphragm.AddVertexAt(rightDiaphragm.NumberOfVertices, new Point2d(widthOfFlange / 2 - widthOfWetJoint / 2, heightOfButtonBulb), 0, 0, 0);
                rightDiaphragm.Closed = true;
                
                double x = widthOfFlange * (numberOfTbeam - 3) / 2 - widthOfFlange * i;
                ModifyTool.MoveByVector(rightDiaphragm, new Vector3d(x, 0, 0));
                Polyline leftDiaphragm = rightDiaphragm.Clone() as Polyline;
                ModifyTool.MirrorEntityByPlane(leftDiaphragm, new Plane(Point3d.Origin, Vector3d.XAxis));
                
                bridgeComponentDiagram.StructureCurves.Add(rightDiaphragm);
                bridgeComponentDiagram.StructureCurves.Add(leftDiaphragm);
            }
            
            //湿接缝多段线
            DBObjectCollection wetJointSections = new DBObjectCollection();
            for (int i = 0; i < numberOfTbeam - 1; i++)
            {
                Polyline wetJoint = new Polyline();
                wetJoint.AddVertexAt(wetJoint.NumberOfVertices, new Point2d(widthOfFlange / 2 + widthOfWetJoint / 2, height), 0, 0, 0);
                wetJoint.AddVertexAt(wetJoint.NumberOfVertices, new Point2d(widthOfFlange / 2 + widthOfWetJoint / 2, heightOfButtonBulb), 0, 0, 0);
                wetJoint.AddVertexAt(wetJoint.NumberOfVertices, new Point2d(widthOfFlange / 2 - widthOfWetJoint / 2, heightOfButtonBulb), 0, 0, 0);
                wetJoint.AddVertexAt(wetJoint.NumberOfVertices, new Point2d(widthOfFlange / 2 - widthOfWetJoint / 2, height), 0, 0, 0);
                wetJoint.Closed = true;
                
                double x = widthOfFlange * (numberOfTbeam - 3) / 2 - widthOfFlange * i;
                ModifyTool.MoveByVector(wetJoint, new Vector3d(x, 0, 0));
                wetJointSections.Add(wetJoint);
                bridgeComponentDiagram.HatchCurves.Add(wetJoint);
            }
            
            //上方一级标注
            Point3dCollection topFirstPoints = new Point3dCollection()
            {
                ((Polyline)beamSections[0]).GetPoint3dAt(13),
                ((Polyline)beamSections[beamSections.Count - 1]).GetPoint3dAt(0)
            };
            DBObjectCollection topFirstDimension = DimensionTool.GetDimensionGroup(topFirstPoints, Math.PI / 2, 25);
            foreach (RotatedDimension dimension in topFirstDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            //左边一级标注
            Point3dCollection leftFirstPoints = new Point3dCollection()
            {
                ((Polyline)beamSections[0]).GetPoint3dAt(13),
                ((Polyline)beamSections[0]).GetPoint3dAt(7)
            };
            DBObjectCollection leftFirstDimension = DimensionTool.GetDimensionGroup(leftFirstPoints, Math.PI, 25);
            bridgeComponentDiagram.Dimensions.Add(leftFirstDimension[0]);
            
            //右边一级标注
            Point3dCollection rightFirstPoints = new Point3dCollection()
            {
                ((Polyline)beamSections[beamSections.Count - 1]).GetPoint3dAt(0),
                ((Polyline)beamSections[beamSections.Count - 1]).GetPoint3dAt(6)
            };
            DBObjectCollection rightFirstDimension = DimensionTool.GetDimensionGroup(rightFirstPoints, 0, 25);
            bridgeComponentDiagram.Dimensions.Add(rightFirstDimension[0]);
            
            
            //下方一级标注
            for (int i = 0; i < beamSections.Count; i++)
            {
                Point3dCollection downFirstPoints;
                if (i == 0)
                {
                    downFirstPoints = new Point3dCollection()
                    {
                        new Point3d(((Polyline)beamSections[0]).GetPoint3dAt(0).X - widthOfWetJoint / 2, 0, 0),
                        ((Polyline)beamSections[0]).GetPoint3dAt(13)
                    };
                    DBObjectCollection downFirstDimensionForFirst = DimensionTool.GetDimensionGroup(downFirstPoints, Math.PI * 3 / 2, 25);
                    bridgeComponentDiagram.Dimensions.Add(downFirstDimensionForFirst[0]);
                }
                
                else if (i == beamSections.Count - 1)
                {
                    downFirstPoints = new Point3dCollection()
                    {
                        new Point3d(((Polyline)beamSections[beamSections.Count - 1]).GetPoint3dAt(13).X + widthOfWetJoint / 2, 0, 0),
                        ((Polyline)beamSections[beamSections.Count - 1]).GetPoint3dAt(0)
                    };
                    DBObjectCollection downFirstDimensionForLast = DimensionTool.GetDimensionGroup(downFirstPoints, Math.PI * 3 / 2, 25);
                    bridgeComponentDiagram.Dimensions.Add(downFirstDimensionForLast[0]);
                }
                else
                {
                    downFirstPoints = new Point3dCollection()
                    {
                        new Point3d(((Polyline)beamSections[i]).GetPoint3dAt(13).X + widthOfWetJoint / 2, 0, 0),
                        new Point3d(((Polyline)beamSections[i]).GetPoint3dAt(0).X - widthOfWetJoint / 2, 0, 0)
                    };
                    DBObjectCollection downFirstDimensionForMiddle = DimensionTool.GetDimensionGroup(downFirstPoints, Math.PI * 3 / 2, 25);
                    bridgeComponentDiagram.Dimensions.Add(downFirstDimensionForMiddle[0]);
                }
            }

            for (int i = 0; i < wetJointSections.Count; i++)
            {
                Point3dCollection downFirstPoints = new Point3dCollection()
                {
                    ((Polyline)wetJointSections[i]).GetPoint3dAt(1),
                    ((Polyline)wetJointSections[i]).GetPoint3dAt(2),
                };
                DBObjectCollection downFirstDimensionForWetJoint = DimensionTool.GetDimensionGroup(downFirstPoints, Math.PI * 3 / 2, 25 + heightOfButtonBulb);
                bridgeComponentDiagram.Dimensions.Add(downFirstDimensionForWetJoint[0]);
            }
            
            
            //下方二级标注
            Point3dCollection downSecondPoints = new Point3dCollection()
            {
                new Point3d(((Polyline)beamSections[0]).GetPoint3dAt(13).X, 0, 0),
            };
            for (int i = 0; i < numberOfTbeam; i++)
            {
                double x = widthOfFlange * i - widthOfFlange * (numberOfTbeam - 1) / 2;
                downSecondPoints.Add(new Point3d(x, 0, 0));
            }
            downSecondPoints.Add(new Point3d(((Polyline)beamSections[beamSections.Count - 1]).GetPoint3dAt(0).X, 0, 0));
            
            
            DBObjectCollection downSecondDimension = DimensionTool.GetDimensionGroup(downSecondPoints, Math.PI * 3 / 2, 50);
            foreach (RotatedDimension dimension in downSecondDimension)
            {
                bridgeComponentDiagram.Dimensions.Add(dimension);
            }
            
            
            return bridgeComponentDiagram;
        }
    }
}