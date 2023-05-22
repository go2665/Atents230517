using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_Console
{
    internal class CharacterBase
    {
        /// <summary>
        /// 캐릭터의 이름
        /// </summary>
        protected string name = string.Empty;

        /// <summary>
        ///  힘 스텟
        /// </summary>
        protected int strength = 3;
        
        /// <summary>
        /// 민첩 스텟
        /// </summary>
        protected int dexterity = 3;
        
        /// <summary>
        /// 지혜 스텟
        /// </summary>
        protected int wisdom = 3;

        /// <summary>
        /// 현재 HP
        /// </summary>
        protected float hp = 100.0f;

        /// <summary>
        /// 최대 HP
        /// </summary>
        protected float hpMax = 100.0f;

        /// <summary>
        /// 현재 MP
        /// </summary>
        protected float mp = 100.0f;

        /// <summary>
        /// 최대 MP
        /// </summary>
        protected float mpMax = 100.0f;

        /// <summary>
        /// 이름
        /// </summary>
        public string Name => name;

        public virtual string JobName => "[Novice]";

        /// <summary>
        /// 생존 여부를 확인할 수 있는 프로퍼티
        /// </summary>
        public bool IsAlive => hp > 0;     // 코드 가독성을 위한 것(의미전달이 더 쉬워진다.)

        /// <summary>
        /// HP를 확인하고 설정할 수 있는 프로퍼티
        /// </summary>
        public float HP
        {
            get => hp;

            protected set // 설정은 protected
            {
                if( hp != value )
                {
                    hp = value;
                    Console.WriteLine($"{name}의 현재 HP는 ({hp:#0.#})입니다.");
                    if (hp > hpMax) // HP가 넘치는 것 방지
                    {
                        hp = hpMax;
                    }
                    if (hp < 0)     // HP가 0 아래로 내려가면 사망
                    {
                        Die();
                    }
                }
            }
        }

        public float MP
        {
            get => mp;

            protected set // 설정은 protected
            {
                if( mp != value )
                {
                    mp = value;
                    Console.WriteLine($"{name}의 현재 MP는 ({mp:#0.#})입니다.");
                    if (mp > mpMax) // MP가 넘치는 것 방지
                    {
                        mp = mpMax;
                    }
                    if (mp < 0)     // MP가 0 아래로 내려가면 0
                    {
                        mp = 0;
                    }
                }
            }
        }

        /// <summary>
        /// 스테이터스를 결정하는 함수. 생성자에서 호출할 예정
        /// </summary>
        protected virtual void StatusRoll()
        {
        }

        /// <summary>
        /// 초기화 하는 함수. 생성자에서 호출할 예정
        /// </summary>
        /// <param name="name">새로 만들어질 캐릭터의 이름</param>
        protected void Initialize(string name)
        {
            this.name = name;                   // 이름 설정하고
            hpMax = 100.0f + strength * 10.0f;  // 힘에 기반해서 HP 증가
            hp = hpMax;
            mpMax = 100.0f + wisdom * 10.0f;    // 지혜에 기반해서 MP 증가
            mp = mpMax;
        }

        /// <summary>
        /// 스테이터스를 다시 설정하고 연관 스텟을 다시 설정하는 함수
        /// </summary>
        public void StatusReroll()
        {
            StatusRoll();
            Initialize(Name);
        }

        /// <summary>
        /// 캐릭터 공격용 함수
        /// </summary>
        /// <param name="target">공격 대상</param>
        public virtual void Attack(CharacterBase target)
        {
            Console.WriteLine($"{name}의 공격");
        }

        /// <summary>
        /// 캐릭터 방어용 함수
        /// </summary>
        /// <param name="damage">받은 데미지</param>
        public virtual void Defence(float damage) 
        {
            Console.WriteLine($"{name}의 방어");
        }

        /// <summary>
        /// 캐릭터 스킬 사용 함수
        /// </summary>
        /// <param name="target">스킬 대상</param>
        public virtual void Skill(CharacterBase target)
        {
            Console.WriteLine($"{name}의 스킬 발동");
        }

        /// <summary>
        /// 캐릭터 사망 처리용 함수
        /// </summary>
        void Die()
        {
            // 죽는 연출 추가
            Console.WriteLine($"{this.name} 사망");
        }

        public void PrintStatus()
        {
            Console.WriteLine($"┌───────────┬─────────────────────────────────────────────┐");
            Console.WriteLine($"│ Name      │ {name,-33}{JobName,10} │");
            Console.WriteLine($"│ HP │ MP   │ {Gauge(HP, hpMax)} ({HP,3:##0}/{hpMax,3:##0}) │ {Gauge(MP, mpMax)} ({MP,3:##0}/{mpMax,3:##0}) │");
            Console.WriteLine($"│ Status    │ Strength({strength:00}) / Dexterity({dexterity:00}) / Wisdom({wisdom:00})   │");
            Console.WriteLine($"└───────────┴─────────────────────────────────────────────┘");
            Console.WriteLine($"");
        }

        protected string Gauge(float current, float max)
        {
            const int size = 10;
            float scaledRatio = current / max * size;       // 0.0f ~ 10.0f, 0~1을 size만큼 스케일
            int ceilValue = (int)(scaledRatio);             // 소수점 제거
            StringBuilder sb = new StringBuilder(size);     // 무조건 10개 글자로 표시
            for(int i=0;i< size; i++)
            {
                if( i < ceilValue )
                {
                    sb.Append("■");                         // HP 표시
                }
                else
                {

                    if (i != ceilValue || ceilValue == scaledRatio)
                    {
                        sb.Append("□");                     // 빈칸 표시(표시되는 부분 다음 칸이 아니거나, 소수점 제거한 것과 안한 것이 같을 때)
                    }
                    else
                    {
                        sb.Append("▲");                     // 소수점으로 남아있는 부분 표시
                    }
                }
                
            }

            return sb.ToString();
        }

        public void Test_HP(float current, float max)
        {
            HP = current;
            hpMax = max;
        }
    }
}
