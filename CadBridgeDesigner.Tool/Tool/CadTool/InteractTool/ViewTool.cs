using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;

namespace CadBridgeDesigner.Tool.Tool.CadTool.InteractTool
{
    public static class ViewTool
    {
        private static Extents3d CalculateExtents(DBObjectCollection entities)
        {
            Extents3d extents3D = new Extents3d();
            bool isFirst = true;
            foreach (Entity entity in entities)
            {
                if (entity == null) continue;

                if (isFirst)
                {
                    extents3D = entity.GeometricExtents;
                    isFirst = false;
                }
                else
                {
                    extents3D.AddExtents(entity.GeometricExtents);
                }
            }
            return extents3D;
        }
        private static void ZoomToExtents(Document document, Extents3d extents)
        {
            Editor editor = document.Editor;
            ViewTableRecord currentView = editor.GetCurrentView();

            double minX = extents.MinPoint.X;
            double minY = extents.MinPoint.Y;
            double maxX = extents.MaxPoint.X;
            double maxY = extents.MaxPoint.Y;

            double centerX = (minX + maxX) / 2;
            double centerY = (minY + maxY) / 2;
            double width = (maxX - minX) * 2;
            double height = (maxY - minY) * 2;

            double startX = currentView.CenterPoint.X;
            double startY = currentView.CenterPoint.Y;
            double startW = currentView.Width;
            double startH = currentView.Height;

            int frames = 30;
            int i = 1;
            while (i <= frames)
            {
                double t = (double)i / frames;

                double x = startX + (centerX - startX) * t;
                double y = startY + (centerY - startY) * t;
                double w = startW + (width - startW) * t;
                double h = startH + (height - startH) * t;

                using (ViewTableRecord viewTableRecord = editor.GetCurrentView())
                {
                    viewTableRecord.CenterPoint = new Point2d(x, y);
                    viewTableRecord.Width = w;
                    viewTableRecord.Height = h;
                    viewTableRecord.ViewDirection = Vector3d.ZAxis;
                    viewTableRecord.Target = Point3d.Origin;
                    editor.SetCurrentView(viewTableRecord);
                }

                System.Threading.Thread.Sleep(15);
                i++;
            }
            editor.Regen();
        }

        public static void ZoomEntities(Document document, ObjectIdCollection entityIds)
        {
            Extents3d extents;
            using (document.LockDocument())
            {
                using (Transaction transaction = document.Database.TransactionManager.StartTransaction())
                {
                    DBObjectCollection entities = new DBObjectCollection();
                    foreach (ObjectId entityId in entityIds)
                    {
                        entities.Add(transaction.GetObject(entityId, OpenMode.ForRead));
                    }
                    extents = CalculateExtents(entities);
                    transaction.Commit();
                }
            }

            ZoomToExtents(document, extents);
            document.Editor.Regen();
        }

        public static void ZoomBlockReference(Document document, ObjectId blockReferenceId)
        {
            if (blockReferenceId.IsNull)
            {
                return;
            }
            Extents3d extents;
            using (document.LockDocument())
            {
                using (Transaction transaction = document.Database.TransactionManager.StartTransaction())
                {
                    BlockReference blockReference = transaction.GetObject(blockReferenceId, OpenMode.ForRead) as BlockReference;
                    if (blockReference == null)
                    {
                        return;
                    }

                    extents = blockReference.GeometricExtents;
                    transaction.Commit();
                }
            }

            ZoomToExtents(document, extents);
            document.Editor.Regen();
        }

    }
}


