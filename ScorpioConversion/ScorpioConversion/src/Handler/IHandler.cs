using System;
using System.Collections.Generic;
using Scorpio.Commons;
namespace Scorpio.Conversion {
    public interface IHandler : IDisposable {
        void Handle(LanguageInfo languageInfo, List<TableBuilder> successTables, SortedDictionary<string, List<TableBuilder>> successSpawns, List<L10NData> l10NDatas, CommandLine command);
    }
}
