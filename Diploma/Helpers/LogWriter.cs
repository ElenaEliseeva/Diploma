using System.IO;
using System.Reflection;

namespace Diploma.Helpers;

public static class LogWriter {
    private static readonly string ExePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

    public static void Write(string logMessage) {
        try {
            using var streamWriter = File.AppendText(ExePath + "\\" + "log.txt");
            streamWriter.Write("\r\nLog Entry : ");
            streamWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            streamWriter.WriteLine("  :");
            streamWriter.WriteLine("  :{0}", logMessage);
            streamWriter.WriteLine("-------------------------------");
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
        }
    }
}