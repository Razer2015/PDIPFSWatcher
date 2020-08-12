using Microsoft.Diagnostics.Tracing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDIPFSWatcher.Tracing
{
    public enum FilterRuleResult
    {
        Skip,
        Include,
        Exclude
    }

    public enum CompareType
    {
        Equals,
        NotEquals,
        Contains,
        NotContains
    }

    public interface IFilterRule
    {
        FilterRuleResult Evaluate(TraceEvent evt);
        bool IsActive { get; set; }
        bool Include { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class FilterAttribute : Attribute
    {
        public string Name { get; }

        public FilterAttribute(string name)
        {
            Name = name;
        }

        public string Description { get; set; }
        public Type ViewModelType { get; set; }
    }

    public class TraceEventFilter
    {
        ObservableCollection<IFilterRule> _filterRules = new ObservableCollection<IFilterRule>();

        public FilterRuleResult DefaultResult { get; set; } = FilterRuleResult.Exclude;

        public IList<IFilterRule> FilterRules => _filterRules;

        public virtual FilterRuleResult EvaluateEvent(TraceEvent evt)
        {
            if (FilterRules.Count == 0)
                return FilterRuleResult.Include;

            foreach (var rule in FilterRules)
            {
                if (rule.IsActive)
                {
                    var result = rule.Evaluate(evt);
                    if (result != FilterRuleResult.Skip)
                        return result;
                }
            }

            return DefaultResult;
        }
    }
}
