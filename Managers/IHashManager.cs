using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformTest.Managers;

interface IHashManager
{
    public Task<string> Encrypt(Stream segment, IProgress<string> progress, int segmentNumber, CancellationToken cancellationToken);
}
