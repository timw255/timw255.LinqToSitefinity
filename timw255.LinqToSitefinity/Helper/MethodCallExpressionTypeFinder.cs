using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace timw255.LinqToSitefinity.Helper
{
    internal class MethodCallExpressionTypeFinder : ExpressionVisitor
    {
        private Type genericType;

        public MethodCallExpressionTypeFinder()
        {
        }

        public Type GetGenericType(Expression exp)
        {
            this.Visit(exp);
            return this.genericType;
        }

        protected override Expression VisitMethodCall(MethodCallExpression expression)
        {
            if (expression.Arguments.Count > 0)
            {
                this.genericType = expression.Method.GetGenericArguments()[0];
            }
            this.Visit(expression.Arguments[0]);
            return expression;
        }
    }
}
