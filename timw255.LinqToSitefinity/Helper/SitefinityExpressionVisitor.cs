using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Modules;

namespace timw255.LinqToSitefinity.Helper
{
    internal class SitefinityExpressionVisitor : ExpressionVisitor
    {
        private Expression expression;

        private readonly StringBuilder _whereClause = new StringBuilder();

        private SitefinityRequest _parameters;
        
        public SitefinityExpressionVisitor(Expression exp)
        {
            this._parameters = new SitefinityRequest();
            this.expression = exp;
        }

        public SitefinityRequest Translate()
        {
            this.Visit(this.expression);
            this._parameters.Filter = this._whereClause.ToString();
            return this._parameters;
        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            this._whereClause.Append("(");

            Expression left = b.Left;
            Expression right = b.Right;

            this.Visit(left);

            switch (b.NodeType)
            {
                case ExpressionType.AndAlso:
                    _whereClause.Append(" AND ");
                    break;
                case ExpressionType.Equal:
                    if (right == null)
                        _whereClause.Append(" IS NULL");
                    else
                        _whereClause.Append(" = ");
                    break;
                case ExpressionType.GreaterThan:
                    _whereClause.Append(" > ");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    _whereClause.Append(" >= ");
                    break;
                case ExpressionType.LessThan:
                    _whereClause.Append(" < ");
                    break;
                case ExpressionType.LessThanOrEqual:
                    _whereClause.Append(" <= ");
                    break;
                case ExpressionType.NotEqual:
                    if (right == null)
                        _whereClause.Append(" IS NOT NULL");
                    else
                        _whereClause.Append(" <> ");
                    break;
                case ExpressionType.OrElse:
                    _whereClause.Append(" OR ");
                    break;
                default:
                    throw new NotSupportedException(string.Format("{0} statement is not supported", b.NodeType.ToString()));
            }

            if (right != null)
                this.Visit(right);

            this._whereClause.Append(")");

            return b;
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            DateTime temp;

            if(DateTime.TryParse(c.ToString(), out temp))
            {
                _whereClause.AppendFormat("DateTime.Parse(\"{0}\")", c.ToString());
            }
            else
            {
                _whereClause.Append(c.ToString());
            }

            return c;
        }

        protected override Expression VisitMemberAccess(MemberExpression m)
        {
            if (m.Expression.Type == typeof(LstringSingleViewModel))
            {
                this.Visit(m.Expression);
            }
            else
            {
                _whereClause.AppendFormat("{0}", m.Member.Name);
            }

            return m;
        }

        protected override Expression VisitMethodCall(MethodCallExpression mExp)
        {
            if (mExp.Method.DeclaringType == typeof(Queryable) || mExp.Method.DeclaringType == typeof(Enumerable))
			{
                switch (mExp.Method.Name)
                {
                    case "Take":
                        this.Visit(mExp.Arguments[0]);
                        this._parameters.Take = (int)((ConstantExpression)mExp.Arguments[1]).Value;
                        return mExp;
                    case "Skip":
                        this.Visit(mExp.Arguments[0]);
                        this._parameters.Skip = (int)((ConstantExpression)mExp.Arguments[1]).Value;
                        return mExp;
                    case "Where":
                        InnermostWhereFinder whereFinder = new InnermostWhereFinder();
                        MethodCallExpression whereExpression = whereFinder.GetInnermostWhere(expression);
                        LambdaExpression lambdaExpression = (LambdaExpression)((UnaryExpression)(whereExpression.Arguments[1])).Operand;

                        lambdaExpression = (LambdaExpression)Evaluator.PartialEval(lambdaExpression);

                        this.Visit(lambdaExpression);
                        return mExp;
                }
            }

            this.Visit(mExp.Object);

            if (mExp.Arguments != null && mExp.Arguments.Count > 0)
            {
                _whereClause.AppendFormat(".{0}({1})", mExp.Method.Name, mExp.Arguments[0].ToString());
            }
            else
            {
                _whereClause.AppendFormat(".{0}({1})", mExp.Method.Name, "");
            }

            return mExp;
        }
    }
}
