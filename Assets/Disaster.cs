using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disaster : MonoBehaviour
{

    //前方向の速度
    private float velocityz = 16f;


    public UnityChanController Unity;
    //Objectを移動させるコンポーネントを入れる（追加）
    private Rigidbody myRigidbody;

    bool isEndcount;
    // Start is called before the first frame update
    public void Start()
    {
        //Rigidbodyコンポーネントを取得
        this.myRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        isEndcount = Unity.IsEndcount();

        //Objectに速度を与える
        this.myRigidbody.velocity = new Vector3(0, 0, velocityz);

        if (isEndcount)
        {
            Destroy(this.gameObject);
        }
    }


    private void OnTriggerEnter(Collider other)
    {

        // 地面に衝突したら自オブジェクト削除
        if (other.gameObject.tag == "CarTag" || other.gameObject.tag == "TrafficConeTag" || other.gameObject.tag == "CoinTag")
        {
            //衝突したら、プレハブたちを消滅！！
            Destroy(other.gameObject);

        }


    }
}
