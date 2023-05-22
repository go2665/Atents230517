using System;

namespace _01_Console
{

    public enum JobType
    {
        Novice = 0,
        Warrior,
        Wizzard,
        Archer
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // 내 이름 입력받기
            string? myName = null;
            while(myName == null)
            {
                Console.Write("당신의 이름을 입력하세요 : ");
                string? tempName = Console.ReadLine();

                Console.Write($"{tempName}으로 결정하시겠습니까? (y/n) : ");
                string? answer = Console.ReadLine();
                if (answer == "y" || answer == "Y" || answer == "yes" || answer == "Yes" || answer == "YES")
                {
                    myName = tempName;
                }
                else
                {
                    myName = null;
                    Console.WriteLine("취소하겠습니다.");
                }
            }
            Console.WriteLine($"당신의 이름은 [{myName}]입니다.");

            // 내 직업 선택하기
            JobType job = JobType.Novice;
            while( job == JobType.Novice )
            {
                Console.Write($"당신의 직업을 선택하세요 (1.{JobType.Warrior} 2.{JobType.Wizzard} 3.{JobType.Archer}) : ");
                string? answer = Console.ReadLine();
                if( int.TryParse(answer, out int select) )
                {
                    switch(select)
                    {
                        case 1:
                            job = JobType.Warrior;
                            Console.WriteLine($"{job}를 선택하였습니다.");
                            break;
                        case 2:
                            job = JobType.Wizzard;
                            Console.WriteLine($"{job}를 선택하였습니다.");
                            break;
                        case 3:
                            job = JobType.Archer;
                            Console.WriteLine($"{job}를 선택하였습니다.");
                            break;
                        default:
                            Console.WriteLine("잘못된 선택입니다.");
                            break;
                    }
                }
            }

            // 내 캐릭터 생성
            CharacterBase player;
            switch (job)
            {
                case JobType.Warrior:
                    player = new Warrior(myName);
                    break;
                case JobType.Wizzard:
                    player = new Wizzard(myName);
                    break;
                case JobType.Archer:
                    player = new Archer(myName);
                    break;
                case JobType.Novice:
                default:
                    player = new CharacterBase();
                    break;
            }

            // 적 캐릭터 생성
            CharacterBase villian;
            Random random = new Random((int)DateTime.Now.Ticks);
            Console.WriteLine("\n\n적이 나타났습니다.");

            string villianName = "Villain";
            string villianJob;
            JobType villianJobType = (JobType)random.Next(1, 4);
            switch (villianJobType)
            {
                case JobType.Warrior:
                    villian = new Warrior(villianName);
                    villianJob = JobType.Warrior.ToString();
                    break;
                case JobType.Wizzard:
                    villian = new Wizzard(villianName);
                    villianJob = JobType.Wizzard.ToString();
                    break;
                case JobType.Archer:
                    villian = new Archer(villianName);
                    villianJob = JobType.Archer.ToString();
                    break;
                case JobType.Novice:
                default:
                    villian = new CharacterBase();
                    villianJob = JobType.Novice.ToString();
                    break;
            }
            Console.WriteLine($"적의 이름은 {villian.Name}. {villianJob} 입니다.");


            // 하나가 죽을 때까지 반복
            int turnCount = 0;
            while(player.IsAlive && villian.IsAlive)
            {
                turnCount++;
                Console.WriteLine($"\n\n{turnCount}턴 시작------------------------------------------------");
                player.PrintStatus();
                villian.PrintStatus();

                Console.Write($"당신의 행동을 선택하세요 (1.일반공격 2.스킬) : ");
                int action = 0;
                while(action == 0)
                {
                    string? answer = Console.ReadLine();
                    if (int.TryParse(answer, out action))
                    {
                        switch (action)
                        {
                            case 1:
                                player.Attack(villian);
                                break;
                            case 2:
                                player.Skill(villian);
                                break;
                            default:
                                Console.WriteLine("잘못된 선택입니다.");
                                break;
                        }
                    }
                }

                if(villian.IsAlive)
                {
                    if( random.NextSingle() < 0.7f )
                    {
                        villian.Attack(player);
                    }
                    else
                    {
                        villian.Skill(player);
                    }
                }
            }

            // 게임 종료
            if( player.IsAlive )
            {
                Console.WriteLine($"\n\n\nGame Clear!\n당신의 승리입니다");
            }
            else
            {
                Console.WriteLine($"\n\n\nGame Over...\n당신의 패배입니다");
            }
        }
    }
}


