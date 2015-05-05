using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using timw255.LinqToSitefinity.Helper;

namespace timw255.LinqToSitefinity
{
    public class SitefinityQueryProvider : IQueryProvider
    {
        public SitefinityContext Context { get; set; }

        public IQueryable CreateQuery(Expression expression)
        {
            Type elementType = TypeSystem.GetElementType(expression.Type);
            try
            {
                return (IQueryable)Activator.CreateInstance(typeof(SitefinityQueryable<>).MakeGenericType(elementType), new object[] { this, expression });
            }
            catch (System.Reflection.TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        public IQueryable<TResult> CreateQuery<TResult>(Expression expression)
        {
            return new SitefinityQueryable<TResult>(this, expression);
        }

        public object Execute(Expression expression)
        {
            Type elementType = TypeSystem.GetElementType(expression.Type);
            Type type = this.GetType();
            Type[] typeArray = new Type[] { elementType };
            return type.GetMethod("Execute", typeArray).Invoke(this, new object[] { expression });
        }

        public TResult Execute<TResult>(Expression expression)
        {
            TResult tResult;
            bool flag = (typeof(TResult).Name == "IEnumerable`1" ? true : typeof(TResult).Name == "IEnumerable");
            Type genericType = (new MethodCallExpressionTypeFinder()).GetGenericType(expression);
            Type[] typeArray = new Type[] { genericType };
            MethodInfo method = this.Context.GetType().GetMethod("Execute", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            MethodInfo methodInfo = method.MakeGenericMethod(typeArray);
            try
            {
                SitefinityContext context = this.Context;
                object[] objArray = new object[] { expression, flag };
                tResult = (TResult)methodInfo.Invoke(context, objArray);
            }
            catch (TargetInvocationException targetInvocationException1)
            {
                TargetInvocationException targetInvocationException = targetInvocationException1;
                if (targetInvocationException.InnerException != null)
                {
                    throw targetInvocationException.InnerException;
                }
                throw;
            }
            return tResult;
        }
    }
}
