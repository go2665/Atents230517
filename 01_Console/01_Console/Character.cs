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
