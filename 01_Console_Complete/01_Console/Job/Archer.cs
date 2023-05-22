using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace _01_Console
{
    internal class Archer : CharacterBase
    {
        Random random = new Random((int)DateTime.Now.Ticks);
        public override string JobName => "[Archer]";

        public Archer(string name)
        {
            StatusRoll();
            Initialize(name);
        }

        protected override void StatusRoll()
        {
            strength = random.Next(3, 10);       // 3 ~ 9
            dexterity = 5 + random.Next(0, 6);   // 5 ~ 5+5
            wisdom = random.Next(3, 10);         // 3 ~ 9
        }

        public override void Attack(CharacterBase target)
        {
            base.Attack(target);

            float damage = Shoot();
            Console.WriteLine($" - {target.Name}에게 ({damage:f1})만큼의 공격을 합니다.");
            target.Defence(damage);
        }

        public override void Defence(float damage)
        {
            base.Defence(damage);

            if (random.NextSingle() > (dexterity * 0.1f))
            {
                Console.WriteLine($" - 적의 공격을 회피하는데 실패합니다. ({damage:f1})만큼의 데미지를 입습니다.");
                HP -= damage;
            }    
            else
            {
                Console.WriteLine($" - 적의 공격을 회피합니다.");
            }
        }

        public override void Skill(CharacterBase target)
        {
            base.Skill(target);

            MultiShoot(target);
        }

        private void MultiShoot(CharacterBase target)
        {
            const int cost = 40;            
            if ( MP > cost)
            {
                int shootCount = random.Next(1, 4);
                float[] damages = new float[shootCount];
                Console.WriteLine($" - 멀티샷 : {target.Name}에게 ({shootCount})만큼의 화살을 쏩니다.");
                for (int i = 0; i < shootCount; i++)
                {
                    damages[i] = Shoot();
                    Console.WriteLine($" - {target.Name}에게 화살을 쏘아 ({damages[i]:f1})만큼의 공격을 합니다.");
                }
                foreach (int damage in damages)
                {
                    target.Defence(damage);
                }
                MP -= cost;
            }
            else
            {
                Console.WriteLine($" - MP가 부족합니다. 멀티샷 발동이 취소됩니다.");
            }
        }

        private float Shoot()
        {
            float damage = dexterity * 0.5f + dexterity * random.NextSingle();  // 기본 데미지는 민첩의 0.5~1.5배
            float maxStatus = 10.0f;
            float criticalRate = dexterity * 0.5f;      // 크리티컬 확률은 민첩의 절반
            if( maxStatus * random.NextSingle() < criticalRate )
            {
                Console.WriteLine("크리티컬!");
                damage *= 2.0f;                         // 크리티컬은 다시 두배
            }
            return damage;
        }
    }
}
