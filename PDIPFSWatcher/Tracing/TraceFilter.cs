﻿using Microsoft.Diagnostics.Tracing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PDIPFSWatcher.Tracing
{
    class TraceFilter
    {
        ObservableCollection<IFilterRule> _filterRules = new ObservableCollection<IFilterRule>();

        public IList<IFilterRule> FilterRules => _filterRules;

        public FilterRuleResult EvaluateEvent(TraceEvent evt)
        {
            foreach (var rule in FilterRules)
            {
                var result = rule.Evaluate(evt);
                if (result != FilterRuleResult.Skip)
                    return result;
            }

            return FilterRuleResult.Include;
        }
    }
}
