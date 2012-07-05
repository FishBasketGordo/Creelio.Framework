namespace Creelio.Framework.Templating.Sql
{
    using System;
    using Creelio.Framework.Core;
    using Creelio.Framework.Core.Extensions;
    using Creelio.Framework.Core.Extensions.MaybeMonad;

    public abstract class ValueConstraint<T> : Constraint, IColumnBasedConstraint
    {
        private Column _column = null;

        private T _value = default(T);

        private bool _nullValue = false;

        private Guard<Column> _columnGuard = null;

        private Guard<T> _valueGuard = null;

        protected ValueConstraint(Column column, T value)
            : this(column, value, true)
        {
        }

        protected ValueConstraint(Column column, T value, bool allowNullValue)
        {
            AllowNullValue = allowNullValue;
            Column = column;
            Value = value;
        }

        protected ValueConstraint(Column column, DBNull value)
            : this(column, default(T), true)
        {
            NullValue = true;
        }

        private ValueConstraint()
        {
        }

        public Column Column
        {
            get
            {
                return _column;
            }

            set
            {
                _column = ColumnGuard.Evaluate(value);
            }
        }

        public T Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = ValueGuard.Evaluate(value);
                _nullValue = IsNull(value);
            }
        }

        public virtual bool NullValue
        {
            get
            {
                return _nullValue;
            }

            set
            {
                _nullValue = value;
                _value = default(T);
            }
        }

        protected virtual Guard<Column> ColumnGuard
        {
            get
            {
                if (_columnGuard == null)
                {
                    _columnGuard = new Guard<Column>()
                        .AddPredicate(
                            c => c != null,
                            c => new ArgumentNullException("Column cannot be null."))
                        .AddPredicate(
                            c => c.Type == typeof(T),
                            c => new ArgumentException(string.Format(
                                 "The type of the column '{0}' does not match the type of the constraint '{1}'.",
                                 c.Type,
                                 typeof(T))));
                }

                return _columnGuard;
            }
        }

        protected virtual Guard<T> ValueGuard
        {
            get
            {
                if (_valueGuard == null)
                {
                    _valueGuard = new Guard<T>()
                        .AddPredicate(
                            v => v != null || AllowNullValue,
                            v => new ArgumentNullException("Value cannot be null."));
                }

                return _valueGuard;
            }
        }

        protected bool AllowNullValue { get; set; }

        public override bool Equals(Constraint other)
        {
            if (!base.Equals(other))
            {
                return false;
            }

            ValueConstraint<T> otherValueConstraint = other as ValueConstraint<T>;

            if (otherValueConstraint == null)
            {
                return false;
            }

            if (!Column.ValueEquals(otherValueConstraint.Column))
            {
                return false;
            }

            if (Value != null && !Value.Equals(otherValueConstraint.Value))
            {
                return false;
            }
            else if (NullValue && !otherValueConstraint.NullValue)
            {
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            return string.Format(
                "{0} {1} {2}", 
                ColumnToString(Column), 
                GetOperatorForValue(Value), 
                ValueToString(Value));
        }

        protected abstract string GetOperatorForValue(T value);

        protected virtual string ColumnToString(Column column)
        {
            var alias = Parent.Return(
                p => p.TableAliases.GetOrReturnDefault(column.Table.TableName), 
                null);

            return string.IsNullOrEmpty(alias)
                 ? column.ToString()
                 : column.ToString(alias);
        }

        protected virtual string ValueToString(T value)
        {
            return ValueToString<T>(value);
        }

        protected string ValueToString<TValue>(TValue value)
        {
            if (NullValue || IsNull(value))
            {
                return "NULL";
            }
            else
            {
                var byteArrayValue = value as byte[];
                string valueAsString;

                if (byteArrayValue != null)
                {
                    valueAsString = byteArrayValue.ToHexString();
                }
                else
                {
                    valueAsString = SqlStringEscape(value.ToString());
                }

                return Column.NeedToSingleQuote(Column.Type)
                     ? valueAsString.ToSingleQuotedString()
                     : valueAsString;
            }
        }

        protected bool IsNull(object value)
        {
            return value == null || value == DBNull.Value;
        }

        protected string SqlStringEscape(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            else
            {
                return s.Replace("'", "''");
            }
        }
    }
}