using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace CadBridgeDesigner.Tool.Tool.CadTool.EntityTool
{
    public static class ModifyTool
    {
        /// <summary>
        /// 根据两个向量之间的夹角旋转实体
        /// </summary>
        /// <param name="entity">要旋转的CAD实体对象</param>
        /// <param name="basePoint">旋转基准点（旋转中心点）</param>
        /// <param name="axis">旋转轴向量（绕哪个轴旋转）</param>
        /// <param name="baseVector">起始方向向量（当前方向）</param>
        /// <param name="targetVector">目标方向向量（要旋转到的方向）</param>
        public static void RotateEntityByTwoVector(Entity entity, Point3d basePoint, Vector3d axis, Vector3d baseVector, Vector3d targetVector)
        {
            if (entity == null) { return; }
            baseVector = baseVector.GetNormal();
            targetVector = targetVector.GetNormal();
            entity.TransformBy(Matrix3d.Rotation(baseVector.GetAngleTo(targetVector, axis), axis, basePoint));
        }

        /// <summary>
        /// 根据指定角度旋转实体
        /// </summary>
        /// <param name="entity">要旋转的CAD实体对象</param>
        /// <param name="basePoint">旋转基准点（旋转中心点）</param>
        /// <param name="axis">旋转轴向量（绕哪个轴旋转）</param>
        /// <param name="angle">旋转角度（弧度制，正值逆时针旋转）</param>
        public static void RotateEntityByAngle(Entity entity, Point3d basePoint, Vector3d axis, double angle)
        {
            if (entity == null) { return; }
            Matrix3d rotationMatrix = Matrix3d.Rotation(angle, axis, basePoint);
            entity.TransformBy(rotationMatrix);
        }

        /// <summary>
        /// 将实体从基准点移动到目标点
        /// </summary>
        /// <param name="entity">要移动的CAD实体对象</param>
        /// <param name="basePoint">移动基准点（移动前的起点位置）</param>
        /// <param name="targetPoint">移动目标点（移动后的终点位置）</param>
        public static void MoveByTwoPoint(Entity entity, Point3d basePoint, Point3d targetPoint)
        {
            if (entity == null) { return; }
            Vector3d vector = targetPoint - basePoint;
            entity.TransformBy(Matrix3d.Displacement(vector));
        }
        
        /// <summary>
        /// 根据向量移动实体
        /// </summary>
        /// <param name="entity">要移动的CAD实体对象</param>
        /// <param name="vector">移动向量</param>
        public static void MoveByVector(Entity entity, Vector3d vector)
        {
            if (entity == null) { return; }
            entity.TransformBy(Matrix3d.Displacement(vector));
        }
        /// <summary>
        /// 根据平面对实体进行镜像
        /// </summary>
        /// <param name="entity">要镜像的CAD实体对象</param>
        /// <param name="plane">镜像平面（实体关于此平面对称翻转）</param>
        public static void MirrorEntityByPlane(Entity entity, Plane plane)
        {
            if (entity == null) { return; }
            Matrix3d mirrorMatrix = Matrix3d.Mirroring(plane);
            entity.TransformBy(mirrorMatrix);
        }
    }
}
    