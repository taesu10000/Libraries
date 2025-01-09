using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cognex.VisionPro;

using Common.Vision.Insepction;

namespace Common.Vision.Cognex
{
    public interface ICognexInspectionResult : IDominoInspectionResult
    {
        ICogRecord Record { get; }
    }
}
