using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChooseHeroPanel : BasePanel
{
    // 左右ボタン
    public Button btnLeft;
    public Button btnRight;

    // 購入ボタン
    public Button btnUnLock;
    public Text txtUnLock;

    // スタートと戻る
    public Button btnStart;
    public Button btnBack;

    // 左上に表示される所持金
    public Text txtMoney;

    // キャラクター情報
    public Text txtName;

    // ヒーロープレハブを生成する位置
    private Transform heroPos;

    // 現在シーンに表示されているオブジェクト
    private GameObject heroObj;
    // 現在使用しているデータ
    private RoleInfo nowRoleData;
    // 現在使用しているデータのインデックス
    private int nowIndex;

    public override void Init()
    {
        // シーン内でオブジェクトプレハブを配置する位置を探す
        heroPos = GameObject.Find("HeroPos").transform;

        // 左上にプレイヤーの所持金を更新
        txtMoney.text = GameDataMgr.Instance.playerData.haveMoney.ToString();

        btnLeft.onClick.AddListener(() =>
        {
            --nowIndex;
            if (nowIndex < 0)
                nowIndex = GameDataMgr.Instance.roleInfoList.Count - 1;
            // モデルの更新
            ChangeHero();
        });

        btnRight.onClick.AddListener(() =>
        {
            ++nowIndex;
            if (nowIndex >= GameDataMgr.Instance.roleInfoList.Count)
                nowIndex = 0;
            // モデルの更新
            ChangeHero();
        });

        btnUnLock.onClick.AddListener(() =>
        {
            // アンロックボタンを押したときの処理
            PlayerData data = GameDataMgr.Instance.playerData;
            // 所持金が十分な場合
            if (data.haveMoney >= nowRoleData.lockMoney)
            {
                // 購入処理
                // コストを差し引く
                data.haveMoney -= nowRoleData.lockMoney;
                // UIの表示を更新
                txtMoney.text = data.haveMoney.ToString();
                // 購入済みIDを記録
                data.buyHero.Add(nowRoleData.id);
                // データを保存
                GameDataMgr.Instance.SavePlayerData();

                // アンロックボタンの更新
                UpdateLockBtn();

                // メッセージパネルで購入成功を表示
                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("Purchase Successful");
            }
            else
            {
                // メッセージパネルでお金不足を表示
                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("Not enough money");
            }
        });

        btnStart.onClick.AddListener(() =>
        {
            // ① 現在選択中のキャラクターを記録
            GameDataMgr.Instance.nowSelRole = nowRoleData;

            // ② 自分を非表示にしてシーン選択画面を表示
            UIManager.Instance.HidePanel<ChooseHeroPanel>();
            UIManager.Instance.ShowPanel<ChooseScenePanel>();
        });

        btnBack.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<ChooseHeroPanel>();
            // カメラを右に回転させた後、開始画面を表示
            Camera.main.GetComponent<CameraAnimator>().TurnRgiht(() =>
            {
                UIManager.Instance.ShowPanel<BeginPanel>();
            });
        });

        // モデル表示を更新
        ChangeHero();
    }

    /// <summary>
    /// シーン上に表示するモデルを更新
    /// </summary>
    private void ChangeHero()
    {
        if (heroObj != null)
        {
            Destroy(heroObj);
            heroObj = null;
        }

        // インデックスに基づいてデータを取得
        nowRoleData = GameDataMgr.Instance.roleInfoList[nowIndex];
        // オブジェクトを生成し、次回切り替え時に削除できるように記録
        heroObj = Instantiate(Resources.Load<GameObject>(nowRoleData.res), heroPos.position, heroPos.rotation);
        // 現在は開始シーンなので、PlayerObjectを削除
        Destroy(heroObj.GetComponent<PlayerObject>());

        // 上部に表示される説明文を更新
        txtName.text = nowRoleData.tips;

        // アンロック状態に応じてボタン表示を切り替える
        UpdateLockBtn();
    }

    /// <summary>
    /// アンロックボタンの表示を更新
    /// </summary>
    private void UpdateLockBtn()
    {
        // このキャラクターがアンロック必要かつ未購入なら、アンロックボタンを表示し、スタートボタンを隠す
        if (nowRoleData.lockMoney > 0 && !GameDataMgr.Instance.playerData.buyHero.Contains(nowRoleData.id))
        {
            // アンロックボタンと価格を更新
            btnUnLock.gameObject.SetActive(true);
            txtUnLock.text = "￥" + nowRoleData.lockMoney;
            // アンロックされていないため、スタートボタンを隠す
            btnStart.gameObject.SetActive(false);
        }
        else
        {
            btnUnLock.gameObject.SetActive(false);
            btnStart.gameObject.SetActive(true);
        }
    }

    public override void HideMe(UnityAction callBack)
    {
        base.HideMe(callBack);
        // 自分を非表示にするときは、表示中の3Dキャラクターモデルを削除
        if (heroObj != null)
        {
            DestroyImmediate(heroObj);
            heroObj = null;
        }
    }
}
