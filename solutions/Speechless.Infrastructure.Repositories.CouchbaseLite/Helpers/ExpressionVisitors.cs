using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Query = Couchbase.Lite.Query;

namespace Speechless.Infrastructure.Repositories.CouchbaseLite.Helpers
{
    public abstract class VisitorBase
    {
        protected readonly Expression Node;

        protected VisitorBase(Expression node)
        {
            Node = node ?? throw new ArgumentNullException(nameof(node));
        }

        public abstract Query.IExpression Visit();

        public static VisitorBase CreateFromExpression(Expression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.IsTrue:
                case ExpressionType.IsFalse:
                case ExpressionType.Constant:
                    return new ConstantVisitor((ConstantExpression)node);

                case ExpressionType.Parameter:
                    return new ParameterVisitor((ParameterExpression)node);

                case ExpressionType.MemberAccess:
                    return new MemberVisitor((MemberExpression)node);

                case ExpressionType.Conditional:
                    return new ConditionalVisitor((ConditionalExpression)node);

                case ExpressionType.Negate:
                case ExpressionType.Not:
                    return new UnaryVisitor((UnaryExpression)node);

                case ExpressionType.Add:
                case ExpressionType.Subtract:
                case ExpressionType.Multiply:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.AndAlso:
                case ExpressionType.OrElse:
                case ExpressionType.NotEqual:
                case ExpressionType.Equal:
                case ExpressionType.GreaterThan:
                case ExpressionType.LessThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThanOrEqual:
                    return new BinaryVisitor((BinaryExpression)node);

                case ExpressionType.Call:
                    return new MethodCallVisitor((MethodCallExpression)node);

                case ExpressionType.Lambda:
                    return new LambdaVisitor((LambdaExpression)node);

                default:
                    return default;
            }
        }
    }

    public class ConstantVisitor : VisitorBase
    {
        protected new ConstantExpression Node => base.Node as ConstantExpression;

        public ConstantVisitor(ConstantExpression node) : base(node)
        {
        }

        public override Query.IExpression Visit()
        {
            if (Node.NodeType == ExpressionType.IsTrue)
                return Query.Expression.Boolean(true);
            if (Node.NodeType == ExpressionType.IsFalse)
                return Query.Expression.Boolean(false);
            if (Node.Value is bool) return Query.Expression.Boolean((bool)Node.Value);
            if (Node.Value is short
                || Node.Value is ushort
                || Node.Value is int
                || Node.Value is uint) return Query.Expression.Int((int)Node.Value);
            if (Node.Value is long
                || Node.Value is ulong) return Query.Expression.Long((long)Node.Value);
            if (Node.Value is float) return Query.Expression.Float((float)Node.Value);
            if (Node.Value is double) return Query.Expression.Double((double)Node.Value);
            if (Node.Value.GetType() == typeof(DateTime)) return Query.Expression.Date(new DateTimeOffset((DateTime)Node.Value));
            if (Node.Value.GetType() == typeof(DateTimeOffset)) return Query.Expression.Date((DateTimeOffset)Node.Value);
            if (Node.Value is IList) return Query.Expression.Array(Node.Value as IList);
            if (Node.Value is string) return Query.Expression.String(Node.Value as string);
            if (Node.Value is IDictionary<string, object>) return Query.Expression.Dictionary(Node.Value as IDictionary<string, object>);
            return Query.Expression.Value(Node.Value);
        }
    }

    public class ParameterVisitor : VisitorBase
    {
        protected new ParameterExpression Node => base.Node as ParameterExpression;

        public ParameterVisitor(ParameterExpression node) : base(node)
        {
        }

        public override Query.IExpression Visit()
        {
            return Query.Expression.Parameter(Node.Name);
        }
    }

    public class MemberVisitor : VisitorBase
    {
        protected new MemberExpression Node => base.Node as MemberExpression;

        public MemberVisitor(MemberExpression node) : base(node)
        {
        }

        public override Query.IExpression Visit()
        {
            switch (Node.Member.MemberType)
            {
                case MemberTypes.Field:
                case MemberTypes.Property:
                    return Query.Expression.Property(Node.Member.Name);

                default:
                    throw new InvalidCastException($"Conversion of {Node} to {nameof(Query.IExpression)} is forbidden! " +
                        "Only field and property expressions are allowed.");
            }
        }
    }

    public class ConditionalVisitor : VisitorBase
    {
        protected new ConditionalExpression Node => base.Node as ConditionalExpression;

        public ConditionalVisitor(ConditionalExpression node) : base(node)
        {
        }

        public override Query.IExpression Visit() => CreateFromExpression(Node.IfTrue).Visit();
    }

    public class UnaryVisitor : VisitorBase
    {
        protected new UnaryExpression Node => base.Node as UnaryExpression;

        public UnaryVisitor(UnaryExpression node) : base(node)
        {
        }

        public override Query.IExpression Visit()
        {
            var expression = CreateFromExpression(Node.Operand).Visit();
            if (Node.NodeType == ExpressionType.Not)
                return Query.Expression.Not(expression);
            if (Node.NodeType == ExpressionType.Negate)
                return Query.Expression.Negated(expression);
            return expression;
        }
    }

    public class BinaryVisitor : VisitorBase
    {
        protected new BinaryExpression Node => base.Node as BinaryExpression;

        public BinaryVisitor(BinaryExpression node) : base(node)
        {
        }

        public override Query.IExpression Visit()
        {
            var left = CreateFromExpression(Node.Left).Visit();
            switch (Node.NodeType)
            {
                case ExpressionType.Add:
                    return left.Add(CreateFromExpression(Node.Right).Visit());

                case ExpressionType.Subtract:
                    return left.Subtract(CreateFromExpression(Node.Right).Visit());

                case ExpressionType.Multiply:
                    return left.Multiply(CreateFromExpression(Node.Right).Visit());

                case ExpressionType.Divide:
                    return left.Divide(CreateFromExpression(Node.Right).Visit());

                case ExpressionType.Modulo:
                    return left.Modulo(CreateFromExpression(Node.Right).Visit());

                case ExpressionType.AndAlso:
                    return left.And(CreateFromExpression(Node.Right).Visit());

                case ExpressionType.OrElse:
                    return left.Or(CreateFromExpression(Node.Right).Visit());

                case ExpressionType.NotEqual when Expression.Lambda<Func<bool>>(Expression.Equal(Node.Left, Expression.Constant(null, typeof(object)))).Compile()():
                    return left.NotNullOrMissing();

                case ExpressionType.NotEqual:
                    return left.NotEqualTo(CreateFromExpression(Node.Right).Visit());

                case ExpressionType.Equal:
                    return left.EqualTo(CreateFromExpression(Node.Right).Visit());

                case ExpressionType.GreaterThan:
                    return left.GreaterThan(CreateFromExpression(Node.Right).Visit());

                case ExpressionType.GreaterThanOrEqual:
                    return left.GreaterThanOrEqualTo(CreateFromExpression(Node.Right).Visit());

                case ExpressionType.LessThan:
                    return left.LessThan(CreateFromExpression(Node.Right).Visit());

                case ExpressionType.LessThanOrEqual:
                    return left.LessThanOrEqualTo(CreateFromExpression(Node.Right).Visit());

                case ExpressionType.Call:
                    if (Node.Method.Name.Equals("Contains", StringComparison.OrdinalIgnoreCase) && left is Query.ArrayExpression)
                        return Query.ArrayFunction.Contains(left, CreateFromExpression(Node.Right).Visit());
                    if (Node.Method.Name.Equals("Length", StringComparison.OrdinalIgnoreCase) && left is Query.ArrayExpression)
                        return Query.ArrayFunction.Length(CreateFromExpression(Node.Right).Visit());
                    if (Node.Method.Name.Equals("StartsWith", StringComparison.OrdinalIgnoreCase)
                        || Node.Method.Name.Equals("EndsWith", StringComparison.OrdinalIgnoreCase))
                        return left.Like(CreateFromExpression(Node.Right).Visit());
                    return CreateFromExpression(Expression.Call(Node.Left, Node.Method, Node.Right)).Visit();
            }
            return default;
        }
    }

    public class MethodCallVisitor : VisitorBase
    {
        protected new MethodCallExpression Node => base.Node as MethodCallExpression;

        public MethodCallVisitor(MethodCallExpression node) : base(node)
        {
        }

        public override Query.IExpression Visit()
        {
            var name = Node.Method.Name;
            if (Node.Object == null && name.Equals("Abs", StringComparison.OrdinalIgnoreCase))
                return Query.Function.Abs(CreateFromExpression(Node.Arguments[0]).Visit());
            if (Node.Object == null && name.Equals("Acos", StringComparison.OrdinalIgnoreCase))
                return Query.Function.Acos(CreateFromExpression(Node.Arguments[0]).Visit());
            if (Node.Object == null && name.Equals("Asin", StringComparison.OrdinalIgnoreCase))
                return Query.Function.Asin(CreateFromExpression(Node.Arguments[0]).Visit());
            if (Node.Object == null && name.Equals("Atan", StringComparison.OrdinalIgnoreCase))
                return Query.Function.Atan(CreateFromExpression(Node.Arguments[0]).Visit());
            if (Node.Object == null && name.Equals("Atan2", StringComparison.OrdinalIgnoreCase))
                return Query.Function.Atan2(CreateFromExpression(Node.Arguments[0]).Visit(), CreateFromExpression(Node.Arguments[1]).Visit());
            if (Node.Object == null && name.Equals("Avg", StringComparison.OrdinalIgnoreCase))
                return Query.Function.Avg(CreateFromExpression(Node.Arguments[0]).Visit());
            if (Node.Object == null && name.Equals("Ceil", StringComparison.OrdinalIgnoreCase))
                return Query.Function.Ceil(CreateFromExpression(Node.Arguments[0]).Visit());
            if ((Node.Method.Name.Equals("Contains", StringComparison.OrdinalIgnoreCase))
                        && Node.Arguments[0] is ConstantExpression left
                        && left.Value is string
                        && Node.Arguments[0] is ConstantExpression right
                        && right.Value is string)
                return Query.Function.Contains(CreateFromExpression(left).Visit(), CreateFromExpression(right).Visit());
            if (Node.Object == null && name.Equals("Cos", StringComparison.OrdinalIgnoreCase))
                return Query.Function.Cos(CreateFromExpression(Node.Arguments[0]).Visit());
            if (Node.Object == null && name.Equals("Count", StringComparison.OrdinalIgnoreCase))
                return Query.Function.Count(CreateFromExpression(Node.Arguments[0]).Visit());
            if (Node.Object == null && name.Equals("E", StringComparison.OrdinalIgnoreCase))
                return Query.Function.E();
            if (Node.Object == null && name.Equals("Exp", StringComparison.OrdinalIgnoreCase))
                return Query.Function.Exp(CreateFromExpression(Node.Arguments[0]).Visit());
            if (Node.Object == null && name.Equals("Floor", StringComparison.OrdinalIgnoreCase))
                return Query.Function.Floor(CreateFromExpression(Node.Arguments[0]).Visit());
            if (Node.Arguments[0] is ConstantExpression larg && larg.Value is string && name.Equals("Length", StringComparison.OrdinalIgnoreCase))
                return Query.Function.Length(CreateFromExpression(Node.Arguments[0]).Visit());
            if (Node.Object == null && name.Equals("Log", StringComparison.OrdinalIgnoreCase))
                return Query.Function.Ln(CreateFromExpression(Node.Arguments[0]).Visit());
            if (Node.Object == null && name.Equals("Log10", StringComparison.OrdinalIgnoreCase))
                return Query.Function.Log(CreateFromExpression(Node.Arguments[0]).Visit());
            if (Node.Arguments[0] is ConstantExpression lowerArg0 && lowerArg0.Value is string && name.Equals("Lower", StringComparison.OrdinalIgnoreCase))
                return Query.Function.Lower(CreateFromExpression(Node.Arguments[0]).Visit());
            if (Node.Arguments[0] is ConstantExpression ltrimArg0 && ltrimArg0.Value is string && name.Equals("TrimStart", StringComparison.OrdinalIgnoreCase))
                return Query.Function.Ltrim(CreateFromExpression(Node.Arguments[0]).Visit());
            if (Node.Arguments[0] is ConstantExpression rtrimArg0 && rtrimArg0.Value is string && name.Equals("TrimEnd", StringComparison.OrdinalIgnoreCase))
                return Query.Function.Rtrim(CreateFromExpression(Node.Arguments[0]).Visit());
            if (Node.Arguments[0] is ConstantExpression trimArg0 && trimArg0.Value is string && name.Equals("Trim", StringComparison.OrdinalIgnoreCase))
                return Query.Function.Trim(CreateFromExpression(Node.Arguments[0]).Visit());

            if (Node.Arguments[0] is ConstantExpression upperArg0 && upperArg0.Value is string && name.Equals("Upper", StringComparison.OrdinalIgnoreCase))
                return Query.Function.Upper(CreateFromExpression(Node.Arguments[0]).Visit());

            if (Node.Object == null && name.Equals("Min", StringComparison.OrdinalIgnoreCase))
                return Query.Function.Min(CreateFromExpression(Node.Arguments[0]).Visit());
            if (Node.Object == null && name.Equals("Max", StringComparison.OrdinalIgnoreCase))
                return Query.Function.Max(CreateFromExpression(Node.Arguments[0]).Visit());
            if (Node.Object == null && name.Equals("Pi", StringComparison.OrdinalIgnoreCase))
                return Query.Function.Pi();
            if (Node.Object == null && name.Equals("Pow", StringComparison.OrdinalIgnoreCase))
                return Query.Function.Power(CreateFromExpression(Node.Arguments[0]).Visit(), CreateFromExpression(Node.Arguments[1]).Visit());
            if (Node.Object == null && name.Equals("Round", StringComparison.OrdinalIgnoreCase))
                return Query.Function.Round(CreateFromExpression(Node.Arguments[0]).Visit());
            if (Node.Object == null && name.Equals("Sign", StringComparison.OrdinalIgnoreCase))
                return Query.Function.Sign(CreateFromExpression(Node.Arguments[0]).Visit());
            if (Node.Object == null && name.Equals("Sin", StringComparison.OrdinalIgnoreCase))
                return Query.Function.Sin(CreateFromExpression(Node.Arguments[0]).Visit());

            if (Node.Object == null && name.Equals("Sqrt", StringComparison.OrdinalIgnoreCase))
                return Query.Function.Sqrt(CreateFromExpression(Node.Arguments[0]).Visit());
            if (Node.Object == null && name.Equals("Tan", StringComparison.OrdinalIgnoreCase))
                return Query.Function.Tan(CreateFromExpression(Node.Arguments[0]).Visit());
            if (Node.Object == null && name.Equals("Truncate", StringComparison.OrdinalIgnoreCase))
                return Query.Function.Trunc(CreateFromExpression(Node.Arguments[0]).Visit());

            return default;
        }
    }

    public class LambdaVisitor : VisitorBase
    {
        private readonly LambdaExpression node;

        public LambdaVisitor(LambdaExpression node) : base(node)
        {
            this.node = node ?? throw new ArgumentNullException(nameof(node));
        }

        public override Query.IExpression Visit() => CreateFromExpression(node.Body).Visit();
    }
}