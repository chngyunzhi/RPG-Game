using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public int comboCounter {  get; private set; }

    private float lastTimeAttack;
    private float comboWindow = 2f;

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _StateMachine, string _animBoolName) : base(_player, _StateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // AudioManager.instance.PlaySFX(4);

        #region Attack Direction
        float attackDir = player.facingDir;

        if (xInput != 0) attackDir = xInput;

        #endregion

        if (comboCounter > 2 || Time.time >= lastTimeAttack + comboWindow) comboCounter = 0;
        
        player.anim.SetInteger("ComboCounter", comboCounter);

        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);

        stateTimer = .1f;
    }
     
    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .1f);
        comboCounter++;
        lastTimeAttack = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0) player.SetZeroVelocity();      //prevent lagging while attack

        if (triggerCalled) stateMachine.ChangeState(player.idleState);
    }
}
