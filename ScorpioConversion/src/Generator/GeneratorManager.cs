using System;
using System.Reflection;
using System.Collections.Generic;
using Scorpio.Commons;
using Scorpio;
public class GeneratorManager {
    public static GeneratorManager Instance { get; } = new GeneratorManager();
    private readonly Type TypeIGenerator = typeof(IGenerator);
    private readonly Type TypeScriptGenerator = typeof(ScriptGenerator);
    private readonly BindingFlags BindingFlags = BindingFlags.Public | BindingFlags.Instance;
    private Dictionary<string, IGenerator> generators = new Dictionary<string, IGenerator> ();
    public void Add(IGenerator generator) {
        Logger.info($"添加生成器 {generator.Language} - {generator.GetType()}");
        generators[generator.Language] = generator;
    }
    public void Add(Assembly assembly) {
        foreach (var type in assembly.GetTypes()) {
            if (TypeIGenerator.IsAssignableFrom(type) && type != TypeIGenerator) {
                foreach (var method in type.GetConstructors(BindingFlags)) {
                    if (method.GetParameters().Length == 0) {
                        Add(method.Invoke(null) as IGenerator);
                    }
                }
            }
        }
    }
    public void Add(string language, ScriptValue scriptValue) {
        Add(new ScriptGenerator(language, scriptValue));
    }
    public IGenerator Get(string language) {
        if (generators.TryGetValue(language, out IGenerator generator)) {
            return generator;
        }
        throw new Exception($"找不到language:{language} 生成器");
    }
}
