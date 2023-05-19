using System;

namespace _01_Console
{
    internal class Program
    {
        enum AgeCatagory
        {
            Child = 0,
            Elementry,
            Middle,
            High,
            Adult
        }

        enum Grade
        {
            A = 0,
            B,
            C,
            D,
            F
        }

        // 리턴타입 : void(리턴 값이 없다는 표시)
        // 함수 이름 : GuGuDan
        // 파라메터 : ()사이에 선언되어 있는 변수, 이 함수는 없음
        // 함수 바디 : {} 사이에 있는 모든 코드
        static void GuGuDan()
        {
            Console.Write("몇단을 출력할까요? : ");
            string str = Console.ReadLine();
            int.TryParse(str, out int num);
            Console.WriteLine($"{num}이(가) 입력되었습니다.");
            for (int i = 1; i < 10; i++)
            {
                Console.WriteLine($"{num} * {i} = {num * i}");
            }
        }

        // 리턴타입 : void(리턴 값이 없다는 표시)
        // 함수 이름 : GuGuDan_Print
        // 파라메터 : int dan
        // 함수 바디 : {} 사이에 있는 모든 코드
        static void GuGuDan_Print(int dan)
        {
            for (int i = 1; i < 10; i++)
            {
                Console.WriteLine($"{dan} * {i} = {dan * i}");
            }
        }

