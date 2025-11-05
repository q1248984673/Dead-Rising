using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    public Image imgHP;
    public Text txtHP;

    public Text txtWave;
    public Text txtMoney;

    // HPバーの初期幅（外部から幅を調整可能）
    public float hpW = 500;

    public Button btnQuit;

    // 下部のタワー建造用複合UIの親オブジェクト（主に表示/非表示の制御に使用）
    public Transform botTrans;

    // 3つの複合ボタンを管理
    public List<TowerBtn> towerBtns = new List<TowerBtn>();

    // 現在ホバー/選択中のタワー建設ポイント
    private TowerPoint nowSelTowerPoint;

    // タワー建設の入力を監視するかどうかのフラグ
    private bool checkInput;

    public override void Init()
    {
        // ボタンイベントを監視
        btnQuit.onClick.AddListener(() =>
        {
            // ゲームUIを非表示
            UIManager.Instance.HidePanel<GamePanel>();
            // 開始画面へ戻る
            SceneManager.LoadScene("BeginScene");
            // その他
        });

        // 起動時は下部のタワー関連UIを非表示
        botTrans.gameObject.SetActive(false);
        // マウスをロック
        Cursor.lockState = CursorLockMode.Confined;
    }

    /// <summary>
    /// 防衛エリア（タワー）のHP表示を更新
    /// </summary>
    /// <param name="hp">現在HP</param>
    /// <param name="maxHP">最大HP</param>
    public void UpdateTowerHp(int hp, int maxHP)
    {
        txtHP.text = hp + "/" + maxHP;
        // HPバーの長さを更新
        (imgHP.transform as RectTransform).sizeDelta = new Vector2((float)hp / maxHP * hpW, 38);
    }

    /// <summary>
    /// 残りウェーブ数を更新
    /// </summary>
    /// <param name="nowNum">現在のウェーブ数</param>
    /// <param name="maxNum">最大ウェーブ数</param>
    public void UpdateWaveNum(int nowNum, int maxNum)
    {
        txtWave.text = nowNum + "/" + maxNum;
    }

    /// <summary>
    /// 所持金を更新
    /// </summary>
    /// <param name="money">現在の所持金</param>
    public void UpdateMoney(int money)
    {
        txtMoney.text = money.ToString();
    }


    /// <summary>
    /// 現在選択中のタワー建設ポイントに応じてUIを更新
    /// </summary>
    public void UpdateSelTower(TowerPoint point)
    {
        // タワー建設ポイントの情報に応じて画面表示を切り替える
        nowSelTowerPoint = point;

        // 渡されたデータがnullの場合
        if (nowSelTowerPoint == null)
        {
            checkInput = false;
            // 下部の建設ボタンを非表示
            botTrans.gameObject.SetActive(false);
        }
        else
        {
            checkInput = true;
            // 下部の建設ボタンを表示
            botTrans.gameObject.SetActive(true);

            // まだタワーを建てていない場合
            if (nowSelTowerPoint.nowTowerInfo == null)
            {
                for (int i = 0; i < towerBtns.Count; i++)
                {
                    towerBtns[i].gameObject.SetActive(true);
                    towerBtns[i].InitInfo(nowSelTowerPoint.chooseIDs[i], "Num" + (i + 1));
                }
            }
            // すでにタワーを建てている場合
            else
            {
                for (int i = 0; i < towerBtns.Count; i++)
                {
                    towerBtns[i].gameObject.SetActive(false);
                }
                towerBtns[1].gameObject.SetActive(true);
                towerBtns[1].InitInfo(nowSelTowerPoint.nowTowerInfo.nextLev, "Space");
            }
        }

    }


    protected override void Update()
    {
        base.Update();
        // 主にタワー建設ポイントでのキーボード入力により建設を行う
        if (!checkInput)
            return;

        // まだタワー未建設の場合は 1/2/3 キーで建設
        if (nowSelTowerPoint.nowTowerInfo == null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.chooseIDs[0]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.chooseIDs[1]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.chooseIDs[2]);
            }
        }
        // すでに建設済みの場合はスペースキーでアップグレード/再建設
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.nowTowerInfo.nextLev);
            }
        }
    }
}
