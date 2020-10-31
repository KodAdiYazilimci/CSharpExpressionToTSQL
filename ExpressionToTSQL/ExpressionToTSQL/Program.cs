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

            //SampleEntity sampleEntity = context.Samples.Where(x => x.Year == 2000).SortByDesc(x => x.Year).FetchFirst();

            //List<SampleEntity> sampleEntities = context.Samples.Where(x => x.Year < 2000).FetchList();

            //List<SampleEntity> sampleEntities = context.Samples.Where(x => x.Year < 2000).SortBy(x => x.Name).FetchList();

            //List<SampleEntity> sortedEntities = context.Samples.Where(x => x.Year >= 1900)
            //                                                   .SortBy(x => x.Name)
            //                                                   .SortBy(x => x.Year)
            //                                                   .SortByDesc(x => x.Year)
            //                                                   .FetchList();

            //List<SampleEntity> skippedAndTakenEntities = context.Samples.Skip(2).Take(3).FetchList();


            var simpleSelects = context.Samples
                                        .Where(x => x.Year > 2000)
                                        .SortByDesc(x => x.Year)
                                        .Select<SampleEntity, OtherEntity>(x => new OtherEntity
                                        {
                                            NumericSomething = x.Id,
                                            SomeText = x.Name
                                        })
                                        .FetchList();

            var joinedSelectedEntities = context.Samples
                                .Where(x => x.Year > 2000)
                                .SortByDesc(x => x.Year)
                                .JoinWith<SampleEntity, OtherSampleEntity>(sampleEntity => sampleEntity.Id, otherEntity => otherEntity.SampleEntityId)
                                .Where(x => !string.IsNullOrEmpty(x.Name))
                                .SortBy(x => x.Name)
                                .Select<OtherSampleEntity, SampleEntity, JoinedEntity>((x, y) => new JoinedEntity
                                {
                                    Id = x.Id,
                                    Name = y.Name,
                                    Year = y.Year
                                })
                                .FetchList();

        }
    }
}
