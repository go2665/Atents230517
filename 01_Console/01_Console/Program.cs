namespace _01_Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 5/18--------------------------------------------------------------------------------
            int age = 20;
            
            //// if구문(statement) : if뒤에 있는 소괄호() 안의 조건이 참이면 중괄호{} 안의 코드를 수행
            //if ( age > 18 )   
            //{
            //    Console.WriteLine("성인입니다.");
            //}

            //if(age < 19)    // (age <= 18)
            //{
            //    Console.WriteLine("미성년자입니다.");
            //}

            //// if-else구문
            //// ()안의 조건이 참이면 if아래의 {}안에 있는 코드 수행. 거짓이면 else 아래의 {}안에 있는 코드 수행
            //if( age > 18 )
            //{
            //    Console.WriteLine("성인입니다.");
            //}
            //else
            //{
            //    Console.WriteLine("미성년자입니다.");
            //}

            //if( age < 10 )
            //{
            //    Console.WriteLine("아동입니다.");
            //}
            //else if( age < 20)
            //{
            //    Console.WriteLine("청소년입니다.");
            //}
            //else if ( age < 70 )
            //{
            //    Console.WriteLine("성인입니다.");
            //}
            //else
            //{
            //    Console.WriteLine("노인입니다.");
            //}

            // 실습 
            // 1. 나이를 입력받기
            // 2. 8살 미만이면 "미취학 아동"으로 출력
            // 3. 13살 미만이면 "초등학생"으로 출력
            // 4. 16살 미만이면 "중학생"으로 출력
            // 5. 19살 미만이면 "고등학생"으로 출력
            Console.Write("나이를 입력해 주세요 : ");
            string str = Console.ReadLine();
            int.TryParse(str, out age);
            int category = -1;
            if( age < 8)
            {
                Console.WriteLine("미취학 아동입니다.");
                category = 0;
            }
            else if( age < 13) 
            {
                Console.WriteLine("초등학생입니다.");
                category = 1;
            }
            else if( age < 16) 
            {
                Console.WriteLine("중학생입니다.");
                category = 2;
            }
            else if( age < 19)
            {
                Console.WriteLine("고등학생입니다.");
                category = 3;
            }
            else
            {
                Console.WriteLine("성인입니다.");
                category = 4;
            }

            switch(category)    // ()에서 받은 변수의 값에 따라 다른 코드를 수행하는 조건문
            {
                case 0:
                    Console.WriteLine("미취학 아동의 버스요금은 무료입니다.");
                    break;
                case 1:
                    Console.WriteLine("초등학생의 버스요금은 300원입니다.");
                    break;
                case 2:
                    Console.WriteLine("중학생의 버스요금은 500원입니다.");
                    break;
                case 3:
                    Console.WriteLine("고등학생의 버스요금은 1000원입니다.");
                    break;
                case 4:
                    Console.WriteLine("성인의 버스요금은 1300원입니다.");
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    break;
            }
            


            // 5/18--------------------------------------------------------------------------------

            // 5/17 -------------------------------------------------------------------------------
            //Console.WriteLine("Hello, World!");
            //Console.WriteLine("고병조입니다.");

            //// 변수 : 변하는 숫자(데이터)를 담아 놓는 곳
            //// 데이터 타입 : 어떤 종류의 데이터를 저장하고 크기가 어떻게 되는지를 설정해 놓은 것
            ////  정수(integer) - 소수점이 없는 숫자(1, 100, -5, 7 등등)
            ////  실수(real number) - 소수점이 있는 숫자(1.5, 3.141592 등등)
            ////  불리언(boolean) - true 아니면 false만 저장하는 데이터 타입
            ////  문자열(string) - 글자 여러개를 저장하는 데이터 타입

            //// int : 32bit짜리 부호있는 정수 데이터 타입(약 -21억~+21억 정도)

            //int age = 41;   // age이름의 int 변수에 41이라는 값을 대입해라.
            //Console.WriteLine($"나는 {age}살입니다.");    // $를 이용해서 문자열과 변수를 조합

            //// float : 32bit짜리 실수 데이터 타입. 태생적으로 정밀도 문제가 있다.
            //// 177.5 => 1775 * 10^-1
            //float height = 177.5f;
            //Console.WriteLine($"내 키는 {height:f2}cm입니다");    // float값을 소수점 둘째자리까지 출력

            //// bool : c#의 불리언 타입
            //bool glasses = true;

            //// c#의 string은 변경 불가능한 데이터 타입
            //string address = "서울";
            //Console.WriteLine($"나는 {address}에 삽니다.");
            //address = "부산";
            //Console.WriteLine($"나는 {address}에 삽니다.");

            //string result = Console.ReadLine();
            //Console.WriteLine($"입력받은 문자열 = {result}");

            // 실습 
            // 1. 이름을 입력받고 출력하기
            // 2. 나이를 입력받고 출력하기
            // 3. 키를 입력받고 출력하기
            // 4. 주소를 입력받고 출력하기
            // 단 모든 입력은 별도의 변수에 저장하라

            //Console.Write("이름을 입력하세요 : ");
            //string? name = Console.ReadLine();
            //Console.WriteLine($"당신의 이름은 {name}입니다.");

            //Console.Write("나이를 입력하세요 : ");
            //string? age = Console.ReadLine();
            //int.TryParse( age, out int ageNum );    // 문자를 숫자로 안전하게 바꾸기(바꿀 수 없는 상황이면 0)
            //Console.WriteLine($"당신의 나이는 {ageNum}살 입니다.");

            //Console.Write("키를 입력하세요 : ");
            //string? height = Console.ReadLine();
            //float.TryParse( height, out float heightNum );
            //Console.WriteLine($"당신의 키는 {heightNum}cm 입니다.");

            //Console.Write("주소을 입력하세요 : ");
            //string? address = Console.ReadLine();
            //Console.WriteLine($"주소 : {address}");

            // c#의 nullable 타입
            // 5/17 -------------------------------------------------------------------------------
        }
    }
}


