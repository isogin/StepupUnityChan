using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnityChanController : MonoBehaviour
{
    //アニメーションするためのコンポーネントを入れる
    private Animator myAnimator;

    //Unityちゃんを移動させるコンポーネントを入れる（追加）
    private Rigidbody myRigidbody;

    //前方向の速度
    private float velocityz = 16f;

    //横方向の速度
    private float velocityx = 10f;

    //上方向の速度
    private float velocityY = 10f;

    //左右の移動できる範囲
    private float movableRange = 3.4f;

    //動きを減速させる係数
    private float coefficient = 0.99f;

    //ゲーム終了の判定
    public  bool isEnd = false;

    //ゲーム終了時に表示するテキスト
    private GameObject stateText;

    //スコアを表示するテキスト
    private GameObject scoreText;

    //得点
    private int score = 0;

    //左ボタン押下の判定
    private bool isLButtonDown = false;

    //右ボタン押下の判定
    private bool isRButtonDown = false;

    //ジャンプボタン押下の判定
    private bool isJButtonDown = false;
    // Start is called before the first frame update
    void Start()
    {
        //Animatorコンポーネントを取得
        this.myAnimator = GetComponent<Animator>();

        //走るアニメーションを開始
        this.myAnimator.SetFloat("Speed", 1);

        //Rigidbodyコンポーネントを取得
        this.myRigidbody = GetComponent<Rigidbody>();

        //シーン中のstateTextオブジェクトを取得
        this.stateText = GameObject.Find("GameResultText");

        //シーン中のscoreTextオブジェクトを取得
        this.scoreText = GameObject.Find("ScoreText");
    }

    // Update is called once per frame
    void Update()
    {
        //ゲーム終了ならUnitytyちゃんの動きを減衰する
        if (this.isEnd)
        {
            this.velocityz *= this.coefficient;
            this.velocityx *= this.coefficient;
            this.velocityY *= this.coefficient;
            this.myAnimator.speed *= this.coefficient;

        }
        //横方向の入力による速度(初期値は０)
        float inputVelocityX = 0;

        //上方向の入力による速度
        float inputVelocityY = 0;

        //Unityちゃんを矢印キーまたはボタンに応じて左右に移動させる
        if ((Input.GetKey(KeyCode.LeftArrow) || this.isLButtonDown) && -this.movableRange < this.transform.position.x)
        //移動範囲の最左よりも今の位置が右にある
        {
            //左方向への速度を代入
            inputVelocityX = -this.velocityx;
        }
        else if ((Input.GetKey(KeyCode.RightArrow) || this.isRButtonDown) && this.transform.position.x < this.movableRange)
        {
            //右方向への速度を代入
            inputVelocityX = this.velocityx;
        }

        //ジャンプしていないときにスペースが押されたらジャンプする
        if ((Input.GetKeyDown(KeyCode.Space)|| this.isJButtonDown) && this.transform.position.y < 0.5f)
        {
            //ジャンプアニメを再生
            this.myAnimator.SetBool("Jump", true);
            //上方向への速度を代入
            inputVelocityY = this.velocityY;
        }
        else
        {
            //現在のY軸の速度を代入
            inputVelocityY = this.myRigidbody.velocity.y;
        }

        //Jumpステートの場合はJumpにfalseをセットする
        if (this.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            this.myAnimator.SetBool("Jump", false);
        }
        //Unityちゃんに速度を与える
        this.myRigidbody.velocity = new Vector3(inputVelocityX, inputVelocityY, this.velocityz);
    }
        //トリガーモードでほかのオブジェクトと接触した場合の処理
        void OnTriggerEnter(Collider other)
        {
            //障害物に衝突した場合
            if(other.gameObject.tag == "CarTag" || other.gameObject.tag == "TrafficConeTag")
            {
                //減速させる関数をonにする
                this.isEnd = true;
            //stateTextにGame Overを表示
            this.stateText.GetComponent<Text>().text = "GAME OVER";

            }

            //コインに衝突した(を獲得した)場合
            if(other.gameObject.tag == "CoinTag")
            {
            //スコアを加算
            this.score += 10;

            //ScoreText獲得した点数を表示
            this.scoreText.GetComponent<Text>().text = "Score" + this.score + "pt";
                //接触したコインのオブジェクトを破棄
                Destroy(other.gameObject);

            //パーティクルを再生(エフェクトを発生させる)
            GetComponent<ParticleSystem>().Play();

            }
            //ゴール地点に到達した場合
            if(other.gameObject.tag == "GoalTag")
            {
            //減速させる
                this.isEnd = true;
            //stateTextに　GAME CLEARを表示
            this.stateText.GetComponent<Text>().text = "GAME CLEAR!!";
            }
        }


    
    public void GetMyJumpButtonDown()
    {
        this.isJButtonDown = true;
    }

    public void GetMyJumpButtonUp()
    {
        this.isJButtonDown = false;
    }

    public void GetMyLeftButtonDown()
    {
        this.isLButtonDown = true;
    }

    public void GetMyRightButtonDown()
    {
        this.isRButtonDown = true;
    }

    public void GetMyLeftButtonUp()
    {
        this.isLButtonDown = false;
    }

    public void GetMyRightButtonUp()
    {
        this.isRButtonDown = false;
    }

    public bool IsEndcount()
    {
        return isEnd;
    }
}
