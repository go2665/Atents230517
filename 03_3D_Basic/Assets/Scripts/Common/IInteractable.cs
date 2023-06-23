using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 인터페이스 : 어떤 클래스가 반드시 이런이런 기능을 가지고 있어야 한다고 명시해 놓은 것

// 인터페이스는 기본적으로 public
// 인터페이스는 상속하는데 갯수의 제한이 없다.
// 인터페이스에는 선언만 있다.(구현이 포함되어있지 않다)
// 인터페이스에는 변수가 들어갈 수 없다.
// 인터페이스를 상속받은 클래스는 반드시 인터페이스의 맴버들을 구현해야 한다.

public interface IInteractable
{    
    bool IsDirectUse
    {
        get;    // 상호작용 가능한 오브젝트가 직접 사용가능한 것인지, 간접 사용 가능한 것인지 표시하기 위한 프로퍼티
    }

    void Use(); // 사용하는 기능이 있다고 선언해 놓은 것
}
