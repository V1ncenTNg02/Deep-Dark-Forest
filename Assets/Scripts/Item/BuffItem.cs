namespace ORZ.Item
{
    public class BuffItem : ConsumeItem
    {
        public override void UseItem()
        {
            PlayAllParticles particleSystemComponent = GetComponent<PlayAllParticles>();
            particleSystemComponent.LocalPlayAllChildParticles();
            GetBuffed getBuffed = ObjectGetter.player.GetComponent<GetBuffed>();
            getBuffed.startBuff();
        }
    }
}

