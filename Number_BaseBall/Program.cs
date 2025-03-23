using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Number_BaseBall
{
    class Program
    {
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
        }
    }
}
