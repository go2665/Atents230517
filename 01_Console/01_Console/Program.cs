namespace _01_Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
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

            Console.Write("이름을 입력하세요 : ");
            string? name = Console.ReadLine();
            Console.WriteLine($"당신의 이름은 {name}입니다.");

            Console.Write("나이를 입력하세요 : ");
            string? age = Console.ReadLine();
            int.TryParse( age, out int ageNum );    // 문자를 숫자로 안전하게 바꾸기(바꿀 수 없는 상황이면 0)
            Console.WriteLine($"당신의 나이는 {ageNum}살 입니다.");
            
            Console.Write("키를 입력하세요 : ");
            string? height = Console.ReadLine();
            float.TryParse( height, out float heightNum );
            Console.WriteLine($"당신의 키는 {heightNum}cm 입니다.");

            Console.Write("주소을 입력하세요 : ");
            string? address = Console.ReadLine();
            Console.WriteLine($"주소 : {address}");

            // c#의 nullable 타입
            // 5/17 -------------------------------------------------------------------------------
        }
    }
}


