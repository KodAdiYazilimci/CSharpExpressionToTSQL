using ExpressionToTSQL.Entity;
using ExpressionToTSQL.Persistence;
using ExpressionToTSQL.Util;

using System;
using System.Linq;

namespace ExpressionToTSQL
{
    static class Program
    {
        static void Main(string[] args)
        {
            Context context = new Context("connection string..");
            SampleEntity sampleEntity = context.Samples.Where(x => x.Year == 2000).FirstOrDefault();


            if (sampleEntity != null)
            {
                Console.WriteLine($"Name:{ sampleEntity.Name }");
                Console.WriteLine($"Year:{sampleEntity.Year}");
            }
            else
                Console.WriteLine("No data fetched");

            Console.ReadKey();
        }
    }
}
