using System;

namespace TankGame;

public interface IDamageable
{
    float Health { get; set; }
    float MaxHealth { get; set; }
    void Damage(float damage);
    event EventHandler<EventArgs> Death;
}