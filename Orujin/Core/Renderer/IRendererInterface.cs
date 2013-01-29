using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orujin.Core.Renderer
{
    public interface IRendererInterface
    {
        RendererPackage PrepareRendererPackage();
        void Update(float elapsedTime);
        void AdjustBrightness(float newBrightness);
    }
}
