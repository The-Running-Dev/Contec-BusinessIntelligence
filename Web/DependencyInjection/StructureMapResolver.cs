using System.Web.Http.Dependencies;

using StructureMap;

namespace BI.Web.DependencyInjection
{
    /// <summary>
    /// The structure map dependency resolver.
    /// </summary>
    public class StructureMapResolver : StructureMapScope, IDependencyResolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMapResolver"/> class.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        public StructureMapResolver(IContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// The begin scope.
        /// </summary>
        /// <returns>
        /// The System.Web.Http.Dependencies.IDependencyScope.
        /// </returns>
        public IDependencyScope BeginScope()
        {
            var child = Container.GetNestedContainer();

            return new StructureMapResolver(child);
        }
    }
}