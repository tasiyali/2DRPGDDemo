using Godot;
using Godot.Collections;
using System;
using System.Collections;

public partial class Player : CharacterBody2D
{
    //声明玩家的属性，包括最大生命值、最大魔法值、移动速度、伤害、暴击几率和暴击伤害
    [ExportGroup("State")]
    [Export]
    public float maxHealth = 100.0f;
    [Export]
    public float maxMana = 100.0f;
    [Export]
    public float moveSpeed = 5.0f;
    [Export]
    public float damage = 20.0f;
    [Export]
    public float critChance = 0.0f;
    [Export]
    public float critDamage = 0.0f;

    //声明玩家的经验值属性，包括基础经验值和经验值倍数
    [ExportGroup("Exp")]
    [Export]
    public float baseExp = 100.0f;
    [Export]
    public float expMultiplier = 2.0f;

    //声明玩家的经验值属性，包括当前经验值和升级所需经验值
    [Export]
    public float currentExp;
    [Export]
    public float nextLevelExp;
    //声明玩家的等级和积分属性
    [Export]
    public int currentLevel = 1;
    [Export]
    public int currentPoints = 0;

    [ExportGroup("extension")]
    //声明玩家的组件，包括动画精灵、有限状态机、生命值组件和敌人检测区域
    [Export]
    public AnimatedSprite2D PlayerSprite{ set; get;}
    [Export]
    public Fsm Fsm{ set; private get;}
    [Export]
    public HealthComponent HealthComponent{ set; get;}
    [Export]
    public Area2D EnemyArea{ set; private get;}
    [Export] 
    public Node2D Weapon{ set; get;}
    [Export]
    public Timer MagicWeapon { set; get;}

    //声明玩家的方向属性，记录玩家上一次移动的方向
    public string lastDirection = "down";
    public float currentMana;

    public float saveDamage;

    public int strValue = 0;
    public int dexValue = 0;
    public int intValue = 0;

    private Enemy _selectedEnemy;

    public Enemy SelectedEnemy
    {
        get => _selectedEnemy;
        set
        {
            //如果当前选中的敌人有效，则取消选中该敌人
            if (IsInstanceValid(_selectedEnemy))
            {
                _selectedEnemy.DeselectEnemy();
            }
            //如果新选中的敌人有效，则选中该敌人
            _selectedEnemy = IsInstanceValid(value) ? value : null;
            _selectedEnemy?.SelectorEnemy();
        }
    }

    // 面板显示的攻击力（基础 + 装备，不含暴击）
    public float AttackPower
    {
        get
        {
            float total = damage;
            foreach (EquipData equip in GameData.Instance.equipment.Values)
            {
                if (equip != null)
                    total += equip.bounsDamage;
            }
            return total;
        }
    }
    

    // 测试程序
    // public override void _Ready()
    // {
    //     base._Ready();
    //     //初始化玩家的状态，包括重置生命值和魔法值
    //     Setup();
    // }

    //处理玩家的输入事件，当按下“ui_accept”键时增加经验值
    public void _input(InputEvent @event)
    {
        if (@event.IsActionPressed("skill_01"))
        {
            UseSkill(0);
        }
        else if (@event.IsActionPressed("skill_02"))
        {
            UseSkill(1);
        }
        else if (@event.IsActionPressed("skill_03"))
        {
            UseSkill(2);
        }
        else if (@event.IsActionPressed("skill_04"))
        {
            UseSkill(3);
        }
    }

    //存储玩家攻击位置的字典，键为方向，值为对应的节点
    public Dictionary<string, Node> attackPositions;

    public override void _Ready()
    {
        base._Ready();
        
        attackPositions = new()
        {
            ["down"] = GetNode("%Down"),
            ["up"] = GetNode("%Up"),
            ["left"] = GetNode("%Left"),
            ["right"] = GetNode("%Right")
        };
    }

    //在每一帧中调用当前状态的UpdateState方法
    public override void _Process(double delta)
    {
        base._Process(delta);
        Fsm.currentState.UpdateState(delta);
    }

    //检查玩家是否正在移动
    public bool IsMoving()
    {
        string[] moveInput = {"move_up", "move_down", "move_left", "move_right"};
        foreach(string input in moveInput)
        {
            if(Input.IsActionPressed(input))
                return true;
        }
        return false;
    }

    //更新玩家的移动方向
    public void UpdateDirection(Vector2 inputVector)
    {
        //如果输入向量为零，则不更新方向
        if(inputVector == Vector2.Zero)
            return;

        //根据输入向量的方向更新last_direction属性
        if(Mathf.Abs(inputVector.X) > Mathf.Abs(inputVector.Y))
        {
            lastDirection = inputVector.X > 0 ? "right" : "left";
        }
        else
        {
            lastDirection = inputVector.Y > 0 ? "down" : "up";
        }
    }

    //增加玩家的经验值，并检查是否升级
    public void AddExp(float value)
    {
        currentExp += value;
        while(currentExp >= nextLevelExp)
        {
            LevelUp();
        }

        //发射经验值更新信号，通知其他系统玩家的经验值已更新
        EventBus.Instance.EmitSignal(EventBus.SignalName.OnPlayerExpUpdated, currentExp, nextLevelExp);
    }

