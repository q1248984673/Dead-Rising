using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// JSONのシリアライズ／デシリアライズに使用する方式
/// </summary>
public enum JsonType
{
    JsonUtlity,
    LitJson,
}

/// <summary>
/// JSONデータ管理クラス  
/// JSONをハードディスクへシリアライズ保存、またはハードディスクからメモリへデシリアライズ読み込みを行う
/// </summary>
public class JsonMgr
{
    private static JsonMgr instance = new JsonMgr();
    public static JsonMgr Instance => instance;

    private JsonMgr() { }

    // JSONデータを保存（シリアライズ）
    public void SaveData(object data, string fileName, JsonType type = JsonType.LitJson)
    {
        // 保存パスを決定
        string path = Application.persistentDataPath + "/" + fileName + ".json";
        // シリアライズしてJSON文字列を取得
        string jsonStr = "";
        switch (type)
        {
            case JsonType.JsonUtlity:
                jsonStr = JsonUtility.ToJson(data);
                break;
            case JsonType.LitJson:
                jsonStr = JsonMapper.ToJson(data);
                break;
        }
        // シリアライズしたJSON文字列を指定ファイルへ書き込む
        File.WriteAllText(path, jsonStr);
    }

    // 指定ファイルからJSONデータを読み込み（デシリアライズ）
    public T LoadData<T>(string fileName, JsonType type = JsonType.LitJson) where T : new()
    {
        // 読み込みパスを決定
        // まずデフォルトのデータフォルダに対象ファイルがあるか確認
        string path = Application.streamingAssetsPath + "/" + fileName + ".json";
        // 存在しない場合は書き込み可能フォルダから探す
        if (!File.Exists(path))
            path = Application.persistentDataPath + "/" + fileName + ".json";
        // どちらにも存在しない場合はデフォルトオブジェクトを返す
        if (!File.Exists(path))
            return new T();

        // デシリアライズ処理
        string jsonStr = File.ReadAllText(path);
        // データオブジェクト
        T data = default(T);
        switch (type)
        {
            case JsonType.JsonUtlity:
                data = JsonUtility.FromJson<T>(jsonStr);
                break;
            case JsonType.LitJson:
                data = JsonMapper.ToObject<T>(jsonStr);
                break;
        }

        // オブジェクトを返す
        return data;
    }
}
