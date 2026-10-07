using System;
using Autodesk.AutoCAD.ApplicationServices;
using CadBridgeDesigner.Core.CadCommandManager;
using CadBridgeDesigner.Tool.Bridge.Beam.Tbeam;
using CadBridgeDesigner.Tool.Bridge.Pier.ColumnPier;
using CadBridgeDesigner.Tool.Loger;
using CadBridgeDesigner.Tool.Tool;

namespace CadBridgeDesigner.Tool.CadCommandManager
{
    public class CadCommandManagerHandler: ICadCommandManagerHandler
    {
        public void AddColumnPierCommand(Document document)
        {
            try
            {
                LogerTool.Log("[AddColumnPierCommand]添加柱式墩");
                if (document == null)
                {
                    LogerTool.Log("[AddColumnPierCommand]文档为空，添加失败");
                    return;
                }
                ColumnPierTool.AddColumnPierCommand(document);
            }
            catch (Exception exception)
            {
                LogerTool.Log("[AddColumnPierCommand]添加失败: " + exception.GetDetail());
            }
        }

        public void AddTbeamCommand(Document document)
        {
            try
            {
                LogerTool.Log("[AddTbeamCommand]添加桥墩");
                if (document == null)
                {
                    LogerTool.Log("[AddTbeamCommand]文档为空，添加失败");
                    return;
                }
                TbeamTool.AddTbeamCommand(document);
            }
            catch (Exception exception)
            {
                LogerTool.Log("[AddTbeamCommand]添加失败: " + exception.GetDetail());
            }
        }

        public void TestMethod1(Document document)
        {
            try
            {
                LogerTool.Log("[TestMethod1]测试方法1");
                if (document == null)
                {
                    LogerTool.Log("[TestMethod1]文档为空，添加失败");
                    return;
                }
                TestMethod.TestMethod1(document);
            }
            catch (Exception exception)
            {
                LogerTool.Log("[TestMethod1]测试失败: " + exception.GetDetail());
            }
        }
    }
}


