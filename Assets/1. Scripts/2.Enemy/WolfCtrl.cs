using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WolfCtrl : MonoBehaviour
{
    public GameObject hudDamageText;
    public Transform hudPos;
    public GameObject dropfood;
    public List<GameObject> food = new List<GameObject>();
    public float waterValue;
    public float hungryValue;
    public enum SkullState { None, Idle, Move, Wait, GoTarget, Atk, Damage, Die }
    public float DelaySecond = 1.2f;

    public Slider hpBar;
    [Header("기본 속성")]

    public SkullState skullState = SkullState.None;

    public float spdMove = 1f;
    public float spdRun = 1f;
    private float NowSpd;

    public GameObject targetCharactor = null;

    public Transform targetTransform = null;

    public Vector3 posTarget = Vector3.zero;
 
    private Animation skullAnimation = null;
    private Transform skullTransform = null;
    public GameObject EffectPosition = null;
    [Header("애니메이션 클립")]
    public AnimationClip IdleAnimClip = null;
    public AnimationClip MoveAnimClip = null;
    public AnimationClip AtkAnimClip = null;
    public AnimationClip DamageAnimClip = null;
    public AnimationClip DieAnimClip = null;


    [Header("전투속성")]
    public float hpvalue = 100f;
    public float hp = 100f;
    public float attackDamage;
    public float AtkRange = 1.5f;
    public GameObject effectDamage = null;
    public GameObject AtkPlayerEffect = null;
    public GameObject effectDie = null;

    private Tweener effectTweener = null;
    private SkinnedMeshRenderer skinnedMeshRenderer = null;
    public CapsuleCollider AtkCapsuleCollider = null;
    private PlayerCtrl playerctrl;
    private GameObject player;

    void OnDmgAnmationFinished()
    {
        Debug.Log("Dmg Animation finished");
    }



    /// <summary>
    /// 애니메이션 이벤트를 추가해주는 함. 
    /// </summary>
    /// <param name="clip">애니메이션 클립 </param>
    /// <param name="funcName">함수명 </param>
    void OnAnimationEvent(AnimationClip clip, string funcName)
    {
        AnimationEvent retEvent = new AnimationEvent();
        retEvent.functionName = funcName;
        retEvent.time = clip.length - 0.4f;
        clip.AddEvent(retEvent);
    }


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerctrl = player.GetComponent<PlayerCtrl>();
        skullState = SkullState.Idle;

        NowSpd = spdMove;
        skullAnimation = GetComponent<Animation>();
        skullTransform = GetComponent<Transform>();


        skullAnimation[IdleAnimClip.name].wrapMode = WrapMode.Loop;
        skullAnimation[MoveAnimClip.name].wrapMode = WrapMode.Loop;
        skullAnimation[AtkAnimClip.name].wrapMode = WrapMode.Once;
        skullAnimation[DamageAnimClip.name].wrapMode = WrapMode.Once;

        skullAnimation[DamageAnimClip.name].layer = 10;
        skullAnimation[DieAnimClip.name].wrapMode = WrapMode.Once;
        skullAnimation[DieAnimClip.name].layer = 10;


        OnAnimationEvent(DamageAnimClip, "OnDmgAnmationFinished");

        skinnedMeshRenderer = this.skullTransform.Find("Wolf 1").GetComponent<SkinnedMeshRenderer>();

        
    }
    public void OnAtkAnmationFinished()
    {
        SoundManager.Instance.SetEffectSoundClip(4);
        Debug.Log("Atk Animation finished");
        playerctrl.GetComponent<PlayerCtrl>().GetDamege(attackDamage);

        Instantiate(AtkPlayerEffect, playerctrl.transform.position, Quaternion.identity);

    }

    void CkState()
    {
        switch (skullState)
        {
            case SkullState.Idle:

                setIdle();
                break;
            case SkullState.GoTarget:
            case SkullState.Move:
                setMove();
                break;
            case SkullState.Atk:
                setAtk();
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

        CkState();
        AnimationCtrl();
        HpBarUpdate();
    }
    void HpBarUpdate()
    {
        hpBar.value = (float)hp / (float)hpvalue;
    }

    void setIdle()
    {
        if (targetCharactor == null)
        {
            posTarget = new Vector3(skullTransform.position.x + Random.Range(-10f, 10f),
                                    skullTransform.position.y + 1f,
                                    skullTransform.position.z + Random.Range(-10f, 10f)
                );
            Ray ray = new Ray(posTarget, Vector3.down);
            RaycastHit infoRayCast = new RaycastHit();
            if (Physics.Raycast(ray, out infoRayCast, Mathf.Infinity) == true)
            {
                posTarget.y = infoRayCast.point.y;
            }

            //skullState = SkullState.Move;
        }
        else
        {
            skullState = SkullState.GoTarget;
        }
    }


    void setMove()
    {
        //출발점 도착점 두 벡터의 차이 
        Vector3 distance = Vector3.zero;
        //어느 방향을 바라보고 가고 있느냐 
        Vector3 posLookAt = Vector3.zero;

        //상태
        switch (skullState)
        {
            // 돌아다니는 경우
            case SkullState.Move:
                //만약 랜덤 위치 값이 제로가 아니면
                if (posTarget != Vector3.zero)
                {
                    //목표 위치에서 해골 있는 위치 차를 구하고
                    distance = posTarget - skullTransform.position;

                    //만약에 움직이는 동안 해골이 목표로 한 지점 보다 작으 
                    if (distance.magnitude < AtkRange)
                    {
                        //대기 동작 함수를 호출
                        StartCoroutine(setWait());
                        //여기서 끝냄
                        return;
                    }

                    //어느 방향을 바라 볼 것인. 랜덤 지역
                    posLookAt = new Vector3(posTarget.x,
                                            //타겟이 높이 있을 경우가 있으니 y값 체크
                                            skullTransform.position.y,
                                            posTarget.z);
                }
                break;
            //캐릭터를 향해서 가는 돌아다니는  경우
            case SkullState.GoTarget:
                //목표 캐릭터가 있을 땟
                if (targetCharactor != null)
                {

                    //목표 위치에서 해골 있는 위치 차를 구하고
                    distance = targetCharactor.transform.position - skullTransform.position;
                    //만약에 움직이는 동안 해골이 목표로 한 지점 보다 작으 
                    if (distance.magnitude < AtkRange)
                    {
                        //공격상태로 변경합니.
                        skullState = SkullState.Atk;
                        //여기서 끝냄
                        return;
                    }
                    //어느 방향을 바라 볼 것인. 랜덤 지역
                    posLookAt = new Vector3(targetCharactor.transform.position.x,
                                            //타겟이 높이 있을 경우가 있으니 y값 체크
                                            skullTransform.position.y,
                                            targetCharactor.transform.position.z);
                }
                break;
            default:
                break;


        }

        //해골 이동할 방향에 크기를 없애고 방향만 가진(normalized)
        Vector3 direction = distance.normalized;

        //방향은 x,z 사용 y는 땅을 파고 들어갈거라 안함
        direction = new Vector3(direction.x, 0f, direction.z);

        //이동량 방향 구하기






            Vector3 amount = direction * NowSpd * Time.deltaTime;
            skullTransform.Translate(amount, Space.World);



        //캐릭터 컨트롤이 아닌 트랜스폼으로 월드 좌표 이용하여 이동

        //캐릭터 방향 정하기
        skullTransform.LookAt(posLookAt);

    }

    /// <summary>
    /// 대기 상태 동작 함 
    /// </summary>
    /// <returns></returns>
    IEnumerator setWait()
    {
        Debug.Log("Wolf wait");
        //상태를 대기 상태로 바꿈
        skullState = SkullState.Wait;
        //대기하는 시간이 오래되지 않게 설정
        float timeWait = Random.Range(0.5f,2);
        //대기 시간을 넣어 준.
        yield return new WaitForSeconds(timeWait);
        //대기 후 다시 준비 상태로 변경
        skullState = SkullState.Idle;

    }

    /// <summary>
    /// 애니메이션을 재생시켜주는 함 
    /// </summary>
    void AnimationCtrl()
    {
        //상태에 따라서 애니메이션 적용
        switch (skullState)
        {
            //대기와 준비할 때 애니메이션 같.
            case SkullState.Wait:
            case SkullState.Idle:
                //준비 애니메이션 실행
                skullAnimation.CrossFade(IdleAnimClip.name);
                break;
            //랜덤과 목표 이동할 때 애니메이션 같.
            case SkullState.Move:
            case SkullState.GoTarget:
                //이동 애니메이션 실행
                NowSpd = spdRun;
                skullAnimation.CrossFade(MoveAnimClip.name);
                break;
            //공격할 때
            case SkullState.Atk:
                Invoke("AtkDelay", 0.2f);
                break;
            //죽었을 때
            case SkullState.Die:
                //죽을 때도 애니메이션 실행
                //EnemyDieDelay();
                break;
            default:
                break;

        }
    }
    ///<summary>
    ///시야 범위 안에 다른 Trigger 또는 캐릭터가 들어오면 호출 된다.
    ///함수 동작은 목표물이 들어오면 목표물을 설정하고 해골을 타겟 위치로 이동 시킨다 
    ///</summary>
    void AtkDelay()
    {
        this.skullAnimation.CrossFade(AtkAnimClip.name);
    }

    void OnCkTarget(GameObject target)
    {
        //목표 캐릭터에 파라메터로 검출된 오브젝트를 넣고 
        targetCharactor = target;
        //목표 위치에 목표 캐릭터의 위치 값을 넣습니다. 
        targetTransform = targetCharactor.transform;

        //목표물을 향해 해골이 이동하는 상태로 변경
        skullState = SkullState.GoTarget;

    }
    /// <summary>
    /// 해골 상태 공격 모드
    /// </summary>
    void setAtk()
    {
        //해골과 캐릭터간의 위치 거리 
        float distance = Vector3.Distance(targetTransform.position, skullTransform.position); //무겁다
        NowSpd = spdMove;
        //공격 거리보다 둘 간의 거리가 멀어 졌다면 
        if (distance > AtkRange + 0.5f)
        {
            //타겟과의 거리가 멀어졌다면 타겟으로 이동 
            skullState = SkullState.GoTarget;
        }
    }


    /// <summary>
    /// 해골 피격 충돌 검출 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        //만약에 해골이 캐릭터 공격에 맞았다면
        if (other.gameObject.CompareTag("PlayerAtk") == true)
        {
            //해골 체력을 10 빼고 
            hp -= playerctrl.AtkDamege;
            SoundManager.Instance.SetEffectSoundClip(0);
            GameObject hudText = Instantiate(hudDamageText);
            hudText.transform.position = -hudPos.position;
            if (hp > 0)
            {
                //피격 이펙트 
                Instantiate(effectDamage, other.transform.position, Quaternion.identity);

                //체력이 0 이상이면 피격 애니메이션을 연출 하고 
                skullAnimation.CrossFade(DamageAnimClip.name);

                //피격 트위닝 이펙트
                effectDamageTween();
            }
            else
            {
                //0 보다 작으면 해골이 죽음 상태로 바꾸어라  
                skullAnimation.CrossFade(DieAnimClip.name);
                skullState = SkullState.Die;
                StartCoroutine("DieDelay");

            }
        }

    }

    IEnumerator DieDelay()
    {
        int randomfood = Random.Range(1, 3);
        Debug.Log(randomfood);
        yield return new WaitForSeconds(DelaySecond);
        //몬스터 죽음 이벤트 
        SoundManager.Instance.SetEffectSoundClip(1);
        Instantiate(effectDie, skullTransform.position, Quaternion.identity);

        //몬스터 삭제 
        Destroy(gameObject);

        if(randomfood==2)
        {
            Instantiate(dropfood, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }
        

    }
    /// <summary>
    /// 피격시 몬스터 몸에서 번쩍번쩍 효과를 준다
    /// </summary>
    void effectDamageTween()
    {

        if (hp > 0)

        {
            //트윈을 돌리다 또 트윈 함수가 진행되면 로직이 엉망이 될 수 있어서 
            //트윈 중복 체크로 미리 차단을 해준다

            //번쩍이는 이펙트 색상을 지정해준다
            Color colorTo = Color.red;


            skinnedMeshRenderer.material.DOColor(colorTo, 0f).OnComplete(OnDamageTweenFinished);

        }
        else
        {

        }
    }

    /// <summary>
    /// 피격이펙트 종료시 이벤트 함수 호출
    /// </summary>
    void OnDamageTweenFinished()
    {
        //트윈이 끝나면 하얀색으로 확실히 색상을 돌려준다
        skinnedMeshRenderer.material.DOColor(Color.white, 2f);
    }
}
