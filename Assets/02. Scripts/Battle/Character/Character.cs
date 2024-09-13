// 김민철
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BuffEffect
{
    public BuffEffect(SkillType type, int amount, int remainingTurns)
    {
        this.type = type;
        this.amount = amount;
        this.remainingTurns = remainingTurns;
    }

    public SkillType type;
    public int amount;
    public int remainingTurns;
}

public class Character : MonoBehaviour
{
    [Header("데이터")]
    // HP(체력)
    protected int currentHp = 100;
    [SerializeField] protected int maxHp = 100;
    [SerializeField] protected int shield = 0;

    [Header("능력치")]
    // 추가 공격력. 데미지 계산 식에 적용
    public int bonusAttackStat = 0;
    // 추가 데미지. 데미지 계산 후, 추가로 들어가는 고정 데미지
    public int bonusDamage = 0;
    // 추가 방어력. 데미지 계산 식에 적용
    public int bonusArmor = 0;

    [Header("일러스트")]
    // 스프라이트
    protected Image imageComponent;

    [Header("HP 바")]
    // HP 바
    [SerializeField] protected Image hpBar;
    [SerializeField] protected TMP_Text hpText;
    // 실드 바
    [SerializeField] protected Image shieldBar;
    
    // 데미지 텍스트
    [SerializeField] protected GameObject damageTextPrefab;

    [Header("상태이상")]
    [SerializeField] protected List<BuffEffect> buffs;
    [SerializeField] protected Transform buffIconContainer;
    [SerializeField] protected List<BuffIcon> buffIcons;
    [SerializeField] protected Transform infoPanelContainer;
    [SerializeField] protected List<BuffInfoPanel> infoPanels;

    // 컴포넌트들을 등록한다.
    protected virtual void EnrollComponents()
    {
        // 스프라이트
        imageComponent = transform.GetChild(0).GetChild(0).GetComponent<Image>();

        // HP 바
        hpBar = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetComponent<Image>();
        hpText = hpBar.transform.GetChild(0).GetComponent<TMP_Text>();

        // 실드 바
        shieldBar = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>();

        // 디버프 효과들(내부 데이터)을 담아둘 리스트
        buffs = new List<BuffEffect>();
        buffIcons = new List<BuffIcon>();

        // 디버프 아이콘들의 부모 컨테이너
        buffIconContainer = transform.GetChild(0).GetChild(1).GetChild(1);
        // debuffIconContainer의 모든 자식 오브젝트를 비활성화. (자식의 자식은 X)
        foreach (Transform icon in buffIconContainer)
        {
            // debuffIcons에 icon의 이미지, 텍스트 컴포넌트를 할당
            buffIcons.Add(new BuffIcon(icon.GetComponent<Image>(), icon.GetChild(0).GetComponent<TMP_Text>()));
            // 그리고 비활성화.
            icon.gameObject.SetActive(false);
        }

        // 디버프 상세정보창을 불러온다. (더미, 행동정보창 제외)
        infoPanelContainer = transform.GetChild(1).GetChild(0);
    }

    public virtual void ResetState()
    {
        // 스탯을 초기화한다.
        ResetStat();

        // 체력 데이터 및 UI를 초기화한다.
        shield = 0;
        UpdateHPUI();
        UpdateShieldUI();

        // 버프를 제거한다.
        CleanseDebuff();
    }

    #region HP
    // 이 오브젝트의 hp를 반환한다.
    public int GetCurrentHP()
    {
        return currentHp;
    }
    
    // 이 오브젝트의 최대 HP를 반환한다.
    public int GetMaxHP()
    {
        return maxHp;
    }

    // 현재 HP를 갱신한다.
    public void UpdateHPUI()
    {
        // 이미지 변경
        // HP 바를 뚫으려 하면
        if (currentHp + shield > maxHp)
        {
            // 비율을 맞춰 현재 체력 바의 크기를 줄이고
            hpBar.fillAmount = (float)currentHp / (maxHp + shield);
        }
        // 아니라면
        else
        {
            // 그대로 출력한다.
            hpBar.fillAmount = (float)currentHp / maxHp;
        }

        // 텍스트 변경
        // 실드가 없다면 현재 체력과 최대 체력을
        string text = currentHp + "/" + maxHp;
        // 그게 아니라면
        if(shield > 0)
        {
            // 방어막도 함께 적어준다.
            text = "(" + currentHp + " + " + shield + ") / " + maxHp;
        }
        hpText.text = text;
    }

    // 이 오브젝트의 hp를 감소시킨다.
    public void DecreaseHP(int damage)
    {
        // 현재 데미지
        int currentDamage = damage;

        // 실드가 있다면 데미지 재계산
        if (shield > 0)
        {
            // currentDamage를 감소시키고
            currentDamage -= shield;
            if (currentDamage < 0)
            {
                // 잔여 데미지가 음수면 0으로 적용한다.
                currentDamage = 0;
            }

            // 실드에선 기존 데미지를 뺀다.
            shield -= damage;
            if (shield < 0)
            {
                // 잔여 방어막이 음수면 0으로 적용한다.
                shield = 0;
            }
        }

        // hp를 잔여 데미지 만큼 감소시킨다.
        currentHp -= currentDamage;

        // UI를 갱신한다.
        UpdateShieldUI();
        UpdateHPUI();

        // 적이 피격될 때 모션, 데미지 텍스트 출력
        if(this != Player.Instance && currentDamage > 0)
        {
            DamageText damageText = Instantiate(damageTextPrefab, transform.GetChild(0)).GetComponent<DamageText>();
            StartCoroutine(damageText.PrintDamageText(currentDamage));
            imageComponent.transform.DOShakePosition(0.5f, 10f);
        }

        // hp가 0 이하가 될 경우
        if (currentHp <= 0)
        {
            currentHp = 0;
            UpdateHPUI();

            // 죽음 이벤트 실행
            Die();
        }
    }

