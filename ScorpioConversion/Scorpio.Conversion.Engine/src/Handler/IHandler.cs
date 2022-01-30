using System;
using System.Collections.Generic;
using Scorpio.Commons;
using Scorpio.Conversion.Engine;

namespace Scorpio.Conversion.Engine {
    public interface IHandler : IDisposable {
        void Handle(LanguageInfo languageInfo, List<TableBuilder> successTables, SortedDictionary<string, List<TableBuilder>> successSpawns, List<L10NData> l10NDatas, CommandLine command);
    }
}
