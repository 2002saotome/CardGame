using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform playerHand, playerField, enemyField;
    [SerializeField] Text playerLeaderHPText;
    [SerializeField] Text enemyLeaderHPText;

    bool isPlayerTurn = true; 
    List<int> deck = new List<int>() { 1, 2, 3, 4, 5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30};  //計30枚カード引けたり使用が可能

    public static GameManager instance;
    public int playerLeaderHP;
    public int enemyLeaderHP;

    public int PlayerEffectTimerFlag = 0;
    public int EnemyEffectTimerFlag = 0;

    public int PlayerTurnFlag =0;
    public int EnemyTurnFlag =0;

    private float EffectCountDown = 5.0f;
    public float EnemyActionTimer = 5.0f;
    public int EnemyActionTimerFlag = 0;

    public Text TurnText;
    public Text EnemyActionText;
    public Text effectTimer;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void AttackToLeader(CardController attackCard, bool isPlayerCard)
    {
        if (attackCard.model.canAttack == true)
        {
            return;
        }
        
        //爆弾の処理
        if (attackCard.model.power==0)
        {
            int RandBomb = Random.Range(1,5);//1～5のランダムな攻撃(爆弾)
            //1
            if(RandBomb==1)
            {
                enemyLeaderHP -= 1;
                playerLeaderHP -= 1;
            }
            //2
            if (RandBomb == 2)
            {
                enemyLeaderHP -= 2;
                playerLeaderHP -= 2;
            }
            //3
            if (RandBomb == 3)
            {
                enemyLeaderHP -= 3;
                playerLeaderHP -= 3;
            }
            //4
            if (RandBomb == 4)
            {
                enemyLeaderHP -= 4;
                playerLeaderHP -= 4;
            }
            //5
            if (RandBomb == 5)
            {
                enemyLeaderHP -= 5;
                playerLeaderHP -= 5;
            }
        }
        enemyLeaderHP -= attackCard.model.power;
        ChangeTurn();

        attackCard.model.canAttack = false;
        attackCard.view.SetCanAttackPanel(false);
        Debug.Log("敵のHPは、" + enemyLeaderHP);
        ShowLeaderHP();
    }

    public void ShowLeaderHP()
    {
        if (playerLeaderHP <= 0)
        {
            playerLeaderHP = 0;
        }
        if (enemyLeaderHP <= 0)
        {
            enemyLeaderHP = 0;
        }

        playerLeaderHPText.text = playerLeaderHP.ToString();
        enemyLeaderHPText.text = enemyLeaderHP.ToString();
    }

    void Start()
    {
        StartGame();
    }

    void StartGame() // 初期値の設定 
    {
        enemyLeaderHP = 20;
        playerLeaderHP = 50;
        ShowLeaderHP();

        // 初期手札を配る
        SetStartHand();

        // ターンの決定
        TurnCalc();
    }

    void CreateCard(int cardID, Transform place)
    {
        CardController card = Instantiate(cardPrefab, place);
        card.Init(cardID);
    }

    void DrawCard(Transform hand) // カードを引く
    {
        // デッキがないなら引かない
        if (deck.Count == 0)
        {
            return;
        }

        // デッキの一番上のカードを抜き取り、手札に加える
        int cardID = Random.Range(1,9); //カードのIDをランダムで取得
        deck.RemoveAt(0);
        if(deck.Count>10)
        {
            CreateCard(cardID, hand);
        }
        
    }

    void SetStartHand() // 手札を5枚配る
    {
        for (int i = 0; i < 4; i++)
        {
            DrawCard(playerHand);
        }
    }

    void TurnCalc() // ターンを管理する
    {
        if (isPlayerTurn)
        {
            PlayerTurn();
            PlayerEffectTimerFlag = 1;
            PlayerTurnFlag = 1;
            if(PlayerTurnFlag==1)
            {
                TurnText.text = "自分のターン";
                EnemyTurnFlag = 0;
            }
        }
        else
        {
            EnemyTurn();
            EnemyEffectTimerFlag = 1;
            EnemyTurnFlag = 1;
            if (EnemyTurnFlag == 1)
            {
                TurnText.text = "敵のターン";
                PlayerTurnFlag = 0;
            }
        }
    }

    public void ChangeTurn() // ターンエンドボタンにつける処理
    {
        isPlayerTurn = !isPlayerTurn; // ターンを逆にする
        TurnCalc(); // ターンを相手に回す
    }

    void PlayerTurn()
    {
        Debug.Log("自分のターン");

        if (EffectCountDown < 0)
        {
            PlayerEffectTimerFlag = 0;
            EffectCountDown = 5.0f;
        }

        if (PlayerEffectTimerFlag == 1)
        {
            effectTimer.text = "自分のターン";
            EffectCountDown -= Time.deltaTime;
        }


        CardController[] playerFieldCardList = playerField.GetComponentsInChildren<CardController>();
        SetAttackableFieldCard(playerFieldCardList, true);

        DrawCard(playerHand); // 手札を一枚加える
    }

    void EnemyTurn()
    {
        Debug.Log("敵のターン");
        if(EnemyActionTimerFlag==1)
        {
            EnemyActionTimer -= 0.1f;
        }

        if(EnemyActionTimer<0)
        {
            EnemyActionTimerFlag = 0;
            EnemyActionTimer = 5.0f;
            ChangeTurn();
        }

        if (EffectCountDown < 0)
        {
            EnemyEffectTimerFlag = 0;
        }

        if (EnemyEffectTimerFlag == 1)
        {
            effectTimer.text = "相手のターン";
            EffectCountDown -= Time.deltaTime;
        }


        //敵の攻撃パターン
        int EnemyAction = Random.Range(1, 6);
        CardController[] enemyFieldCardList = enemyField.GetComponentsInChildren<CardController>();

        //敵の攻撃
        if (EnemyAction == 1)
        {
            enemyLeaderHP += 5; //5回復
            EnemyActionText.text = "スライムは5回復した";
            EnemyActionTimerFlag = 1; // ターンエンドする
        }
        if (EnemyAction == 2)
        {
            playerLeaderHP -= 1; //1攻撃
            EnemyActionText.text = "スライムは1攻撃した";
            EnemyActionTimerFlag = 1; // ターンエンドする
        }
        if (EnemyAction == 3)
        {
            playerLeaderHP -= 2; //2攻撃
            EnemyActionText.text = "スライムは2攻撃した";
            EnemyActionTimerFlag = 1; // ターンエンドする
        }
        if (EnemyAction == 4)
        {
            playerLeaderHP -= 3; //3攻撃
            EnemyActionText.text = "スライムは3攻撃した";
            EnemyActionTimerFlag = 1; // ターンエンドする
        }
        if (EnemyAction == 5)
        {
            playerLeaderHP -= 5; //5攻撃
            EnemyActionText.text = "スライムは5攻撃した";
            EnemyActionTimerFlag = 1; // ターンエンドする
        }
    }

    public void CardBattle(CardController attackCard, CardController defenceCard)
    {
        // 攻撃カードがアタック可能でなければ攻撃しないで処理終了する
        if (attackCard.model.canAttack == false)
        {
            attackCard.model.power -= enemyLeaderHP;
            return;
        }

        // 攻撃側のパワーが高かった場合、攻撃されたカードを破壊する
        //if (attackCard.model.power > defenceCard.model.power)
        //{
        //    defenceCard.DestroyCard(defenceCard);
        //}

        //// 攻撃された側のパワーが高かった場合、攻撃側のカードを破壊する
        //if (attackCard.model.power < defenceCard.model.power)
        //{
        //    attackCard.DestroyCard(attackCard);
        //}

        //// パワーが同じだった場合、両方のカードを破壊する
        //if (attackCard.model.power == defenceCard.model.power)
        //{
        //    attackCard.DestroyCard(attackCard);
        //    defenceCard.DestroyCard(defenceCard);
        //}

        attackCard.model.canAttack = false;
        attackCard.view.SetCanAttackPanel(false);
    }

    void SetAttackableFieldCard(CardController[] cardList, bool canAttack)
    {
        foreach (CardController card in cardList)
        {
            card.model.canAttack = canAttack;
            card.view.SetCanAttackPanel(canAttack);
        }
    }
}
