using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statify.Tests
{
    public class MockSession : ISession
    {
        private readonly Dictionary<string, byte[]> _sessionData = new Dictionary<string, byte[]>();

        public string Id => "TestId";

        public bool IsAvailable => true;

        public IEnumerable<string> Keys => _sessionData.Keys;

        public void Clear()
        {
            _sessionData.Clear();
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task LoadAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public void Remove(string key)
        {
            _sessionData.Remove(key);
        }

        public void Set(string key, byte[] value)
        {
            _sessionData[key] = value;
        }

        public bool TryGetValue(string key, out byte[] value)
        {
            return _sessionData.TryGetValue(key, out value!);
        }
    }
}
