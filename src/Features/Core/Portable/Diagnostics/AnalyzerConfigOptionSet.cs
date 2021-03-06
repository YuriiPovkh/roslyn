﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Options;

namespace Microsoft.CodeAnalysis.Diagnostics
{
    /// <summary>
    /// This class proxies requests for option values first to the <see cref="AnalyzerConfigOptions" /> then to a backup <see cref="OptionSet" /> if provided.
    /// </summary>
    internal sealed class AnalyzerConfigOptionSet : OptionSet
    {
        private readonly AnalyzerConfigOptions _analyzerConfigOptions;
        private readonly OptionSet? _optionSet;

        public AnalyzerConfigOptionSet(AnalyzerConfigOptions analyzerConfigOptions, OptionSet? optionSet)
        {
            _analyzerConfigOptions = analyzerConfigOptions;
            _optionSet = optionSet;
        }

        public override object GetOption(OptionKey optionKey)
        {
            // First try to find the option from the .editorconfig options parsed by the compiler.
            if (_analyzerConfigOptions.TryGetEditorConfigOption<object>(optionKey.Option, out var value))
            {
                return value;
            }

            // Fallback to looking for option from the document's optionset if unsuccessful.
            return _optionSet?.GetOption(optionKey) ?? optionKey.Option.DefaultValue!;
        }

        public override OptionSet WithChangedOption(OptionKey optionAndLanguage, object? value)
        {
            throw new NotImplementedException();
        }

        internal override IEnumerable<OptionKey> GetChangedOptions(OptionSet optionSet)
        {
            throw new NotImplementedException();
        }
    }
}
