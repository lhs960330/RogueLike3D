
public class Define
{
    // ���� �̰� ������ ����� Map�� ������ �׶����� �߰��� ����
    public enum ActionMap
    {
        Player,
        UI,
    }
    public enum Layer
    {
        Default,
        TramsparentFX,
        IgnoreRaycast,
        Water = 4,
        UI,
        Player,
        Monster,
        Ground,

    }
    public enum PlayerAni
    {
        None,
        Attack,
        Dash,
        Jump,
        //추가(스킬?)
    }
}
