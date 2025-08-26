using System.Collections.Generic;

public class Enemy
{
    public SerializableGuid ID { get; }
    public EnemyData EnemyData { get; }

    public Enemy(EnemyData enemyData)
    {
        ID = SerializableGuid.NewGuid();
        this.EnemyData = enemyData;
    }

}
