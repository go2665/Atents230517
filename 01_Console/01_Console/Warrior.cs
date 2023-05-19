using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_Console
{
    internal class Warrior : CharacterBase
    {
        public Warrior(string name) 
        { 
            Random r = new Random();
            strength = 5 + r.Next(0, 6);    // 5 ~ 5+5
            dexterity = r.Next(3, 10);      // 3 ~ 9
            wisdom = r.Next(3, 10);         // 3 ~ 9
            Initialize(name);
        }

        public override void Attack(CharacterBase target)
        {
            float damage = strength * 1.5f;
            target.Defence(damage);
        }

        public override void Defence(float damage)
        {
            float finaleDamage = damage * strength * 0.05f;
            HP -= finaleDamage;
        }

        public override void Skill(CharacterBase target)
        {
            Strike(target);
        }

        private void Strike(CharacterBase target)
        {
            const int cost = 10;            
            if ( MP > cost)
            {
                float damage = strength * 3.5f;
                target.Defence(damage);
                MP -= cost;
            }
        }
    }
}
