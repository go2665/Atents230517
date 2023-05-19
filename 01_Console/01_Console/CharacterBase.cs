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
        /// 생존 여부를 확인할 수 있는 프로퍼티
        /// </summary>
        public bool IsAlive => hp < 0;     // 코드 가독성을 위한 것(의미전달이 더 쉬워진다.)

        /// <summary>
        /// HP를 확인하고 설정할 수 있는 프로퍼티
        /// </summary>
        public float HP
        {
            get => hp;

            protected set // 설정은 protected
            {
                hp = value;
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

        public float MP
        {
            get => mp;

            protected set // 설정은 protected
            {
                mp = value;
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

        /// <summary>
        /// 초기화 하는 함수. 생성자에서 호출할 예정
        /// </summary>
        /// <param name="name">새로 만들어질 캐릭터의 이름</param>
        protected void Initialize(string name)
        {
            this.name = name;                   // 이름 설정하고
            hpMax = 100.0f + strength * 10.0f;  // 힘에 기반해서 HP 증가
            mpMax = 100.0f + wisdom * 10.0f;    // 지혜에 기반해서 MP 증가
        }

        /// <summary>
        /// 캐릭터 공격용 함수
        /// </summary>
        /// <param name="target">공격 대상</param>
        public virtual void Attack(CharacterBase target)
        {
        }

        /// <summary>
        /// 캐릭터 방어용 함수
        /// </summary>
        /// <param name="damage">받은 데미지</param>
        public virtual void Defence(float damage) 
        {
        }

        /// <summary>
        /// 캐릭터 스킬 사용 함수
        /// </summary>
        /// <param name="target">스킬 대상</param>
        public virtual void Skill(CharacterBase target)
        {
        }

        /// <summary>
        /// 캐릭터 사망 처리용 함수
        /// </summary>
        void Die()
        {
            if(IsAlive) // 살아있을 때만 죽는다.
            {
                // 죽는 연출 추가
                Console.WriteLine($"{this.name}이(가) 죽었습니다.");
            }
        }
    }
}
