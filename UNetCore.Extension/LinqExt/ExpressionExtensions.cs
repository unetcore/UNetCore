
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

public static class ExpressionExtensions
{
    public static Expression And(this Expression left, Expression right)
    {
        ConvertExpressions(ref left, ref right);
        return Expression.And(left, right);
    }

    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> current, Expression<Func<T, bool>> other)
    {
        Expression body = current.Complex(other, (left, right) => Expression.And(left, right));
        if (body == null)
        {
            return null;
        }
        if (body.NodeType == ExpressionType.Lambda)
        {
            return (Expression<Func<T, bool>>)body;
        }
        return Expression.Lambda<Func<T, bool>>(body, current.Parameters);
    }

    public static Expression<Func<T1, T2, bool>> And<T1, T2>(this Expression<Func<T1, T2, bool>> current, Expression<Func<T1, T2, bool>> other)
    {
        Expression body = current.Complex(other, (left, right) => Expression.And(left, right));
        if (body == null)
        {
            return null;
        }
        if (body.NodeType == ExpressionType.Lambda)
        {
            return (Expression<Func<T1, T2, bool>>)body;
        }
        return Expression.Lambda<Func<T1, T2, bool>>(body, current.Parameters);
    }

    public static Expression<Func<T1, T2, T3, bool>> And<T1, T2, T3>(this Expression<Func<T1, T2, T3, bool>> current, Expression<Func<T1, T2, T3, bool>> other)
    {
        Expression body = current.Complex(other, (left, right) => Expression.And(left, right));
        if (body == null)
        {
            return null;
        }
        if (body.NodeType == ExpressionType.Lambda)
        {
            return (Expression<Func<T1, T2, T3, bool>>)body;
        }
        return Expression.Lambda<Func<T1, T2, T3, bool>>(body, current.Parameters);
    }

    private static Expression Complex(this LambdaExpression current, LambdaExpression other, Func<Expression, Expression, Expression> func)
    {
        if ((current == null) && (other == null))
        {
            return null;
        }
        if (other == null)
        {
            return current;
        }
        if (current == null)
        {
            return other;
        }
        if (current.Parameters.Count != other.Parameters.Count)
        {
            return null;
        }
        ReadOnlyCollection<ParameterExpression> parameters = current.Parameters;
        Expression body = current.Body;
        Expression expression2 = ExpressionReplacer.Replace(other.Body, parameters);
        return func(body, expression2);
    }

    private static void ConvertExpressions(ref Expression expression1, ref Expression expression2)
    {
        if (expression1.Type != expression2.Type)
        {
            bool flag = expression1.Type.IsNullOrEmpty();
            bool flag2 = expression2.Type.IsNullOrEmpty();
            if ((flag || flag2) && (expression1.Type.GetNotNullableType() == expression2.Type.GetNotNullableType()))
            {
                if (!flag)
                {
                    expression1 = Expression.Convert(expression1, expression2.Type);
                }
                else if (!flag2)
                {
                    expression2 = Expression.Convert(expression2, expression1.Type);
                }
            }
        }
    }

    public static Expression Equal(this Expression left, Expression right)
    {
        ConvertExpressions(ref left, ref right);
        return Expression.Equal(left, right);
    }

    public static Expression GreaterThan(this Expression left, Expression right)
    {
        ConvertExpressions(ref left, ref right);
        return Expression.GreaterThan(left, right);
    }

    public static Expression GreaterThanOrEqual(this Expression left, Expression right)
    {
        ConvertExpressions(ref left, ref right);
        return Expression.GreaterThanOrEqual(left, right);
    }

    public static Expression Join(this IEnumerable<Expression> list, ExpressionType binarySeparator)
    {
        Func<Expression, Expression, Expression> func = null;
        if (list != null)
        {
            Expression[] source = list.ToArray<Expression>();
            if (source.Length > 0)
            {
                if (func == null)
                {
                    func = (x1, x2) => Expression.MakeBinary(binarySeparator, x1, x2);
                }
                return source.Aggregate<Expression>(func);
            }
        }
        return null;
    }

    public static Expression LessThan(this Expression left, Expression right)
    {
        ConvertExpressions(ref left, ref right);
        return Expression.LessThan(left, right);
    }

    public static Expression LessThanOrEqual(this Expression left, Expression right)
    {
        ConvertExpressions(ref left, ref right);
        return Expression.LessThanOrEqual(left, right);
    }

    public static Expression NotEqual(this Expression left, Expression right)
    {
        ConvertExpressions(ref left, ref right);
        return Expression.NotEqual(left, right);
    }

    public static Expression Or(this Expression expression1, Expression expression2)
    {
        ConvertExpressions(ref expression1, ref expression2);
        return Expression.Or(expression1, expression2);
    }

    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> current, Expression<Func<T, bool>> other)
    {
        Expression body = current.Complex(other, (left, right) => Expression.Or(left, right));
        if (body == null)
        {
            return null;
        }
        if (body.NodeType == ExpressionType.Lambda)
        {
            return (Expression<Func<T, bool>>)body;
        }
        return Expression.Lambda<Func<T, bool>>(body, current.Parameters);
    }

    public static Expression<Func<T1, T2, bool>> Or<T1, T2>(this Expression<Func<T1, T2, bool>> current, Expression<Func<T1, T2, bool>> other)
    {
        Expression body = current.Complex(other, (left, right) => Expression.Or(left, right));
        if (body == null)
        {
            return null;
        }
        if (body.NodeType == ExpressionType.Lambda)
        {
            return (Expression<Func<T1, T2, bool>>)body;
        }
        return Expression.Lambda<Func<T1, T2, bool>>(body, current.Parameters);
    }

    public static Expression<Func<T1, T2, T3, bool>> Or<T1, T2, T3>(this Expression<Func<T1, T2, T3, bool>> current, Expression<Func<T1, T2, T3, bool>> other)
    {
        Expression body = current.Complex(other, (left, right) => Expression.Or(left, right));
        if (body == null)
        {
            return null;
        }
        if (body.NodeType == ExpressionType.Lambda)
        {
            return (Expression<Func<T1, T2, T3, bool>>)body;
        }
        return Expression.Lambda<Func<T1, T2, T3, bool>>(body, current.Parameters);
    }

    public static Expression[] Split(this Expression expression, params ExpressionType[] binarySeparators)
    {
        List<Expression> list = new List<Expression>();
        Split(expression, list, binarySeparators);
        return list.ToArray();
    }

    private static void Split(Expression expression, List<Expression> list, ExpressionType[] binarySeparators)
    {
        if (expression != null)
        {
            if (binarySeparators.Contains<ExpressionType>(expression.NodeType))
            {
                BinaryExpression expression2 = expression as BinaryExpression;
                if (expression2 != null)
                {
                    Split(expression2.Left, list, binarySeparators);
                    Split(expression2.Right, list, binarySeparators);
                }
            }
            else
            {
                list.Add(expression);
            }
        }
    }
}


