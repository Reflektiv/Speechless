using System;
using System.Linq.Expressions;
using Query = Couchbase.Lite.Query;

namespace Speechless.Infrastructure.Repositories.CouchbaseLite.Helpers
{
    public abstract class Visitor
    {
        private readonly Expression node;
        public ExpressionType NodeType => this.node.NodeType;

        protected Visitor(Expression node)
        {
            this.node = node;
        }

        public abstract Query.IExpression Visit();

        public static Visitor CreateFromExpression(Expression node)
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

                case ExpressionType.Lambda:
                    return new LambdaVisitor((LambdaExpression)node);

                default:
                    return default;
            }
        }
    }

    public class ConstantVisitor : Visitor
    {
        private readonly ConstantExpression node;

        public ConstantVisitor(ConstantExpression node) : base(node)
        {
            this.node = node ?? throw new ArgumentNullException(nameof(node));
        }

        public override Query.IExpression Visit()
        {
            if (node.NodeType == ExpressionType.IsTrue)
                return Query.Expression.Boolean(true);
            if (node.NodeType == ExpressionType.IsFalse)
                return Query.Expression.Boolean(false);
            return Query.Expression.Value(node.Value);
        }
    }

    public class ParameterVisitor : Visitor
    {
        private readonly ParameterExpression node;

        public ParameterVisitor(ParameterExpression node) : base(node)
        {
            this.node = node ?? throw new ArgumentNullException(nameof(node));
        }

        public override Query.IExpression Visit() => Query.Expression.Parameter(node.Name);
    }

    public class MemberVisitor : Visitor
    {
        private readonly MemberExpression node;

        public MemberVisitor(MemberExpression node) : base(node)
        {
            this.node = node ?? throw new ArgumentNullException(nameof(node));
        }

        public override Query.IExpression Visit() => Query.Expression.Property(node.Member.Name);
    }

    public class ConditionalVisitor : Visitor
    {
        private readonly ConditionalExpression node;

        public ConditionalVisitor(ConditionalExpression node) : base(node)
        {
            this.node = node ?? throw new ArgumentNullException(nameof(node));
        }

        public override Query.IExpression Visit() => CreateFromExpression(node.IfTrue).Visit();
    }

    public class UnaryVisitor : Visitor
    {
        private readonly UnaryExpression node;

        public UnaryVisitor(UnaryExpression node) : base(node)
        {
            this.node = node ?? throw new ArgumentNullException(nameof(node));
        }

        public override Query.IExpression Visit()
        {
            var expression = CreateFromExpression(node.Operand).Visit();
            if (node.NodeType == ExpressionType.Not)
                return Query.Expression.Not(expression);
            if (node.NodeType == ExpressionType.Negate)
                return Query.Expression.Negated(expression);
            return expression;
        }
    }

    public class BinaryVisitor : Visitor
    {
        private readonly BinaryExpression node;

        public BinaryVisitor(BinaryExpression node) : base(node)
        {
            this.node = node ?? throw new ArgumentNullException(nameof(node));
        }

        public override Query.IExpression Visit()
        {
            var expression = CreateFromExpression(node.Left).Visit();
            switch (node.NodeType)
            {
                case ExpressionType.Add:
                    return expression.Add(CreateFromExpression(node.Right).Visit());

                case ExpressionType.Subtract:
                    return expression.Subtract(CreateFromExpression(node.Right).Visit());

                case ExpressionType.Multiply:
                    return expression.Multiply(CreateFromExpression(node.Right).Visit());

                case ExpressionType.Divide:
                    return expression.Divide(CreateFromExpression(node.Right).Visit());

                case ExpressionType.Modulo:
                    return expression.Modulo(CreateFromExpression(node.Right).Visit());

                case ExpressionType.AndAlso:
                    return expression.And(CreateFromExpression(node.Right).Visit());

                case ExpressionType.OrElse:
                    return expression.Or(CreateFromExpression(node.Right).Visit());

                case ExpressionType.NotEqual when Expression.Lambda<Func<bool>>(Expression.Equal(node.Left, Expression.Constant(null, typeof(object)))).Compile()():
                    return expression.NotNullOrMissing();

                case ExpressionType.NotEqual:
                    return expression.NotEqualTo(CreateFromExpression(node.Right).Visit());

                case ExpressionType.Equal:
                    return expression.EqualTo(CreateFromExpression(node.Right).Visit());

                case ExpressionType.GreaterThan:
                    return expression.GreaterThan(CreateFromExpression(node.Right).Visit());

                case ExpressionType.GreaterThanOrEqual:
                    return expression.GreaterThanOrEqualTo(CreateFromExpression(node.Right).Visit());

                case ExpressionType.LessThan:
                    return expression.LessThan(CreateFromExpression(node.Right).Visit());

                case ExpressionType.LessThanOrEqual:
                    return expression.LessThanOrEqualTo(CreateFromExpression(node.Right).Visit());
            }

            return expression;
        }
    }

    public class LambdaVisitor : Visitor
    {
        private readonly LambdaExpression node;

        public LambdaVisitor(LambdaExpression node) : base(node)
        {
            this.node = node ?? throw new ArgumentNullException(nameof(node));
        }

        public override Query.IExpression Visit() => CreateFromExpression(node.Body).Visit();
    }
}
