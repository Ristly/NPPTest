using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PlatformTest.Managers;

class HashManager : IHashManager
{
    public async Task<string> Encrypt(Stream segment, IProgress<string> progress, int segmentNumber, CancellationToken cancellationToken)
    {

        //Было написано для того чтобы успеть прервать обработку
        //progress.Report($"Processing encryption of {segmentNumber} segment");
        //await Task.Delay(3000);

        if(cancellationToken.IsCancellationRequested)
        {
            Console.WriteLine("Segment ->" + segmentNumber + " Not Encrypted");
            cancellationToken.ThrowIfCancellationRequested();
        }
        if (segment == null || segment is { Length : < 1}) throw new ArgumentNullException("segment");
        string output = string.Empty;
        using (SHA256 hash = SHA256.Create())
        {       
            output = Convert.ToHexString(await hash.ComputeHashAsync(segment));
            progress.Report("Segment ->"+ segmentNumber + " Encrypted");
            
        }

        return output;
    }
}
