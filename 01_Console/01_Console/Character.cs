using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace _01_Console
{
    class Character // Character라는 이름의 클래스를 선언
    {
        private string name;
        private int level = 1;
        private float hp = 100.0f;
        private float exp = 0.0f;

        public static int test_staticValue = 10;

        // 기본 생성자
        public Character() 
        {
            name = "이름";
            level = 1;
            hp = 100.0f;
            exp = 0.0f;
        }

        // hp만 설정하는 생성자
        public Character(string name, float hp)
        {
            this.name = name;
            this.hp = hp;
            level = 1;
            exp = 0.0f;
        }

        // level과 hp를 설정하는 생성자
        public Character(string name, int level, float hp)  
        {
            this.name = name;
            this.level = level; // 맴버인 level에 파라메터로 받은 level을 복사해라.
            this.hp = hp;
            exp = 0.0f;
        }

        /// <summary>
        /// 공격을 하는 함수
        /// </summary>
        /// <param name="target">공격할 대상</param>
        public void Attack(Character target)
        {
            Console.WriteLine($"공격한다.");
            target.Defence(10);
        }

        /// <summary>
        /// 상대의 공격을 방어하는 함수
        /// </summary>
        /// <param name="damage">받은 데미지</param>
        public void Defence(float damage)
        {
            hp -= damage;
            Console.WriteLine("방어한다.");
        }

        // 공격력과 방어력 변수를 만들고 적용해보기

        private void Die()
        {
            Console.WriteLine("죽었다.");
        }

        public void Test()
        {
            test_staticValue += 10;
        }
        
        public static void Test_Static()
        {
            Console.WriteLine("aaaa");
            //level = 2;
            // static 맴버 함수는 static 맴버만 접근 가능하다.
        }
    }
}
