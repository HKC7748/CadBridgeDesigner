using System.Text;
using Exception = System.Exception;

namespace CadBridgeDesigner.Tool.Tool
{
    public static class ExceptionTool
    {
        /// <summary>
        /// 获取异常的所有详细信息，包括内部异常和堆栈跟踪
        /// </summary>
        public static string GetDetail(this Exception exception)
        {
            if (exception == null)
                return string.Empty;

            StringBuilder stringBuilder = new StringBuilder();

            Exception current = exception;
            bool isFirst = true;
    
            while (current != null)
            {
                if (!isFirst)
                {
                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine("--- 内部异常 ---");
                }
        
                stringBuilder.AppendLine($"异常类型: {current.GetType().FullName}");
                stringBuilder.AppendLine($"消息: {current.Message}");
        
                if (current is Autodesk.AutoCAD.Runtime.Exception acadEx)
                {
                    stringBuilder.AppendLine($"ErrorStatus: {acadEx.ErrorStatus}");
                }
                if (!string.IsNullOrEmpty(current.Source))
                {
                    stringBuilder.AppendLine($"来源: {current.Source}");
                }
                if (current.TargetSite != null)
                {
                    stringBuilder.AppendLine($"方法: {current.TargetSite}");
                }
                if (!string.IsNullOrEmpty(current.StackTrace))
                {
                    stringBuilder.AppendLine("堆栈跟踪:");
                    stringBuilder.AppendLine(current.StackTrace);
                }

                current = current.InnerException;
                isFirst = false;
            }

            return stringBuilder.ToString();
        }
    }
}