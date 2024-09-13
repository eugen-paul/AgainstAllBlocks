public interface Hitable
{
    /// <summary>
    /// is called when the ball hits.
    /// </summary>
    /// <param name="scoreBonus"></param>
    /// <param name="hitPower"></param>
    public void Hit(int scoreBonus, int hitPower = 1);
}