using System.Linq;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;


namespace CadBridgeDesigner.Tool.Tool.CadTool.InteractTool
{
    /// <summary>
    /// 编辑器工具类，提供与AutoCAD命令行交互相关的静态方法
    /// </summary>
    public static class EditorTool
    {
        /// <summary>
        /// 从编辑器获取用户输入的字符串
        /// </summary>
        /// <param name="document">当前CAD文档对象</param>
        /// <param name="promptMessage">命令行提示消息</param>
        /// <returns>用户输入的字符串，取消则返回空字符串string.Empty</returns>
        public static string GetString(Document document, string promptMessage)
        {
            Editor editor = document.Editor;
            PromptStringOptions promptOptions = new PromptStringOptions($"\n{promptMessage}") { AllowSpaces = true };
            PromptResult promptResult = editor.GetString(promptOptions);
            if (promptResult.Status != PromptStatus.OK)
            {
                editor.WriteMessage("用户取消了操作\n");
                return string.Empty;
            }
            return promptResult.StringResult;
        }

        /// <summary>
        /// 从编辑器获取用户输入的整数
        /// </summary>
        /// <param name="document">当前CAD文档对象</param>
        /// <param name="promptMessage">命令行提示消息</param>
        /// <returns>用户输入的整数，取消则返回-1</returns>
        public static int GetInteger(Document document, string promptMessage)
        {
            Editor editor = document.Editor;
            PromptIntegerOptions promptOptions = new PromptIntegerOptions($"\n{promptMessage}");
            PromptIntegerResult promptResult = editor.GetInteger(promptOptions);
            if (promptResult.Status != PromptStatus.OK)
            {
                editor.WriteMessage("用户取消了操作\n");
                return -1;
            }
            return promptResult.Value;
        }

        /// <summary>
        /// 从编辑器获取用户输入的双精度浮点数
        /// </summary>
        /// <param name="document">当前CAD文档对象</param>
        /// <param name="promptMessage">命令行提示消息</param>
        /// <returns>用户输入的双精度浮点数，取消则返回double.NaN</returns>
        public static double GetDouble(Document document, string promptMessage)
        {
            Editor editor = document.Editor;
            PromptDoubleOptions promptOptions = new PromptDoubleOptions($"\n{promptMessage}");
            PromptDoubleResult promptResult = editor.GetDouble(promptOptions);
            if (promptResult.Status != PromptStatus.OK)
            {
                editor.WriteMessage("用户取消了操作\n");
                return double.NaN;
            }
            return promptResult.Value;
        }

        /// <summary>
        /// 从编辑器获取用户拾取的三维点坐标
        /// </summary>
        /// <param name="document">当前CAD文档对象</param>
        /// <param name="promptMessage">命令行提示消息</param>
        /// <returns>用户拾取的三维点，取消则返回NaN点 new Point3d(double.NaN, double.NaN, double.NaN)</returns>
        private static Point3d GetPoint3D(Document document , string promptMessage)
        {
            Editor editor = document.Editor;
            PromptPointOptions promptOptions = new PromptPointOptions($"\n{promptMessage}");
            PromptPointResult promptResult = document.Editor.GetPoint(promptOptions);
            if (promptResult.Status != PromptStatus.OK)
            {
                editor.WriteMessage("用户取消了操作\n");
                return new Point3d(double.NaN, double.NaN, double.NaN);
            }
            return promptResult.Value;
        }
        
        /// <summary>
        /// 从编辑器获取用户选择的曲线对象
        /// </summary>
        /// <param name="document">当前CAD文档对象</param>
        /// <param name="promptMessage">命令行提示消息</param>
        /// <returns>用户选择的曲线对象，取消则返回null</returns>
        public static Curve3d GetCurve3D(Document document, string promptMessage)
        {
            Editor editor = document.Editor;
            PromptEntityOptions promptEntityOptions = new PromptEntityOptions($"\n{promptMessage}");
            promptEntityOptions.SetRejectMessage("\n请选择一条曲线:");
            promptEntityOptions.AddAllowedClass(typeof(Curve), false);
            PromptEntityResult promptEntityResult = editor.GetEntity(promptEntityOptions);
            if (promptEntityResult.Status != PromptStatus.OK)
            {
                editor.WriteMessage("用户取消了操作\n");
                return null;
            }

            using (document.LockDocument())
            {
                using (Transaction transaction = document.Database.TransactionManager.StartTransaction())
                {
                    Curve curve = transaction.GetObject(promptEntityResult.ObjectId, OpenMode.ForRead) as Curve;
                    if (curve == null) { return null; }
                    Curve3d curve3D = curve.GetGeCurve();
                    return curve3D;
                }
            }
            
        }
        
        
        /// <summary>
        /// 从编辑器获取用户选择的直线对象的ObjectId
        /// </summary>
        /// <param name="document">当前CAD文档对象</param>
        /// <param name="promptMessage">命令行提示消息</param>
        /// <returns>直线的ObjectId，取消则返回ObjectId.Null</returns>
        public static ObjectId GetLineId(Document document, string promptMessage)
        {
            Editor editor = document.Editor;
            PromptEntityOptions promptEntityOptions = new PromptEntityOptions($"\n{promptMessage}");
            promptEntityOptions.SetRejectMessage("\n请选择一条直线:");
            promptEntityOptions.AddAllowedClass(typeof(Line), true);
            PromptEntityResult promptEntityResult = editor.GetEntity(promptEntityOptions);
            if (promptEntityResult.Status != PromptStatus.OK)
            {
                editor.WriteMessage("用户取消了操作\n");
                return ObjectId.Null;
            }
            return promptEntityResult.ObjectId;
        }

