This project converts the expressions to T-SQL queries.

For an example, the below lambda query will be converted to the SQL query which below on it.

var joinedSelectedEntitiesForOne = context.Samples
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
                            .FetchFirst();


SELECT 
TOP 1 
othersampleentity.Id AS Id, 
sampleentity.Name AS Name, 
sampleentity.Year AS Year  
FROM SampleEntity sampleEntity 
JOIN OtherSampleEntity othersampleentity ON sampleentity.Id = othersampleentity.SampleEntityId 
WHERE sampleentity.Year > 2000 AND !(othersampleentity.Name = '')
ORDER BY sampleentity.Name ASC, othersampleentity.Year DESC
