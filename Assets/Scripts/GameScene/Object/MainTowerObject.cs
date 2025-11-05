using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTowerObject : MonoBehaviour
{
    // HP関連
    private int hp;
    private int maxHp;
    // 死亡状態かどうか
    private bool isDead;

    // 他のオブジェクトが位置を簡単に取得できるようにする
    private static MainTowerObject instance;
    public static MainTowerObject Instance => instance;

    private void Awake()
    {
        instance = this;
    }

    // HPを更新
    public void UpdateHp(int hp, int maxHP)
    {
        this.hp = hp;
        this.maxHp = maxHP;

        // UI上のHP表示を更新
        UIManager.Instance.GetPanel<GamePanel>().UpdateTowerHp(hp, maxHP);
    }

    // ダメージを受ける処理
    public void Wound(int dmg)
    {
        // 防衛エリアがすでに破壊されている場合は、HPを減らす必要がない
        if (isDead)
            return;
        // ダメージを受ける
        hp -= dmg;
        // 死亡処理
        if (hp <= 0)
        {
            hp = 0;
            isDead = true;
            // ゲームオーバー
            GameOverPanel panel = UIManager.Instance.ShowPanel<GameOverPanel>();
            // 報酬の半分を獲得
            panel.InitInfo((int)(GameLevelMgr.Instance.player.money * 0.5f), false);
        }

        // HPを更新
        UpdateHp(hp, maxHp);
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