        /// <summary>
        /// 从编辑器获取用户选择的多段线对象的ObjectId
        /// </summary>
        /// <param name="document">当前CAD文档对象</param>
        /// <param name="promptMessage">命令行提示消息</param>
        /// <returns>多段线的ObjectId，取消则返回ObjectId.Null</returns>
        public static ObjectId GetPolylineId(Document document, string promptMessage)
        {
            Editor editor = document.Editor;
            PromptEntityOptions promptEntityOptions = new PromptEntityOptions(promptMessage);
            promptEntityOptions.SetRejectMessage("\n请选择一条多段线");
            promptEntityOptions.AddAllowedClass(typeof(Polyline), true);
            PromptEntityResult promptEntityResult = editor.GetEntity(promptEntityOptions);
            if (promptEntityResult.Status != PromptStatus.OK)
            {
                editor.WriteMessage("用户取消了操作\n");
                return ObjectId.Null;
            }
            return promptEntityResult.ObjectId;
        }

        /// <summary>
        /// 通过编辑器交互获取两个点创建三维直线
        /// </summary>
        /// <param name="document">当前CAD文档对象</param>
        /// <returns>创建的三维直线对象，取消则返回null</returns>
        public static LineSegment3d GetLineSegment3DByTwoPoint(Document document)
        {
            Editor editor = document.Editor;
            Point3d startPoint3D = GetPoint3D(document, "选择起始点");
            if (double.IsNaN(startPoint3D.X))
            {
                editor.WriteMessage("用户取消了操作\n");
                return null;
            }
            Point3d endPoint3D = GetPoint3D(document, "选择终点");
            if (double.IsNaN(endPoint3D.X))
            {
                editor.WriteMessage("用户取消了操作\n");
                return null;
            }
            LineSegment3d lineSegment3D = new LineSegment3d(startPoint3D, endPoint3D);
            return lineSegment3D;
        }

        /// <summary>
        /// 通过编辑器交互获取两个点创建三维直线
        /// </summary>
        /// <param name="document">当前CAD文档对象</param>
        /// <returns>创建的三维直线对象，取消则返回null</returns>
        public static Line3d GetLine3DByTwoPoint(Document document)
        {
            Editor editor = document.Editor;
            Point3d startPoint3D = GetPoint3D(document, "选择起始点");
            if (startPoint3D == new Point3d(double.NaN, double.NaN, double.NaN))
            {
                editor.WriteMessage("用户取消了操作\n");
                return null;
            }
            Point3d endPoint3D = GetPoint3D(document, "选择终点");
            if (endPoint3D == new Point3d(double.NaN, double.NaN, double.NaN))
            {
                editor.WriteMessage("用户取消了操作\n");
                return null;
            }
            Line3d line3D = new Line3d(startPoint3D, endPoint3D);
            return line3D;
        }

        /// <summary>
        /// 通过编辑器交互获取三个点创建三维圆弧
        /// </summary>
        /// <param name="document">当前CAD文档对象</param>
        /// <returns>创建的三维圆弧对象，取消则返回null</returns>
        public static CircularArc3d GetCircularArc3DByThreePoints(Document document)
        {
            Editor editor = document.Editor;
            Point3d startPoint = GetPoint3D(document, "\n选择起始点");
            if (double.IsNaN(startPoint.X))
            {
                editor.WriteMessage("用户取消了操作\n");
                return null;
            }
            Point3d middlePoint = GetPoint3D(document, "\n请选择曲中点");
            if (double.IsNaN(middlePoint.X))
            {
                editor.WriteMessage("用户取消了操作\n");
                return null;
            }
            Point3d endPoint = GetPoint3D(document, "\n请选择终点");
            if (double.IsNaN(endPoint.X))
            {
                editor.WriteMessage("用户取消了操作\n");
                return null;
            }
            CircularArc3d arc = new CircularArc3d(startPoint, middlePoint, endPoint);
            return arc;
        }

        /// <summary>
        /// 根据ObjectId列表设置CAD的选择集并高亮显示
        /// </summary>
        /// <param name="document">当前CAD文档对象</param>
        /// <param name="objectIds">要选中实体的ObjectId列表</param>
        public static void SetSelectionByObjectIds(Document document, ObjectIdCollection objectIds)
        {
            Editor editor = document.Editor;
            ObjectId[] objectIdArray = objectIds.Cast<ObjectId>().ToArray();
            editor.SetImpliedSelection(objectIdArray);
        }
    }
}