    public float GetDamage(float skillDamage = 0)
    {
        float totalDmg = damage + skillDamage;

        //装备伤害加成
        foreach(EquipData equip in GameData.Instance.equipment.Values)
        {
            if(equip != null)
            {
                totalDmg += equip.bounsDamage;
            }
        }

        //计算暴击伤害
        if(GD.Randf() * 100.0f <= critChance)
        {
            totalDmg += 1.0f + (critDamage / 100.0f);
        }
        return totalDmg;
    }

    //提升属性
    public void UpdateStat(string atr)
    {
        if(currentPoints <= 0)
        {
            return;
        }

        currentPoints -= 1;
        switch (atr)
        {
            case "STR":
                strValue += 1;
                damage += 1.5f;
                maxHealth += 3;
                ResetHealth();
                break;
            case "DEX":
                dexValue += 1;
                moveSpeed += 2;
                critChance += 2;
                break;
            case "INT":
                intValue += 1;
                maxMana += 15;
                critDamage += 5;
                ResetMana();
                break;
        }
        EventBus.Instance.EmitSignal(EventBus.SignalName.OnPlayerStateUpdated);
    }

    //升级玩家的等级，增加积分，并更新升级所需经验值
    public void LevelUp()
    {
        currentExp -= nextLevelExp;
        currentLevel++;
        currentPoints += 4;
        nextLevelExp *= expMultiplier;
        EventBus.Instance.EmitSignal(EventBus.SignalName.OnPlayerStateUpdated);
        Refs.Instance.CreateNewLevelFx(GlobalPosition + (Vector2.Up * -2));
    }

    //设置玩家的初始状态，包括重置生命值和魔法值
    public void Setup()
    {
        ResetHealth();
        ResetMana();
        nextLevelExp = baseExp;
    }

    public void UseSkill(int index)
    {
        if(index < 0 || index >= GameData.Instance.skillSlot.Count)
        {
            return;
        }
        
        SkillData skill = GameData.Instance.skillSlot[index];
        if(skill == null)
        {
            return;
        }
        Enemy selectedEnemy = SelectedEnemy;
        if(skill.manaCost > currentMana)
        {
            return;
        }

        
        if(skill.itemId == "MagicWeapon")
        {
            saveDamage = damage;
            damage *= 2;
            SkillEffect skillEffect = skill.skillEffectScene.Instantiate<SkillEffect>();
            skillEffect.GlobalPosition = GlobalPosition;
            GetTree().Root.AddChild(skillEffect);
            MagicWeapon.WaitTime = 5.0f;
            MagicWeapon.Start();
            SoundManager.Instance.Play((int)E_Sound.SkillBoost);
        }
        else
        {
            if(!IsInstanceValid(selectedEnemy))
            {
                return;
            }
            float totalDamage = GetDamage(skill.baseDamage);
            selectedEnemy.HealthComponent.TakeDamage(totalDamage);
            SkillEffect skillEffect = skill.skillEffectScene.Instantiate<SkillEffect>();
            skillEffect.GlobalPosition = selectedEnemy.GlobalPosition;
            GetTree().Root.AddChild(skillEffect);
            Refs.Instance.CreateDamageText(selectedEnemy.GlobalPosition, totalDamage);
        }
        UseMana(skill.manaCost);
        
    }

    //播放指定方向的动画
    public void PlayDirectionAnimation(string animName)
    {
        PlayerSprite.Play(animName + "_" + lastDirection);
    }

    //重置玩家的生命值为最大值，并发射生命值更新信号
    public void ResetHealth()
    {
        HealthComponent.Setup(maxHealth);
        //发射生命值更新信号，通知其他系统玩家的生命值已重置
        EventBus.Instance.EmitSignal(EventBus.SignalName.OnPlayerHealthUpdated, maxHealth, maxHealth);
    }

    //重置玩家的魔法值为最大值，并发射魔法值更新信号
    public void ResetMana()
    {
        //重置魔法值为最大值
        currentMana = maxMana;
        //发射魔法值更新信号，通知其他系统玩家的魔法值已重置
        EventBus.Instance.EmitSignal(EventBus.SignalName.OnPlayerManaUpdated, maxMana, maxMana);
    }

    //使用魔法值，并发射魔法值更新信号
    public void UseMana(float value)
    {
        currentMana -= value;
        currentMana = Mathf.Max(currentMana, 0);
        EventBus.Instance.EmitSignal(EventBus.SignalName.OnPlayerManaUpdated, currentMana, maxMana);
    }

    public void AddMana(float value)
    {
        currentMana += value;
        currentMana = Mathf.Min(currentMana, maxMana);
        EventBus.Instance.EmitSignal(EventBus.SignalName.OnPlayerManaUpdated, currentMana, maxMana);
    }

    public void EnableWeaponCollision(bool value)
    {
        EnemyArea.Monitoring = value;
    }

    public void OnHealthComponentOnDead()
    {
        QueueFree();
    }

    public void OnHealthComponentOnHealthChanged(float currentHealth)
    {
        EventBus.Instance.EmitSignal(EventBus.SignalName.OnPlayerHealthUpdated, currentHealth, maxHealth);
    }

    public void OnEnemyAttackAreaAreaEntered(Area2D area)
    {
        float damage = GetDamage();
        if(area is Enemy enemy)
        {
            enemy.HealthComponent.TakeDamage(damage);
            Refs.Instance.CreateDamageFx(enemy.GlobalPosition);
            Refs.Instance.CreateDamageText(enemy.GlobalPosition, damage);
        }
        SoundManager.Instance.Play((int)E_Sound.Import);
    }

    public void OnMagicWeaponTimeout()
    {
        damage = saveDamage;
    }
}
