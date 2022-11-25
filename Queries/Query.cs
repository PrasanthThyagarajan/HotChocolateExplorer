using HotChocolateExplorer.DBConfig;
using HotChocolateExplorer.Models;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using System.Linq;

namespace HotChocolateExplorer.Queries
{
    public class Query
    {
        /// <summary>
        /// Gets all students.
        /// </summary>
        [UseOffsetPaging]
        [UseSorting]
        [UseFiltering]
        [GraphQLName("GetStudents")]
        public IQueryable<Student> GetStudents([Service] SchoolContext schoolContext) =>
            schoolContext.Students.AsQueryable();
    }

    public class QueryType : ObjectType<Query>
    {
        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {
            descriptor
                .Field(f => f.GetStudents(default))
                .Type<ListType<NonNullType<StringType>>>()
                .UseFiltering();
        }
    }
}
