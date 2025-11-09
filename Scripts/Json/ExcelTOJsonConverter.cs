using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

//namespace ExcelToJsonConverter
//{
    public static class ExcelToJsonConverter
    {
        /// <summary>
        /// 将文件夹中的所有CSV文件转换为JSON格式
        /// </summary>
        /// <param name="inputFolder">包含CSV文件的输入文件夹路径</param>
        /// <param name="outputFolder">保存JSON文件的输出文件夹路径</param>
        /// <returns>转换成功的文件数量</returns>
        public static int ConvertFolder(string inputFolder, string outputFolder)
        {
            // 验证输入文件夹
            if (!Directory.Exists(inputFolder))
                throw new DirectoryNotFoundException($"输入文件夹不存在: {inputFolder}");

            // 确保输出文件夹存在
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            int processedCount = 0;

            // 处理文件夹中所有CSV文件
            foreach (string csvFile in Directory.GetFiles(inputFolder, "*.csv", SearchOption.TopDirectoryOnly))
            {
                string fileName = Path.GetFileNameWithoutExtension(csvFile);
                string jsonFile = Path.Combine(outputFolder, $"{fileName}.json");

                try
                {
                    string csvContent = File.ReadAllText(csvFile, Encoding.UTF8);
                    var data = ParseCsv(csvContent);

                    if (data.Count == 0)
                    {
                        // 跳过空文件
                        continue;
                    }

                    string jsonContent = ConvertToJson(data);
                    File.WriteAllText(jsonFile, jsonContent, Encoding.UTF8);

                    processedCount++;
                }
                catch (Exception ex)
                {
                    // 在实际应用中，可以添加日志记录
                    throw new InvalidOperationException($"转换文件失败: {Path.GetFileName(csvFile)}", ex);
                }
            }

            return processedCount;
        }

        /// <summary>
        /// 解析CSV内容为数据列表
        /// </summary>
        private static List<Dictionary<string, object>> ParseCsv(string csvContent)
        {
            var result = new List<Dictionary<string, object>>();
            var lines = csvContent.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length < 2) return result; // 至少需要标题行和数据行

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
//}