public interface IMonsterState
{
    public void OnEnterState();
    public void OnUpdate(FSM fsm);
    public void OnExitState(FSM fsm);
}