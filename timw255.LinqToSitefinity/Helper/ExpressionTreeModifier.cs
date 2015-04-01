using System;
using System.Linq;
using System.Linq.Expressions;

namespace timw255.LinqToSitefinity.Helper
{
    internal class ExpressionTreeModifier<T> : ExpressionVisitor
    {
        private IQueryable<T> queryableItems;

        internal ExpressionTreeModifier(IQueryable<T> items)
        {
            this.queryableItems = items;
        }

        internal Expression CopyAndModify(Expression expression)
        {
            return this.Visit(expression);
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            // Replace the constant QueryableSitefinityData arg with the queryable collection.
            if (c.Type == typeof(SitefinityQueryable<T>))
                return Expression.Constant(this.queryableItems);
            else
                return c;
        }
    }
}
