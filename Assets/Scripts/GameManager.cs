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
    List<int> deck = new List<int>() { 1, 2, 3, 4, 5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30};  //�v30���J�[�h��������g�p���\

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
        
        //���e�̏���
        if (attackCard.model.power==0)
        {
            int RandBomb = Random.Range(1,5);//1�`5�̃����_���ȍU��(���e)
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
        Debug.Log("�G��HP�́A" + enemyLeaderHP);
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

    void StartGame() // �����l�̐ݒ� 
    {
        enemyLeaderHP = 20;
        playerLeaderHP = 50;
        ShowLeaderHP();

        // ������D��z��
        SetStartHand();

        // �^�[���̌���
        TurnCalc();
    }

    void CreateCard(int cardID, Transform place)
    {
        CardController card = Instantiate(cardPrefab, place);
        card.Init(cardID);
    }

    void DrawCard(Transform hand) // �J�[�h������
    {
        // �f�b�L���Ȃ��Ȃ�����Ȃ�
        if (deck.Count == 0)
        {
            return;
        }

        // �f�b�L�̈�ԏ�̃J�[�h�𔲂����A��D�ɉ�����
        int cardID = Random.Range(1,9); //�J�[�h��ID�������_���Ŏ擾
        deck.RemoveAt(0);
        if(deck.Count>10)
        {
            CreateCard(cardID, hand);
        }
        
    }

    void SetStartHand() // ��D��5���z��
    {
        for (int i = 0; i < 4; i++)
        {
            DrawCard(playerHand);
        }
    }

    void TurnCalc() // �^�[�����Ǘ�����
    {
        if (isPlayerTurn)
        {
            PlayerTurn();
            PlayerEffectTimerFlag = 1;
            PlayerTurnFlag = 1;
            if(PlayerTurnFlag==1)
            {
                TurnText.text = "�����̃^�[��";
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
                TurnText.text = "�G�̃^�[��";
                PlayerTurnFlag = 0;
            }
        }
    }

    public void ChangeTurn() // �^�[���G���h�{�^���ɂ��鏈��
    {
        isPlayerTurn = !isPlayerTurn; // �^�[�����t�ɂ���
        TurnCalc(); // �^�[���𑊎�ɉ�
    }

    void PlayerTurn()
    {
        Debug.Log("�����̃^�[��");

        if (EffectCountDown < 0)
        {
            PlayerEffectTimerFlag = 0;
            EffectCountDown = 5.0f;
        }

        if (PlayerEffectTimerFlag == 1)
        {
            effectTimer.text = "�����̃^�[��";
            EffectCountDown -= Time.deltaTime;
        }


        CardController[] playerFieldCardList = playerField.GetComponentsInChildren<CardController>();
        SetAttackableFieldCard(playerFieldCardList, true);

        DrawCard(playerHand); // ��D���ꖇ������
    }

    void EnemyTurn()
    {
        Debug.Log("�G�̃^�[��");
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
            effectTimer.text = "����̃^�[��";
            EffectCountDown -= Time.deltaTime;
        }


        //�G�̍U���p�^�[��
        int EnemyAction = Random.Range(1, 6);
        CardController[] enemyFieldCardList = enemyField.GetComponentsInChildren<CardController>();

        //�G�̍U��
        if (EnemyAction == 1)
        {
            enemyLeaderHP += 5; //5��
            EnemyActionText.text = "�X���C����5�񕜂���";
            EnemyActionTimerFlag = 1; // �^�[���G���h����
        }
        if (EnemyAction == 2)
        {
            playerLeaderHP -= 1; //1�U��
            EnemyActionText.text = "�X���C����1�U������";
            EnemyActionTimerFlag = 1; // �^�[���G���h����
        }
        if (EnemyAction == 3)
        {
            playerLeaderHP -= 2; //2�U��
            EnemyActionText.text = "�X���C����2�U������";
            EnemyActionTimerFlag = 1; // �^�[���G���h����
        }
        if (EnemyAction == 4)
        {
            playerLeaderHP -= 3; //3�U��
            EnemyActionText.text = "�X���C����3�U������";
            EnemyActionTimerFlag = 1; // �^�[���G���h����
        }
        if (EnemyAction == 5)
        {
            playerLeaderHP -= 5; //5�U��
            EnemyActionText.text = "�X���C����5�U������";
            EnemyActionTimerFlag = 1; // �^�[���G���h����
        }
    }

    public void CardBattle(CardController attackCard, CardController defenceCard)
    {
        // �U���J�[�h���A�^�b�N�\�łȂ���΍U�����Ȃ��ŏ����I������
        if (attackCard.model.canAttack == false)
        {
            attackCard.model.power -= enemyLeaderHP;
            return;
        }

        // �U�����̃p���[�����������ꍇ�A�U�����ꂽ�J�[�h��j�󂷂�
        //if (attackCard.model.power > defenceCard.model.power)
        //{
        //    defenceCard.DestroyCard(defenceCard);
        //}

        //// �U�����ꂽ���̃p���[�����������ꍇ�A�U�����̃J�[�h��j�󂷂�
        //if (attackCard.model.power < defenceCard.model.power)
        //{
        //    attackCard.DestroyCard(attackCard);
        //}

        //// �p���[�������������ꍇ�A�����̃J�[�h��j�󂷂�
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
