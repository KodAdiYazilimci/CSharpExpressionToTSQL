using ExpressionToTSQL.Entity;
using ExpressionToTSQL.Other;
using ExpressionToTSQL.Persistence;
using ExpressionToTSQL.Util;

using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionToTSQL
{
    static class Program
    {
        static void Main(string[] args)
        {
            //Test.TestExpressions();

            Context context = new Context("connection string..");

            //SampleEntity sampleEntity = context.Samples.Where(x => x.Year == 2000).FetchFirst();

            SampleEntity sampleEntity = context.Samples.Where(x => x.Year == 2000).SortByDesc(x => x.Year).FetchFirst();

            //List<SampleEntity> sampleEntities = context.Samples.Where(x => x.Year < 2000).FetchList();

            List<SampleEntity> sortedEntities = context.Samples.Where(x => x.Year >= 1900)
                                                               .SortBy(x => x.Name)
                                                               .SortBy(x => x.Year)
                                                               .FetchList();
        }
    }
}
