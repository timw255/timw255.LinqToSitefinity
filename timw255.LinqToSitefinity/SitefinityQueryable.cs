using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace timw255.LinqToSitefinity
{
    public class SitefinityQueryable<TData> : IQueryable<TData>
    {
        #region Constructors
        /// <summary>
        /// This constructor is called by the client to create the data source.
        /// </summary>
        public SitefinityQueryable(SitefinityContext context)
        {
            Provider = new SitefinityQueryProvider();
            Expression = Expression.Constant(this);
            ((SitefinityQueryProvider)this.Provider).Context = context;
        }

        /// <summary>
        /// This constructor is called by Provider.CreateQuery().
        /// </summary>
        /// <param name="expression"></param>
        internal SitefinityQueryable(SitefinityQueryProvider provider, Expression expression)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (!typeof(IQueryable<TData>).IsAssignableFrom(expression.Type))
            {
                throw new ArgumentOutOfRangeException("expression");
            }

            Provider = provider;
            Expression = expression;
        }
        #endregion

        #region Properties

        public IQueryProvider Provider { get; private set; }
        public Expression Expression { get; private set; }

        public Type ElementType
        {
            get { return typeof(TData); }
        }

        #endregion

        #region Enumerators
        public IEnumerator<TData> GetEnumerator()
        {
            return (this.Provider.Execute<IEnumerable<TData>>(Expression)).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (this.Provider.Execute<System.Collections.IEnumerable>(Expression)).GetEnumerator();
        }
        #endregion
    }
}
