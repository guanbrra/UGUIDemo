using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum JsonType
{
    JsonUtility, // Unity 内置库
    LitJson      // 第三方库
}

/// <summary>
/// JSON管理类（JsonMgr），采用「单例模式」设计
/// 确保全局仅存在一个实例，统一处理JSON相关操作（如序列化、反序列化、配置读取等）
/// 避免多实例导致的配置不一致、资源浪费问题
/// </summary>
public class JsonMgr
{
    private static JsonMgr instance;
    public static JsonMgr Instance => instance ?? (instance = new JsonMgr());
    private JsonMgr() { }

    //存储Json数据序列化
    public void SaveData(object data, JsonType jsonType = JsonType.LitJson, string fileName = "data")
    {
        string path =$"{Application.persistentDataPath}/{fileName}.json";
        string json = "";
        switch (jsonType)
        {
            case JsonType.JsonUtility:
                json = JsonUtility.ToJson(data);
                break;
            case JsonType.LitJson:
                json = JsonMapper.ToJson(data);
                break;
        }

        //存储到指定文件中
        File.WriteAllText(path, json);

    }
    /// <summary>
    /// 异步加载 JSON 数据并反序列化为 T 类型
    /// </summary>
    /// <typeparam name="T">目标类型（需有无参构造函数；若用 JsonUtility，需加 [Serializable] 特性）</typeparam>
    /// <param name="jsonType">JSON 解析库</param>
    /// <param name="fileName">JSON 文件名（无需带 .json 后缀）</param>
    /// <returns>反序列化后的 T 对象（文件不存在/解析失败时返回 new T()）</returns>
    public T LoadData<T>(JsonType jsonType = JsonType.LitJson, string fileName = "data") where T : new()
    {
        //泛型约束 where T : new()：要求 T 必须有 无参默认构造函数，因为当文件不存在时会返回 new T()（避免空引用）。
        //读取指定文件中的Json数据
        string path = $"{Application.streamingAssetsPath}/{fileName}.json";
        if (!File.Exists(path))
        {
            path = $"{Application.persistentDataPath}/{fileName}.json";
        }
        if (!File.Exists(path))
            return new T();
        string json = File.ReadAllText(path); // 同步读取文件内容为字符串
        T data = default(T); // 初始化默认值（值类型为默认值，引用类型为 null）

        // 根据选择的 JSON 库反序列化
        switch (jsonType)
        {
            case JsonType.JsonUtility:
                data = JsonUtility.FromJson<T>(json);
                break;
            case JsonType.LitJson:
                data = JsonMapper.ToObject<T>(json);
                break;
        }
        return data;
    }
}