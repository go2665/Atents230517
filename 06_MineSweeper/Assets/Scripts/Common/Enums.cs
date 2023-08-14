using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum OpenCellType
{
    Empty = 0,
    Number1,
    Number2,
    Number3,
    Number4,
    Number5,
    Number6,
    Number7,
    Number8,
    Mine_NotFound,
    Mine_Explotion,
    Mine_Mistake
}

public enum CloseCellType
{
    Close = 0,
    Close_Press,
    Question,
    Question_Press,
    Flag
}