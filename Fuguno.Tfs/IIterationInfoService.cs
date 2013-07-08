using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuguno.Tfs
{
    public interface IIterationInfoService
    {
        IterationInfo GetCurrentIterationInfo();
    }
}
