using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitIssueManager.Core.Abstractions
{
    public interface IIssueServiceResolver
    {
        IIssueService Resolve(string platform);
    }
}