        static void Triangle_Print(int num)
        {
            for (int i = 0; i < num; i++)
            {
                for (int j = 0; j < i + 1; j++)   // 오름차순
                //for(int j=0;j<num-i;j++)      // 내림차수
                {
                    Console.Write("*");
                }
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            // 5/19 -------------------------------------------------------------------------------
            //Character mike = new Character("마이크", 1, 120);
            //Character jack = new Character("잭", 1, 100);

            //mike.Attack(jack);  // 마이크가 잭을 공격하기


            //TestInherit_Child test = new TestInherit_Child();

            //TestInherit_Parent p = test;
            //p.Test_Func();

            //int[] result = new int[10];            
            //Random r = new Random();
            //for (int i = 0; i<10000000;i++)
            //{
            //    int rand = r.Next(0, 10);
            //    result[rand]++;
            //}

            CharacterBase mike = new Warrior("Mike");
            CharacterBase jack = new Warrior("Jack");
            mike.Attack(jack);
            jack.Skill(mike);

            int j = 0;


            // 5/19 -------------------------------------------------------------------------------

            // 5/18--------------------------------------------------------------------------------
            //int age = 20;

            ////// if구문(statement) : if뒤에 있는 소괄호() 안의 조건이 참이면 중괄호{} 안의 코드를 수행
            ////if ( age > 18 )   
            ////{
            ////    Console.WriteLine("성인입니다.");
            ////}

            ////if(age < 19)    // (age <= 18)
            ////{
            ////    Console.WriteLine("미성년자입니다.");
            ////}

            ////// if-else구문
            ////// ()안의 조건이 참이면 if아래의 {}안에 있는 코드 수행. 거짓이면 else 아래의 {}안에 있는 코드 수행
            ////if( age > 18 )
            ////{
            ////    Console.WriteLine("성인입니다.");
            ////}
            ////else
            ////{
            ////    Console.WriteLine("미성년자입니다.");
            ////}

            ////if( age < 10 )
            ////{
            ////    Console.WriteLine("아동입니다.");
            ////}
            ////else if( age < 20)
            ////{
            ////    Console.WriteLine("청소년입니다.");
            ////}
            ////else if ( age < 70 )
            ////{
            ////    Console.WriteLine("성인입니다.");
            ////}
            ////else
            ////{
            ////    Console.WriteLine("노인입니다.");
            ////}

            //// 실습 
            //// 1. 나이를 입력받기
            //// 2. 8살 미만이면 "미취학 아동"으로 출력
            //// 3. 13살 미만이면 "초등학생"으로 출력
            //// 4. 16살 미만이면 "중학생"으로 출력
            //// 5. 19살 미만이면 "고등학생"으로 출력
            //Console.Write("나이를 입력해 주세요 : ");
            //string str = Console.ReadLine();
            //int.TryParse(str, out age);
            //int category = -1;
            //AgeCatagory eCategory = AgeCatagory.Child;

            //if ( age < 8)
            //{
            //    Console.WriteLine("미취학 아동입니다.");
            //    category = 0;
            //    eCategory = AgeCatagory.Child;
            //}
            //else if( age < 13) 
            //{
            //    Console.WriteLine("초등학생입니다.");
            //    category = 1;
            //    eCategory = AgeCatagory.Elementry;
            //}
            //else if( age < 16) 
            //{
            //    Console.WriteLine("중학생입니다.");
            //    category = 2;
            //    eCategory = AgeCatagory.Middle;
            //}
            //else if( age < 19)
            //{
            //    Console.WriteLine("고등학생입니다.");
            //    category = 3;
            //    eCategory = AgeCatagory.High;
            //}
            //else
            //{
            //    Console.WriteLine("성인입니다.");
            //    category = 4;
            //    eCategory = AgeCatagory.Adult;
            //}

            ////switch(category)    // switch: ()에서 받은 변수의 값에 따라 다른 코드를 수행하는 조건문
            ////{
            ////    case 0:
            ////        Console.WriteLine("미취학 아동의 버스요금은 무료입니다.");
            ////        break;
            ////    case 1:
            ////        Console.WriteLine("초등학생의 버스요금은 300원입니다.");
            ////        break;
            ////    case 2:
            ////        Console.WriteLine("중학생의 버스요금은 500원입니다.");
            ////        break;
            ////    case 3:
            ////        Console.WriteLine("고등학생의 버스요금은 1000원입니다.");
            ////        break;
            ////    case 4:
            ////        Console.WriteLine("성인의 버스요금은 1300원입니다.");
            ////        break;
            ////    default:
            ////        Console.WriteLine("잘못된 입력입니다.");
            ////        break;
            ////}

            //switch (eCategory)
            //{
            //    case AgeCatagory.Child:
            //        Console.WriteLine("미취학 아동의 버스요금은 무료입니다.");
            //        break;
            //    case AgeCatagory.Elementry:
            //        Console.WriteLine("초등학생의 버스요금은 300원입니다.");
            //        break;
            //    case AgeCatagory.Middle:
            //        Console.WriteLine("중학생의 버스요금은 500원입니다.");
            //        break;
            //    case AgeCatagory.High:
            //        Console.WriteLine("고등학생의 버스요금은 1000원입니다.");
            //        break;
            //    case AgeCatagory.Adult:
            //        Console.WriteLine("성인의 버스요금은 1300원입니다.");
            //        break;
            //    default:
            //        Console.WriteLine("잘못된 입력입니다.");
            //        break;
            //}

            //// 1. 성적을 입력 받기(0~100점 사이로 받기)
            //// 2. 범위가 벗어나면 잘못된 입력입니다 라고 출력하기
            //// 3. 91~100점 사이는 A등급, 81~90 : B, 71~80 : C, 61~70 : D, 나머지는 F
            //// 4. 등급은 enum으로 만든 후 변수에 저장하기
            //// 5. switch문을 이용해서 받은 등급을 출력해주기
            //string str = Console.ReadLine();
            //int score = 0;
            //int.TryParse(str, out score);

            //// 연산자(operator)
            //// 산술연산자 : int i = 10 + 5; // +-*/%는 산술연산자
            //// 대입연산자 : =, 오른쪽에 있는 값을 왼쪽에다가 복사해라
            //// 비교연산자 : <, >, <=, >=, ==(같다), !=(다르다)
            //// 논리연산자 : &&(and - 양쪽의 값이 둘다 true일때만 true), ||(or - 양쪽의 값이 하나라도 true면 true) 

            //if((score > 100) || (score < 0))
            //{
            //    Console.WriteLine("잘못된 입력입니다.");
            //}
            //else
            //{
            //    Grade grade = Grade.F;
            //    if(score > 90)
            //    {
            //        grade = Grade.A;
            //    }
            //    else if(score > 80)
            //    {
            //        grade = Grade.B;
            //    }
            //    else if(score > 70)
            //    {
            //        grade = Grade.C;
            //    }
            //    else if( score > 60)
            //    {
            //        grade = Grade.D;
            //    }

            //    Console.WriteLine($"성적은 {grade}입니다.");
            //}


            //Console.Write("*을 몇개 출력할까요? : ");
            //string str = Console.ReadLine();
            //int.TryParse(str, out int num);
            //Console.WriteLine($"{num}이(가) 입력되었습니다.");

            //// while;
            //int count = 0;
            //while(count < num) // ()안이 true면 {}안의 내용 반복해서 실행
            //{
            //    Console.Write("*");
            //    //count = count + 1;
            //    count++;
            //}
            //Console.WriteLine("");

            //// 증감연산자
            //// ++ : 변수에다가 1을 더한 다음 대입한다.
            //// -- : 변수에다가 1을 뺀 다음 대입한다.
            //// += : 왼쪽에 있는 변수에 오른쪽에 있는 값을 더한 후 대입
            //// -= : 왼쪽에 있는 변수에 오른쪽에 있는 값을 뺀 후 대입
            //// *= : 왼쪽에 있는 변수에 오른쪽에 있는 값을 곱한 후 대입
            //// /= : 왼쪽에 있는 변수에 오른쪽에 있는 값을 나눈 후 대입

            //// do-while;
            //count = 0;
            //do
            //{
            //    Console.Write("*");
            //    count++;
            //}
            //while (count < num);
            //Console.WriteLine("");

            //// for
            //for(int i = 0 ; i<num ; i++)    // 증감변수 선언 및 초기화 ; 증감변수로 조건 확인 ; 증감변수를 증감
            //{
            //    Console.Write("*");
            //}
            //Console.WriteLine("");

            // 실습
            // 1. 숫자를 입력받아서 *로 삼각형 그리기            
            // *
            // **
            // ***

            //Console.Write("*을 몇층으로 출력할까요? : ");
            //string str = Console.ReadLine();
            //int.TryParse(str, out int num);
            //Console.WriteLine($"{num}이(가) 입력되었습니다.");

            //for(int i=0;i<num;i++)
            //{
            //    for (int j = 0; j < i+1; j++)   // 오름차순
            //    //for(int j=0;j<num-i;j++)      // 내림차수
            //    {
            //        Console.Write("*");
            //    }
            //    Console.WriteLine();
            //}

            //// 2. 숫자를 입력받아서 입력 받은 구구단 출력하기
            //Console.Write("몇단을 출력할까요? : ");
            //str = Console.ReadLine();
            //int.TryParse(str, out num);
            //Console.WriteLine($"{num}이(가) 입력되었습니다.");
            //for(int i=1; i<10; i++)
            //{
            //    Console.WriteLine($"{num} * {i} = {num * i}");
            //}

            //GuGuDan();
            //GuGuDan_Print(5);

            // 삼각형 찍는 함수 만들기
            //Triangle_Print(15);

            //Character player = new Character();
            //player.Attack();

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


