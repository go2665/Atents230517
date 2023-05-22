using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _01_Console
{
    internal class Warrior : CharacterBase
    {
        public override string JobName => "[Warrior]";

        public Warrior(string name) 
        {
            StatusRoll();
            Initialize(name);
        }

        protected override void StatusRoll()
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            strength = 5 + r.Next(0, 6);    // 5 ~ 5+5
            dexterity = r.Next(3, 10);      // 3 ~ 9
            wisdom = r.Next(3, 10);         // 3 ~ 9
        }

        public override void Attack(CharacterBase target)
        {
            base.Attack(target);

            float damage = strength * 1.5f; //힘의 1.5배
            Console.WriteLine($" - {target.Name}에게 ({damage:f1})만큼의 공격을 합니다.");
            target.Defence(damage);
        }

        public override void Defence(float damage)
        {
            base.Defence(damage);
            
            // 받은 데미지를 힘에 비례해서 %로 감소(힘이 10일 때 50% 감소, 힘이 1일 때 5% 감소)
            float finaleDamage = damage * strength * 0.05f;
            Console.WriteLine($" - 적의 공격을 방어하여 최종적으로 ({finaleDamage:f1})만큼의 데미지를 입습니다.");
            HP -= finaleDamage;
        }

        public override void Skill(CharacterBase target)
        {
            base.Skill(target);

            Strike(target);
        }

        private void Strike(CharacterBase target)
        {
            const int cost = 50;                    // MP 소모 10
            if ( MP > cost)
            {
                float damage = strength * 3.5f;     // 힘의 3.5배로 데미지를 줌
                Console.WriteLine($" - 스트라이크 : {target.Name}에게 ({damage:f1})만큼의 공격을 합니다.");
                MP -= cost;
                target.Defence(damage);
            }
            else
            {
                Console.WriteLine($" - MP가 부족합니다. 스트라이크 발동이 취소됩니다.");
            }
        }
    }
}
