namespace ORZ.Item
{
    public class BulletItem : ConsumeItem
    {
        public override void UseItem()
        {   
            PlayAllParticles particleSystemComponent = GetComponent<PlayAllParticles>();
            particleSystemComponent.PlayAllChildParticles();
            Destroy(gameObject);
        }
    }

}
