using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExcelToJson
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("请提供Excel文件路径作为参数（需先另存为CSV格式）");
                Console.WriteLine("用法: ExcelToJsonConverter.exe <ExcelFilePath>");
                Console.WriteLine("注意：必须先将Excel文件另存为CSV（UTF-8）格式");
                return;
            }

            string filePath = args[0];

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"错误: 文件不存在: {filePath}");
                return;
            }

            string extension = Path.GetExtension(filePath).ToLower();
            if (extension != ".csv")
            {
                Console.WriteLine("错误: 仅支持CSV文件（请先将Excel另存为CSV格式）");
                Console.WriteLine("如何另存为CSV: Excel中点击'文件'->'另存为'->选择CSV(逗号分隔)格式");
                return;
            }

            Console.WriteLine($"正在处理CSV文件: {filePath}");
            string csvContent = File.ReadAllText(filePath, Encoding.UTF8);

            // 解析CSV为数据列表
            var data = ParseCsv(csvContent);
            if (data.Count == 0)
            {
                Console.WriteLine("错误: CSV文件为空或无效");
                return;
            }

            // 生成JSON
            string jsonContent = ConvertToJson(data);
            string outputJsonPath = Path.ChangeExtension(filePath, ".json");

            try
            {
                File.WriteAllText(outputJsonPath, jsonContent, Encoding.UTF8);
                Console.WriteLine($"成功生成JSON: {outputJsonPath}");
                Console.WriteLine($"共转换 {data.Count} 条记录");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"错误: 保存JSON文件失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 解析CSV内容为对象列表
        /// </summary>
        private static List<Dictionary<string, object>> ParseCsv(string csvContent)
        {
            var result = new List<Dictionary<string, object>>();
            var lines = csvContent.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length == 0) return result;

            // 提取标题行
            var headers = SplitCsvLine(lines[0]);
            if (headers.Length == 0) return result;

            // 解析数据行
            for (int i = 1; i < lines.Length; i++)
            {
                var values = SplitCsvLine(lines[i]);
                if (values.Length == 0) continue;

                var row = new Dictionary<string, object>();
                for (int j = 0; j < headers.Length; j++)
                {
                    if (j >= values.Length) break;

                    string header = headers[j].Trim();
                    string value = values[j].Trim();

                    // 类型推断
                    if (string.IsNullOrWhiteSpace(value))
                        row[header] = null;
                    else if (IsNumeric(value))
                        row[header] = Convert.ToDecimal(value);
                    else if (IsBoolean(value))
                        row[header] = bool.Parse(value);
                    else
                        row[header] = value;
                }
                result.Add(row);
            }
            return result;
        }

        /// <summary>
        /// 分割CSV行（简单实现，不处理引号内逗号）
        /// </summary>
        private static string[] SplitCsvLine(string line)
        {
            return line.Split(',');
        }

        /// <summary>
        /// 检查字符串是否为数字
        /// </summary>
        private static bool IsNumeric(string value)
        {
            return decimal.TryParse(value, out _);
        }

        /// <summary>
        /// 检查字符串是否为布尔值
        /// </summary>
        private static bool IsBoolean(string value)
        {
            return bool.TryParse(value, out _);
        }

        /// <summary>
        /// 手动将数据转换为JSON（无库依赖）
        /// </summary>
        private static string ConvertToJson(List<Dictionary<string, object>> data)
        {
            var sb = new StringBuilder();
            sb.AppendLine("[");

            for (int i = 0; i < data.Count; i++)
            {
                sb.Append("  {");
                var entries = new List<string>();

                foreach (var kvp in data[i])
                {
                    string key = EscapeJson(kvp.Key);
                    string value = FormatJsonValue(kvp.Value);
                    entries.Add($"\"{key}\": {value}");
                }

                sb.AppendLine(string.Join(", ", entries));
                sb.Append("}");

                if (i < data.Count - 1)
                    sb.Append(",");
                sb.AppendLine();
            }

            sb.AppendLine("]");
            return sb.ToString();
        }

        /// <summary>
        /// 格式化JSON值（处理null、字符串、数字、布尔值）
        /// </summary>
        private static string FormatJsonValue(object value)
        {
            if (value == null) return "null";

            if (value is string str)
                return $"\"{EscapeJson(str)}\"";

            if (value is bool boolVal)
                return boolVal.ToString().ToLower();

            // 数字类型
            if (value is decimal || value is int || value is float || value is double)
                return value.ToString();

            // 其他类型转为字符串
            return $"\"{EscapeJson(value.ToString())}\"";
        }

        /// <summary>
        /// 转义JSON特殊字符
        /// </summary>
        private static string EscapeJson(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            return text
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\b", "\\b")
                .Replace("\f", "\\f")
                .Replace("\n", "\\n")
                .Replace("\r", "\\r")
                .Replace("\t", "\\t");
        }
    }
}