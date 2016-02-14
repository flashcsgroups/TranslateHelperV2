using System;
using System.Collections.Generic;
using System.Linq;
using PortableCore.BL.Contracts;
using PortableCore.DL;
using PortableCore.DAL;

namespace PortableCore.BL.Managers
{
	public interface ISourceExpressionManager
    {
        IEnumerable<SourceExpression> GetSourceExpressionCollection(string sourceText, TranslateDirection direction);
    }
}

