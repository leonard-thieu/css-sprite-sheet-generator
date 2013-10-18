using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CssSpriteSheetGenerator.Models
{
    /// <summary>
    /// Represents a CSS rule.
    /// </summary>
    public class CssRule
    {
        private Dictionary<string, string> _Declarations;
        private Collection<string> _Selectors;

        /// <summary>
        /// CSS declarations consisting of property/value pairs.
        /// </summary>
        public Dictionary<string, string> Declarations { get { return _Declarations ?? (_Declarations = new Dictionary<string, string>()); } }

        /// <summary>
        /// One or more CSS selectors.
        /// </summary>
        public Collection<string> Selectors { get { return _Selectors ?? (_Selectors = new Collection<string>()); } }

        /// <summary>
        /// Outputs the selectors and declarations of this CSS rule as formatted CSS.
        /// </summary>
        /// <returns>The CSS representation of this CSS rule.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            // Build selectors
            for (int i = 0; i < Selectors.Count; i++)
            {
                sb.Append(".");
                sb.Append(Selectors[i]);
                if (i != Selectors.Count - 1)
                    sb.Append(", ");
            }
            sb.AppendLine();

            // Build declarations
            sb.AppendLine("{");
            foreach (var declaration in Declarations)
            {
                sb.Append("    ");
                sb.Append(declaration.Key);
                sb.Append(": ");
                sb.Append(declaration.Value);
                sb.AppendLine(";");
            }
            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}
