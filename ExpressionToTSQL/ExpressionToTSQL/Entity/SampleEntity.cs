using System.Collections.Generic;

namespace ExpressionToTSQL.Entity
{
    public class SampleEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }

        ////to explain what is relationship
        public ICollection<OtherSampleEntity> OtherSamples { get; set; }
    }

    public class OtherSampleEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SampleEntityId { get; set; }

        //to explain what is relationship
        public SampleEntity SampleEntity { get; set; }
    }

    public class JoinedEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
    }

    public class OtherEntity
    {
        public int NumericSomething { get; set; }
        public string SomeText { get; set; }
    }
}
