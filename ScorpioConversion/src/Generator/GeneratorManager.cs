using System.Reflection;
using System.Collections.Generic;
public class GeneratorManager {
    public static GeneratorManager Instance { get; } = new GeneratorManager();
    private Dictionary<string, IGenerator> generators = new Dictionary<string, IGenerator> ();
    public void Add(IGenerator generator) {
        generators[generator.Language] = generator;
    }
    public void Add(Assembly assembly) {
        foreach (var type in assembly.GetTypes()) {
            if (type.IsAssignableFrom(typeof(IGenerator))) {
                Add(System.Activator.CreateInstance(type) as IGenerator);
            }
        }
    }
    public IGenerator Get(string language) {
        if (generators.TryGetValue(language, out IGenerator generator)) {
            return generator;
        }
        throw new System.Exception($"找不到language:{language} 生成器");
    }
}
