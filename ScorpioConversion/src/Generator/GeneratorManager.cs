using Scorpio;
public class GeneratorManager : BaseManager<IGenerator> {
    public static GeneratorManager Instance { get; } = new GeneratorManager();
    protected override string Name { get; } = "Generator";
    public void Add(string name, ScriptValue scriptValue) {
        Add(new ScriptGenerator(name, scriptValue));
    }
}
