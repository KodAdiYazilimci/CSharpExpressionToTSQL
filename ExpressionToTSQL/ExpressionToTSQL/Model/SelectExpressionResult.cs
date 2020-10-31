using System;
using System.Collections.Generic;

namespace ExpressionToTSQL.Model
{
    public class SelectExpressionResult<T>
    {
        public List<PropertyAssignmentModel> PropertyAssignments { get; set; } = new List<PropertyAssignmentModel>();

        public T Build()
        {
            return default(T);
        }
    }

    public class PropertyAssignmentModel
    {
        public string PropertyName { get; set; }
        public Type FromType { get; set; }
        public string FromProperty { get; set; }
    }
}
