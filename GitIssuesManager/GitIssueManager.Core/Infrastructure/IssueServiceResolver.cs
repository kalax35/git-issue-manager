using GitIssueManager.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitIssueManager.Core.Infrastructure
{
    public class IssueServiceResolver : IIssueServiceResolver
    {
        private readonly Dictionary<string, IIssueService> _map;

        public IssueServiceResolver(IEnumerable<IIssueService> services)
        {
            _map = services.ToDictionary(s => s.Platform.ToLowerInvariant(), s => s);
        }

        public IIssueService Resolve(string platform)
        {
            var key = platform.ToLowerInvariant();
            if (!_map.TryGetValue(key, out var service))
                throw new KeyNotFoundException($"Unsupported platform '{platform}'.");
            return service;
        }
    }
}
