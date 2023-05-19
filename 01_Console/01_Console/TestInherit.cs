using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_Console
{
    class TestInherit_Parent
    {
        public int a = 10;
        protected int b = 20;

        public virtual void Test_Func()
        {
            Console.WriteLine("부모");
        }
    }

    // TestInherit_Child클래스가 TestInherit_Parent를 상속받았다.
    class TestInherit_Child : TestInherit_Parent
    {
        public int c = 30;

        public void Test()
        {
            a += 1;
            b += 1;
            c += 1;
        }

        public override void Test_Func()
        {
            base.Test_Func();   // 내가 상속받은 부모의 참조를 가지는 키워드
            Console.WriteLine("자식");
        }
    }
}
