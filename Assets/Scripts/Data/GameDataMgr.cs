using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// データを管理する専用クラス
/// </summary>
public class GameDataMgr
{
    private static GameDataMgr instance = new GameDataMgr();
    public static GameDataMgr Instance => instance;

    // 選択中のキャラクターデータを記録し、後でゲームシーンで生成するために使用
    public RoleInfo nowSelRole;

    // サウンド関連データ
    public MusicData musicData;

    // プレイヤー関連データ
    public PlayerData playerData;

    // すべてのキャラクターデータ
    public List<RoleInfo> roleInfoList;

    // すべてのシーンデータ
    public List<SceneInfo> sceneInfoList;

    // すべてのモンスターデータ
    public List<MonsterInfo> monsterInfoList;

    // すべてのタワーデータ
    public List<TowerInfo> towerInfoList;

    private GameDataMgr()
    {
        // いくつかのデフォルトデータを初期化
        musicData = JsonMgr.Instance.LoadData<MusicData>("MusicData");
        // プレイヤーデータを初期化
        playerData = JsonMgr.Instance.LoadData<PlayerData>("PlayerData");
        // キャラクターデータを読み込む
        roleInfoList = JsonMgr.Instance.LoadData<List<RoleInfo>>("RoleInfo");
        // シーンデータを読み込む
        sceneInfoList = JsonMgr.Instance.LoadData<List<SceneInfo>>("SceneInfo");
        // モンスターデータを読み込む
        monsterInfoList = JsonMgr.Instance.LoadData<List<MonsterInfo>>("MonsterInfo");
        // タワーデータを読み込む
        towerInfoList = JsonMgr.Instance.LoadData<List<TowerInfo>>("TowerInfo");
    }

    /// <summary>
    /// サウンドデータを保存
    /// </summary>
    public void SaveMusicData()
    {
        JsonMgr.Instance.SaveData(musicData, "MusicData");
    }

    /// <summary>
    /// プレイヤーデータを保存
    /// </summary>
    public void SavePlayerData()
    {
        JsonMgr.Instance.SaveData(playerData, "PlayerData");
    }

    /// <summary>
    /// 効果音を再生するメソッド
    /// </summary>
    /// <param name="resName"></param>
    public void PlaySound(string resName)
    {
        GameObject musicObj = new GameObject();
        AudioSource a = musicObj.AddComponent<AudioSource>();
        a.clip = Resources.Load<AudioClip>(resName);
        a.volume = musicData.soundValue;
        a.mute = !musicData.soundOpen;
        a.Play();

        GameObject.Destroy(musicObj, 1);
    }
}
