// See https://aka.ms/new-console-template for more information
using PlatformTest.Managers;
using System.ComponentModel.DataAnnotations;
using System.Text;

Console.WriteLine("Укажите полный путь к файлу:");
var path = Console.ReadLine();

while (string.IsNullOrWhiteSpace(path) || string.IsNullOrEmpty(path))
{
    Console.WriteLine("Введена пустая строка или пробел\nУкажите полный путь к файлу:");
    path = Console.ReadLine();
}

Console.WriteLine("Укажите количество сегментов:");
int n;
var line =Console.ReadLine();
while (string.IsNullOrWhiteSpace(path) || string.IsNullOrEmpty(path) || !int.TryParse(line, out n))
{
    Console.WriteLine("Указанно не корректное значение\nУкажите количество сегментов:");
    line = Console.ReadLine();
}

IHashManager hashManager = new HashManager();

using FileStream fs = File.OpenRead(path);


if (n <= 0) n = 1;

var temp = fs.Length / n;

int len;

if (temp > int.MaxValue)
    len = (int)(temp / int.MaxValue);
else 
    len = (int)temp;

var stringBuilder = new StringBuilder();

IProgress<string> progress = new Progress<string>(value => Console.WriteLine(value)) ;

List<Task<string>> tasks = new List<Task<string>>();

//для прерывания обработки
CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
CancellationToken token = cancelTokenSource.Token;
for (int i = 1; i <= n; i++)
{ 
    
    var buffer = new byte[len];
    fs.Read(buffer, 0, len);
    tasks.Add(hashManager.Encrypt(new MemoryStream(buffer), progress, i, token));

}



try
{

    
    Task.WaitAll(tasks.ToArray());
    
    foreach (var task in tasks)
    {
        
        stringBuilder.Append(task.Result);
    }
    Console.WriteLine(stringBuilder.ToString());
}
catch(AggregateException)
{
    Console.Write("Хэширование было прервано");
}

cancelTokenSource.Dispose();

