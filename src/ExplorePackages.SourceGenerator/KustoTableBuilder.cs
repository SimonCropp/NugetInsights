﻿using Microsoft.CodeAnalysis;
using System.Text;

namespace Knapcode.ExplorePackages
{
    public class KustoTableBuilder : IPropertyVisitor
    {
        private readonly int _indent;
        private readonly StringBuilder _builder;

        public KustoTableBuilder(int indent)
        {
            _indent = indent;
            _builder = new StringBuilder();
        }

        public void OnProperty(IPropertySymbol symbol, string prettyPropType)
        {
            if (_builder.Length > 0)
            {
                _builder.Append(',');
                _builder.AppendLine();
            }

            _builder.Append(' ', _indent);
            _builder.AppendFormat("{0}: {1}", symbol.Name, PropertyHelper.GetKustoDataType(symbol));
        }

        public void Finish()
        {
        }

        public string GetResult() => _builder.ToString();
    }
}
