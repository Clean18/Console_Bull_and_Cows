using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Number_BaseBall
{
    class Program
    {
        static public int[] numbers; // 랜덤으로 뽑은 int 배열
        static public int currentCount; // 현재 기회
        static public int maxCount;     // 최대 기회
        static public bool isEnd;
        static void Main(string[] args)
        {
            // 컴퓨터는 1~9 중에 랜덤한 4자리 숫자를 뽑는다. 단, 중복은 허용하지 않는다.

            // 유저는 10번의 기회가 있다.

            // 플레이어가 수를 입력하면 컴퓨터는 아래조건에 맞추어 결과를 알려준다.

            // 1. Ball : 자리수는 다르지만 포함된 경우
            // 2. Strike : 자리수와 값이 동일한 경우
            // 3. Out : 숫자가 하나도 맞지 않을 경우
            // 4. HomeRun : 모든 숫자가 자리수와 값이 동일한 경우
            // 5. 예시 : 정답이 3629 일 때, 1234 -> 2Ball / 2649 ->2Strike 1Ball / 4518 -> Out

            // 10번의 기회 소진 전까지 정답을 맞추면 승리하며, 모든 기회를 소진하면 패배한다.

            Init();

            StartScreen();

            while (!isEnd)
            {
                int answer = 0;
                Render();
                StringInput(Console.ReadLine(), ref answer);
                CountCheck();
            }

            End();
        }

        static void Init() // 게임 초기화
        {
            currentCount = 1;
            maxCount = 10;

            isEnd = false;

            RandomNumberInit();
        }

        static void StartScreen()
        {
            Console.Clear();
            Console.WriteLine("***********************************");
            Console.WriteLine("************ 숫자 야구 ************");
            Console.WriteLine("***********************************");
        }

        static void RandomNumberInit() // 랜덤 수 생성 중복X
        {
            Console.WriteLine("랜덤 수 생성중입니다...");

            int[] ranNum = new int[4];
            int i = 0;
            Random random = new Random();

            while (true)
            {
                // 랜덤 수 생성
                int ran = random.Next(1, 10); // 1 ~ 9

                // 랜덤 수 검사
                bool isSame = false;

                foreach (int n in ranNum)
                {
                    if (n == ran)
                    {
                        isSame = true;
                        break;
                    }
                }
                // 배열에 같은 숫자가 없으면 추가
                if (!isSame)
                {
                    ranNum[i] = ran;
                    i++;
                }

                // 4개 생성했으면 탈출
                if (i == 4)
                {
                    numbers = ranNum;
                    break;
                }
            }
        }

        static void Render()
        {
            Console.WriteLine();
            Console.WriteLine($"========== {currentCount} 번째 기회 ==========");
            Console.Write("숫자를 입력하세요 : ");
        }

        static void StringInput(string input, ref int answer)
        {
            // 입력한 값이 숫자인지 체크
            if (!int.TryParse(input, out answer)) // 숫자가 아닐 때
            {
                PrintFalseAnswer();
                return;
            }
            else if (int.TryParse(input, out answer))
            {
                AnswerCheck(answer);
            }
        }

        static void PrintFalseAnswer()
        {
            Console.WriteLine();
            Console.WriteLine("## 네자리 숫자를 입력해주세요 ##");
            Console.WriteLine();
        }

        static void AnswerCheck(int answer)
        {
            if (answer >= 10000)     // 10000보다 크면
            {
                PrintFalseAnswer();
            }
            else if (answer < 10000) // 10000보다 작을 때 9999 ~ 1234
            {
                // 정답 체크
                // 1. Ball : 자리수는 다르지만 포함된 경우
                // 2. Strike : 자리수와 값이 동일한 경우
                // 3. Out : 숫자가 하나도 맞지 않을 경우
                // 4. HomeRun : 모든 숫자가 자리수와 값이 동일한 경우
                // 5. 예시 : 정답이 3629 일 때, 1234 -> 2Ball / 2649 ->2Strike 1Ball / 4518 -> Out

                // 10번의 기회 소진 전까지 정답을 맞추면 승리하며, 모든 기회를 소진하면 패배한다.

                int ballCount = 0;
                int strikeCount = 0;

                bool isOut = false;
                bool isHomeRun = false;

                // answer를 천의 자리부터 0부터 넣는 배열 만들기
                int[] answerArray = new int[4];
                int figure = 1000;
                for (int i = 0; i < 4; i++)
                {
                    answerArray[i] = answer / figure;       // 현재 자리수 배열에 넣기
                    answer -= (answerArray[i] * figure);    // 해당 자리수 제거
                    figure /= 10;                           // 다음 자리수 준비
                }

                // 랜덤 배열과 정답 배열 비교
                for (int numbersIndex = 0; numbersIndex < 4; numbersIndex++)
                {
                    for (int answerIndex = 0; answerIndex < 4; answerIndex++)
                    {
                        // 숫자가 같을 때
                        if (numbers[numbersIndex] == answerArray[answerIndex])
                        {
                            // 숫자 위치가 같으면 스트라이크
                            if (numbersIndex == answerIndex)
                            {
                                strikeCount++;
                            }
                            // 숫자 위치가 다르면 볼
                            else
                            {
                                ballCount++;
                            }
                        }
                    }
                }
                if (strikeCount == 4) // 스트라이크가 4개 == 전부 맞으면
                {
                    isHomeRun = true;
                    isEnd = true;
                    return;
                }
                else if (strikeCount == 0 && ballCount == 0) // 볼 스트라이크가 0개면
                {
                    isOut = true;
                }
                currentCount++;
                PrintStrikeBall(strikeCount, ballCount);
            }
        }

        static void End()
        {
            if (currentCount > 10)
            {
                // 카운트 초과 실패
                PrintLose();
            }
            else
            {
                PrintHomeRun();
            }
        }

        static void PrintHomeRun()
        {
            Console.WriteLine("Home Run!");
            Console.WriteLine("승리했습니다!");
        }

        static void PrintLose()
        {
            Console.WriteLine("기회 초과!");
            Console.WriteLine("패배했습니다ㅠㅠ");
        }

        static void PrintStrikeBall(int strike, int ball)
        {
            Console.WriteLine($"Strike : {strike}  Ball : {ball}");
        }

        static void CountCheck()
        {
            if (currentCount > 10)
            {
                isEnd = true;
            }
        }
    }
}
