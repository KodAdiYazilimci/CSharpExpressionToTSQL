using ExpressionToTSQL.Entity;

using System;
using System.Collections.Generic;

namespace ExpressionToTSQL.Mock
{
    public static class MockProvider
    {
        public static List<SampleEntity> GetSampleEntities()
        {
            List<SampleEntity> entities = new List<SampleEntity>();

            for (int i = 0; i < 100; i++)
            {
                SampleEntity sampleEntity = new SampleEntity();
                sampleEntity.Name = GetRandomWord();
                sampleEntity.Year = 1900 + i;
                entities.Add(sampleEntity);
            }

            return entities;
        }

        private static string GetRandomWord()
        {
            string word = string.Empty;

            int randomLength = new Random().Next(3, 10);

            for (int i = 0; i < randomLength; i++)
            {
                bool lowerOrUpper = new Random().Next(0, 10) % 2 == 0;

                if (lowerOrUpper)
                    word += ((char)new Random().Next(65, 90)).ToString();
                else
                    word += ((char)new Random().Next(97, 122)).ToString();
            }

            return word;
        }
    }
}
