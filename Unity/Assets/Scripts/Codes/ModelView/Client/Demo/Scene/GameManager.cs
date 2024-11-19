namespace ET.Client
{
    [ComponentOf(typeof (Scene))]
    public class GameManager: Entity, IAwake, IDestroy, ILoad, IUpdate, IFixedUpdate
    {
        [StaticField]
        public static GameManager Instance;
        
        //Test
        public long SingleTimer;
    }
}