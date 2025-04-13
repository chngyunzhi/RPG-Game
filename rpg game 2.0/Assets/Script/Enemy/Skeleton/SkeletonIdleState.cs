using UnityEngine;

public class SkeletonIdleState : SkeletonGroundState
{

    public SkeletonIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName , Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();


        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();

        // AudioManager.instance.PlaySFX(9, enemy.transform);

    }

    public override void Update()
    {
        base.Update();      // can go to skeletonGroundState, because inherit from skeletonGroundState 

        if (stateTimer < 0) stateMachine.ChangeState(enemy.moveState);

    }

}