    // 이 오브젝트의 hp를 감소시킨다.
    public void IncreaseHP(int heal)
    {
        // hp를 damage만큼 감소시킨다.
        currentHp += heal;

        // hp가 최대치 이상이 될 경우
        if (currentHp > maxHp)
        {
            // 최대치로 맞추기
            currentHp = maxHp;
        }

        // UI 갱신
        UpdateShieldUI();
        UpdateHPUI();
    }
    #endregion HP

    #region 실드
    // 방어막을 얻는다.
    public void GetShield(int amount)
    {
        // 방어막을 추가하고
        shield += amount;

        // UI 변경
        UpdateShieldUI();
        UpdateHPUI();
    }

    // 실드 UI를 갱신한다.
    private void UpdateShieldUI()
    {
        // 실드가 없다면
        if(shield == 0)
        {
            // 0으로 만들고 종료
            shieldBar.fillAmount = 0;
            return;
        }

        // HP 바를 뚫으려 하면
        if(currentHp + shield > maxHp)
        {
            // 100으로 만들고
            shieldBar.fillAmount = 1;
        }

        // 그 외는
        else
        {
            // 이미지를 변경한다. Shield를 뒤에 깔고 HP를 앞으로 빼서, 아래 식이 쓰인다.
            shieldBar.fillAmount = (float)(currentHp + shield) / (maxHp);
        }
    }
    #endregion 실드

    public virtual void Die()
    {
        // 죽음과 관련된 효과 처리
        // 죽음 애니메이션
    }

    #region 버프
    // 버프 효과를 턴 시작 이벤트에 등록한다.
    public void EnrollBuff(BuffEffect buff)
    {
        // 버프 리스트에 추가
        buffs.Add(buff);

        // BuffIcon과 BuffInfoPanel을 풀에서 가져와서
        BuffIcon buffIcon = ObjectPoolManager.Instance.GetGo("InfoPanel").GetComponent<BuffIcon>();
        BuffInfoPanel infoPanel = ObjectPoolManager.Instance.GetGo("InfoPanel").GetComponent<BuffInfoPanel>();

        // 리스트에 등록한다.
        buffIcons.Add(buffIcon);
        infoPanels.Add(infoPanel);

        // 버프 UI 갱신
        UpdateBuffIcon(buffs.Count - 1);
    }

    // 디버프를 전부 제거한다.
    public void CleanseDebuff()
    {
        buffs.Clear();

        UpdateAllBuffIcon();
    }

    public void UpdateBuffIcon(int index)
    {
        if (buffs[index].remainingTurns <= 0)
        {
            return;
        }

        // 아이콘과 숫자를 변경한다.
        buffIcons[index].image.sprite = CardInfo.Instance.skillIcons[(int)buffs[index].type];
        buffIcons[index].tmp_Text.text = buffs[index].remainingTurns.ToString();


        // 스킬 텍스트를 만든다.
        SkillText buffText = DebuffInfo.GetSkillText(buffs[index]);
        // 이름과 설명을 변경한다.
        infoPanels[index].name.text = buffText.name;
        infoPanels[index].description.text = buffText.description;
    }

    public void UpdateAllBuffIcon()
    {
        // 모든 현재 디버프에 대해(i번째 디버프에 대해)
        int i = 0;
        for (; i < buffs.Count; ++i)
        {
            UpdateBuffIcon(i);
        }
    }

    public void GetBuffAll()
    {
        // 스탯 초기화
        ResetStat();

        // 모든 적용 중인 출혈 효과에 대해
        for (int i = 0; i < buffs.Count; ++i)
        {
            // 스킬 발동
            CardInfo.Instance.ActivateSkill(buffs[i], this, this);

            // 남은 턴 1 감소
            --buffs[i].remainingTurns;
            // 남은 턴이 0 이하라면
            if (buffs[i].remainingTurns <= 0)
            {
                // 해당 효과를 삭제한다.
                buffs.RemoveAt(i);
                // 뒤의 디버프들이 1칸씩 앞으로 땡겨졌으니, 인덱스도 1 앞으로 조정
                --i;
            }

            // 이펙트 재생

            // 0.1초 딜레이
            // yield return new WaitForSeconds(0.1f);
        }

        // 아이콘 최신화
        UpdateAllBuffIcon();
    }
    #endregion 디버프

    #region 스텟
    public void ResetStat()
    {
        bonusAttackStat = 0;
        bonusDamage = 0;
        bonusArmor = 0;
    }

    public void GetBonusAttackStat(int amount)
    {
        bonusAttackStat += amount;
    }

    public void GetBonusDamage(int amount)
    {
        bonusDamage += amount;
    }
    #endregion 스텟
}
