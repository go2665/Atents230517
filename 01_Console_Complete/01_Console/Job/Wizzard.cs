using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_Console
{
    internal class Wizzard : CharacterBase
    {
        Random random = new Random((int)DateTime.Now.Ticks);
        public override string JobName => "[Wizzard]";

        public Wizzard(string name)
        {
            StatusRoll();
            Initialize(name);
        }

        protected override void StatusRoll()
        {
            strength = random.Next(3, 10);   // 3 ~ 9
            dexterity = random.Next(3, 10);  // 3 ~ 9
            wisdom = 5 + random.Next(0, 6);  // 5 ~ 5+5
        }

        public override void Attack(CharacterBase target)
        {
            base.Attack(target);

            float damage = wisdom * (0.8f + random.NextSingle() * 1.2f);   // 지혜의 0.8배 ~ 2.0배
            Console.WriteLine($" - {target.Name}에게 ({damage:f1})만큼의 공격을 합니다.");
            target.Defence(damage);
        }

        public override void Defence(float damage)
        {
            base.Defence(damage);

            // 데미지가 shieldCapacity 이하면 데미지의 10%만, shieldCapacity를 초과하면 HP와 MP 둘 다 피해
            const float shieldCapacity = 10.0f;
            if( damage > shieldCapacity )
            {
                float mpDamage = damage * 0.5f;
                Console.WriteLine($" - 적의 공격에 방어막이 파괴됩니다.");
                Console.WriteLine($" - ({damage:f1})만큼의 피해를 입습니다. ({mpDamage:f1})만큼의 마나가 손상됩니다.");
                HP -= damage;
                MP -= mpDamage;
            }
            else
            {
                float finaleDamage = damage * 0.1f;
                Console.WriteLine($" - 적의 공격을 방어막으로 막아 ({finaleDamage:f1})만큼의 데미지만 입습니다.");
                HP -= finaleDamage;
            }
        }

        public override void Skill(CharacterBase target)
        {
            base.Skill(target);

            Fireball(target);
        }

        private void Fireball(CharacterBase target)
        {
            const int cost = 10;            
            if ( MP > cost)
            {
                float damage = wisdom * random.NextSingle() * 10.0f; // 0 ~ 지혜의 10배
                Console.WriteLine($" - 파이어볼 : {target.Name}에게 ({damage:f1})만큼의 공격을 합니다.");
                target.Defence(damage);
                MP -= cost;
            }
            else
            {
                Console.WriteLine($" - MP가 부족합니다. 파이어볼 발동이 취소됩니다.");
            }
        }
    }
}
