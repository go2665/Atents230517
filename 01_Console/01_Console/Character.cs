using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_Console
{
    class Character // Character라는 이름의 클래스를 선언
    {
        private int level = 1;
        private float hp = 100.0f;
        private float exp = 0.0f;

        // 기본 생성자
        public Character() 
        {
            level = 1;
            hp = 100.0f;
            exp = 0.0f;
        }

        // hp만 설정하는 생성자


        // level과 hp를 설정하는 생성자
        public Character(int level, float hp)  
        {
            this.level = level; // 맴버인 level에 파라메터로 받은 level을 복사해라.
            this.hp = hp;
            exp = 0.0f;
        }


        public void Attack()
        {
            Console.WriteLine("공격한다.");
        }

        public void Defence()
        {
            Console.WriteLine("방어한다.");
        }

        private void Die()
        {
            Console.WriteLine("죽었다.");
        }
    }
}
