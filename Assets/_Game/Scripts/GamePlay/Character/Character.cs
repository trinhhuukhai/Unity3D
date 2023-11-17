using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : BaseCharacter, IHit
{

    [SerializeField] GameObject mask;

    public const float TIME_DELAY_THROW = 0.4f;
    public const float ATT_RANGE = 5f;
    public const float MAX_SIZE = 4f;
    public const float MIN_SIZE = 1f;

    protected Character target;
    protected float size = 1;


    private string animName;
    private List<Character> Listtargets = new List<Character>();

    private Vector3 targetPoint;

    private int score;


    public int Score => score;
    public float Size => size;
    public bool IsDead { get; protected set; }

    public bool IsCanAttack => currentSkin.Weapon.IsCanAttack;

    //khoi tao trang thai
    public override void OnInit()
    {
        IsDead = false;
        score = 0;
        WearClothes();
        ClearTarget();

    }

    public virtual void WearClothes()
    {

    }

    public virtual void TakeOffClothes()
    {
        currentSkin?.OnDespawn();
        SimplePool.Despawn(currentSkin);
    }

    //bat dau trang tai tan cong -> chuyen anim
    public override void OnAttack()
    {
        target = GetTargetInRange();

        if (IsCanAttack && target != null && !target.IsDead)
        {
            targetPoint = target.TF.position;
            TF.LookAt(targetPoint + (TF.position.y - targetPoint.y) * Vector3.up);
            ChangeAnim(Constant.ANIM_ATTACK);
        }

    }

    //ban ra vien dan
    public void Throw()
    {
        currentSkin.Weapon.Throw(this, targetPoint, size);
    }

    //chuyen trang thai die
    public override void OnDeath()
    {
        ChangeAnim(Constant.ANIM_DIE);
        LevelManager.Ins.CharacterDeath(this);
    }

    //dung lai
    public override void OnMoveStop()
    {

    }

    //ket thuc viec su dung nhan vat nay
    public override void OnDespawn()
    {
        //tra ve tat ca nhung object pool
        TakeOffClothes();
    }

    //va cham voi dan
    public void OnHit(UnityAction hitAction)
    {
        if (!IsDead)
        {
            IsDead = true;
            hitAction?.Invoke();
            OnDeath();
        }
    }

    //danh sau muc tieu
    public void SetMask(bool active)
    {
        mask.SetActive(active);
    }


    public void AddScore(int amount = 1)
    {
        SetScore(score + amount);
    }

    public void SetScore(int score)
    {
        this.score = score > 0 ? score : 0;
        SetSize(1 + this.score * 0.1f);
    }

    //thay doi kich thuoc
    protected virtual void SetSize(float size)
    {
        size = Mathf.Clamp(size, MIN_SIZE, MAX_SIZE);
        this.size = size;
        TF.localScale = size * Vector3.one;
    }

    //them muc tieu
    public virtual void AddTarget(Character target)
    {
        Listtargets.Add(target);
    }

    //xoas muc tieu
    public virtual void RemoveTarget(Character target)
    {
        Listtargets.Remove(target);
        this.target = null;
    }

    //tim tat ca muc tieu trong tam danh
    public Character GetTargetInRange()
    {
        Character target = null;
        float distance = float.PositiveInfinity;

        for (int i = 0; i < Listtargets.Count; i++)
        {
            if (Listtargets[i] != null && Listtargets[i] != this && !Listtargets[i].IsDead && Vector3.Distance(TF.position, Listtargets[i].TF.position) <= ATT_RANGE * size + Listtargets[i].size)
            {
                float dis = Vector3.Distance(TF.position, Listtargets[i].TF.position);

                if (dis < distance)
                {
                    distance = dis;
                    target = Listtargets[i];
                }
            }
        }

        return target;
    }



    public void ResetAnim()
    {
        animName = "";
    }

    [SerializeField] protected Skin currentSkin;

    public void ChangeWeapon(WeaponType weaponType)
    {
        currentSkin.ChangeWeapon(weaponType);
    }

    public void ChangeSkin(SkinType skinType)
    {
        currentSkin = SimplePool.Spawn<Skin>((PoolType)skinType, TF);
    }

    public void ChangeAccessory(AccessoryType accessoryType)
    {
        currentSkin.ChangeAccessory(accessoryType);
    }

    public void ChangeHat(HatType hatType)
    {
        currentSkin.ChangeHat(hatType);
    }

    public void ChangePant(PantType pantType)
    {
        currentSkin.ChangePant(pantType);
    }

    protected void ClearTarget()
    {
        Listtargets.Clear();
    }

    public void ChangeAnim(string animName)
    {
        if (this.animName != animName)
        {
            currentSkin.Anim.ResetTrigger(this.animName);
            this.animName = animName;
            currentSkin.Anim.SetTrigger(this.animName);
        }
    }
}
