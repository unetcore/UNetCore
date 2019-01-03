using System;
using System.Collections.Generic;
using System.Linq;

    using System.Linq.Expressions;

    public class ExpressionReplacer :  ExpressionVisitor
    {
        private ICollection<ParameterExpression> pars;

        public ExpressionReplacer(ICollection<ParameterExpression> pars)
        {
            this.pars = pars;
        }

        public static Expression Replace(Expression expression, ICollection<ParameterExpression> pars)
        {
            ExpressionReplacer replacer = new ExpressionReplacer(pars);
            return replacer.Visit(expression);
        }

        protected override Expression VisitMember(MemberExpression memberExp)
        {
            Func<ParameterExpression, bool> predicate = null;
            ParameterExpression parExp = memberExp.Expression as ParameterExpression;
            if (parExp != null)
            {
                if (predicate == null)
                {
                    predicate = s => s.Type == parExp.Type;
                }
                ParameterExpression expression = this.pars.FirstOrDefault<ParameterExpression>(predicate);
                if (expression != null)
                {
                    return Expression.MakeMemberAccess(expression, memberExp.Member);
                }
            }
            return memberExp;
        }
    }


