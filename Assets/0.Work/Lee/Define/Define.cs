
public class Define
{
    // ���� �̰� ������ ����� Map�� ������ �׶����� �߰��� ����
    public enum ActionMap
    {
        Player,
        UI,
    }
    public enum PlayerAni
    {
        None,
        Attack,
        Dash,
        Jump,
        //추가(스킬?)
    }
    public enum PlayerState
    {
        Idle,
        Moving,
        Jumping,
        Falling,
        Landing
    }
    
}
